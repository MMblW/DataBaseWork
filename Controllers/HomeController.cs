using System.Diagnostics;
using DataBaseWork.Data;
using Microsoft.AspNetCore.Mvc;
using DataBaseWork.Models;
using SimpleSite.Models;

namespace SimpleSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult StudentQueries()
        {
            var model = new StudentQueriesViewModel
            {
                // Запрос 1: Студенты группы РИС-23
                GroupRIS23 = _context.Student
                    .Where(s => s.GroupCode == "РИС-23")
                    .ToList(),

                // Запрос 2: Оценки по программированию
                ProgrammingGrades = _context.Student
                    .Join(_context.Examination,
                        s => s.GradebookNumber,
                        e => e.GradebookNumber,
                        (s, e) => new { Student = s, Exam = e })
                    .Where(x => x.Exam.DisciplineName == "Программирование")
                    .Select(x => new ProgrammingGradeResult
                    {
                        Surname = x.Student.Surname,
                        StudentName = x.Student.StudentName,
                        Patronymic = x.Student.Patronymic,
                        Grade = x.Exam.Grade
                    })
                    .ToList(),

                // Запрос 3: Студенты с пересдачами по программированию
                ProgrammingRetakes = _context.Student
                    .Join(_context.Examination.Where(e => e.DisciplineName == "Программирование" && e.NumberOfReexamination > 0),
                        s => s.GradebookNumber,
                        e => e.GradebookNumber,
                        (s, e) => new ProgrammingGradeResult
                        {
                            Surname = s.Surname,
                            StudentName = s.StudentName,
                            Patronymic = s.Patronymic,
                            Grade = e.Grade
                        })
                    .ToList(),

                // Запрос 4: Студенты со стипендией
                ScholarshipStudents = _context.Student
                    .Where(s => s.HasScholarship)
                    .Select(s => new StudentBasicInfo
                    {
                        Surname = s.Surname,
                        StudentName = s.StudentName,
                        Patronymic = s.Patronymic
                    })
                    .ToList(),

                // Запрос 5: Дисциплины гуманитарного факультета
                HumanitiesDisciplines = _context.Disciplines_on_faculties
                    .Where(d => d.FacultyName == "Гуманитарный факультет" && d.IsStudied)
                    .Select(d => d.DisciplineName)
                    .ToList(),

                // Запрос 6: Мужчины старше 18 лет
                AdultMaleStudents = _context.Student
                    .Where(s => s.Gender == "М" && s.DateOfBirth < new DateTime(2006, 1, 1))
                    .Select(s => new AdultStudentInfo
                    {
                        Surname = s.Surname,
                        StudentName = s.StudentName,
                        Patronymic = s.Patronymic,
                        DateOfBirth = s.DateOfBirth,
                        GroupCode = s.GroupCode
                    })
                    .ToList(),

                // Запрос 7: Студенты электротехнического факультета
                TechFacultyStudents = _context.Student
                    .Join(_context.Study_Groups,
                        s => s.GroupCode,
                        g => g.GroupCode,
                        (s, g) => new { Student = s, Group = g })
                    .Join(_context.Speciality,
                        sg => sg.Group.SpecialityName,
                        sp => sp.SpecialityName,
                        (sg, sp) => new { sg.Student, Speciality = sp })
                    .Where(x => x.Speciality.FacultyName == "Электротехнический факультет")
                    .Select(x => new TechStudentInfo
                    {
                        Surname = x.Student.Surname,
                        StudentName = x.Student.StudentName,
                        Patronymic = x.Student.Patronymic,
                        GroupCode = x.Student.GroupCode,
                        SpecialityName = x.Speciality.SpecialityName
                    })
                    .ToList(),

                // Запрос 8: Экзамены студента ЭТФ-24-1 за период
                StudentExams = _context.Examination
                    .Where(e => e.GradebookNumber == "ЭТФ-24-1" &&
                               e.DateOfExamination >= new DateTime(2025, 1, 24) &&
                               e.DateOfExamination <= new DateTime(2025, 1, 31))
                    .Select(e => new ExamInfo
                    {
                        DisciplineName = e.DisciplineName,
                        Grade = e.Grade,
                        DateOfExamination = e.DateOfExamination,
                        NumberOfReexamination = e.NumberOfReexamination
                    })
                    .ToList()
            };
            return View(model);
        }
    }
}
using DataBaseWork.Data;
using DataBaseWork.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DataBaseWork.Controllers
{
    public class QueriesController : Controller
    {
        private readonly AppDbContext _context;

        public QueriesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new AllTablesViewModel
            {
                Faculties = _context.Faculty.ToList(),
                Specialities = _context.Speciality.ToList(),
                StudyGroups = _context.Study_Groups.ToList(),
                Students = _context.Student.ToList(),
                Disciplines = _context.Discipline.ToList(),
                Examinations = _context.Examination.ToList(),
                DisciplinesOnFaculties = _context.Disciplines_on_faculties.ToList()
            };

            return View(model);
        }
    }

    public class AllTablesViewModel
    {
        public List<Faculty> Faculties { get; set; }
        public List<Speciality> Specialities { get; set; }
        public List<StudyGroup> StudyGroups { get; set; }
        public List<Student> Students { get; set; }
        public List<Discipline> Disciplines { get; set; }
        public List<Examination> Examinations { get; set; }
        public List<DisciplinesOnFaculty> DisciplinesOnFaculties { get; set; }
    }
}
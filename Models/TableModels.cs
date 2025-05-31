/*// Models/Student.cs
using System.ComponentModel.DataAnnotations;

namespace DataBaseWork.Models
{
    public class Faculty
    {
        [Key]
        public string faculty_name { get; set; }
    }
    public class Speciality
    {
        [Key]
        public string SpecialityName { get; set; }
        public string faculty_name { get; set; }
        public int NumberOfGroups { get; set; }
    }
    public class Study_Groups
    {
        [Key]
        public string GroupCode { get; set; }
        public string SpecialityName { get; set; }
        public string TutorSurname { get; set; }
    }
    public class Student
    {
        [Key] // Указываем, что это первичный ключ
        public string GradebookNumber { get; set; } // Номер зачетки как первичный ключ
        public string GroupCode { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public bool HasScholarship { get; set; }
    }
    public class Discipline
    {
        [Key]
        public string DisciplineName { get; set; }
        public string TutorSurname { get; set; }
    }
    public class Examination
    {
        [Key]
        public string ExaminationID { get; set; }
        public string GradebookNumber { get; set; }
        public string DisciplineName { get; set; }
        public string Grade { get; set; }
        public int NumberOfReexamination { get; set; }
        public DateTime DateOfExamination { get; set; }
    }
    public class Disciplines_On_Faculties
    {
        public string faculty_name { get; set; }
        public string DisciplineName { get; set; }
        public bool IsStudied { get; set; }
    }
}
*/

// Models/Faculty.cs
using System.Collections.Generic;
namespace DataBaseWork.Models
{
    public class Faculty
    {
        public string FacultyName { get; set; }
        public ICollection<Speciality> Specialities { get; set; }
        public ICollection<DisciplinesOnFaculty> Disciplines { get; set; }
    }

    // Models/Speciality.cs

    public class Speciality
    {
        public string SpecialityName { get; set; }
        public string FacultyName { get; set; }
        public int NumberOfGroups { get; set; }
        public Faculty Faculty { get; set; }
        public ICollection<StudyGroup> StudyGroups { get; set; }
    }

    // Models/StudyGroup.cs

    public class StudyGroup
    {
        public string GroupCode { get; set; }
        public string SpecialityName { get; set; }
        public string TutorSurname { get; set; }
        public Speciality Speciality { get; set; }
        public ICollection<Student> Students { get; set; }
    }

    // Models/Student.cs

    public class Student
    {
        public string GradebookNumber { get; set; }
        public string GroupCode { get; set; }
        public string Surname { get; set; }
        public string StudentName { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public bool HasScholarship { get; set; }
        public StudyGroup StudyGroup { get; set; }
        public ICollection<Examination> Examinations { get; set; }
    }

    // Models/Discipline.cs

    public class Discipline
    {
        public string DisciplineName { get; set; }
        public string TutorSurname { get; set; }
        public ICollection<Examination> Examinations { get; set; }
        public ICollection<DisciplinesOnFaculty> Faculties { get; set; }
    }

    // Models/Examination.cs

    public class Examination
    {
        public int ExaminationId { get; set; }
        public string GradebookNumber { get; set; }
        public string DisciplineName { get; set; }
        public string Grade { get; set; }
        public int NumberOfReexamination { get; set; }
        public DateTime DateOfExamination { get; set; }
        public Student Student { get; set; }
        public Discipline Discipline { get; set; }
    }

    // Models/DisciplinesOnFaculty.cs
    public class DisciplinesOnFaculty
    {
        public string FacultyName { get; set; }
        public string DisciplineName { get; set; }
        public bool IsStudied { get; set; }
        public Faculty Faculty { get; set; }
        public Discipline Discipline { get; set; }
    }
    public class StudentQueriesViewModel
    {
        public List<Student> GroupRIS23 { get; set; }
        public List<ProgrammingGradeResult> ProgrammingGrades { get; set; }
        public List<ProgrammingGradeResult> ProgrammingRetakes { get; set; }
        public List<StudentBasicInfo> ScholarshipStudents { get; set; }
        public List<string> HumanitiesDisciplines { get; set; }
        public List<AdultStudentInfo> AdultMaleStudents { get; set; }
        public List<TechStudentInfo> TechFacultyStudents { get; set; }
        public List<ExamInfo> StudentExams { get; set; }
    }

    public class ProgrammingGradeResult
    {
        public string Surname { get; set; }
        public string StudentName { get; set; }
        public string Patronymic { get; set; }
        public string Grade { get; set; }
    }

    public class StudentBasicInfo
    {
        public string Surname { get; set; }
        public string StudentName { get; set; }
        public string Patronymic { get; set; }
    }

    public class AdultStudentInfo
    {
        public string Surname { get; set; }
        public string StudentName { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string GroupCode { get; set; }
    }

    public class TechStudentInfo
    {
        public string Surname { get; set; }
        public string StudentName { get; set; }
        public string Patronymic { get; set; }
        public string GroupCode { get; set; }
        public string SpecialityName { get; set; }
    }

    public class ExamInfo
    {
        public string DisciplineName { get; set; }
        public string Grade { get; set; }
        public DateTime DateOfExamination { get; set; }
        public int NumberOfReexamination { get; set; }
    }
}
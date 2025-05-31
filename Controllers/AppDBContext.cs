using Microsoft.EntityFrameworkCore;
using DataBaseWork.Models;

namespace DataBaseWork.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<Speciality> Speciality { get; set; }
        public DbSet<StudyGroup> Study_Groups { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Discipline> Discipline { get; set; }
        public DbSet<Examination> Examination { get; set; }
        public DbSet<DisciplinesOnFaculty> Disciplines_on_faculties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("Faculty");
                entity.HasKey(e => e.FacultyName);
                entity.Property(e => e.FacultyName).HasColumnName("faculty_name").HasMaxLength(50);
            });

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.ToTable("Speciality");
                entity.HasKey(e => e.SpecialityName);

                entity.Property(e => e.SpecialityName)
                      .HasColumnName("speciality_name")
                      .HasMaxLength(50);

                entity.Property(e => e.FacultyName)
                      .HasColumnName("faculty_name")
                      .HasMaxLength(50);

                entity.Property(e => e.NumberOfGroups)
                      .HasColumnName("number_of_groups");

                entity.HasOne(e => e.Faculty)
                      .WithMany(f => f.Specialities)
                      .HasForeignKey(e => e.FacultyName);
            });

            modelBuilder.Entity<StudyGroup>(entity =>
            {
                entity.ToTable("Study_Groups");
                entity.HasKey(e => e.GroupCode);
                entity.Property(e => e.GroupCode).HasColumnName("group_code").HasMaxLength(10);
                entity.Property(e => e.SpecialityName).HasColumnName("speciality_name").HasMaxLength(50);
                entity.Property(e => e.TutorSurname).HasColumnName("tutor_surname").HasMaxLength(50);
                entity.HasOne(e => e.Speciality).WithMany(s => s.StudyGroups).HasForeignKey(e => e.SpecialityName);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");
                entity.HasKey(e => e.GradebookNumber);
                entity.Property(e => e.GradebookNumber).HasColumnName("gradebook_number").HasMaxLength(10);
                entity.Property(e => e.GroupCode).HasColumnName("group_code").HasMaxLength(10);
                entity.Property(e => e.Surname).HasColumnName("surname").HasMaxLength(50);
                entity.Property(e => e.StudentName).HasColumnName("student_name").HasMaxLength(50);
                entity.Property(e => e.Patronymic).HasColumnName("patronymic").HasMaxLength(50);
                entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
                entity.Property(e => e.Gender).HasColumnName("gender").HasMaxLength(1);
                entity.Property(e => e.HasScholarship).HasColumnName("has_scholarship");
                entity.HasOne(e => e.StudyGroup).WithMany(g => g.Students).HasForeignKey(e => e.GroupCode);
            });

            modelBuilder.Entity<Discipline>(entity =>
            {
                entity.ToTable("Discipline");
                entity.HasKey(e => e.DisciplineName);
                entity.Property(e => e.DisciplineName).HasColumnName("discipline_name").HasMaxLength(50);
                entity.Property(e => e.TutorSurname).HasColumnName("tutor_surname").HasMaxLength(50);
            });

            modelBuilder.Entity<Examination>(entity =>
            {
                entity.ToTable("Examination");
                entity.HasKey(e => e.ExaminationId);
                entity.Property(e => e.ExaminationId).HasColumnName("examination_id").ValueGeneratedOnAdd();
                entity.Property(e => e.GradebookNumber).HasColumnName("gradebook_number").HasMaxLength(10);
                entity.Property(e => e.DisciplineName).HasColumnName("discipline_name").HasMaxLength(50);
                entity.Property(e => e.Grade).HasColumnName("grade").HasMaxLength(10);
                entity.Property(e => e.NumberOfReexamination).HasColumnName("number_of_reexamination");
                entity.Property(e => e.DateOfExamination).HasColumnName("date_of_examination");
                entity.HasOne(e => e.Student).WithMany(s => s.Examinations).HasForeignKey(e => e.GradebookNumber);
                entity.HasOne(e => e.Discipline).WithMany(d => d.Examinations).HasForeignKey(e => e.DisciplineName);
            });

            modelBuilder.Entity<DisciplinesOnFaculty>(entity =>
            {
                entity.ToTable("Disciplines_on_faculties");
                entity.HasKey(e => new { e.FacultyName, e.DisciplineName });
                entity.Property(e => e.FacultyName).HasColumnName("faculty_name").HasMaxLength(50);
                entity.Property(e => e.DisciplineName).HasColumnName("discipline_name").HasMaxLength(50);
                entity.Property(e => e.IsStudied).HasColumnName("is_studied");
                entity.HasOne(e => e.Faculty).WithMany(f => f.Disciplines).HasForeignKey(e => e.FacultyName);
                entity.HasOne(e => e.Discipline).WithMany(d => d.Faculties).HasForeignKey(e => e.DisciplineName);
            });
        }
    }
}
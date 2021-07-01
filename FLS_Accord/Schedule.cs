using FLS_Accord.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FLS_Accord
{

    public class GenerateTimetableInput
    {
        public List<CourseInput> Courses = new List<CourseInput>();
        public List<SubjectInput> Subjects = new List<SubjectInput>();
        public List<LecturerInput> Lecturers = new List<LecturerInput>();
        public List<SubjectRegister> SubjectRegisters = new List<SubjectRegister>();
        public  GenerateTimetableInput(List<CourseInput> courses, List<SubjectInput> subjects, List<LecturerInput> lecturers, List<SubjectRegister> subjectRegisters)
        {
            Courses = courses;
            Subjects = subjects;
            Lecturers = lecturers;
            SubjectRegisters = subjectRegisters;
        }
    }

    class Schedule
    {
        private string lecturerCode;
        private int subjectCode;

        public string LecturerCode { get => lecturerCode; set => lecturerCode = value; }
        public int SubjectCode { get => subjectCode; set => subjectCode = value; }
    }

    public class SubjectInput
    {
        public string Name { get; set; }

        public List<CourseInput> Courses { get; set; }

        public SubjectInput(string name, List<CourseInput> courses)
        {
            Name = name;
            this.Courses = courses;
        }
    }

    public class CourseInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubjectId { get; set; }
        public int SemesterId { get; set; }
        public int LecturerId { get; set; }

        public virtual Semester Semester { get; set; }
        public virtual Subject Subject { get; set; }
        public LecturerInput Lecturer { get; set; }
    }

    public class LecturerInput
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public int MaxCourse { get; set; }
        public int MinCourse { get; set; }
        public int LecturerTypeId { get; set; }
        public int PriorityPoint { get; set; }
        public int DepartmentId { get; set; }
        public string LecturerCode { get; set; }

        public virtual Department Department { get; set; }
        public virtual LecturerType LecturerType { get; set; }
        public virtual List<LectureSemesterRegister> LectureSemesterRegister { get; set; }
        public virtual List<SubjectRegister> SubjectRegister { get; set; }
        public virtual List<TeachableSubject> TeachableSubject { get; set; }
        public List<Course> OccupiedCourse { get; set; }
    }
}

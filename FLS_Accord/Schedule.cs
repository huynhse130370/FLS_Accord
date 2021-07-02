using FLS_Accord.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FLS_Accord
{

    public class GenerateTimetableInput : IDisposable
    {
        public List<CourseInput> Courses = new List<CourseInput>();
        public List<SubjectInput> Subjects = new List<SubjectInput>();
        public List<LecturerInput> Lecturers = new List<LecturerInput>();
        public List<SubjectRegister> SubjectRegisterInputs = new List<SubjectRegister>();
        public List<TeachableSubjectInput> TeachableSubjectInputs = new List<TeachableSubjectInput>();

        public int TimetableType { get; set; }

        public GenerateTimetableInput()
        {
        }

        public GenerateTimetableInput(List<CourseInput> courses, List<SubjectInput> subjects, List<LecturerInput> lecturers, List<SubjectRegister> subjectRegisters, List<TeachableSubjectInput> teachableSubjectInputs, int timetableType)
        {
            Courses = courses;
            Subjects = subjects;
            Lecturers = lecturers;
            SubjectRegisterInputs = subjectRegisters;
            TeachableSubjectInputs = teachableSubjectInputs;
            TimetableType = timetableType;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    class Schedule
    {
        private string lecturerCode;
        private string subjectCode;
        private string course;

        public string LecturerCode { get => lecturerCode; set => lecturerCode = value; }
        public string SubjectCode { get => subjectCode; set => subjectCode = value; }

        public string Course { get => course; set => course = value; }
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
        public virtual SubjectInput Subject { get; set; }
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
        public List<CourseInput> OccupiedCourse { get; set; }

    }

    public partial class TeachableSubjectInput
    {
        public int LecturerId { get; set; }
        public int SubjectId { get; set; }
        public int PreferPoint { get; set; }
        public int MatchPoint { get; set; }

        public virtual LecturerInput Lecturer { get; set; }
        public virtual SubjectInput Subject { get; set; }
    }

    public class LecturerCourse
    {
        public LecturerInput Lecturer { get; set; }
        public CourseInput Course { get; set; }
    }
}

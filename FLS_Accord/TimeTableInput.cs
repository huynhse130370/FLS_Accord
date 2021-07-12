using FLS_Accord.Models;
using System;
using System.Collections.Generic;

namespace FLS_Accord
{

    public class GenerateTimetableInput : IDisposable
    {
        public List<CourseInput> Courses = new List<CourseInput>();
        public List<SubjectInput> Subjects = new List<SubjectInput>();
        public List<LecturerInput> Lecturers = new List<LecturerInput>();
        public List<TimeSlotInput> TimeSlots = new List<TimeSlotInput>();
        public List<SubjectRegisterInput> SubjectRegisterInputs = new List<SubjectRegisterInput>();
        public List<TeachableSubjectInput> TeachableSubjectInputs = new List<TeachableSubjectInput>();
        public List<StudentGroupInput> StudentGroupInputs = new List<StudentGroupInput>();

        public GenerateTimetableInput(List<CourseInput> courses, List<SubjectInput> subjects, List<LecturerInput> lecturers, List<TimeSlotInput> timeSlots, List<SubjectRegisterInput> subjectRegisterInputs, List<TeachableSubjectInput> teachableSubjectInputs, List<StudentGroupInput> studentGroupInputs)
        {
            Courses = courses;
            Subjects = subjects;
            Lecturers = lecturers;
            TimeSlots = timeSlots;
            SubjectRegisterInputs = subjectRegisterInputs;
            TeachableSubjectInputs = teachableSubjectInputs;
            StudentGroupInputs = studentGroupInputs;
        }


        public GenerateTimetableInput()
        {
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    public class SubjectInput
    {
        public string Name { get; set; }
        public string SubjectCode { get; set; }
    }

    public class CourseInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubjectId { get; set; }
        public int SemesterId { get; set; }
        public int LecturerId { get; set; }

        public Semester Semester { get; set; }
        public SubjectInput Subject { get; set; }
        public LecturerInput Lecturer { get; set; }
        public StudentGroupInput StudentGroup { get; set; }
    }

    public class CourseSlotInput
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int TimeSlotId { get; set; }

        public CourseInput Course { get; set; }
        public List<TimeSlotInput> TimeSlot { get; set; }
    }

    public class TimeSlotInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int DayOfWeek { get; set; }
    }


    public class StudentGroupInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TimeSlotInput> OccupiedTimeSlot { get; set; }
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


        public Department Department { get; set; }
        public LecturerType LecturerType { get; set; }
        public List<LectureSemesterRegister> LectureSemesterRegister { get; set; }
        public List<SubjectRegister> SubjectRegister { get; set; }
        public List<TeachableSubject> TeachableSubject { get; set; }
        public List<CourseInput> OccupiedCourse { get; set; }
        public List<TimeSlotInput> OccupiedTimeSlot { get; set; }

    }

    public class TeachableSubjectInput
    {
        public int LecturerId { get; set; }
        public int SubjectId { get; set; }
        public int PreferPoint { get; set; }
        public int MatchPoint { get; set; }

        public LecturerInput Lecturer { get; set; }
        public SubjectInput Subject { get; set; }
    }

    public class SubjectRegisterInput
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }
        public int SubjectId { get; set; }
        public int SemesterPlanId { get; set; }

        public LecturerInput Lecturer { get; set; }
        public SemesterPlan SemesterPlan { get; set; }
        public SubjectInput Subject { get; set; }
    }

    public class LecturerCourseSlot
    {
        public LecturerInput Lecturer { get; set; }
        public CourseInput Course { get; set; }
        public List<TimeSlotInput> TimeSlots { get; set; }
        public StudentGroupInput StudentGroupInput { get; set; }
    }
}

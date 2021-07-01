using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class Lecturer
    {
        public Lecturer()
        {
            LectureSemesterRegister = new HashSet<LectureSemesterRegister>();
            LecturerRating = new HashSet<LecturerRating>();
            SubjectRegister = new HashSet<SubjectRegister>();
            TeachableSubject = new HashSet<TeachableSubject>();
            TimeSlotRegister = new HashSet<TimeSlotRegister>();
        }

        public int Id { get; set; }
        public bool Status { get; set; }
        public int MaxCourse { get; set; }
        public int MinCourse { get; set; }
        public int LecturerTypeId { get; set; }
        public int PriorityPoint { get; set; }
        public int DepartmentId { get; set; }
        public int UserId { get; set; }
        public string LecturerCode { get; set; }

        public virtual Department Department { get; set; }
        public virtual LecturerType LecturerType { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<LectureSemesterRegister> LectureSemesterRegister { get; set; }
        public virtual ICollection<LecturerRating> LecturerRating { get; set; }
        public virtual ICollection<SubjectRegister> SubjectRegister { get; set; }
        public virtual ICollection<TeachableSubject> TeachableSubject { get; set; }
        public virtual ICollection<TimeSlotRegister> TimeSlotRegister { get; set; }
    }
}

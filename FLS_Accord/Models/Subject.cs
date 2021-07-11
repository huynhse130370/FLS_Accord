using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Course = new HashSet<Course>();
            SubjectRegister = new HashSet<SubjectRegister>();
            TeachableSubject = new HashSet<TeachableSubject>();
        }

        public int Id { get; set; }
        public string SubjectCode { get; set; }
        public string Name { get; set; }
        public string PreviousCode { get; set; }
        public int DepartmentId { get; set; }
        public int SlotInWeek { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Course> Course { get; set; }
        public virtual ICollection<SubjectRegister> SubjectRegister { get; set; }
        public virtual ICollection<TeachableSubject> TeachableSubject { get; set; }
    }
}

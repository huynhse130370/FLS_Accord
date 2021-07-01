using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class Semester
    {
        public Semester()
        {
            Course = new HashSet<Course>();
            LectureSemesterRegister = new HashSet<LectureSemesterRegister>();
            MasterPlan = new HashSet<MasterPlan>();
            SemesterPlan = new HashSet<SemesterPlan>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Course> Course { get; set; }
        public virtual ICollection<LectureSemesterRegister> LectureSemesterRegister { get; set; }
        public virtual ICollection<MasterPlan> MasterPlan { get; set; }
        public virtual ICollection<SemesterPlan> SemesterPlan { get; set; }
    }
}

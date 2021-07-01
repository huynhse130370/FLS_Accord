using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class SemesterPlan
    {
        public SemesterPlan()
        {
            LecturerRating = new HashSet<LecturerRating>();
            SubjectRegister = new HashSet<SubjectRegister>();
            TimeSlotRegister = new HashSet<TimeSlotRegister>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SemesterId { get; set; }
        public int MasterPlanId { get; set; }

        public virtual MasterPlan MasterPlan { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual ICollection<LecturerRating> LecturerRating { get; set; }
        public virtual ICollection<SubjectRegister> SubjectRegister { get; set; }
        public virtual ICollection<TimeSlotRegister> TimeSlotRegister { get; set; }
    }
}

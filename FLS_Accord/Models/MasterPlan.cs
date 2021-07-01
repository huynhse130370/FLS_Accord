using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class MasterPlan
    {
        public MasterPlan()
        {
            SemesterPlan = new HashSet<SemesterPlan>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SemesterId { get; set; }

        public virtual Semester Semester { get; set; }
        public virtual ICollection<SemesterPlan> SemesterPlan { get; set; }
    }
}

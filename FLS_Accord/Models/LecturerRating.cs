using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class LecturerRating
    {
        public int LecturerId { get; set; }
        public int SemesterPlanId { get; set; }
        public int RatePoint { get; set; }

        public virtual Lecturer Lecturer { get; set; }
        public virtual SemesterPlan SemesterPlan { get; set; }
    }
}

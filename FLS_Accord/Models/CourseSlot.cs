using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class CourseSlot
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int TimeSlotId { get; set; }

        public virtual Course Course { get; set; }
        public virtual TimeSlot TimeSlot { get; set; }
    }
}

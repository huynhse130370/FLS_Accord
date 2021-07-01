using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class TimeSlot
    {
        public TimeSlot()
        {
            TimeSlotRegister = new HashSet<TimeSlotRegister>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int PriorityPoint { get; set; }

        public virtual ICollection<TimeSlotRegister> TimeSlotRegister { get; set; }
    }
}

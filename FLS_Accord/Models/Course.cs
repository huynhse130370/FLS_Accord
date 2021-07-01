using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubjectId { get; set; }
        public int SemesterId { get; set; }

        public virtual Semester Semester { get; set; }
        public virtual Subject Subject { get; set; }
    }
}

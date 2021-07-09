using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class Department
    {
        public Department()
        {
            DepartmentBlog = new HashSet<DepartmentBlog>();
            Lecturer = new HashSet<Lecturer>();
            Subject = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<DepartmentBlog> DepartmentBlog { get; set; }
        public virtual ICollection<Lecturer> Lecturer { get; set; }
        public virtual ICollection<Subject> Subject { get; set; }
    }
}

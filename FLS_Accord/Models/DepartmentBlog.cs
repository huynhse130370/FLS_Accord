using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class DepartmentBlog
    {
        public int DepartmentId { get; set; }
        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual Department Department { get; set; }
    }
}

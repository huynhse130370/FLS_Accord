using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class Blog
    {
        public Blog()
        {
            DepartmentBlog = new HashSet<DepartmentBlog>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ModifiledDate { get; set; }
        public int BlogCategoryId { get; set; }

        public virtual BlogCategory BlogCategory { get; set; }
        public virtual ICollection<DepartmentBlog> DepartmentBlog { get; set; }
    }
}

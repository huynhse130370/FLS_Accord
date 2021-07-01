using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class BlogCategory
    {
        public BlogCategory()
        {
            Blog = new HashSet<Blog>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Blog> Blog { get; set; }
    }
}

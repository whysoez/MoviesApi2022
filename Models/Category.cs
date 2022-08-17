using System;
using System.Collections.Generic;

namespace MoviesApi2022.Models
{
    public partial class Category
    {
        public Category()
        {
            Movies = new HashSet<Movie>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}

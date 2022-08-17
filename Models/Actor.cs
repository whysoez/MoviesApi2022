using System;
using System.Collections.Generic;

namespace MoviesApi2022.Models
{
    public partial class Actor
    {
        public int Id { get; set; }
        public string ActorName { get; set; } = null!;
        public int Age { get; set; }
        public string Email { get; set; } = null!;
        public int MovieId { get; set; }

        public virtual Movie? Movie { get; set; }
    }
}

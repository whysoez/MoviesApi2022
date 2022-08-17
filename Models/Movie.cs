using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi2022.Models
{
    public class Movie
    {
        public Movie()
        {
            Actors = new HashSet<Actor>();
        }

        [Key]
        [Required]
        [DisplayName("Mã phim")]
        public int MovieId { get; set; }

        [Required]
        [StringLength(50),MinLength(5)]
        [DisplayName("Tên")]
        public string Name { get; set; } = null!;

        [Required]
        [DisplayName("Thể loại")]
        public string Genre { get; set; } = null!;
        [Required]
        [DisplayName("Thời gian")]
        public string Duration { get; set; } = null!;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Thời gian ra mắt")]
        public DateTime ReleaseDate { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Actor> Actors { get; set; }
    }
}

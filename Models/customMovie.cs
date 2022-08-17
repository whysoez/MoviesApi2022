using System.Collections.Generic;

namespace MoviesApi2022.Models
{
    public class customMovie
    {
        public int Id { get; set; }
        public List<autoHead>? headers { get; set; }
        public ICollection<Movie>? Movies { get; set; }

    }

    public class autoHead
    {
        public string? propertyType { get; set; }
        public string? propertyName { get; set; }
        public string? propertyValue { get; set; }
    }
}

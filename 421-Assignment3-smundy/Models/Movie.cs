using System.ComponentModel.DataAnnotations;

namespace _421_Assignment3_smundy.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string? IMDBLink { get; set; }
        public string? Genre { get; set; }
        public int? ReleaseYear { get; set; }
        public byte[]? Poster { get; set; }
    }
}

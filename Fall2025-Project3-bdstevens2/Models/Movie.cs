using System.ComponentModel.DataAnnotations;

namespace Fall2025_Project3_bdstevens2.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Display(Name = "IMDB Link")]
        [Url]
        public string ImdbUrl { get; set; }

        [Required]
        public string Genre { get; set; }

        [Display(Name = "Release Year")]
        public int ReleaseYear { get; set; }

        // This will store the image
        public byte[]? Poster { get; set; }

        // Navigation property for the join table
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}

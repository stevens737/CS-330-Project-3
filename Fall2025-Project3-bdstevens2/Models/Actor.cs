using System.ComponentModel.DataAnnotations;

namespace Fall2025_Project3_bdstevens2.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Gender { get; set; }

        public int Age { get; set; }

        [Display(Name = "IMDB Link")]
        [Url]
        public string ImdbUrl { get; set; }

        public byte[]? Photo { get; set; }

        // Navigation property for the join table
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}

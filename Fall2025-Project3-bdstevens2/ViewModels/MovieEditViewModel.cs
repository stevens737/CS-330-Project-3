using System.ComponentModel.DataAnnotations;

namespace Fall2025_Project3_bdstevens2.ViewModels
{
    public class MovieEditViewModel
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

        [Display(Name = "Upload New Poster")]
        public IFormFile? PosterFile { get; set; }
        public byte[]? ExistingPoster { get; set; }
    }
}

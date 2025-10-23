using System.ComponentModel.DataAnnotations;

namespace Fall2025_Project3_bdstevens2.ViewModels
{
    public class MovieCreateViewModel
    {
        [Required]
        public string Title { get; set; }

        [Display(Name = "IMDB Link")]
        [Url]
        public string ImdbUrl { get; set; }

        [Required]
        public string Genre { get; set; }

        [Display(Name = "Release Year")]
        public int ReleaseYear { get; set; }

        [Display(Name = "Poster Image")]
        public IFormFile? PosterFile { get; set; }
    }
}

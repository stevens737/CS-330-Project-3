using System.ComponentModel.DataAnnotations;

namespace Fall2025_Project3_bdstevens2.ViewModels
{
    public class ActorCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        public string? Gender { get; set; }

        public int Age { get; set; }

        [Display(Name = "IMDB Link")]
        [Url]
        public string ImdbUrl { get; set; }

        [Display(Name = "Actor Photo")]
        public IFormFile? PhotoFile { get; set; }
    }
}

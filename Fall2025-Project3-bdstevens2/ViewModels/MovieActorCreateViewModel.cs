using Microsoft.AspNetCore.Mvc.Rendering;
using Fall2025_Project3_bdstevens2.Models;

namespace Fall2025_Project3_bdstevens2.ViewModels
{
    public class MovieActorCreateViewModel
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }

        public SelectList? MovieList { get; set; }
        public SelectList? ActorList { get; set; }
    }
}

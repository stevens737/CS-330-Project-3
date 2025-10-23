using System.ComponentModel.DataAnnotations;

namespace Fall2025_Project3_bdstevens2.Models
{
    public class MovieActor
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        public int ActorId { get; set; }
        public Actor Actor { get; set; } = null!;
    }
}

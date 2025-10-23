using Fall2025_Project3_bdstevens2.Models;

namespace Fall2025_Project3_bdstevens2.ViewModels
{
    public class MovieDetailViewModel
    {
        public Movie Movie { get; set; } = null!;
        public List<Actor> Actors { get; set; } = new List<Actor>();
        public List<ReviewSentiment> Reviews { get; set; } = new List<ReviewSentiment>();
        public double OverallSentiment { get; set; }
    }
}

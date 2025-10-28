using Fall2025_Project3_bdstevens2.Models;

namespace Fall2025_Project3_bdstevens2.ViewModels
{
    public class ActorDetailViewModel
    {
        public Actor Actor { get; set; } = null!;
        public List<Movie> Movies { get; set; } = new List<Movie>();
        public List<TweetSentiment> Tweets { get; set; } = new List<TweetSentiment>();
        public double OverallSentiment { get; set; }
    }
}

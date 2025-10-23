using Fall2025_Project3_bdstevens2.Models;

namespace Fall2025_Project3_bdstevens2.ViewModels
{
    public class ActorDetailViewModel
    {
        public Actor Actor { get; set; } = null!;

        // A list of all movies this actor is in
        public List<Movie> Movies { get; set; } = new List<Movie>();

        // The list of AI-generated tweets
        public List<TweetSentiment> Tweets { get; set; } = new List<TweetSentiment>();

        // The single, calculated average sentiment score
        public double OverallSentiment { get; set; }
    }
}

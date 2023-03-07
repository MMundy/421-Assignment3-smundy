namespace _421_Assignment3_smundy.Models
{
    public class ActorVM
    {
        public Actor Actor { get; set; }
        public List<TweetSentiment> TweetSentiments { get; set; }
        public double OverallSentimentScore { get; set; }
        public List<Movie> Movies { get; set; }
    }
}

namespace _421_Assignment3_smundy.Models
{
    public class ActorTweetSentimentVM
    {
        public Actor Actor { get; set; }
        public List<TweetSentiment> TweetSentiments { get; set;}
        public double OverallSentimentScore { get; set; }
        public List<MovieActor> Movies { get; set; }
    }
}

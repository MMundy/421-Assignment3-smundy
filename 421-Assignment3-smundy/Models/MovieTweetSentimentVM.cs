namespace _421_Assignment3_smundy.Models
{
    public class MovieTweetSentimentVM
    {
        public Movie Movie { get; set; }
        public List<TweetSentiment> TweetSentiments { get; set; }
        public double OverallSentimentScore { get; set; }
        public List<MovieActor> Actors { get; set; }
    }
}

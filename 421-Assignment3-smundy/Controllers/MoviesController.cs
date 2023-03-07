using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _421_Assignment3_smundy.Data;
using _421_Assignment3_smundy.Models;
using Tweetinvi;
using VaderSharp2;

namespace _421_Assignment3_smundy.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
              return View(await _context.Movie.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            MovieTweetSentimentVM movieTweetSentimentVM = new MovieTweetSentimentVM();
            movieTweetSentimentVM.Movie = movie;
            var name = movie.Title;
            movieTweetSentimentVM.Actors = _context.MovieActor.Where(m => m.MovieID == movie.Id).Include(m => m.Actor).ToList();

            List<TweetSentiment> tweetSentiment = new List<TweetSentiment>();
            var userClient = new TwitterClient("AAx9UfdCemph0Pg0t8Moq5c6L", "LbhoERpFGjBESYSNjTHuRvE0R80cGxZBx5lJWanM5lFpO2Hs63", "1455230009153503238-WTxQgoYUAQ3D9PTSsUu8stHkmJvuVe", "2ZVnM9tWbCSNAhyJcyC4WPIgiIbUWZ77MTLSx2Qb8TkW3");
            var searchResponse = await userClient.SearchV2.SearchTweetsAsync(movieTweetSentimentVM.Movie.Title);
            Console.WriteLine(movieTweetSentimentVM.Movie.Title);
            var tweets = searchResponse.Tweets; //fetches 100 tweets and stores them in an array
            var analyzer = new SentimentIntensityAnalyzer();

            //List<TweetSentiment> tweetSentiment = new List<TweetSentiment>();
            Double tweetTotal = 0;
            for (int i = 0; i < tweets.Length; i++) //create a new 
            { //stores each tweet and its sentiment score in the list
                Console.WriteLine(i.ToString() + " : " + tweets[i].Text);
                var results = analyzer.PolarityScores(tweets[i].Text);
                
                tweetSentiment.Add(new TweetSentiment { Tweet = tweets[i].Text, SentimentScore = results.Compound });
                Console.WriteLine("Compound score: " + results.Compound);
                tweetTotal += results.Compound;
            }
            movieTweetSentimentVM.OverallSentimentScore = tweetTotal / tweets.Length;
            movieTweetSentimentVM.TweetSentiments = tweetSentiment;

            return View(movieTweetSentimentVM);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,IMDBLink,Genre,ReleaseYear,Poster")] Movie movie, IFormFile Poster)
        {
            if (Poster != null && Poster.Length > 0) // This stores the photo in the photo field, which stores the photo in the DB when the saveChangesAsync runs
            {
                var memoryStream = new MemoryStream();
                await Poster.CopyToAsync(memoryStream);
                movie.Poster = memoryStream.ToArray();
            }
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        public async Task<IActionResult> GetMoviePoster(int id)
        {
            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            var imageData = movie.Poster;

            return File(imageData, "image/jpg");
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IMDBLink,Genre,ReleaseYear,Poster")] Movie movie, IFormFile Poster)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }
            if (Poster != null && Poster.Length > 0) // This stores the photo in the photo field, which stores the photo in the DB when the saveChangesAsync runs
            {
                var memoryStream = new MemoryStream();
                await Poster.CopyToAsync(memoryStream);
                movie.Poster = memoryStream.ToArray();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return _context.Movie.Any(e => e.Id == id);
        }
    }
}

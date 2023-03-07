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
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
              return View(await _context.Actor.ToListAsync());
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Actor == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            ActorTweetSentimentVM actorTweetSentimentVM = new ActorTweetSentimentVM();
            actorTweetSentimentVM.Actor = actor;
            var name = actor.Name;
            actorTweetSentimentVM.Movies = _context.MovieActor.Where(m => m.ActorID == actor.Id).Include(m => m.Movie).ToList();

            List<TweetSentiment> tweetSentiment = new List<TweetSentiment>();
            var userClient = new TwitterClient("AAx9UfdCemph0Pg0t8Moq5c6L", "LbhoERpFGjBESYSNjTHuRvE0R80cGxZBx5lJWanM5lFpO2Hs63", "1455230009153503238-WTxQgoYUAQ3D9PTSsUu8stHkmJvuVe", "2ZVnM9tWbCSNAhyJcyC4WPIgiIbUWZ77MTLSx2Qb8TkW3");
            //var searchResponse = await userClient.SearchV2.SearchTweetsAsync($"actorTweetSentimentVM.Actor.Name");
            var searchResponse = await userClient.SearchV2.SearchTweetsAsync(actorTweetSentimentVM.Actor.Name);
            Console.WriteLine(actorTweetSentimentVM.Actor.Name);
            var tweets = searchResponse.Tweets; //fetches 100 tweets and stores them in an array
            var analyzer = new SentimentIntensityAnalyzer();

            Double tweetTotal = 0;
            for (int i = 0; i < tweets.Length; i++) //create a new 
            { //stores each tweet and its sentiment score in the list
                Console.WriteLine(i.ToString() + " : " + tweets[i].Text);
                var results = analyzer.PolarityScores(tweets[i].Text);
                tweetSentiment.Add(new TweetSentiment { Tweet = tweets[i].Text, SentimentScore = results.Compound });
                Console.WriteLine("Compound score: " + results.Compound);
                tweetTotal += results.Compound;
            }
            actorTweetSentimentVM.OverallSentimentScore = tweetTotal / tweets.Length;
            actorTweetSentimentVM.TweetSentiments = tweetSentiment;
            return View(actorTweetSentimentVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Gender,Age,IMDBLink,Photo")] Actor actor, IFormFile Photo)
        {
            if (Photo != null && Photo.Length > 0) // This stores the photo in the photo field, which stores the photo in the DB when the saveChangesAsync runs
            {
                var memoryStream = new MemoryStream();
                await Photo.CopyToAsync(memoryStream);
                actor.Photo = memoryStream.ToArray();
            }
            if (ModelState.IsValid)
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        public async Task<IActionResult> GetActorPhoto(int id)
        {
            var actor = await _context.Actor.FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            var imageData = actor.Photo;

            return File(imageData, "image/jpg");
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Actor == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Gender,Age,IMDBLink,Photo")] Actor actor, IFormFile Photo)
        {
            if (id != actor.Id)
            {
                return NotFound();
            }
            if (Photo != null && Photo.Length > 0) // This stores the photo in the photo field, which stores the photo in the DB when the saveChangesAsync runs
            {
                var memoryStream = new MemoryStream();
                await Photo.CopyToAsync(memoryStream);
                actor.Photo = memoryStream.ToArray();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Id))
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

            return View(actor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Actor == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Actor == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Actor'  is null.");
            }
            var actor = await _context.Actor.FindAsync(id);
            if (actor != null)
            {
                _context.Actor.Remove(actor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
          return _context.Actor.Any(e => e.Id == id);
        }
    }
}

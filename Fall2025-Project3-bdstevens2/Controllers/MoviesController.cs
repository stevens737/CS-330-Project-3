using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_bdstevens2.Data;
using Fall2025_Project3_bdstevens2.Models;

namespace Fall2025_Project3_bdstevens2.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Services.OpenAIService _openAIService;
        private readonly VaderSharp2.SentimentIntensityAnalyzer _sentimentAnalyzer;

        public MoviesController(ApplicationDbContext context,
                                Services.OpenAIService openAIService,
                                VaderSharp2.SentimentIntensityAnalyzer sentimentAnalyzer)
        {
            _context = context;
            _openAIService = openAIService;
            _sentimentAnalyzer = sentimentAnalyzer;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.OrderBy(m => m.Title).ToListAsync();
            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            // 1. Get AI Reviews
            string rawReviews = await _openAIService.GetMovieReviewsAsync(movie.Title);
            string[] reviewStrings = rawReviews.Split(new[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);

            var reviews = new List<ViewModels.ReviewSentiment>();
            double totalSentiment = 0;

            // 2. Analyze Sentiment
            foreach (var reviewStr in reviewStrings)
            {
                var sentiment = _sentimentAnalyzer.PolarityScores(reviewStr);
                double score = sentiment.Compound;
                totalSentiment += score;
                reviews.Add(new ViewModels.ReviewSentiment
                {
                    Text = reviewStr.Trim(),
                    Sentiment = score
                });
            }

            // 3. Create ViewModel
            var vm = new ViewModels.MovieDetailViewModel
            {
                Movie = movie,
                Actors = movie.MovieActors.Select(ma => ma.Actor).ToList(),
                Reviews = reviews,
                OverallSentiment = reviews.Count > 0 ? totalSentiment / reviews.Count : 0
            };

            return View(vm);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View(new ViewModels.MovieCreateViewModel());
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViewModels.MovieCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Movie movie = new Movie
                {
                    Title = vm.Title,
                    ImdbUrl = vm.ImdbUrl,
                    Genre = vm.Genre,
                    ReleaseYear = vm.ReleaseYear
                };

                if (vm.PosterFile != null && vm.PosterFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await vm.PosterFile.CopyToAsync(memoryStream);
                        movie.Poster = memoryStream.ToArray();
                    }
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var vm = new ViewModels.MovieEditViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                ImdbUrl = movie.ImdbUrl,
                Genre = movie.Genre,
                ReleaseYear = movie.ReleaseYear,
                ExistingPoster = movie.Poster 
            };

            return View(vm);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ViewModels.MovieEditViewModel vm)
        {
            // Make sure the ID from the route matches the ID from the form's hidden field
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var movieToUpdate = await _context.Movies.FindAsync(vm.Id);
                    if (movieToUpdate == null)
                    {
                        return NotFound();
                    }

                    movieToUpdate.Title = vm.Title;
                    movieToUpdate.ImdbUrl = vm.ImdbUrl;
                    movieToUpdate.Genre = vm.Genre;
                    movieToUpdate.ReleaseYear = vm.ReleaseYear;

                    if (vm.PosterFile != null && vm.PosterFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await vm.PosterFile.CopyToAsync(memoryStream);
                            movieToUpdate.Poster = memoryStream.ToArray();
                        }
                    }

                    _context.Update(movieToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Movies.Any(e => e.Id == vm.Id))
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

            if (vm.ExistingPoster == null)
            {
                var originalMovie = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == vm.Id);
                if (originalMovie != null)
                {
                    vm.ExistingPoster = originalMovie.Poster;
                }
            }
            return View(vm);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
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
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}

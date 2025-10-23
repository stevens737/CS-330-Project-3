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
            return View(await _context.Actors.ToListAsync());
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View(new ViewModels.ActorCreateViewModel());
        }

        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViewModels.ActorCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Map ViewModel to the Actor model
                Actor actor = new Actor
                {
                    Name = vm.Name,
                    Gender = vm.Gender,
                    Age = vm.Age,
                    ImdbUrl = vm.ImdbUrl
                };

                // Handle the file upload
                if (vm.PhotoFile != null && vm.PhotoFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await vm.PhotoFile.CopyToAsync(memoryStream);
                        actor.Photo = memoryStream.ToArray(); // Save as byte[]
                    }
                }

                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // If not valid, return the view with the data the user entered
            return View(vm);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return NotFound();

            var vm = new ViewModels.ActorEditViewModel
            {
                Id = actor.Id,
                Name = actor.Name,
                Gender = actor.Gender,
                Age = actor.Age,
                ImdbUrl = actor.ImdbUrl,
                ExistingPhoto = actor.Photo
            };
            return View(vm);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ViewModels.ActorEditViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var actorToUpdate = await _context.Actors.FindAsync(vm.Id);
                    if (actorToUpdate == null) return NotFound();

                    actorToUpdate.Name = vm.Name;
                    actorToUpdate.Gender = vm.Gender;
                    actorToUpdate.Age = vm.Age;
                    actorToUpdate.ImdbUrl = vm.ImdbUrl;

                    if (vm.PhotoFile != null && vm.PhotoFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await vm.PhotoFile.CopyToAsync(memoryStream);
                            actorToUpdate.Photo = memoryStream.ToArray();
                        }
                    }

                    _context.Update(actorToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Actors.Any(e => e.Id == vm.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate existing photo if model state is invalid
            if (vm.ExistingPhoto == null)
            {
                var originalActor = await _context.Actors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == vm.Id);
                if (originalActor != null)
                {
                    vm.ExistingPhoto = originalActor.Photo;
                }
            }
            return View(vm);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
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
            var actor = await _context.Actors.FindAsync(id);
            if (actor != null)
            {
                _context.Actors.Remove(actor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}

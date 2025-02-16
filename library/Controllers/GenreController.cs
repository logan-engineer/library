using library.Data;
using library.Models;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers
{
   
    
        public class GenreController : Controller
        {
            private readonly ApplicationDbContext _context;

            public GenreController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: Genres/Create or Genres/Edit (depending on if there's an ID)
            public IActionResult Create(int? id)
            {
                if (id == null)
                    return View(new Genre());
                else
                    return View(_context.Genres.FirstOrDefault(g => g.Id == id));
            }
       

        // POST: Genres/Create or Genres/Edit
        [HttpPost]
            [ValidateAntiForgeryToken]
            public  IActionResult Save(Genre genre)
            {
                if (ModelState.IsValid)
                {
                    if (genre.Id == 0)
                    {
                        _context.Add(genre);
                    }
                    else
                    {
                        _context.Update(genre);
                    }
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

                return View("Create", genre);
            }

            // GET: Genres/Index
            public IActionResult Index()
            {
                return View(_context.Genres.ToList());
            }
             public IActionResult Delete(int id)
        {
            var genre = _context.Genres.FirstOrDefault(g => g.Id == id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Delete/5
               [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                 public IActionResult DeleteConfirmed(int id)
        {
            var genre = _context.Genres.FirstOrDefault(g => g.Id == id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }


        }


}

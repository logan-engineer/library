using library.Data;
using library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library.Controllers
{
   
        public class AuthorController : Controller
        {
            private readonly ApplicationDbContext _context;

            public AuthorController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: Authors
            public IActionResult Index()
            {
                var authors = _context.Authors.ToList();
                return View(authors);
            }

            // GET: Authors/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Authors/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(Author author)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(author);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(author);
            }

            // GET: Authors/Edit/5
            public IActionResult Edit(int id)
            {
                var author = _context.Authors.Find(id);
                if (author == null)
                {
                    return NotFound();
                }
                return View(author);
            }

            // POST: Authors/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(int id, Author author)
            {
                if (id != author.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(author);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_context.Authors.Any(a => a.Id == id))
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
                return View(author);
            }

            // GET: Authors/Delete/5
            public IActionResult Delete(int id)
            {
                var author = _context.Authors.Find(id);
                if (author == null)
                {
                    return NotFound();
                }
                return View(author);
            }

            // POST: Authors/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(int id)
            {
                var author = _context.Authors.Find(id);
                if (author != null)
                {
                    _context.Authors.Remove(author);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
        }
    

}

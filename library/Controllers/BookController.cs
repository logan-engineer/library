using library.Data;
using library.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace library.Controllers
{
   

    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

      
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Genre)
                .ToListAsync();
            return View(books);
        }

       
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string newfilename = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, @"image\book");
                var extension = Path.GetExtension(file[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, newfilename + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                book.Image = @"\image\book\" + newfilename + extension;
            }
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
            return View(book);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
            return View(book);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ISBN,AuthorId,PublisherId,GenreId,PublishedDate,Price,PageCount,Description,Image,IsAvailable,LowasPrice")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }
            string webRootPath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string newfilename = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, @"image\book");
                var extension = Path.GetExtension(file[0].FileName);
                //delete old image
                var objfrmpath = _context.Books.AsNoTracking().FirstOrDefault(x => x.Id == book.Id);
                if (objfrmpath.Image != null)
                {
                    var oldimgpath = Path.Combine(webRootPath, objfrmpath.Image);
                    if (System.IO.File.Exists(oldimgpath))
                    {
                        System.IO.File.Delete(oldimgpath);
                    }
                }
                using (var fileStream = new FileStream(Path.Combine(upload, newfilename + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                book.Image = @"\image\book\" + newfilename + extension;
            }
            

            if (ModelState.IsValid)
            {
                try
                    {
                        var objfrmpath = _context.Books.AsNoTracking().FirstOrDefault(x => x.Id == book.Id);
                        objfrmpath.Title = book.Title;
                    objfrmpath.ISBN = book.ISBN;
                    objfrmpath.Title = book.Title;
                    objfrmpath.PublisherId = book.PublisherId;
                    objfrmpath.GenreId = book.GenreId;
                    objfrmpath.Price = book.Price;
                    objfrmpath.LowasPrice = book.LowasPrice;

                    objfrmpath.PageCount = book.PageCount;
                    objfrmpath.PublishedDate = book.PublishedDate;
                    objfrmpath.Description = book.Description;
                    objfrmpath.IsAvailable = book.IsAvailable;
                    objfrmpath.AuthorId = book.AuthorId;
                        if (book.Image != null)
                        {
                            objfrmpath.Image = book.Image;
                        }
                        _context.Books.Update(objfrmpath);
                        await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
            return View(book);
        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
       

            // GET: Books/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var book = await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Publisher)
                    .Include(b => b.Genre)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (book == null)
                {
                    return NotFound();
                }

                return View(book);
            }

            [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }


}

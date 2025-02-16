using System.Diagnostics;
using library.Data;
using library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace library.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string search, decimal? minPrice, decimal? maxPrice)
        {
            // Start with the base query to get all members
            IQueryable<Book> books = _context.Books.Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Genre);

            // Apply search filter if search term is provided
            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(m => m.Title.Contains(search));
               
            }
           

            // Apply price range filter if minPrice and maxPrice are provided
            if (minPrice.HasValue)
            {
                // Assuming Price is a property of BookLoan or Book
                books = books.Where(bl => bl.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                books = books.Where(bl => bl.Price <= maxPrice.Value);
            }

            // Execute the query and return the results
            var book = await books.ToListAsync();
            return View(book);
        }
      
        public async Task<IActionResult> Details(int id)
        {

            var book = await _context.Books.Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

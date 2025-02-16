using library.Data;
using library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library.Controllers
{
    public class DashboardController : Controller
    {
      
            private readonly ApplicationDbContext _context;

            public DashboardController(ApplicationDbContext context)
            {
                _context = context;
            }
        [Authorize(Roles ="Admin")]
            public IActionResult Index()
            {
                // Book stats
                var totalBooks = _context.Books.Count();
                var availableBooks = _context.Books.Count(b => b.IsAvailable);
                var totalGenres = _context.Books.Select(b => b.Genre).Distinct().Count();

                // Transaction stats
                var totalTransactions = _context.BookLoans.Count();
                var overdueBooks = _context.BookLoans.Where(t => t.ReturnDate < DateTime.Now && t.ReturnDate == null).Count();

                // Recent Transactions
                var recentTransactions = _context.BookLoans.Include(b => b.Book).Include(b => b.Member)
                    .OrderByDescending(t => t.LoanDate)
                    .Take(5)
                    .ToList();

                // Most Borrowed Books
                //var mostBorrowedBooks = _context.BookLoans
                //    .GroupBy(t => t.BookId)
                //    .OrderByDescending(g => g.Count())
                //    .Take(5)
                //    .Select(g => new { BookId = g.Key, Count = g.Count() })
                //    .Join(_context.Books, g => g.BookId, b => b.Id, (g, b) => new { b.Title, g.Count })
                //    .ToList();

                // Dashboard Data
                var dashboardData = new DashboardViewModel
                {
                    TotalBooks = totalBooks,
                    AvailableBooks = availableBooks,
                    TotalTransactions = totalTransactions,
                    OverdueBooks = overdueBooks,
                    TotalGenres = totalGenres,
                    RecentBookloans = recentTransactions,
                    //MostBorrowedBooks = mostBorrowedBooks
                };

                return View(dashboardData);
            }
        }

    
}

using library.Data;
using library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace library.Controllers
{
   

    public class BookLoanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookLoanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookLoans
        public async Task<IActionResult> Index()
        {
            var bookLoans = await _context.BookLoans
                .Include(b => b.Book)
                .Include(m => m.Member)
                .ToListAsync();
            return View(bookLoans);
        }

        // GET: BookLoans/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "MemberName");
            return View();
        }

        // POST: BookLoans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,MemberId,LoanDate,ReturnDate")] BookLoan bookLoan)
        {
            if (ModelState.IsValid)
            {
                _context.BookLoans.Add(bookLoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", bookLoan.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "MemberName", bookLoan.MemberId);
            return View(bookLoan);
        }

        // GET: BookLoans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLoan = await _context.BookLoans.FindAsync(id);
            if (bookLoan == null)
            {
                return NotFound();
            }

            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", bookLoan.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", bookLoan.MemberId);
            return View(bookLoan);
        }

        // POST: BookLoans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,MemberId,LoanDate,ReturnDate")] BookLoan bookLoan)
        {
            if (id != bookLoan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookLoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookLoanExists(bookLoan.Id))
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

            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", bookLoan.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", bookLoan.MemberId);
            return View(bookLoan);
        }

        // GET: BookLoans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLoan = await _context.BookLoans
                .Include(b => b.Book)
                .Include(m => m.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookLoan == null)
            {
                return NotFound();
            }

            return View(bookLoan);
        }

        // POST: BookLoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookLoan = await _context.BookLoans.FindAsync(id);
            _context.BookLoans.Remove(bookLoan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookLoanExists(int id)
        {
            return _context.BookLoans.Any(e => e.Id == id);
        }
    }


}


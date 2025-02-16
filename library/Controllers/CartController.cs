using library.Data;
using Microsoft.AspNetCore.Mvc;
using library.carts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using library.Models;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Authorization;
namespace library.Controllers
{
    public class CartController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
            _cart = new Cart(); // You can store the cart in a session for persistence across requests
        }
        [Authorize]
        // GET: Cart
        public async Task<IActionResult> Index()
        {
            IQueryable<CartItem> Item = _context.CartItems;
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            Item = Item.Where(b => b.UserId == id);
            var book = await Item.ToListAsync();

            if (id == null)
            {
                return NotFound();
            }
           
              
            return View(book); // Pass the current cart to the view
        }
        public async Task<IActionResult> AddToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Cart/AddToCart/5
        [HttpPost, ActionName("AddToCart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id,CartItem cartItem)
        {
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            var existingWish = await _context.CartItems
               .FirstOrDefaultAsync(w => w.BookId== book.Id && w.UserId == userid);
            int quantity=1;
            if (existingWish == null)
            {


                var cartItem1 = new CartItem
                {

                    UserId = userid,
                    BookId = book.Id,
                    BookTitle = book.Title,
                    Price = book.Price,
                    Quantity = quantity
                };
                _context.Add(cartItem1);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            else
            {
                existingWish.Quantity += quantity;
            }

            return RedirectToAction(nameof(Index));
           
        
        }

        //POST: Cart/RemoveFromCart/5
        [HttpPost]
        public IActionResult RemoveFromCart(int bookId)
        {
            var item = _context.CartItems.FirstOrDefault(i => i.BookId == bookId);
            _context.CartItems.Remove(item);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Go back to the cart page
        }

        // POST: Cart/Clear
        [HttpPost]
        public IActionResult ClearCart()
        {
            //_context.Clear();
            return RedirectToAction(nameof(Index)); // Go back to the cart page
        }
      

        

      
    }


}

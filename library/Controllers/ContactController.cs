using library.carts;
using library.Data;
using library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;


        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Contact model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model);
            
                _context.SaveChanges();
            // Logic to process the form (e.g., send an email)
            TempData["SuccessMessage"] = "Thank you for contacting us!";
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace library.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public string UserId { get; set; }
     
        public int BookId { get; set; }   // ID of the Book
        public string BookTitle { get; set; }   // Title of the Book
        public decimal Price { get; set; }  // Price of the Book
        public int Quantity { get; set; }  // Quantity of the Book
    }
}

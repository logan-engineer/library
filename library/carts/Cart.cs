using library.Models;
using Microsoft.EntityFrameworkCore;

namespace library.carts
{
    public class Cart
    {
        private List<CartItem> _items = new List<CartItem>();

        public List<CartItem> Items => _items;

        public void AddItem(string Userid, int bookId, string bookTitle, decimal price, int quantity = 1)
        {
            var existingItem = _items.FirstOrDefault(i => i.BookId == bookId);

            if (existingItem == null)
            {
                _items.Add(new CartItem {UserId=Userid, BookId = bookId, BookTitle = bookTitle, Price = price, Quantity = quantity });
                 
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }

        public void RemoveItem(int bookId)
        {
            var item = _items.FirstOrDefault(i => i.BookId == bookId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        public void Clear() => _items.Clear();

        public decimal TotalAmount => _items.Sum(item => item.Price * item.Quantity);
    }

}

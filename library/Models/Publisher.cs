using System.ComponentModel.DataAnnotations;

namespace library.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public ICollection<Book>? Books { get; set; }
    }

}

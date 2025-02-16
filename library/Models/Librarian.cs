using System.ComponentModel.DataAnnotations;

namespace library.Models
{
    public class Librarian
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Contact { get; set; }
    }

}

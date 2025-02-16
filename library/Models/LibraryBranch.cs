using System.ComponentModel.DataAnnotations;

namespace library.Models
{
    public class LibraryBranch
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        public ICollection<Book>? Books { get; set; }
    }

}

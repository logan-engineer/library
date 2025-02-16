using System.ComponentModel.DataAnnotations;

namespace library.Models
{
    public class Member
    {
        public int Id { get; set; }
        [Required]
        public string MemberName { get; set; }
        [Required]
        public string Contact { get; set; }
        public ICollection<BookLoan>? BookLoans { get; set; }
    }

}

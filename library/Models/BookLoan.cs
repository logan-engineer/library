using System.ComponentModel.DataAnnotations;

namespace library.Models
{
   
        public class BookLoan
        {
            public int Id { get; set; }

            // Foreign Key for Book
            [Required]
            public int BookId { get; set; }
            public Book? Book { get; set; }

            // Foreign Key for Member
            [Required]
            public int MemberId { get; set; }
            public Member? Member { get; set; }

            // The date the book was loaned
            [Required]
            public DateTime LoanDate { get; set; }

            // The date the book was returned (nullable as the book might not have been returned yet)
            public DateTime? ReturnDate { get; set; }

            // Additional properties such as Loan Status could also be added (e.g., overdue, returned, etc.)
            public string LoanStatus => ReturnDate.HasValue ? "Returned" : "On Loan";
        }
    


}

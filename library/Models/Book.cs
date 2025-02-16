using System.ComponentModel.DataAnnotations;

namespace library.Models
{
  

   
        public class Book
        {
            public int Id { get; set; }

            [Required]
            [StringLength(255)]
            public string Title { get; set; }

            [Required]
            public string ISBN { get; set; }

            public int AuthorId { get; set; } // Foreign key to Author
            public Author? Author { get; set; } // Navigation property to Author

            public int PublisherId { get; set; } // Foreign key to Publisher
            public Publisher? Publisher { get; set; } // Navigation property to Publisher

            public int GenreId { get; set; } // Foreign key to Genre
            public Genre? Genre { get; set; } // Navigation property to Genre

            [DataType(DataType.Date)]
            public DateTime PublishedDate { get; set; }

            public decimal Price { get; set; }

            public decimal LowasPrice { get; set; }
             public int PageCount { get; set; }

            public string Description { get; set; }

            public string Image { get; set; }
            public bool IsAvailable { get; set; }
    }
    



}

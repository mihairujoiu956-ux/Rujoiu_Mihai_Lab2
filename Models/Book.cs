using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace Rujoiu_Mihai_Lab2.Models
{
    public class Book
    {
        public int ID { get; set; }
        [Display(Name = "Book Title")]
        public string? Title { get; set; }
        
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishingDate { get; set; }


        public int? PublisherID { get; set; }
        public Publisher? Publisher { get; set; }
        // NOU: Cheia străină și proprietatea de navigare pentru Author
        public int? AuthorID { get; set; } // Cheia străină
        public Author? Author { get; set; } // Proprietatea de navigare
        public ICollection<BookCategory>? BookCategories { get; set; }

    }
}

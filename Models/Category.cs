using Microsoft.AspNetCore.Authorization;

namespace Rujoiu_Mihai_Lab2.Models
{
    [Authorize(Roles = "Admin")]
    public class Category
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public ICollection<BookCategory>? BookCategories { get; set; }
    }
}

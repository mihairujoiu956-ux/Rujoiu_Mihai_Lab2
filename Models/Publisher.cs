using Microsoft.AspNetCore.Authorization;

namespace Rujoiu_Mihai_Lab2.Models
{
    [Authorize(Roles = "Admin")]
    public class Publisher
    {
        public int ID { get; set; }
        public string? PublisherName { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}

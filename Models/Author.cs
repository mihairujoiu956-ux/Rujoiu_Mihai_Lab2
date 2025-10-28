using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rujoiu_Mihai_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }

        [Display(Name = "Author's")]
        public string FirstName { get; set; }
        [Display(Name = "Name")]
        public string LastName { get; set; }
        public ICollection<Book>? Books { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }
}

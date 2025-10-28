using Microsoft.AspNetCore.Mvc; // Adaugă using pentru [BindProperty]
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; // Adaugă using pentru SelectList
using Microsoft.EntityFrameworkCore;
using Rujoiu_Mihai_Lab2.Models;
using Rujoiu_Mihai_Lab2.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rujoiu_Mihai_Lab2.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly Rujoiu_Mihai_Lab2.Data.Rujoiu_Mihai_Lab2Context _context;

        public IndexModel(Rujoiu_Mihai_Lab2.Data.Rujoiu_Mihai_Lab2Context context)
        {
            _context = context;
        }

        // Proprietățile necesare pentru datele afișate
        public IList<Book> Book { get; set; }
        public BookData BookD { get; set; }
        public int BookID { get; set; }
        public int CategoryID { get; set; }

        // NOILE PROPRIETĂȚI PENTRU FILTRAREA DUPĂ AUTOR (pentru a rezolva CS1061)
        public SelectList AuthorNames { get; set; } // Lista de autori pentru Dropdown

        [BindProperty(SupportsGet = true)]
        public int? BookAuthorID { get; set; } // ID-ul autorului selectat

        public async Task OnGetAsync(int? id, int? categoryID)
        {
            // 1. Încărcăm lista completă de autori pentru dropdown (pentru AuthorNames)
            AuthorNames = new SelectList(_context.Author, "ID", "FullName");

            BookD = new BookData();

            var booksIQ = _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.Author) // Includem Author
                .Include(b => b.BookCategories)
                    .ThenInclude(b => b.Category)
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .AsQueryable(); // Pornim ca IQueryable pentru a aplica Where

            // Aplicăm filtrarea dacă un autor a fost selectat
            if (BookAuthorID.HasValue)
            {
                booksIQ = booksIQ.Where(b => b.AuthorID == BookAuthorID.Value);
            }

            BookD.Books = await booksIQ.ToListAsync();
            Book = BookD.Books.ToList();

            // Logica de evidențiere a detaliilor
            if (id != null)
            {
                BookID = id.Value;
                Book book = BookD.Books
                    .Where(i => i.ID == id.Value).Single();

                BookD.Categories = book.BookCategories.Select(s => s.Category);
            }
            // Notă: Eroarea ENC0046 (await) dispare de obicei după o reconstruire a soluției.
        }
    }
}
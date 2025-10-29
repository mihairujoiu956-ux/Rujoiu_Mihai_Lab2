using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rujoiu_Mihai_Lab2.Data;
using Rujoiu_Mihai_Lab2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Rujoiu_Mihai_Lab2.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly Rujoiu_Mihai_Lab2Context _context;

        public IndexModel(Rujoiu_Mihai_Lab2Context context)
        {
            _context = context;
        }

        // Proprietăți pentru afișarea și filtrarea datelor
        public IList<Book> Book { get; set; }
        public BookData BookD { get; set; }
        public int BookID { get; set; }
        public int CategoryID { get; set; }
        public string TitleSort { get; set; }
        public string AuthorSort { get; set; }
        public string CurrentFilter { get; set; }

        // Dropdown list pentru autori
        public SelectList AuthorNames { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? BookAuthorID { get; set; }

        public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string
            searchString)
        {
            BookD = new BookData();
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            AuthorSort = sortOrder == "author" ? "author_desc" : "";

            CurrentFilter = searchString;

            // 1. Începem interogarea (IQueryable). NU chemăm ToListAsync() încă.
            //    Interogarea se construiește în pași.
            IQueryable<Book> booksQuery = _context.Book
                .Include(b => b.Author) // Include Autorul (necesar pt. sortare/căutare)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                .ThenInclude(b => b.Category)
                .AsNoTracking();

            // 2. Aplicăm filtrul de CĂUTARE (dacă există)
            if (!String.IsNullOrEmpty(searchString))
            {
                booksQuery = booksQuery.Where(s => s.Author.FirstName.Contains(searchString)
                                                     || s.Author.LastName.Contains(searchString)
                                                     || s.Title.Contains(searchString));
            }

            // 3. Aplicăm filtrul de CATEGORIE (dacă există)
            //    *** ACESTA ESTE CODUL NOU PENTRU SARCINA TA ***
            if (categoryID != null)
            {
                // Adăugăm un filtru 'Where' care selectează doar cărțile (x)
                // care au 'Oricare' (Any) înregistrare în BookCategories (bc)
                // al cărei CategoryID este egal cu cel primit.
                booksQuery = booksQuery.Where(x => x.BookCategories.Any(bc => bc.CategoryID == categoryID));
            }

            // 4. Aplicăm SORTAREA
            switch (sortOrder)
            {
                case "title_desc":
                    booksQuery = booksQuery.OrderByDescending(s => s.Title);
                    break;
                case "author_desc":
                    booksQuery = booksQuery.OrderByDescending(s => s.Author.FullName);
                    break;
                case "author":
                    booksQuery = booksQuery.OrderBy(s => s.Author.FullName);
                    break;
                default:
                    booksQuery = booksQuery.OrderBy(s => s.Title);
                    break;
            }

            // 5. Executăm interogarea finală (acum chemăm ToListAsync)
            //    Baza de date va returna doar rezultatele filtrate și sortate.
            BookD.Books = await booksQuery.ToListAsync();

            // 6. Păstrăm logica ta existentă pentru selectarea unei singure cărți
            //    (care folosește parametrul 'id' pentru a afișa categoriile acelei cărți)
            if (id != null)
            {
                BookID = id.Value;
                // Găsim cartea în lista deja filtrată și sortată
                Book book = BookD.Books
                    .Where(i => i.ID == id.Value).SingleOrDefault(); // Folosim SingleOrDefault pentru siguranță

                if (book != null)
                {
                    BookD.Categories = book.BookCategories.Select(s => s.Category);
                }
            }
        }

        // ------------- SE TERMINĂ METODA MODIFICATĂ -------------
    }
}


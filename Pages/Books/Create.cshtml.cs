using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rujoiu_Mihai_Lab2.Data;
using Rujoiu_Mihai_Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rujoiu_Mihai_Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]

    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Rujoiu_Mihai_Lab2.Data.Rujoiu_Mihai_Lab2Context _context;

        public CreateModel(Rujoiu_Mihai_Lab2.Data.Rujoiu_Mihai_Lab2Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // dacă ai adăugat o proprietate FullName în clasa Author:
            var authorList = _context.Author.Select(x => new
            {
                x.ID,
                FullName = x.LastName + " " + x.FirstName
            });

            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName");
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");

            var book = new Book();
            book.BookCategories = new List<BookCategory>();
            PopulateAssignedCategoryData(_context, book);

            return Page();
        }

        [BindProperty]
        public Book Book { get; set; }

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            var newBook = new Book();

            if (selectedCategories != null)
            {
                newBook.BookCategories = new List<BookCategory>();
                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    };
                    newBook.BookCategories.Add(catToAdd);
                }
            }

            Book.BookCategories = newBook.BookCategories;
            _context.Book.Add(Book);

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch
            {
                // Reîncarcă datele pentru a afișa formularul cu erori de validare
                PopulateAssignedCategoryData(_context, newBook);
                ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName", newBook.PublisherID);
                ViewData[nameof(Book.AuthorID)] = new SelectList(_context.Set<Author>(), "ID", "FullName", newBook.AuthorID);
                return Page();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rujoiu_Mihai_Lab2.Data;
using Rujoiu_Mihai_Lab2.Models;

namespace Rujoiu_Mihai_Lab2.Pages.Borrowings
{
    public class EditModel : PageModel
    {
        private readonly Rujoiu_Mihai_Lab2.Data.Rujoiu_Mihai_Lab2Context _context;

        public EditModel(Rujoiu_Mihai_Lab2.Data.Rujoiu_Mihai_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Borrowing Borrowing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowing.FirstOrDefaultAsync(m => m.ID == id);
            if (borrowing == null)
            {
                return NotFound();
            }
            Borrowing = borrowing;

            // 1. MODIFICARE PENTRU LISTA MEMBRILOR (afiseaza Numele Membrului)
            // Presupunem ca Member are o proprietate numita FullName
            ViewData["MemberID"] = new SelectList(_context.Member.OrderBy(m => m.FullName), "ID", "FullName", Borrowing.MemberID);

            // 2. MODIFICARE PENTRU LISTA CĂRȚILOR (afiseaza Titlu - Autor)
            // Trebuie sa includem Author pentru a putea accesa numele autorului (Title - Author.FullName)
            var booksForDropdown = _context.Book
                .Include(b => b.Author)
                .Select(b => new
                {
                    b.ID,
                    // Câmpul de afișat va fi o combinație: Titlu carte - Nume autor
                    BookDetails = b.Title + " - " + b.Author.FullName
                })
                .OrderBy(b => b.BookDetails)
                .ToList();

            ViewData["BookID"] = new SelectList(booksForDropdown, "ID", "BookDetails", Borrowing.BookID);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reincarcam listele derulante in caz de eroare de validare
                // pentru a nu afisa din nou ID-urile in loc de nume
                ViewData["MemberID"] = new SelectList(_context.Member.OrderBy(m => m.FullName), "ID", "FullName", Borrowing.MemberID);

                var booksForDropdown = _context.Book
                    .Include(b => b.Author)
                    .Select(b => new
                    {
                        b.ID,
                        BookDetails = b.Title + " - " + b.Author.FullName
                    })
                    .OrderBy(b => b.BookDetails)
                    .ToList();

                ViewData["BookID"] = new SelectList(booksForDropdown, "ID", "BookDetails", Borrowing.BookID);

                return Page();
            }

            _context.Attach(Borrowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(Borrowing.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BorrowingExists(int id)
        {
            return _context.Borrowing.Any(e => e.ID == id);
        }
    }
}
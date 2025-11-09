using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Rujoiu_Mihai_Lab2.Data
{
    // Adaugă moștenirea din IdentityDbContext
    public class LibraryIdentityContext : IdentityDbContext
    {
        // Este necesar să adaugi și un constructor
        public LibraryIdentityContext(DbContextOptions<LibraryIdentityContext> options)
            : base(options)
        {
        }
    }
}

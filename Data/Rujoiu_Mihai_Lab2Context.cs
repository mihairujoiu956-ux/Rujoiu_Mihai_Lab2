using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rujoiu_Mihai_Lab2.Models;

namespace Rujoiu_Mihai_Lab2.Data
{
    public class Rujoiu_Mihai_Lab2Context : DbContext
    {
        public Rujoiu_Mihai_Lab2Context (DbContextOptions<Rujoiu_Mihai_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Rujoiu_Mihai_Lab2.Models.Book> Book { get; set; } = default!;
        public DbSet<Rujoiu_Mihai_Lab2.Models.Publisher> Publisher { get; set; } = default!;
        public DbSet<Rujoiu_Mihai_Lab2.Models.Category> Category { get; set; } = default!;
        public DbSet<Rujoiu_Mihai_Lab2.Models.Author> Author { get; set; } = default!;
        public DbSet<Rujoiu_Mihai_Lab2.Models.Member> Member { get; set; } = default!;
        public DbSet<Rujoiu_Mihai_Lab2.Models.Borrowing> Borrowing { get; set; } = default!;
    }
}

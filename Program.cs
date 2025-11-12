using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rujoiu_Mihai_Lab2.Data;
using Microsoft.AspNetCore.Identity;
using Rujoiu_Mihai_Lab2.DataLibraryIdentity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
   policy.RequireRole("Admin"));
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Books");
    options.Conventions.AllowAnonymousToPage("/Books/Index");
    options.Conventions.AllowAnonymousToPage("/Books/Details");
    options.Conventions.AuthorizeFolder("/Members", "AdminPolicy");
});


// Contextul principal al aplicației
builder.Services.AddDbContext<Rujoiu_Mihai_Lab2Context>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Rujoiu_Mihai_Lab2Context")
        ?? throw new InvalidOperationException("Connection string 'Rujoiu_Mihai_Lab2Context' not found.")));

// Contextul pentru Identity (autentificare)
builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Rujoiu_Mihai_Lab2Context")
        ?? throw new InvalidOperationException("Connection string 'Rujoiu_Mihai_Lab2Context' not found.")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LibraryIdentityContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();


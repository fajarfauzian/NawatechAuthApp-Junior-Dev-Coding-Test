using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NawatechAuthApp.Data;
using NawatechAuthApp.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NawatechAuthApp.Pages.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public IActionResult OnGet()
        {
            PopulateCategories();
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public SelectList Categories { get; set; } = default!;

        private void PopulateCategories()
        {
            var categories = _context.ProductCategories.ToList();
            Categories = new SelectList(categories, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateCategories();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            Product.CreatedById = user.Id;
            Product.CreatedAt = DateTime.UtcNow;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "products");
                Directory.CreateDirectory(uploadsFolder); // CreateDirectory is safe even if directory exists

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                Product.ImageUrl = $"/uploads/products/{fileName}";
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = "Product created successfully.";
            return RedirectToPage("./Index");
        }
    }
}
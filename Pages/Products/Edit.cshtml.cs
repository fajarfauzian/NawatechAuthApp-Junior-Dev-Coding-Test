using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NawatechAuthApp.Data;
using NawatechAuthApp.Models;
using System.Threading.Tasks;

namespace NawatechAuthApp.Pages.Products
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            
            PopulateCategories();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateCategories();
                return Page();
            }

            var productToUpdate = await _context.Products.FindAsync(Product.Id);

            if (productToUpdate == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            productToUpdate.Name = Product.Name;
            productToUpdate.Description = Product.Description;
            productToUpdate.Price = Product.Price;
            productToUpdate.Stock = Product.Stock;
            productToUpdate.CategoryId = Product.CategoryId;
            productToUpdate.UpdatedById = user.Id;
            productToUpdate.UpdatedAt = DateTime.UtcNow;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Save product image
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "products");
                Directory.CreateDirectory(uploadsFolder); // CreateDirectory is safe even if directory exists

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                // Delete old image if exists
                if (!string.IsNullOrEmpty(productToUpdate.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, productToUpdate.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                productToUpdate.ImageUrl = "/uploads/products/" + fileName;
            }

            try
            {
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Product updated successfully.";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExistsAsync(Product.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        private async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NawatechAuthApp.Data;
using NawatechAuthApp.Models;
using System;
using System.Threading.Tasks;

namespace NawatechAuthApp.Pages.Categories
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [BindProperty]
        public ProductCategory Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.ProductCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var categoryToUpdate = await _context.ProductCategories.FindAsync(Category.Id);

            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            categoryToUpdate.Name = Category.Name;
            categoryToUpdate.Description = Category.Description;
            categoryToUpdate.UpdatedById = user.Id;
            categoryToUpdate.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Category updated successfully.";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryExistsAsync(Category.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        private async Task<bool> CategoryExistsAsync(int id)
        {
            return await _context.ProductCategories.AnyAsync(e => e.Id == id);
        }
    }
}
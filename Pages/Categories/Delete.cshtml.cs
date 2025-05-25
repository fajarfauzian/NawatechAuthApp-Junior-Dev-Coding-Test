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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

            var category = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.Id == id)
                .ConfigureAwait(false);

            if (category == null)
            {
                return NotFound();
            }
            
            Category = category;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.ProductCategories
                .FindAsync(id)
                .ConfigureAwait(false);
                
            if (category == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User)
                .ConfigureAwait(false);
                
            if (user == null)
            {
                return NotFound("User not found.");
            }
            
            // Soft delete
            category.IsDeleted = true;
            category.DeletedById = user.Id;
            category.DeletedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
                
            TempData["StatusMessage"] = "Category deleted successfully.";
            
            return RedirectToPage("./Index");
        }
    }
}
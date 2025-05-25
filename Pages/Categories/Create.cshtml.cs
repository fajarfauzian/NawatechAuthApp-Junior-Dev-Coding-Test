using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NawatechAuthApp.Data;
using NawatechAuthApp.Models;
using System;
using System.Threading.Tasks;

namespace NawatechAuthApp.Pages.Categories
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProductCategory Category { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user.");
            }

            // Perbaikan: Jangan buat objek baru, gunakan yang sudah di-bind
            Category.CreatedById = user.Id;
            Category.CreatedAt = DateTime.UtcNow;

            _context.ProductCategories.Add(Category);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = "Category created successfully.";
            return RedirectToPage("./Index");
        }
    }
}
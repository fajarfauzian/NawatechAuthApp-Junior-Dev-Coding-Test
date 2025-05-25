using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NawatechAuthApp.Data;
using NawatechAuthApp.Models;
using System.Threading.Tasks;

namespace NawatechAuthApp.Pages.Categories
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public ProductCategory? Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.ProductCategories
                .Include(c => c.Products!.Where(p => !p.IsDeleted))
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (category == null)
            {
                return NotFound();
            }
            
            Category = category;
            return Page();
        }
    }
}
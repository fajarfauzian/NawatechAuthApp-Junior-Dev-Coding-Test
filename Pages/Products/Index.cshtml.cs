using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NawatechAuthApp.Data;
using NawatechAuthApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NawatechAuthApp.Pages.Products
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; } = new List<Product>();
        public IList<ProductCategory> Categories { get; set; } = new List<ProductCategory>();

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            // Load products with their categories
            Products = await _context.Products
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            // Load all categories for the filter dropdown
            Categories = await _context.ProductCategories
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}
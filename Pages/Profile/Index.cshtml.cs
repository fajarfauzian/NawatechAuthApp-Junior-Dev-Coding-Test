using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NawatechAuthApp.Models;
using NawatechAuthApp.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace NawatechAuthApp.Pages.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        [TempData]
        public string? StatusMessage { get; set; }

        public string? Username { get; set; }
        public string? ProfilePicture { get; set; }
        public string? GravatarUrl { get; set; }
        public string? Email { get; set; }

        public class InputModel
        {
            [Display(Name = "First Name")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            public string? FirstName { get; set; }

            [Display(Name = "Last Name")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            public string? LastName { get; set; }

            [Display(Name = "Profile Picture")]
            public IFormFile? ProfilePicture { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            Username = userName;
            Email = email;
            ProfilePicture = user.ProfilePicture;
            GravatarUrl = user.GravatarUrl;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            if (Input.ProfilePicture != null)
            {
                // Save profile picture
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Input.ProfilePicture.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfilePicture.CopyToAsync(fileStream);
                }

                // Update user profile picture path
                user.ProfilePicture = "/uploads/profiles/" + fileName;
            }

            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadAsync(user);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
        
        public async Task<IActionResult> OnPostRemoveProfilePictureAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Remove uploaded profile picture to use Gravatar
            if (!string.IsNullOrEmpty(user.ProfilePicture))
            {
                var filePath = Path.Combine(_environment.WebRootPath, user.ProfilePicture.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                user.ProfilePicture = null;
            }

            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            
            StatusMessage = "Profile picture removed. Now using Gravatar.";
            return RedirectToPage();
        }
    }
}
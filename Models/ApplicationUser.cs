using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using NawatechAuthApp.Services;

namespace NawatechAuthApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [MaxLength(100)]
        public string? FirstName { get; set; }
        
        [PersonalData]
        [MaxLength(100)]
        public string? LastName { get; set; }
        
        [PersonalData]
        public string? ProfilePicture { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual ICollection<Product>? Products { get; set; }
        
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string GravatarUrl => GravatarHelper.GetGravatarUrl(Email ?? "", 200, "identicon");
        
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string ProfilePictureUrl => !string.IsNullOrEmpty(ProfilePicture) ? ProfilePicture : GravatarUrl;
    }
}
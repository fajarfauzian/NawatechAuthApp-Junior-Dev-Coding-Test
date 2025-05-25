using System.ComponentModel.DataAnnotations;

namespace NawatechAuthApp.Models
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public string? CreatedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? DeletedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        
        public virtual ApplicationUser? CreatedBy { get; set; }
        public virtual ApplicationUser? UpdatedBy { get; set; }
        public virtual ApplicationUser? DeletedBy { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
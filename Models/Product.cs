using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NawatechAuthApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public int Stock { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public string? CreatedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? DeletedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        
        [ForeignKey("CategoryId")]
        public virtual ProductCategory? Category { get; set; }
        public virtual ApplicationUser? CreatedBy { get; set; }
        public virtual ApplicationUser? UpdatedBy { get; set; }
        public virtual ApplicationUser? DeletedBy { get; set; }
    }
}
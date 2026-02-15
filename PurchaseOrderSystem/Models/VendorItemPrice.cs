using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderSystem.Models
{
    public class VendorItemPrice
    {
        public int Id { get; set; } // Auto ID

        [Required]
        public int VendorId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Unit Price is required")]
        [Range(1, 1000000, ErrorMessage = "Price must be greater than 0")]
        public decimal UnitPrice { get; set; }
    }
}

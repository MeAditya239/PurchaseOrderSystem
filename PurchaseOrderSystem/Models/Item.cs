using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderSystem.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Item Name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$",
             ErrorMessage = "Item Name cannot contain special characters or numbers")]
        public string ItemName {  get; set; }

        [Required(ErrorMessage = "Unit Price is required")]
        [Range(1, 1000000, ErrorMessage = "Unit Price must be greater than 0")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Available Quantity is required")]
        [Range(0, 100000, ErrorMessage = "Quantity cannot be negative")]
        public int AvailableQuantity { get; set; }

        public bool IsActive { get; set; }
    }
}

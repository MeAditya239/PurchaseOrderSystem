using System.ComponentModel.DataAnnotations;


namespace PurchaseOrderSystem.Models
{
    public class Vendor
    {
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Vendor Name is Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$",
            ErrorMessage = "Vendor Name cannot contain numbers or special symbols")]
        public string VendorName { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [RegularExpression(@"^\d{10}$",
            ErrorMessage = "Contact Number must be exactly 10 digits")]
        public string ContactNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string? Email { get; set; }

        public bool IsActive { get; set; }
    }
}

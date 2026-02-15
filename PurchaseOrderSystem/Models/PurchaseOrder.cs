using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderSystem.Models
{
    public class PurchaseOrder
    {
        public int PONumber { get; set; }

        [Required]
        public DateTime PODate { get; set; }

        [Required]
        public int VendorId { get; set; }

        public string VendorName { get; set; }

        public decimal TotalAmount => Items.Sum(i => i.LineTotal);

        public List<PurchaseOrderItem> Items { get; set; } = new();
    }
}

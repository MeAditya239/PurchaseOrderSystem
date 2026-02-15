using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderSystem.Models
{
    public class PurchaseOrderItem
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        [Range(1, 1000, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal LineTotal => Quantity * UnitPrice;
    }
}

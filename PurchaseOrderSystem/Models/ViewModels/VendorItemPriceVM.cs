namespace PurchaseOrderSystem.Models.ViewModels
{
    public class VendorItemPriceVM
    {
        public int Id { get; set; }

        public string VendorName { get; set; }

        public string ItemName { get; set; }

        public decimal UnitPrice { get; set; }
    }
}

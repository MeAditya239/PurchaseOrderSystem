using PurchaseOrderSystem.Models;

namespace PurchaseOrderSystem.Services.Interfaces
{
    public interface IVendorItemPriceService
    {
        List<VendorItemPrice> GetAll();
        VendorItemPrice? GetById(int id);

        void Add(VendorItemPrice mapping);
        void Update(VendorItemPrice mapping);
        void Delete(int id);
    }
}

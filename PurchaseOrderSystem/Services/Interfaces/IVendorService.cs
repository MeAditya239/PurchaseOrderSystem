using PurchaseOrderSystem.Models;

namespace PurchaseOrderSystem.Services.Interfaces
{
    public interface IVendorService
    {
        List<Vendor> GetAll();
        Vendor? GetById(int id);

        void Add(Vendor vendor);
        void Update(Vendor vendor);
        void Delete(int id);
    }
}

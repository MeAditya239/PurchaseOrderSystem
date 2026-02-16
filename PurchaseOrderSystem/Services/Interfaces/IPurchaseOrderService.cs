using PurchaseOrderSystem.Models;

namespace PurchaseOrderSystem.Services.Interfaces
{
    public interface IPurchaseOrderService
    {
        void Save(PurchaseOrder po);
        List<PurchaseOrder> GetAll();
        PurchaseOrder? GetById(int poNumber);

        void Update(PurchaseOrder po);

        void Delete(int poNumber);

    }
}

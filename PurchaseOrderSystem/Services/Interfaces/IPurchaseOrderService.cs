using PurchaseOrderSystem.Models;

namespace PurchaseOrderSystem.Services.Interfaces
{
    public interface IPurchaseOrderService
    {
        void Save(PurchaseOrder po);
        List<PurchaseOrder> GetAll();
    }
}

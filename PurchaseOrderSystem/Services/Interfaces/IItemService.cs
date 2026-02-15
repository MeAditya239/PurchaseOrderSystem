using PurchaseOrderSystem.Models;

namespace PurchaseOrderSystem.Services.Interfaces
{
    public interface IItemService
    {
        List<Item> GetAll();
        Item? GetById(int id);

        void Add(Item item);
        void Update(Item item);
        void Delete(int id);
    }
}

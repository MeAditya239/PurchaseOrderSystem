using PurchaseOrderSystem.Data;
using PurchaseOrderSystem.Models;
using PurchaseOrderSystem.Services.Interfaces;

namespace PurchaseOrderSystem.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly JsonRepository<Item> _repo;
        private readonly string _filePath;

        public ItemService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath,
                "Data/JsonFiles/items.json");

            _repo = new JsonRepository<Item>(_filePath);
        }

        public List<Item> GetAll()
        {
            return _repo.GetAll();
        }

        public Item? GetById(int id)
        {
            return GetAll().FirstOrDefault(i => i.ItemId == id);
        }

        public void Add(Item item)
        {
            var items = GetAll();

            item.ItemId = items.Count > 0
                ? items.Max(i => i.ItemId) + 1
                : 1;

            items.Add(item);
            _repo.SaveAll(items);
        }

        public void Update(Item item)
        {
            var items = GetAll();

            var existing = items.FirstOrDefault(i => i.ItemId == item.ItemId);

            if (existing == null)
            {
                throw new Exception("Item not found!");
            }
 

            existing.ItemName = item.ItemName;
            existing.UnitPrice = item.UnitPrice;
            existing.AvailableQuantity = item.AvailableQuantity;
            existing.IsActive = item.IsActive;

            _repo.SaveAll(items);
        }

        public void Delete(int id)
        {
            var items = GetAll();

            var item = items.FirstOrDefault(i => i.ItemId == id);

            if (item == null)
            {
                throw new Exception("Item not found!");
            }
 

            items.Remove(item);
            _repo.SaveAll(items);
        }
    }
}

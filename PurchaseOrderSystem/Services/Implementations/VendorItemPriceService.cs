using PurchaseOrderSystem.Data;
using PurchaseOrderSystem.Models;
using PurchaseOrderSystem.Services.Interfaces;

namespace PurchaseOrderSystem.Services.Implementations
{
    public class VendorItemPriceService : IVendorItemPriceService
    {
        private readonly JsonRepository<VendorItemPrice> _repo;
        private readonly string _filePath;

        public VendorItemPriceService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath,
                "Data/JsonFiles/vendorItemPrices.json");

            _repo = new JsonRepository<VendorItemPrice>(_filePath);
        }

        public List<VendorItemPrice> GetAll()
        {
            return _repo.GetAll();
        }

        public VendorItemPrice? GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public void Add(VendorItemPrice mapping)
        {
            var list = GetAll();

            // Prevent duplicate vendor-item mapping
            if (list.Any(x => x.VendorId == mapping.VendorId && x.ItemId == mapping.ItemId))
            {
                throw new Exception("This Vendor already has price defined for this Item!");
            }
         

            mapping.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;

            list.Add(mapping);
            _repo.SaveAll(list);
        }

        public void Update(VendorItemPrice mapping)
        {
            var list = GetAll();

            var existing = list.FirstOrDefault(x => x.Id == mapping.Id);

            if (existing == null)
            {
                throw new Exception("Mapping not found!");
            }
               

            existing.VendorId = mapping.VendorId;
            existing.ItemId = mapping.ItemId;
            existing.UnitPrice = mapping.UnitPrice;

            _repo.SaveAll(list);
        }

        public void Delete(int id)
        {
            var list = GetAll();

            var existing = list.FirstOrDefault(x => x.Id == id);

            if (existing == null)
            {
                throw new Exception("Mapping not found!");
            }
                

            list.Remove(existing);
            _repo.SaveAll(list);
        }
    }
}

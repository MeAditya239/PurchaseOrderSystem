using PurchaseOrderSystem.Data;
using PurchaseOrderSystem.Models;
using PurchaseOrderSystem.Services.Interfaces;

namespace PurchaseOrderSystem.Services.Implementations
{
    public class VendorService: IVendorService
    {
        private readonly JsonRepository<Vendor> _repo;
        private readonly string _filePath;

        public VendorService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath,
                "Data/JsonFiles/vendors.json");

            _repo = new JsonRepository<Vendor>(_filePath);
        }

        public List<Vendor> GetAll()
        {
            return _repo.GetAll();
        }

        public Vendor? GetById(int id)
        {
            return GetAll().FirstOrDefault(v => v.VendorId == id);
        }

        public void Add(Vendor vendor)
        {
            var vendors = GetAll();

            vendor.VendorId = vendors.Count > 0
                ? vendors.Max(v => v.VendorId) + 1
                : 1;

            vendors.Add(vendor);
            _repo.SaveAll(vendors);
        }

        public void Update(Vendor vendor)
        {
            var vendors = GetAll();

            var existing = vendors.FirstOrDefault(v => v.VendorId == vendor.VendorId);

            if (existing == null)
            {
                throw new Exception("Vendor not found!");
            }

            existing.VendorName = vendor.VendorName;
            existing.ContactNumber = vendor.ContactNumber;
            existing.Email = vendor.Email;
            existing.IsActive = vendor.IsActive;

            _repo.SaveAll(vendors);
        }

        public void Delete(int id)
        {
            var vendors = GetAll();

            var vendor = vendors.FirstOrDefault(v => v.VendorId == id);

            if (vendor == null)
            {
                throw new Exception("Vendor not found!");
            }

            vendors.Remove(vendor);
            _repo.SaveAll(vendors);
        }


    }
}

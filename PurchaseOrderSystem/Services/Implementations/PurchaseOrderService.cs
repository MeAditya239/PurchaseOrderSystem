using PurchaseOrderSystem.Data;
using PurchaseOrderSystem.Models;
using PurchaseOrderSystem.Services.Interfaces;

namespace PurchaseOrderSystem.Services.Implementations
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly JsonRepository<PurchaseOrder> _repo;
        private readonly string _filePath;

        private readonly IVendorService _vendorService;
        private readonly IItemService _itemService;
        private readonly IVendorItemPriceService _priceService;

        public PurchaseOrderService(
            IWebHostEnvironment env,
            IVendorService vendorService,
            IItemService itemService,
            IVendorItemPriceService priceService)
        {
            _filePath = Path.Combine(env.ContentRootPath,
                "Data/JsonFiles/purchaseOrders.json");

            _repo = new JsonRepository<PurchaseOrder>(_filePath);

            _vendorService = vendorService;
            _itemService = itemService;
            _priceService = priceService;
        }

        public List<PurchaseOrder> GetAll()
        {
            return _repo.GetAll();
        }

        public void Save(PurchaseOrder po)
        {
            var orders = GetAll();

            //  Auto PO Number
            po.PONumber = orders.Count > 0
                ? orders.Max(x => x.PONumber) + 1
                : 1;

            // Vendor Validation
            var vendor = _vendorService.GetById(po.VendorId);

            if (vendor == null || !vendor.IsActive)
            {
                throw new Exception("Only Active Vendors can be selected!");
            }
              

            po.VendorName = vendor.VendorName;

            // Item Validations
            foreach (var item in po.Items)
            {
                var masterItem = _itemService.GetById(item.ItemId);

                if (masterItem == null || !masterItem.IsActive)
                {
                    throw new Exception("Only Active Items can be selected!");
                }
                  

                //  Quantity Rule
                if (item.Quantity <= 0)
                {
                    throw new Exception("Quantity must be greater than 0!");
                }
                   

                if (item.Quantity > masterItem.AvailableQuantity)
                {
                    throw new Exception($"Ordered quantity cannot exceed stock for {masterItem.ItemName}");
                }
                   

                //  Vendor-wise Price Rule
                var mappedPrice = _priceService.GetAll()
                    .FirstOrDefault(x => x.VendorId == po.VendorId && x.ItemId == item.ItemId);

                if (mappedPrice == null)
                {
                    throw new Exception("This item is not mapped with selected vendor!");
                }
                   

                item.UnitPrice = mappedPrice.UnitPrice;
                item.ItemName = masterItem.ItemName;

                //  Deduct Stock After Purchase
                masterItem.AvailableQuantity -= item.Quantity;

                // Update Item Master JSON
                _itemService.Update(masterItem);

            }

            orders.Add(po);
            _repo.SaveAll(orders);
        }


        public PurchaseOrder? GetById(int poNumber)
        {
            return GetAll().FirstOrDefault(x => x.PONumber == poNumber);
        }


        public void Update(PurchaseOrder po)
        {
            var orders = GetAll();

            var existingPO = orders.FirstOrDefault(x => x.PONumber == po.PONumber);

            if (existingPO == null)
                throw new Exception("Purchase Order not found!");

            // Vendor Validation
            var vendor = _vendorService.GetById(po.VendorId);

            if (vendor == null || !vendor.IsActive)
                throw new Exception("Only Active Vendors can be selected!");

            po.VendorName = vendor.VendorName;

            // Restore old stock before updating
            foreach (var oldItem in existingPO.Items)
            {
                var masterItem = _itemService.GetById(oldItem.ItemId);
                if (masterItem != null)
                {
                    masterItem.AvailableQuantity += oldItem.Quantity;
                    _itemService.Update(masterItem);
                }
            }

            // Validate and Deduct New Stock
            foreach (var item in po.Items)
            {
                var masterItem = _itemService.GetById(item.ItemId);

                if (masterItem == null || !masterItem.IsActive)
                    throw new Exception("Only Active Items can be selected!");

                if (item.Quantity <= 0)
                    throw new Exception("Quantity must be greater than 0!");

                if (item.Quantity > masterItem.AvailableQuantity)
                    throw new Exception($"Ordered quantity cannot exceed stock for {masterItem.ItemName}");

                var mappedPrice = _priceService.GetAll()
                    .FirstOrDefault(x => x.VendorId == po.VendorId && x.ItemId == item.ItemId);

                if (mappedPrice == null)
                    throw new Exception("This item is not mapped with selected vendor!");

                item.UnitPrice = mappedPrice.UnitPrice;
                item.ItemName = masterItem.ItemName;

                // Deduct stock
                masterItem.AvailableQuantity -= item.Quantity;
                _itemService.Update(masterItem);
            }

            // Update PO Data
            existingPO.PODate = po.PODate;
            existingPO.VendorId = po.VendorId;
            existingPO.VendorName = po.VendorName;
            existingPO.Items = po.Items;

            _repo.SaveAll(orders);
        }



        public void Delete(int poNumber)
        {
            var orders = GetAll();

            var existing = orders.FirstOrDefault(x => x.PONumber == poNumber);

            if (existing == null)
                throw new Exception("Purchase Order not found!");

            orders.Remove(existing);

            _repo.SaveAll(orders);
        }

    }
}

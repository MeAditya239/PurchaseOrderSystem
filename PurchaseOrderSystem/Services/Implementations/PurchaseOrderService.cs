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
            }

            orders.Add(po);
            _repo.SaveAll(orders);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PurchaseOrderSystem.Models;
using PurchaseOrderSystem.Services.Interfaces;

namespace PurchaseOrderSystem.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly IVendorService _vendorService;
        private readonly IItemService _itemService;
        private readonly IVendorItemPriceService _priceService;
        private readonly IPurchaseOrderService _poService;

        public PurchaseOrderController(
            IVendorService vendorService,
            IItemService itemService,
            IVendorItemPriceService priceService,
            IPurchaseOrderService poService)
        {
            _vendorService = vendorService;
            _itemService = itemService;
            _priceService = priceService;
            _poService = poService;
        }

        public IActionResult Create()
        {
            LoadVendors();
            return View(new PurchaseOrder { PODate = DateTime.Today });
        }

        [HttpPost]
        public IActionResult Create(PurchaseOrder po)
        {
            LoadVendors();

            try
            {
                _poService.Save(po);
                TempData["Success"] = "Purchase Order Saved Successfully!";
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(po);
            }
        }

        [HttpGet]
        public JsonResult GetVendorItems(int vendorId)
        {
            var mappings = _priceService.GetAll()
                .Where(x => x.VendorId == vendorId)
                .ToList();

            var items = _itemService.GetAll();

            var result = mappings.Select(m => new
            {
                ItemId = m.ItemId,
                ItemName = items.First(i => i.ItemId == m.ItemId).ItemName,
                UnitPrice = m.UnitPrice
            }).ToList();

            return Json(result);
        }

        private void LoadVendors()
        {
            ViewBag.Vendors = _vendorService.GetAll()
                .Where(v => v.IsActive)
                .Select(v => new SelectListItem
                {
                    Value = v.VendorId.ToString(),
                    Text = v.VendorName
                }).ToList();
        }
    }
}

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
            // ✅ Workflow Validation (Doc Business Rule)

            if (!_vendorService.GetAll().Any())
            {
                TempData["Error"] = "Please create Vendor first before Purchase Order!";
                return RedirectToAction("Index", "Vendor");
            }

            if (!_itemService.GetAll().Any())
            {
                TempData["Error"] = "Please create Items first before Purchase Order!";
                return RedirectToAction("Index", "Item");
            }

            if (!_priceService.GetAll().Any())
            {
                TempData["Error"] = "Please map Vendor-Item Prices first before Purchase Order!";
                return RedirectToAction("Index", "VendorItemPrice");
            }

            LoadVendors();

            ViewBag.POs = _poService.GetAll();

            return View(new PurchaseOrder { PODate = DateTime.Today });
        }



        [HttpPost]
        public IActionResult Create(PurchaseOrder po)
        {
            LoadVendors();
            ViewBag.POs = _poService.GetAll();

            try
            {
                if (po.PONumber == 0)
                {
                    _poService.Save(po);
                    TempData["Success"] = "Purchase Order Created Successfully!";
                }
                else
                {
                    _poService.Update(po);
                    TempData["Success"] = "Purchase Order Updated Successfully!";
                }

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


        [HttpPost]
        public IActionResult DeletePO(int poNumber)
        {
            try
            {
                _poService.Delete(poNumber);
                TempData["Success"] = "Purchase Order Deleted Successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Create");
        }

        [HttpGet]
        public JsonResult GetPO(int poNumber)
        {
            var po = _poService.GetById(poNumber);
            return Json(po);
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

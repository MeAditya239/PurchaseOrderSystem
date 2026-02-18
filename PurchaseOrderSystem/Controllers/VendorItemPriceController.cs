using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PurchaseOrderSystem.Models;
using PurchaseOrderSystem.Services.Interfaces;

namespace PurchaseOrderSystem.Controllers
{
    public class VendorItemPriceController : Controller
    {
        private readonly IVendorItemPriceService _mappingService;
        private readonly IVendorService _vendorService;
        private readonly IItemService _itemService;

        private readonly ILogger<VendorItemPriceController> _logger;

        public VendorItemPriceController(
            IVendorItemPriceService mappingService,
            IVendorService vendorService,
            IItemService itemService,
            ILogger<VendorItemPriceController> logger)
        {
            _mappingService = mappingService;
            _vendorService = vendorService;
            _itemService = itemService;
            _logger = logger;
        }

        // ✅ List Mappings
        public IActionResult Index()
        {
            var mappings = _mappingService.GetAll();
            var vendors = _vendorService.GetAll();
            var items = _itemService.GetAll();

            var result = mappings.Select(m => new Models.ViewModels.VendorItemPriceVM
            {
                Id = m.Id,
                VendorName = vendors.First(v => v.VendorId == m.VendorId).VendorName,
                ItemName = items.First(i => i.ItemId == m.ItemId).ItemName,
                UnitPrice = m.UnitPrice
            }).ToList();

            return View(result);
        }

        // ✅ Create Mapping
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult Create(VendorItemPrice mapping)
        {
            LoadDropdowns();

            if (!ModelState.IsValid)
            {
                return View(mapping);
            }

            try
            {
                _mappingService.Add(mapping);

                _logger.LogInformation(
                    "Vendor-Item Price Mapping Created. VendorId={VendorId}, ItemId={ItemId}",
                    mapping.VendorId, mapping.ItemId);

                TempData["Success"] = "Mapping Added Successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating Vendor-Item Mapping");

                ViewBag.Error = ex.Message;
                return View(mapping);
            }
        }

        // ✅ Edit Mapping
        public IActionResult Edit(int id)
        {
            LoadDropdowns();

            var mapping = _mappingService.GetById(id);

            if (mapping == null)
            {
                return NotFound();
            }

            return View(mapping);
        }

        [HttpPost]
        public IActionResult Edit(VendorItemPrice mapping)
        {
            LoadDropdowns();

            if (!ModelState.IsValid)
            {
                return View(mapping);
            }

            try
            {
                _mappingService.Update(mapping);

                _logger.LogInformation(
                    "Vendor-Item Price Mapping Updated. MappingId={Id}",
                    mapping.Id);

                TempData["Success"] = "Mapping Updated Successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating Vendor-Item Mapping");

                ViewBag.Error = ex.Message;
                return View(mapping);
            }
        }

        // ✅ Delete Mapping
        public IActionResult Delete(int id)
        {
            var mapping = _mappingService.GetById(id);

            if (mapping == null)
            {
                return NotFound();
            }

            return View(mapping);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _mappingService.Delete(id);

                _logger.LogInformation(
                    "Vendor-Item Price Mapping Deleted. MappingId={Id}",
                    id);

                TempData["Success"] = "Mapping Deleted Successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting Vendor-Item Mapping");

                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        // ✅ Dropdown Loader
        private void LoadDropdowns()
        {
            ViewBag.Vendors = _vendorService.GetAll()
                .Where(v => v.IsActive)
                .Select(v => new SelectListItem
                {
                    Value = v.VendorId.ToString(),
                    Text = v.VendorName
                }).ToList();

            ViewBag.Items = _itemService.GetAll()
                .Where(i => i.IsActive)
                .Select(i => new SelectListItem
                {
                    Value = i.ItemId.ToString(),
                    Text = i.ItemName
                }).ToList();
        }
    }
}

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

        public VendorItemPriceController(
            IVendorItemPriceService mappingService,
            IVendorService vendorService,
            IItemService itemService)
        {
            _mappingService = mappingService;
            _vendorService = vendorService;
            _itemService = itemService;
        }

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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(mapping);
            }
        }

        public IActionResult Edit(int id)
        {
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
            if (!ModelState.IsValid)
            {
                return View(mapping);
            }
               

            _mappingService.Update(mapping);

            return RedirectToAction("Index");
        }

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
            _mappingService.Delete(id);
            return RedirectToAction("Index");
        }


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

using Microsoft.AspNetCore.Mvc;
using PurchaseOrderSystem.Models;
using PurchaseOrderSystem.Services.Interfaces;

namespace PurchaseOrderSystem.Controllers
{
    public class VendorController : Controller
    {
        private readonly IVendorService _vendorService;
        private readonly ILogger<VendorController> _logger;

        public VendorController(IVendorService vendorService,
            ILogger<VendorController> logger)
        {
            _vendorService = vendorService;
            _logger = logger;
        }


        //  List Vendors
        public IActionResult Index()
        {
            return View(_vendorService.GetAll()); ;
        }

        //  Create Vendor
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Vendor vendor)
        {
            if (!ModelState.IsValid)
            {
                return View(vendor);
            }

            try
            {
                _vendorService.Add(vendor);

                _logger.LogInformation("Vendor Created Successfully: {VendorName}",
                    vendor.VendorName);

                TempData["Success"] = "Vendor Added Successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating Vendor");

                ViewBag.Error = ex.Message;
                return View(vendor);
            }
            
        }

        //  Edit Vendor
        public IActionResult Edit(int id)
        {
            var vendor = _vendorService.GetById(id);

            if (vendor == null)
            { 
                return NotFound(); 
            }

            return View(vendor);
        }

        [HttpPost]
        public IActionResult Edit(Vendor vendor)
        {
            Console.WriteLine("VendorId Received = " + vendor.VendorId);

            if (!ModelState.IsValid)
            {
                return View(vendor);
            }

            try
            {
                _vendorService.Update(vendor);

                _logger.LogInformation("Vendor Updated Successfully. VendorId: {VendorId}",
                    vendor.VendorId);

                TempData["Success"] = "Vendor Updated Successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating Vendor");

                ViewBag.Error = ex.Message;
                return View(vendor);
            }
        }

        //  Delete Vendor
        public IActionResult Delete(int id)
        {
            var vendor = _vendorService.GetById(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _vendorService.Delete(id);

                _logger.LogInformation("Vendor Deleted Successfully. VendorId: {VendorId}", id);

                TempData["Success"] = "Vendor Deleted Successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting Vendor");

                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}

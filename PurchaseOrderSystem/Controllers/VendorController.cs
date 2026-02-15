using Microsoft.AspNetCore.Mvc;
using PurchaseOrderSystem.Models;
using PurchaseOrderSystem.Services.Interfaces;

namespace PurchaseOrderSystem.Controllers
{
    public class VendorController : Controller
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
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

            _vendorService.Add(vendor);
            return RedirectToAction("Index");
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

            _vendorService.Update(vendor);
            return RedirectToAction("Index");
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
            _vendorService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}

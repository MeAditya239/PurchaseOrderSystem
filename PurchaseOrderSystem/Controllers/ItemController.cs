using Microsoft.AspNetCore.Mvc;
using PurchaseOrderSystem.Models;
using PurchaseOrderSystem.Services.Interfaces;

namespace PurchaseOrderSystem.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        //  List Items
        public IActionResult Index()
        {
            return View(_itemService.GetAll());
        }

        //  Create Item
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Item item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            _itemService.Add(item);
            return RedirectToAction("Index");
        }

        // Edit Item
        public IActionResult Edit(int id)
        {
            var item = _itemService.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Item item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            _itemService.Update(item);
            return RedirectToAction("Index");
        }

        //  Delete Item
        public IActionResult Delete(int id)
        {
            var item = _itemService.GetById(id);

            if (item == null)
            {
                return NotFound();
            }


            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _itemService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}

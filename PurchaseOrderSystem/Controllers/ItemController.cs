using Microsoft.AspNetCore.Mvc;
using PurchaseOrderSystem.Models;
using PurchaseOrderSystem.Services.Interfaces;

namespace PurchaseOrderSystem.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly ILogger<ItemController> _logger;

        public ItemController(
            IItemService itemService,
            ILogger<ItemController> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }

        // ✅ List Items
        public IActionResult Index()
        {
            return View(_itemService.GetAll());
        }

        // ✅ Create Item
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

            try
            {
                _itemService.Add(item);

                _logger.LogInformation("Item Created Successfully: {ItemName}",
                    item.ItemName);

                TempData["Success"] = "Item Added Successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating Item");

                ViewBag.Error = ex.Message;
                return View(item);
            }
        }

        // ✅ Edit Item
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

            try
            {
                _itemService.Update(item);

                _logger.LogInformation("Item Updated Successfully. ItemId: {ItemId}",
                    item.ItemId);

                TempData["Success"] = "Item Updated Successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating Item");

                ViewBag.Error = ex.Message;
                return View(item);
            }
        }

        // ✅ Delete Item
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
            try
            {
                _itemService.Delete(id);

                _logger.LogInformation("Item Deleted Successfully. ItemId: {ItemId}", id);

                TempData["Success"] = "Item Deleted Successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting Item");

                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBD4.CustomControllers;
using NBD4.Data;
using NBD4.Models;

namespace NBD4.Controllers
{
    public class InventoryController : LookupsController
    {
        private readonly NBDContext _context;

        public InventoryController(NBDContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public IActionResult Index()
        {
            return Redirect(ViewData["returnURL"].ToString());
        }

        // GET: Inventory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .Include(i => i.MaterialType)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventory/Create
      [Authorize(Roles = "Admin, Designer, Sales Associate")]
        public IActionResult Create()
        {
            ViewData["MaterialTypeID"] = new SelectList(_context.MaterialTypes, "ID", "MaterialTypeName");
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
      [Authorize(Roles = "Admin, Designer, Sales Associate")]
        public async Task<IActionResult> Create([Bind("ID,Code,Description,Size,ListCost,MaterialTypeID")] Inventory inventory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(inventory);
                    await _context.SaveChangesAsync();
                    return Redirect(ViewData["returnURL"].ToString());
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            //Decide if we need to send the Validaiton Errors directly to the client
            if (!ModelState.IsValid && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                //Was an AJAX request so build a message with all validation errors
                string errorMessage = "";
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errorMessage += error.ErrorMessage + "|";
                    }
                }
                //Note: returning a BadRequest results in HTTP Status code 400
                return BadRequest(errorMessage);
            }
            ViewData["MaterialTypeID"] = new SelectList(_context.MaterialTypes, "ID", "MaterialTypeName", inventory.MaterialTypeID);
            return View(inventory);
        }

        // GET: Inventory/Edit/5
      [Authorize(Roles = "Admin, Designer, Sales Associate")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            ViewData["MaterialTypeID"] = new SelectList(_context.MaterialTypes, "ID", "MaterialTypeName", inventory.MaterialTypeID);
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
      [Authorize(Roles = "Admin, Designer, Sales Associate")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Code,Description,Size,ListCost,MaterialTypeID")] Inventory inventory)
        {
            var inventoryToUpdate = await _context.Inventories
                    .FirstOrDefaultAsync(m => m.ID == id);

            //Check that you got it or exit with a not found error
            if (inventoryToUpdate == null)
            {
                return NotFound();
            }

            //Try updating it with the values posted
            if (await TryUpdateModelAsync<Inventory>(inventoryToUpdate, "",
                d => d.ID, d => d.Code, d => d.Description, d => d.Size, d => d.ListCost, d => d.MaterialTypeID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return Redirect(ViewData["returnURL"].ToString());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventoryToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            ViewData["MaterialTypeID"] = new SelectList(_context.MaterialTypes, "ID", "MaterialTypeName", inventory.MaterialTypeID);
            return View(inventoryToUpdate);
        }

        // GET: Inventory/Delete/5
      [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .Include(i => i.MaterialType)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
      [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inventories == null)
            {
                return Problem("Inventory is null.");
            }
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(m => m.ID == id);
            try
            {
                if (inventory != null)
                {
                    _context.Inventories.Remove(inventory);
                }
                await _context.SaveChangesAsync();
                return Redirect(ViewData["returnURL"].ToString());
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete " + ViewData["ControllerFriendlyName"] +
                        ". Remember, you cannot delete a " + ViewData["ControllerFriendlyName"] + " that has related records.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(inventory);
        }

        private bool InventoryExists(int id)
        {
          return _context.Inventories.Any(e => e.ID == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NBD4.CustomControllers;
using NBD4.Data;
using NBD4.Models;

namespace NBD4.Controllers
{
    public class MaterialTypeController : LookupsController
    {
        private readonly NBDContext _context;

        public MaterialTypeController(NBDContext context)
        {
            _context = context;
        }

		// GET: MaterialType
		public IActionResult Index()
		{
			return Redirect(ViewData["returnURL"].ToString());
		}

		// GET: MaterialType/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MaterialTypes == null)
            {
                return NotFound();
            }

            var materialType = await _context.MaterialTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (materialType == null)
            {
                return NotFound();
            }

            return View(materialType);
        }

        // GET: MaterialType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MaterialType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MaterialTypeName")] MaterialType materialType)
        {
			try
			{
				if (ModelState.IsValid)
				{
					_context.Add(materialType);
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
			return View(materialType);
		}

        // GET: MaterialType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MaterialTypes == null)
            {
                return NotFound();
            }

            var materialType = await _context.MaterialTypes.FindAsync(id);
            if (materialType == null)
            {
                return NotFound();
            }
            return View(materialType);
        }

        // POST: MaterialType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MaterialTypeName")] MaterialType materialType)
        {
            
			var materialTypeToUpdate = await _context.MaterialTypes
					.FirstOrDefaultAsync(m => m.ID == id);

			//Check that you got it or exit with a not found error
			if (materialTypeToUpdate == null)
			{
				return NotFound();
			}

			//Try updating it with the values posted
			if (await TryUpdateModelAsync<MaterialType>(materialTypeToUpdate, "",
				d => d.MaterialTypeName))
			{
				try
				{
					await _context.SaveChangesAsync();
					return Redirect(ViewData["returnURL"].ToString());
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!MaterialTypeExists(materialTypeToUpdate.ID))
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
			return View(materialTypeToUpdate);
		}

        // GET: MaterialType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MaterialTypes == null)
            {
                return NotFound();
            }

            var materialType = await _context.MaterialTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (materialType == null)
            {
                return NotFound();
            }

            return View(materialType);
        }

        // POST: MaterialType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            

			if (_context.MaterialTypes == null)
			{
				return Problem("Material Types is null.");
			}
			var materialType = await _context.MaterialTypes
				.FirstOrDefaultAsync(m => m.ID == id);
			try
			{
				if (materialType != null)
				{
					_context.MaterialTypes.Remove(materialType);
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
			return View(materialType);
		}

        private bool MaterialTypeExists(int id)
        {
          return _context.MaterialTypes.Any(e => e.ID == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class LabourTypeInfoController : LookupsController
    {
        private readonly NBDContext _context;

        public LabourTypeInfoController(NBDContext context)
        {
            _context = context;
        }

		// GET: LabourTypeInfo
		public IActionResult Index()
		{
			return Redirect(ViewData["returnURL"].ToString());
		}

		// GET: LabourTypeInfo/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LabourTypeInfos == null)
            {
                return NotFound();
            }

            var labourTypeInfo = await _context.LabourTypeInfos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (labourTypeInfo == null)
            {
                return NotFound();
            }

            return View(labourTypeInfo);
        }

        // GET: LabourTypeInfo/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LabourTypeInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ID,LabourTypeName,PricePerHour,CostPerHour")] LabourTypeInfo labourTypeInfo)
        {
           
			try
			{
				if (ModelState.IsValid)
				{
					_context.Add(labourTypeInfo);
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
			return View(labourTypeInfo);
		}

        // GET: LabourTypeInfo/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LabourTypeInfos == null)
            {
                return NotFound();
            }

            var labourTypeInfo = await _context.LabourTypeInfos.FindAsync(id);
            if (labourTypeInfo == null)
            {
                return NotFound();
            }
            return View(labourTypeInfo);
        }

        // POST: LabourTypeInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LabourTypeName,PricePerHour,CostPerHour")] LabourTypeInfo labourTypeInfo)
        {
			var labourTypeInfoToUpdate = await _context.LabourTypeInfos
				   .FirstOrDefaultAsync(m => m.ID == id);

			//Check that you got it or exit with a not found error
			if (labourTypeInfoToUpdate == null)
			{
				return NotFound();
			}

			//Try updating it with the values posted
			if (await TryUpdateModelAsync<LabourTypeInfo>(labourTypeInfoToUpdate, "",
				d => d.LabourTypeName, d => d.PricePerHour, d => d.CostPerHour))
			{
				try
				{
					await _context.SaveChangesAsync();
					return Redirect(ViewData["returnURL"].ToString());
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!LabourTypeInfoExists(labourTypeInfoToUpdate.ID))
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
			return View(labourTypeInfoToUpdate);
			


        }

        // GET: LabourTypeInfo/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LabourTypeInfos == null)
            {
                return NotFound();
            }

            var labourTypeInfo = await _context.LabourTypeInfos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (labourTypeInfo == null)
            {
                return NotFound();
            }

            return View(labourTypeInfo);
        }

        // POST: LabourTypeInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			if (_context.LabourTypeInfos == null)
			{
				return Problem("Labour Type is null.");
			}
			var labourTypeInfo = await _context.LabourTypeInfos
				.FirstOrDefaultAsync(m => m.ID == id);
			try
			{
				if (labourTypeInfo!= null)
				{
					_context.LabourTypeInfos.Remove(labourTypeInfo);
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
			return View(labourTypeInfo);
		}

        private bool LabourTypeInfoExists(int id)
        {
          return _context.LabourTypeInfos.Any(e => e.ID == id);
        }
    }
}

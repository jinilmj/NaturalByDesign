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
    public class StaffRoleController : LookupsController
    {
        private readonly NBDContext _context;

        public StaffRoleController(NBDContext context)
        {
            _context = context;
        }

		// GET: StaffRole

		public IActionResult Index()
		{
			return Redirect(ViewData["returnURL"].ToString());
		}

		// GET: StaffRole/Details/5

		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StaffRoles == null)
            {
                return NotFound();
            }

            var staffRole = await _context.StaffRoles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (staffRole == null)
            {
                return NotFound();
            }

            return View(staffRole);
        }

        // GET: StaffRole/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: StaffRole/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ID,StaffRoleName")] StaffRole staffRole)
        {
			try
			{
				if (ModelState.IsValid)
				{
					_context.Add(staffRole);
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
			return View(staffRole);
        }

        // GET: StaffRole/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StaffRoles == null)
            {
                return NotFound();
            }

            var staffRole = await _context.StaffRoles.FindAsync(id);
            if (staffRole == null)
            {
                return NotFound();
            }
            return View(staffRole);
        }

        // POST: StaffRole/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
			var staffRoleToUpdate = await _context.StaffRoles
				   .FirstOrDefaultAsync(m => m.ID == id);

			//Check that you got it or exit with a not found error
			if (staffRoleToUpdate == null)
			{
				return NotFound();
			}

			//Try updating it with the values posted
			if (await TryUpdateModelAsync<StaffRole>(staffRoleToUpdate, "",
				d => d.StaffRoleName))
			{
				try
				{
					await _context.SaveChangesAsync();
					return Redirect(ViewData["returnURL"].ToString());
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!StaffRoleExists(staffRoleToUpdate.ID))
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
			return View(staffRoleToUpdate);
		}

        // GET: StaffRole/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StaffRoles == null)
            {
                return NotFound();
            }

            var staffRole = await _context.StaffRoles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (staffRole == null)
            {
                return NotFound();
            }

            return View(staffRole);
        }

        // POST: StaffRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StaffRoles == null)
            {
                return Problem("Entity set 'NBDContext.StaffRole  is null.");
            }
            var staffRole = await _context.StaffRoles
                .FirstOrDefaultAsync(m => m.ID == id);
          
            try
            {
                if (staffRole != null)
                {
                    _context.StaffRoles.Remove(staffRole);
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
            return View(staffRole);
        }

        private bool StaffRoleExists(int id)
        {
          return _context.StaffRoles.Any(e => e.ID == id);
        }
    }
}

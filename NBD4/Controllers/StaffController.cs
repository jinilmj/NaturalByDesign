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
    public class StaffController : LookupsController
    {
        private readonly NBDContext _context;

        public StaffController(NBDContext context)
        {
            _context = context;
        }

        // GET: Staff
        public IActionResult Index()
        {
            return Redirect(ViewData["returnURL"].ToString());
        }

        // GET: Staff/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .Include(s => s.StaffRole)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staff/Create
        public IActionResult Create()
        {
            ViewData["StaffRoleID"] = new SelectList(_context.StaffRoles, "ID", "StaffRoleName");
            return View();
        }

        // POST: Staff/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StaffFirstName,StaffMiddleName,StaffLastName,Email,Phone,StaffRoleID")] Staff staff)
        {
           

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(staff);
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

            ViewData["StaffRoleID"] = new SelectList(_context.StaffRoles, "ID", "StaffRoleName", staff.StaffRoleID);
            return View(staff);
        }

        // GET: Staff/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewData["StaffRoleID"] = new SelectList(_context.StaffRoles, "ID", "StaffRoleName", staff.StaffRoleID);
            return View(staff);
        }

        // POST: Staff/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StaffFirstName,StaffMiddleName,StaffLastName,Email,Phone,StaffRoleID")] Staff staff)
        {
            var staffToUpdate = await _context.Staffs
                    .FirstOrDefaultAsync(m => m.ID == id);

            //Check that you got it or exit with a not found error
            if (staffToUpdate == null)
            {
                return NotFound();
            }

            //Try updating it with the values posted
            if (await TryUpdateModelAsync<Staff>(staffToUpdate, "",
                d => d.ID, d => d.StaffFirstName, d => d.StaffMiddleName, d => d.StaffLastName, d => d.Email, d => d.Phone, d => d.StaffRoleID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return Redirect(ViewData["returnURL"].ToString());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staffToUpdate.ID))
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
            ViewData["StaffRoleID"] = new SelectList(_context.StaffRoles, "ID", "StaffRoleName", staff.StaffRoleID);
            return View(staffToUpdate);
        }

        // GET: Staff/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .Include(s => s.StaffRole)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Staffs == null)
            {
                return Problem("Entity set 'NBDContext.Staffs'  is null.");
            }
            var staff = await _context.Staffs.FindAsync(id);
            try
            {
                if (staff != null)
                {
                    _context.Staffs.Remove(staff);
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
            return View(staff);
        }

        private bool StaffExists(int id)
        {
          return _context.Staffs.Any(e => e.ID == id);
        }
    }
}

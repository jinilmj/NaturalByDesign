using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBD4.Data;
using NBD4.Models;

namespace NBD4.Controllers
{
    public class StaffRoleController : Controller
    {
        private readonly NBDContext _context;

        public StaffRoleController(NBDContext context)
        {
            _context = context;
        }

        // GET: StaffRole
        public async Task<IActionResult> Index()
        {
              return View(await _context.StaffRoles.ToListAsync());
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: StaffRole/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StaffRoleName")] StaffRole staffRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staffRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staffRole);
        }

        // GET: StaffRole/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,StaffRoleName")] StaffRole staffRole)
        {
            if (id != staffRole.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staffRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffRoleExists(staffRole.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(staffRole);
        }

        // GET: StaffRole/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StaffRoles == null)
            {
                return Problem("Entity set 'NBDContext.StaffRoles'  is null.");
            }
            var staffRole = await _context.StaffRoles.FindAsync(id);
            if (staffRole != null)
            {
                _context.StaffRoles.Remove(staffRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffRoleExists(int id)
        {
          return _context.StaffRoles.Any(e => e.ID == id);
        }
    }
}

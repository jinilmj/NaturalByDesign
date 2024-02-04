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
    public class ProvinceController : Controller
    {
        private readonly NBDContext _context;

        public ProvinceController(NBDContext context)
        {
            _context = context;
        }

        // GET: Province
        public async Task<IActionResult> Index()
        {
              return View(await _context.Provinces.ToListAsync());
        }

        // GET: Province/Details/5
        

        // GET: Province/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Province/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ID")] Province province)
        {
            try
            {
               if (ModelState.IsValid)
            {
                _context.Add(province);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            
            return View(province);
        }

        // GET: Province/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Provinces == null)
            {
                return NotFound();
            }

            var province = await _context.Provinces.FindAsync(id);
            if (province == null)
            {
                return NotFound();
            }
            return View(province);
        }

        // POST: Province/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name")] Province province)
        {
            
            var provinceToUpdate = await _context.Provinces.FirstOrDefaultAsync(p => p.ID == id);

            //Check that you got it or exit with a not found error
            if (provinceToUpdate == null)
            {
                return NotFound();
            }

            //Try updating it with the values posted
            if (await TryUpdateModelAsync<Province>(provinceToUpdate, "",
                d => d.Name ,d=>d.ID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvinceExists(provinceToUpdate.ID))
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
            return View(provinceToUpdate);
        }

        // GET: Province/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Provinces == null)
            {
                return NotFound();
            }

            var province = await _context.Provinces
                .FirstOrDefaultAsync(m => m.ID == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        // POST: Province/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Provinces == null)
            {
                return Problem("Entity set 'NBDContext.Provinces'  is null.");
            }
            var province = await _context.Provinces.FindAsync(id);
            try
            {
                if (province != null)
                {
                    _context.Provinces.Remove(province);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete Province. Remember, you cannot delete a Province that is used in the system.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            return View(province);
            
        }

        private bool ProvinceExists(string id)
        {
          return _context.Provinces.Any(e => e.ID == id);
        }
    }
}

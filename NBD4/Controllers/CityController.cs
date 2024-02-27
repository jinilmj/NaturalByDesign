using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBD4.CustomControllers;
using NBD4.Data;
using NBD4.Models;

namespace NBD4.Controllers
{
    public class CityController : LookupsController
    {
        private readonly NBDContext _context;

        public CityController(NBDContext context)
        {
            _context = context;
        }

		// GET: City
		public IActionResult Index()
		{
			return Redirect(ViewData["returnURL"].ToString());
		}

		// GET: City/Details/5


		// GET: City/Create
		public IActionResult Create()
        {
            ViewData["ProvinceID"] = new SelectList(_context.Provinces, "ID", "Name");
            return View();
        }

        // POST: City/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,ProvinceID")] City city)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(city);
                    await _context.SaveChangesAsync();
					return Redirect(ViewData["returnURL"].ToString());
				}
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            ViewData["ProvinceID"] = new SelectList(_context.Provinces, "ID", "Name", city.ProvinceID);
            return View(city);
        }

        // GET: City/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["ProvinceID"] = new SelectList(_context.Provinces, "ID", "Name", city.ProvinceID);
            return View(city);
        }

        // POST: City/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var cityToUpdate = await _context.Cities.FirstOrDefaultAsync(p => p.ID == id);
            if (cityToUpdate == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<City>(cityToUpdate, "",
                d => d.Name, d => d.ProvinceID))
            {
                try
                {
                    await _context.SaveChangesAsync();
					return Redirect(ViewData["returnURL"].ToString());
				}
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(cityToUpdate.ID))
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
            
            ViewData["ProvinceID"] = new SelectList(_context.Provinces, "ID", "Name", cityToUpdate.ProvinceID);
            return View(cityToUpdate);
        }

        // GET: City/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.Province)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: City/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cities == null)
            {
                return Problem("Entity set 'NBDContext.Cities'  is null.");
            }
            var city = await _context.Cities.FindAsync(id);
            try
            {
                if (city != null)
                {
                    _context.Cities.Remove(city);
                    await _context.SaveChangesAsync();
                    return Redirect(ViewData["returnURL"].ToString());
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete City. Remember, you cannot delete a City that is used in the system.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            if (city != null)
            {
                _context.Cities.Remove(city);
            }
            return View(city);
            
            
        }

        private bool CityExists(int id)
        {
          return _context.Cities.Any(e => e.ID == id);
        }
    }
}

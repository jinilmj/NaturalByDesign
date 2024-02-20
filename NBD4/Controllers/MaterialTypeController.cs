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
    public class MaterialTypeController : Controller
    {
        private readonly NBDContext _context;

        public MaterialTypeController(NBDContext context)
        {
            _context = context;
        }

        // GET: MaterialType
        public async Task<IActionResult> Index()
        {
              return View(await _context.MaterialTypes.ToListAsync());
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
            if (ModelState.IsValid)
            {
                _context.Add(materialType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
            if (id != materialType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materialType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialTypeExists(materialType.ID))
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
            return View(materialType);
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
                return Problem("Entity set 'NBDContext.MaterialTypes'  is null.");
            }
            var materialType = await _context.MaterialTypes.FindAsync(id);
            if (materialType != null)
            {
                _context.MaterialTypes.Remove(materialType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialTypeExists(int id)
        {
          return _context.MaterialTypes.Any(e => e.ID == id);
        }
    }
}

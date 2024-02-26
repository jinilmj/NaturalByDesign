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
    public class LabourTypeInfoController : Controller
    {
        private readonly NBDContext _context;

        public LabourTypeInfoController(NBDContext context)
        {
            _context = context;
        }

        // GET: LabourTypeInfo
        public async Task<IActionResult> Index()
        {
              return View(await _context.LabourTypeInfos.ToListAsync());
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: LabourTypeInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LabourTypeName,PricePerHour,CostPerHour")] LabourTypeInfo labourTypeInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labourTypeInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(labourTypeInfo);
        }

        // GET: LabourTypeInfo/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,LabourTypeName,PricePerHour,CostPerHour")] LabourTypeInfo labourTypeInfo)
        {
            if (id != labourTypeInfo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labourTypeInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabourTypeInfoExists(labourTypeInfo.ID))
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
            return View(labourTypeInfo);
        }

        // GET: LabourTypeInfo/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LabourTypeInfos == null)
            {
                return Problem("Entity set 'NBDContext.LabourTypeInfos'  is null.");
            }
            var labourTypeInfo = await _context.LabourTypeInfos.FindAsync(id);
            if (labourTypeInfo != null)
            {
                _context.LabourTypeInfos.Remove(labourTypeInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabourTypeInfoExists(int id)
        {
          return _context.LabourTypeInfos.Any(e => e.ID == id);
        }
    }
}

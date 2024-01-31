using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBD4.Data;
using NBD4.Models;

namespace NBD4.Controllers
{
    public class ClientController : Controller
    {
        private readonly NBDContext _context;

        public ClientController(NBDContext context)
        {
            _context = context;
        }

        // GET: Client
        public async Task<IActionResult> Index()
        {
            var nBDContext = _context.Clients.Include(c => c.City);
            return View(await nBDContext.ToListAsync());
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.City)
                .Include(c=>c.Projects)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Client/Create
        public IActionResult Create()
        {
            //ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name");
            PopulateDropDownLists();
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Email,Phone,Street,PostalCode,CityID")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name", client.CityID);
            PopulateDropDownLists(client);
            return View(client);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            //ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name", client.CityID);
            PopulateDropDownLists(client);
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Email,Phone,Street,PostalCode,CityID")] Client client)
        {
            if (id != client.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ID))
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
            //ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name", client.CityID);
            PopulateDropDownLists();
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'NBDContext.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private SelectList ProvinceSelectList(string selectedId)
        {
            return new SelectList(_context.Provinces
                .OrderBy(d => d.Name), "ID", "Name", selectedId);
        }
        private SelectList CitySelectList(string ProvinceID, int? selectedId)
        {
            var query = from c in _context.Cities
                        where c.ProvinceID == ProvinceID
                        select c;
            return new SelectList(query.OrderBy(p => p.Name), "ID", "Summary", selectedId);
        }

        private void PopulateDropDownLists(Client client = null)
        {

            if ((client?.CityID).HasValue)
            {   
                if (client.City == null)
                {
                    client.City = _context.Cities.Find(client.CityID);
                }
                ViewData["ProvinceID"] = ProvinceSelectList(client.City.ProvinceID);
                ViewData["CityID"] = CitySelectList(client.City.ProvinceID, client.CityID);
            }
            else
            {
                ViewData["ProvinceID"] = ProvinceSelectList(null);
                ViewData["CityID"] = CitySelectList(null, null);
            }
        }

        [HttpGet]
        public JsonResult GetCities(string ProvinceID)
        {
            return Json(CitySelectList(ProvinceID, null));
        }
        private bool ClientExists(int id)
        {
          return _context.Clients.Any(e => e.ID == id);
        }
    }
}

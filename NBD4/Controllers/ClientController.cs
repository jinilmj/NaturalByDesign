using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NBD4.CustomControllers;
using NBD4.Data;
using NBD4.Models;
using NBD4.Utilities;

namespace NBD4.Controllers
{
    public class ClientController : ElephantController
    {
        private readonly NBDContext _context;

        public ClientController(NBDContext context)
        {
            _context = context;
        }

        // GET: Client
        public async Task<IActionResult> Index(string SearchString, string SearchStringPh, string SearchClient,
            int? page, int? pageSizeID, string actionButton, string sortDirection = "asc", string sortField = "Client")
        {
            ViewData["Filtering"] = "btn-outline-secondary";
            int numberFilters = 0;

			string[] sortOptions = new[] { "Name", "Contact" };

			var clients = _context
                .Clients
                .Include(c => c.City)
                .AsNoTracking();


            if (!String.IsNullOrEmpty(SearchString))
            {
                clients= clients.Where(p => p.Name.ToUpper().Contains(SearchString.ToUpper()));
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(SearchStringPh))
            {
                clients = clients.Where(p => p.Phone.Contains(SearchStringPh));
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(SearchClient))
            {
                clients = clients.Where(p => p.Name.ToUpper() == SearchClient.ToUpper());
            }

            if (numberFilters != 0)
            {
                //Toggle the Open/Closed state of the collapse depending on if we are filtering
                ViewData["Filtering"] = " btn-danger";
                //Show how many filters have been applied
                ViewData["numberFilters"] = "(" + numberFilters.ToString()
                    + " Filter" + (numberFilters > 1 ? "s" : "") + " Applied)";
            }
			if (!String.IsNullOrEmpty(actionButton)) //Form Submitted!
			{
                page = 1;

                if (sortOptions.Contains(actionButton))//Change of sort is requested
				{
					if (actionButton == sortField) //Reverse order on same field
					{
						sortDirection = sortDirection == "asc" ? "desc" : "asc";
					}
					sortField = actionButton;//Sort by the button clicked
				}
			}
			//Now we know which field and direction to sort by
			if (sortField == "Name")
			{
				if (sortDirection == "asc")
				{
					clients = clients
						.OrderBy(p => p.Name);
				}
				else
				{
					clients = clients
						.OrderByDescending(p => p.Name);
				}
			}
			else 
			{
				if (sortDirection == "asc")
				{
					clients = clients
						.OrderBy(p => p.ContactLastName)
						.ThenBy(p => p.ContactFirstName);
				}
				else
				{
					clients = clients
						.OrderByDescending(p => p.ContactLastName)
						.ThenByDescending(p => p.ContactFirstName);
				}
			}
			//Set sort for next time
			ViewData["sortField"] = sortField;
			ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID,ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.AsNoTracking()
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
        public async Task<IActionResult> Create([Bind("ID,Name,ContactFirstName,ContactMiddleName,ContactLastName,Email,Phone,Street,PostalCode,CityID")] Client client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(client);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { client.ID });
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

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

            var client = await _context.Clients
                .Include(c => c.City).FirstOrDefaultAsync(c => c.ID == id);
            if (client == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(client);
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var clientToUpdate = await _context.Clients
                .Include(c => c.City)
                .FirstOrDefaultAsync(c => c.ID == id);
            if (clientToUpdate == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<Client>(clientToUpdate, "", c => c.Name, c=> c.ContactFirstName, 
                c => c.ContactMiddleName, c => c.ContactLastName, c=>c.Email, c=>c.Phone, c=>c.Street, c=>c.PostalCode, c=>c.CityID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { clientToUpdate.ID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(clientToUpdate.ID))
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
            
            PopulateDropDownLists(clientToUpdate);
            return View(clientToUpdate);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .AsNoTracking()
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
            try
            {
                if (client != null)
                {
                    _context.Clients.Remove(client);
                }
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return Redirect(ViewData["returnURL"].ToString());
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete Client. Remember, you cannot delete a Client that has projects assigned.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(client);

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

        public JsonResult GetClients(string term)
        {
            var result = from d in _context.Clients
                         where d.Name.ToUpper().Contains(term.ToUpper())
                         orderby d.Name
                         select new { value = d.Name  };
            return Json(result);
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

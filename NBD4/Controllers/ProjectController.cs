using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBD4.CustomControllers;
using NBD4.Data;
using NBD4.Models;
using NBD4.Utilities;

namespace NBD4.Controllers
{
 [Authorize]
    public class ProjectController : ElephantController
    {
        private readonly NBDContext _context;

        public ProjectController(NBDContext context)
        {
            _context = context;
        }

        // GET: Project
        public async Task<IActionResult> Index(string SearchString, int? ClientID,
           int? page, int? pageSizeID, string actionButton, string sortDirection = "asc", string sortField = "Project")
        {
            ViewData["Filtering"] = "btn-outline-secondary";
            int numberFilters = 0;

            PopulateDropDownLists();

			string[] sortOptions = new[] { "Start Date", "End Date", "Clients" };

			var projects = _context
                .Projects
                .Include(p => p.Client)
                .AsNoTracking();

            if (ClientID.HasValue)
            {
                projects = projects.Where(p => p.ClientID == ClientID);
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(SearchString))
            {
                projects = projects.Where(p => p.Site.ToUpper().Contains(SearchString.ToUpper()));
                numberFilters++;
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
				if (sortOptions.Contains(actionButton))//Change of sort is requested
				{
                    page = 1;

                    if (actionButton == sortField) //Reverse order on same field
					{
						sortDirection = sortDirection == "asc" ? "desc" : "asc";
					}
					sortField = actionButton;//Sort by the button clicked
				}
			}

			//Now we know which field and direction to sort by
			if (sortField == "Start Date")
			{
				if (sortDirection == "asc")
				{
					projects = projects
						.OrderBy(p => p.StartDate);
				}
				else
				{
					projects = projects
						.OrderByDescending(p => p.StartDate);
				}
			}
			else if (sortField == "End Date")
			{
				if (sortDirection == "asc")
				{
					projects = projects
						.OrderByDescending(p => p.EndDate);
				}
				else
				{
					projects = projects
						.OrderBy(p => p.EndDate);
				}
			}
			else 
			{
				if (sortDirection == "asc")
				{
					projects = projects
						.OrderBy(p => p.Client.Name);
				}
				else
				{
					projects = projects
						.OrderByDescending(p => p.Client.Name);
				}
			}
			//Set sort for next time
			ViewData["sortField"] = sortField;
			ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID,ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Project>.CreateAsync(projects.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }


        // GET: Project/CreateBid
        [Authorize(Roles = "Admin, Designer")]
        public IActionResult CreateBid(int? projectId)
        {
            if (projectId == null)
            {
                return NotFound();
            }

            return RedirectToAction("Create", "Bid", new { projectId = projectId }); // Redirect to the create bid page.
        }






        // GET: Project/Details/5
        [Authorize(Roles = "Admin, Designer, Sales Associate")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.AsNoTracking()
                .Include(p => p.Client)
                .Include(p => p.Bids)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        public IActionResult GetClients(string searchString)
        {
            searchString ??= ""; // Ensure searchString is not null
            var clients = _context.Clients
                .AsEnumerable()
                .Where(c => c.Name.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Name)
                .Select(c => new { id = c.ID, name = c.Name })
                .ToList();

            return Json(clients);
        }


        // GET: Project/Create
      [Authorize(Roles = "Admin, Designer")]
        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
      [Authorize(Roles = "Admin, Designer")]
        public async Task<IActionResult> Create([Bind("ID,StartDate,EndDate,Site,Amount,ClientID")] Project project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { project.ID });
                }
            }
            catch (DbUpdateException )
            {               
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");               
            }
            
            PopulateDropDownLists(project);
            return View(project);
        }

        // GET: Project/Edit/5
      [Authorize(Roles = "Admin, Designer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(project);
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
      [Authorize(Roles = "Admin, Designer")]
        public async Task<IActionResult> Edit(int id)
        {
            //Go get the patient to update
            var projectToUpdate = await _context.Projects
                .Include(p => p.Client)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (projectToUpdate == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<Project>(projectToUpdate, "",
                p => p.StartDate, p => p.EndDate, p => p.Site,p=>p.Amount, p => p.ClientID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { projectToUpdate.ID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(projectToUpdate.ID))
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
            //ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Email", project.ClientID);
            PopulateDropDownLists(projectToUpdate);
            return View(projectToUpdate);
        }

        // GET: Project/Delete/5
      [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Client)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
      [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Project has already removed from the system.");
            }
            var project = await _context.Projects.FindAsync(id);
            try
            {
                if (project != null)
                {
                    _context.Projects.Remove(project);
                }
                await _context.SaveChangesAsync();
                return Redirect(ViewData["returnURL"].ToString());
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to delete record. Try again, and if the problem persists see your system administrator.");
            }
            return View(project);           
            
        }
        private SelectList ClientSelectList(int? selectedId)
        {
            return new SelectList(_context.Clients
                .OrderBy(c => c.Name), "ID", "Name", selectedId);
        }
        private void PopulateDropDownLists(Project project = null)
        {
            ViewData["ClientID"] = ClientSelectList(project?.ClientID);
            //var dQuery = from c in _context.Clients
            //             orderby c.Name
            //             select c;
            //ViewData["ClientID"] = new SelectList(dQuery, "ID", "Name", project?.ClientID);
        }
        private bool ProjectExists(int id)
        {
          return _context.Projects.Any(e => e.ID == id);
        }
    }
}

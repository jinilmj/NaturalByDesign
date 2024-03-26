using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NBD4.CustomControllers;
using NBD4.Data;
using NBD4.Models;
using NBD4.Utilities;
using NBD4.ViewModels;
using NuGet.Packaging.Signing;

namespace NBD4.Controllers
{
    [Authorize]
    public class BidController : ElephantController
    {
        private readonly NBDContext _context;

        public BidController(NBDContext context)
        {
            _context = context;
        }

        // GET: Bid
        public async Task<IActionResult> Index(string SearchStringPh, string SearchClient, int? pageSizeID, string actionButton, int? page,
            string approvalButton, string sortDirection = "asc",string sortField = "Bid")
        {
            var loggedInEmployeeDesigner = User.IsInRole("Designer");
            var loggedInEmployeeSales = User.IsInRole("Sales Associate");
            ViewData["Filtering"] = "btn-outline-secondary";
            int numberFilters = 0;

            string[] sortOptions = new[] { "Bid Date", "Client", "Project" };

            var bids = _context.Bids
                .Include(b => b.Project).ThenInclude(p=>p.Client)
                .Include(p=>p.BidInventories).ThenInclude(p=>p.Inventory)
                .Include(b=>b.BidStaffs).ThenInclude(p=>p.Staff)
                .Include(b => b.BidLabourTypeInfos).ThenInclude(p => p.LabourTypeInfo)
                .AsNoTracking();

            //if (loggedInEmployeeDesigner )
            //{
            //    string loggedInEmployeeFullName = "Tamara"; 
            //    bids = bids.Where(b => b.BidStaffs.Any(s => s.Staff.StaffFirstName == loggedInEmployeeFullName));
            //}
            if (loggedInEmployeeSales)
            {
                string loggedInEmployeeFullName = "Bob"; 
                bids = bids.Where(b => b.BidStaffs.Any(s => s.Staff.StaffFirstName == loggedInEmployeeFullName));
            }
            if (!String.IsNullOrEmpty(SearchStringPh))
            {
                bids = bids.Where(p => p.Project.Site.ToUpper().Contains(SearchStringPh.ToUpper()));
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(SearchClient))
            {
                bids = bids.Where(p => p.Project.Client.Name.ToUpper().Contains(SearchClient.ToUpper()));
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(approvalButton))
            {
                bids = bids.Where(p => p.ApprovalStage != ApprovalStage.NEW && p.ApprovalStage != ApprovalStage.APPROVED);
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
            if (sortField == "Client")
            {
                if (sortDirection == "asc")
                {
                    bids = bids
                        .OrderBy(p => p.Project.Client.Name);
                }
                else
                {
                    bids = bids
                        .OrderByDescending(p => p.Project.Client.Name);
                }
            }
            else if (sortField == "Bid Date")
            {
                if (sortDirection == "asc")
                {
                    bids = bids
                        .OrderBy(p => p.BidDate);
                }
                else
                {
                    bids = bids
                        .OrderByDescending(p => p.BidDate);
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    bids = bids
                        .OrderBy(p => p.Project.Site);
                }
                else
                {
                    bids = bids
                        .OrderByDescending(p => p.Project.Site);
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Bid>.CreateAsync(bids.AsNoTracking(), page ?? 1, pageSize);

            return View(await bids.ToListAsync());
        }

        // GET: Bid/Details/5
        [Authorize(Roles = "Admin, Designer, Sales Associate")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bids == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids
                .Include(b => b.Project)
                .Include(p => p.BidInventories).ThenInclude(p => p.Inventory)
                .Include(b => b.BidStaffs).ThenInclude(p => p.Staff)
                .Include(b => b.BidLabourTypeInfos).ThenInclude(p => p.LabourTypeInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bid == null)
            {
                return NotFound();
            }

            return View(bid);
        }

        public IActionResult GetProjects(string searchString)
        {
            searchString ??= ""; // Ensure searchString is not null
            var projects = _context.Projects
                .AsEnumerable()
                .Where(c => c.Site.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Site)
                .Select(c => new { id = c.ID, name = c.Site })
                .ToList();

            return Json(projects);
        }
        // GET: Bid/Create
        [Authorize(Roles = "Admin, Designer")]
        public IActionResult Create()
        {
            var bid = new Bid();
            PopulateAssignedStaffData(bid);
            PopulateAssignedLabourData(bid);
            PopulateAssignedInventoryData(bid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "Site");
            return View();
        }

        // POST: Bid/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Designer")]
        public async Task<IActionResult> Create([Bind("ID,BidDate,BidAmount,BidNBDApproved,BidNBDApprovalNotes,BidClientApproved,BidClientApprovalNotes," +
            "BidMarkForRedesign,BidMarkForRedisignNotes,ReviewDate,ReviewedBy,ProjectID")] Bid bid, string[] selectedStaffOptions, string[] selectedLabourOptions, string[] selectedOptions)
        {
            if (selectedStaffOptions != null)
            {
                foreach (var staff in selectedStaffOptions)
                {
                    var staffToAdd = new BidStaff { BidID = bid.ID, StaffID = int.Parse(staff) };
                    bid.BidStaffs.Add(staffToAdd);
                }
            }
            if (selectedLabourOptions != null)
            {
                foreach (var labourTypeInfoId in selectedLabourOptions)
                {
                   
                    int labourId = int.Parse(labourTypeInfoId);
                    var hours = int.Parse(Request.Form[$"selectedLabourHours[{labourId}]"]);

                    // Fetch the LabourTypeInfo from the database
                    var labourTypeInfo = await _context.LabourTypeInfos.FindAsync(labourId);

                    var labourToAdd = new BidLabourTypeInfo { BidID = bid.ID, LabourTypeInfoID = labourId, Hours = hours, LabourTypeInfo = labourTypeInfo };
                    bid.BidLabourTypeInfos.Add(labourToAdd);
                    labourToAdd.CalculateLabourCharge(); // Calculate Labour Charge for the new labour
                    
                }
            }
            if (selectedOptions != null)
            {
                List<int> plantIDs = new List<int>();
                List<int> potteryIDs = new List<int>();
                List<int> otherIDs = new List<int>();

                for (int j = 0; j < selectedOptions.Count(); j++)
                {
                    int matID = int.Parse(selectedOptions[j]);
                    int invID = ((Inventory)await _context.Inventories.FindAsync(matID)).MaterialTypeID;

                    if (invID == 1)
                    {
                        plantIDs.Add(matID);
                    }
                    else if (invID == 2)
                    {
                        potteryIDs.Add(matID);
                    }
                    else if (invID == 3)
                    {
                        otherIDs.Add(matID);
                    }
                }

                foreach (int invId in plantIDs)
                {
                    var quantityPlant = int.Parse(Request.Form[$"selectedInventoryQuantitiesPlant[{invId}]"]);

                    // Fetch the Inventory from the database
                    var inventory = await _context.Inventories.FindAsync(invId);

                    var inventoryToAdd = new BidInventory { BidID = bid.ID, InventoryID = invId, MaterialQuantity = quantityPlant, Inventory = inventory };
                    bid.BidInventories.Add(inventoryToAdd);
                    inventoryToAdd.CalculateExtendPrice();
                }
                foreach (int invId in potteryIDs)
                {
                    var quantityPottery = int.Parse(Request.Form[$"selectedInventoryQuantitiesPottery[{invId}]"]);

                    // Fetch the Inventory from the database
                    var inventory = await _context.Inventories.FindAsync(invId);

                    var inventoryToAdd = new BidInventory { BidID = bid.ID, InventoryID = invId, MaterialQuantity = quantityPottery, Inventory = inventory };
                    bid.BidInventories.Add(inventoryToAdd);
                    inventoryToAdd.CalculateExtendPrice();
                }
                foreach (int invId in otherIDs)
                {
                    var quantityOther = int.Parse(Request.Form[$"selectedInventoryQuantitiesOther[{invId}]"]);

                    // Fetch the Inventory from the database
                    var inventory = await _context.Inventories.FindAsync(invId);

                    var inventoryToAdd = new BidInventory { BidID = bid.ID, InventoryID = invId, MaterialQuantity = quantityOther, Inventory = inventory };
                    bid.BidInventories.Add(inventoryToAdd);
                    inventoryToAdd.CalculateExtendPrice();
                }
            }
            //UpdateBidInventories(selectedOptions, bid);
            if (ModelState.IsValid)
            {
                bid.CalculateTotalBidAmount();
                _context.Add(bid);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { bid.ID });
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "Site", bid.ProjectID);
            PopulateAssignedStaffData(bid);
            PopulateAssignedLabourData(bid);
            PopulateAssignedInventoryData(bid);
            return View(bid);
        }

        // GET: Bid/Edit/5
        [Authorize(Roles = "Admin, Designer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bids == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids
                .Include(p => p.BidStaffs).ThenInclude(p => p.Staff)
                .Include(b => b.BidLabourTypeInfos).ThenInclude(p => p.LabourTypeInfo)
                .Include(d => d.BidInventories).ThenInclude(d => d.Inventory)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (bid == null)
            {
                return NotFound();
            }
            PopulateAssignedStaffData(bid);
            PopulateAssignedLabourData(bid);
            PopulateAssignedInventoryData(bid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "Site", bid.ProjectID);
            return View(bid);
        }

        // POST: Bid/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Designer")]
        public async Task<IActionResult> Edit(int id, string[] selectedStaffOptions, string[] selectedLabourOptions, string[] selectedOptions)
        {
            var bidToUpdate = await _context.Bids
               .Include(p => p.BidStaffs).ThenInclude(p => p.Staff)
               .Include(b => b.BidLabourTypeInfos).ThenInclude(p => p.LabourTypeInfo)
               .Include(d => d.BidInventories).ThenInclude(d => d.Inventory)
                .FirstOrDefaultAsync(p => p.ID == id); 

            if (bidToUpdate == null)
            {
                return NotFound();
            }

            UpdateBidStaffs(selectedStaffOptions, bidToUpdate);
            UpdateBidLabours(selectedLabourOptions, bidToUpdate);
            UpdateBidInventories(selectedOptions, bidToUpdate);

            if (await TryUpdateModelAsync<Bid>(bidToUpdate, "", b => b.BidDate, b => b.BidAmount, b => b.BidNBDApproved, b => b.BidNBDApprovalNotes, b => b.BidClientApproved, b => b.BidClientApprovalNotes
                    , b => b.BidMarkForRedesign,b=>b.BidMarkForRedisignNotes ,b => b.ReviewDate, b => b.ReviewedBy, b => b.ProjectID, b => b.ApprovalStage))
            {
                try
                {
                    foreach (var labourTypeInfoId in selectedLabourOptions)
                    {
                        int labourId = int.Parse(labourTypeInfoId);
                        var hours = int.Parse(Request.Form[$"selectedLabourHours[{labourId}]"]);
                        var labourTypeInfo = await _context.LabourTypeInfos.FindAsync(labourId);
                        var labourToUpdate = bidToUpdate.BidLabourTypeInfos.FirstOrDefault(b => b.LabourTypeInfoID == labourId);
                        if (labourToUpdate != null)
                        {
                            labourToUpdate.Hours = hours;
                            labourToUpdate.LabourTypeInfo = labourTypeInfo;
                            labourToUpdate.CalculateLabourCharge();
                        }
                    }

                    List<int> plantIDs = new List<int>();
                    List<int> potteryIDs = new List<int>();
                    List<int> otherIDs = new List<int>();

                    bidToUpdate.BidInventories.Clear();                    

                    for (int j = 0; j < selectedOptions.Count(); j++)
                    {
                        int matID = int.Parse(selectedOptions[j]);
                        int invID = ((Inventory)await _context.Inventories.FindAsync(matID)).MaterialTypeID;

                        if (invID == 1)
                        {
                            plantIDs.Add(matID);
                        }
                        else if (invID == 2)
                        {
                            potteryIDs.Add(matID);
                        }
                        else if (invID == 3)
                        {
                            otherIDs.Add(matID);
                        }
                    }

                    foreach (int invId in plantIDs)
                    {
                        var quantityPlant = int.Parse(Request.Form[$"selectedInventoryQuantitiesPlant[{invId}]"]);

                        //var inventoryToUpdate = bidToUpdate.BidInventories.FirstOrDefault(b => b.InventoryID == invId);

                        // Fetch the Inventory from the database
                        var inventory = await _context.Inventories.FindAsync(invId);

                        var inventoryToAdd = new BidInventory { BidID = bidToUpdate.ID, InventoryID = invId, MaterialQuantity = quantityPlant, Inventory = inventory };
                        bidToUpdate.BidInventories.Add(inventoryToAdd);
                        inventoryToAdd.CalculateExtendPrice();
                    }
                    foreach (int invId in potteryIDs)
                    {
                        var quantityPottery = int.Parse(Request.Form[$"selectedInventoryQuantitiesPottery[{invId}]"]);

                        //var inventoryToUpdate = bidToUpdate.BidInventories.FirstOrDefault(b => b.InventoryID == invId);

                        // Fetch the Inventory from the database
                        var inventory = await _context.Inventories.FindAsync(invId);

                        var inventoryToAdd = new BidInventory { BidID = bidToUpdate.ID, InventoryID = invId, MaterialQuantity = quantityPottery, Inventory = inventory };
                        bidToUpdate.BidInventories.Add(inventoryToAdd);
                        inventoryToAdd.CalculateExtendPrice();
                    }
                    foreach (int invId in otherIDs)
                    {
                        var quantityOther = int.Parse(Request.Form[$"selectedInventoryQuantitiesOther[{invId}]"]);

                        //var inventoryToUpdate = bidToUpdate.BidInventories.FirstOrDefault(b => b.InventoryID == invId);

                        // Fetch the Inventory from the database
                        var inventory = await _context.Inventories.FindAsync(invId);

                        var inventoryToAdd = new BidInventory { BidID = bidToUpdate.ID, InventoryID = invId, MaterialQuantity = quantityOther, Inventory = inventory };
                        bidToUpdate.BidInventories.Add(inventoryToAdd);
                        inventoryToAdd.CalculateExtendPrice();

                        //////// KEEPING THIS AS A TEMPORYARY BACKUP ///////
                        //var inventoryToUpdate = bidToUpdate.BidInventories.FirstOrDefault(b => b.InventoryID == invId);
                        //var inventory = await _context.Inventories.FindAsync(invId);

                        //if (inventoryToUpdate != null)
                        //{
                        //    inventoryToUpdate.MaterialQuantity = quantityOther;
                        //    inventoryToUpdate.Inventory = inventory;
                        //    inventoryToUpdate.CalculateExtendPrice();
                        //}
                    }

                    bidToUpdate.CalculateTotalBidAmount();
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { bidToUpdate.ID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BidExists(bidToUpdate.ID))
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
            PopulateAssignedStaffData(bidToUpdate);
            PopulateAssignedLabourData(bidToUpdate);
            PopulateAssignedInventoryData(bidToUpdate);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "Site", bidToUpdate.ProjectID);
            return View(bidToUpdate);
        }
        // GET: Bid/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bids == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids
                .Include(b => b.Project)
                 .Include(p => p.BidInventories).ThenInclude(p => p.Inventory)
                .Include(b => b.BidStaffs).ThenInclude(p => p.Staff)
                .Include(b => b.BidLabourTypeInfos).ThenInclude(p => p.LabourTypeInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bid == null)
            {
                return NotFound();
            }

            return View(bid);
        }

        // POST: Bid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bids == null)
            {
                return Problem("Entity set 'NBDContext.Bids'  is null.");
            }
            var bid = await _context.Bids
                .Include(p => p.BidInventories).ThenInclude(p => p.Inventory)
                .Include(b => b.BidStaffs).ThenInclude(p => p.Staff)
                .Include(b => b.BidLabourTypeInfos).ThenInclude(p => p.LabourTypeInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bid != null)
            {
                _context.Bids.Remove(bid);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private void PopulateAssignedStaffData(Bid bid)
        {
            //For this to work, you must have Included the BidStaffs 
            //in the Bid
            var allOptions = _context.Staffs;
            var currentOptionIDs = new HashSet<int>(bid.BidStaffs.Select(b => b.StaffID));
            var checkBoxes = new List<CheckOptionVM>();
            foreach (var option in allOptions)
            {
                checkBoxes.Add(new CheckOptionVM
                {
                    ID = option.ID,
                    DisplayText = option.FullName,
                    Assigned = currentOptionIDs.Contains(option.ID)
                });
            }
            ViewData["StaffOptions"] = checkBoxes;
        }
        private void UpdateBidStaffs(string[] selectedStaffOptions, Bid bidToUpdate)
        {
            if (selectedStaffOptions == null)
            {
                bidToUpdate.BidStaffs = new List<BidStaff>();
                return;
            }

            var selectedStaffOptionsHS = new HashSet<string>(selectedStaffOptions);
            var bidOptionsHS = new HashSet<int>
                (bidToUpdate.BidStaffs.Select(c => c.StaffID));//IDs of the currently selected staffs
            foreach (var option in _context.Staffs)
            {
                if (selectedStaffOptionsHS.Contains(option.ID.ToString())) //It is checked
                {
                    if (!bidOptionsHS.Contains(option.ID))  //but not currently in the history
                    {
                        bidToUpdate.BidStaffs.Add(new BidStaff { BidID = bidToUpdate.ID, StaffID = option.ID });
                    }
                }
                else
                {
                    //Checkbox Not checked
                    if (bidOptionsHS.Contains(option.ID)) //but it is currently in the history - so remove it
                    {
                        BidStaff staffToRemove = bidToUpdate.BidStaffs.SingleOrDefault(c => c.StaffID == option.ID);
                        _context.Remove(staffToRemove);
                    }
                }
            }
        }
        private void PopulateAssignedLabourData(Bid bid)
        {
            var allOptions = _context.LabourTypeInfos.ToList();
            var currentOptionIDs = new HashSet<int>(bid.BidLabourTypeInfos.Select(b => b.LabourTypeInfoID));
            var dropdownOptions = allOptions.Select(option => new CheckOptionVM
            {
                ID = option.ID,
                DisplayText = option.LabourTypeName,
                Assigned = currentOptionIDs.Contains(option.ID),
                Hours = bid.BidLabourTypeInfos.FirstOrDefault(b => b.LabourTypeInfoID == option.ID)?.Hours ?? 0
            }).ToList();
            ViewData["LabourTypeInfoOptions"] = dropdownOptions;
        }
        private void PopulateAssignedInventoryData(Bid bid)
        {
            var allOptions = _context.Inventories.ToList();
            var currentOptionIDs = new HashSet<int>(bid.BidInventories.Select(b => b.InventoryID));
            var dropdownOptions = allOptions.Select(option => new SelectListVM
            {
                ID = option.ID,
                DisplayText = option.Description,
                Assigned = currentOptionIDs.Contains(option.ID),
                Quantities = bid.BidInventories.FirstOrDefault(b => b.InventoryID == option.ID)?.MaterialQuantity ?? 0,
                MaterialTypeID = option.MaterialTypeID
            }).ToList();

            ViewData["InventoryOptionsPlant"] = dropdownOptions.FindAll(i => i.MaterialTypeID == 1);
            ViewData["InventoryOptionsPottery"] = dropdownOptions.FindAll(i => i.MaterialTypeID == 2);
            ViewData["InventoryOptionsOther"] = dropdownOptions.FindAll(i => i.MaterialTypeID == 3);
        }
        private void UpdateBidInventories(string[] selectedOptions, Bid bidToUpdate)
        {
            if (selectedOptions == null)
            {
                bidToUpdate.BidInventories = new List<BidInventory>();
                return;
            }

            var selectedInventoryOptionsHS = new HashSet<string>(selectedOptions);
            var bidOptionsHS = new HashSet<int>
                (bidToUpdate.BidInventories.Select(c => c.InventoryID));
            foreach (var option in _context.Inventories)
            {
                if (selectedInventoryOptionsHS.Contains(option.ID.ToString())) //It is checked
                {
                    if (!bidOptionsHS.Contains(option.ID))
                    {
                        bidToUpdate.BidInventories.Add(new BidInventory { BidID = bidToUpdate.ID, InventoryID = option.ID });
                    }
                }
                else
                {
                    //Checkbox Not checked
                    if (bidOptionsHS.Contains(option.ID))
                    {
                        BidInventory inventoryToRemove = bidToUpdate.BidInventories.SingleOrDefault(c => c.InventoryID == option.ID);
                        _context.Remove(inventoryToRemove);
                    }
                }
            }
        }

        private void UpdateBidLabours(string[] selectedLabourOptions, Bid bidToUpdate)
        {
            if (selectedLabourOptions == null)
            {
                bidToUpdate.BidLabourTypeInfos = new List<BidLabourTypeInfo>();
                return;
            }

            var selectedLabourOptionsHS = new HashSet<string>(selectedLabourOptions);
            var bidOptionsHS = new HashSet<int>
                (bidToUpdate.BidLabourTypeInfos.Select(c => c.LabourTypeInfoID));
            foreach (var option in _context.LabourTypeInfos)
            {
                if (selectedLabourOptionsHS.Contains(option.ID.ToString())) //It is checked
                {
                    if (!bidOptionsHS.Contains(option.ID))
                    {
                        bidToUpdate.BidLabourTypeInfos.Add(new BidLabourTypeInfo { BidID = bidToUpdate.ID, LabourTypeInfoID = option.ID });
                    }
                }
                else
                {
                    //Checkbox Not checked
                    if (bidOptionsHS.Contains(option.ID))
                    {
                        BidLabourTypeInfo labourToRemove = bidToUpdate.BidLabourTypeInfos.SingleOrDefault(c => c.LabourTypeInfoID == option.ID);
                        _context.Remove(labourToRemove);
                    }
                }
            }
        }
        
        //private void PopulateAssignedInventoryData(Bid bid)
        //{
        //    //For this to work, you must have Included the child collection in the parent object
        //    var allOptions = _context.Inventories;
        //    var currentOptionsHS = new HashSet<int>(bid.BidInventories.Select(b => b.InventoryID));
        //    //Instead of one list with a boolean, we will make two lists
        //    var selected = new List<ListOptionVM>();
        //    var available = new List<ListOptionVM>();
        //    foreach (var s in allOptions)
        //    {
        //        if (currentOptionsHS.Contains(s.ID))
        //        {
        //            selected.Add(new ListOptionVM
        //            {
        //                ID = s.ID,
        //                DisplayText = s.Description
        //            });
        //        }
        //        else
        //        {
        //            available.Add(new ListOptionVM
        //            {
        //                ID = s.ID,
        //                DisplayText = s.Description
        //            });
        //        }
        //    }

        //    ViewData["selOpts"] = new MultiSelectList(selected.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        //    ViewData["availOpts"] = new MultiSelectList(available.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        //}
        //private void UpdateBidInventories(string[] selectedOptions, Bid bidToUpdate)
        //{
        //    if (selectedOptions == null)
        //    {
        //        bidToUpdate.BidInventories = new List<BidInventory>();
        //        return;
        //    }

        //    var selectedOptionsHS = new HashSet<string>(selectedOptions);
        //    var currentOptionsHS = new HashSet<int>(bidToUpdate.BidInventories.Select(b => b.InventoryID));
        //    foreach (var s in _context.Inventories)
        //    {
        //        if (selectedOptionsHS.Contains(s.ID.ToString()))//it is selected
        //        {
        //            if (!currentOptionsHS.Contains(s.ID))
        //            {
        //                bidToUpdate.BidInventories.Add(new BidInventory
        //                {
        //                    InventoryID = s.ID,
        //                    BidID = bidToUpdate.ID
        //                });
        //            }
        //        }
        //        //else //not selected
        //        //{
        //        //    if (currentOptionsHS.Contains(s.ID))//but is currently in the Doctor's collection - Remove it!
        //        //    {
        //        //        BidInventory invToRemove = bidToUpdate.BidInventories.FirstOrDefault(d => d.InventoryID == s.ID);
        //        //        _context.Remove(invToRemove);
        //        //    }
        //        //}
        //    }
        //}

        private bool BidExists(int id)
        {
          return _context.Bids.Any(e => e.ID == id);
        }
    }
}

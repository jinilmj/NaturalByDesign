using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NBD4.Data;
using NBD4.Models;
using NBD4.ViewModels;

namespace NBD4.Controllers
{
    public class BidController : Controller
    {
        private readonly NBDContext _context;

        public BidController(NBDContext context)
        {
            _context = context;
        }

        // GET: Bid
        public async Task<IActionResult> Index()
        {
            var nBDContext = _context.Bids
                .Include(b => b.Project)
                .Include(p=>p.BidInventories).ThenInclude(p=>p.Inventory)
                .Include(b=>b.BidStaffs).ThenInclude(p=>p.Staff)
                .AsNoTracking();
            return View(await nBDContext.ToListAsync());
        }

        // GET: Bid/Details/5
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
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bid == null)
            {
                return NotFound();
            }

            return View(bid);
        }

        // GET: Bid/Create
        public IActionResult Create()
        {
            var bid = new Bid();
            PopulateAssignedStaffData(bid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "Site");
            return View();
        }

        // POST: Bid/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BidDate,BidAmount,BidNBDApproved,BidNBDApprovalNotes,BidClientApproved,BidClientApprovalNotes,BidMarkForRedesign,ReviewDate,ReviewedBy,ProjectID")] Bid bid, string[] selectedOptions)
        {
            if (selectedOptions != null)
            {
                foreach (var staff in selectedOptions)
                {
                    var staffToAdd = new BidStaff { BidID = bid.ID, StaffID = int.Parse(staff) };
                    bid.BidStaffs.Add(staffToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(bid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "Site", bid.ProjectID);
            PopulateAssignedStaffData(bid);
            return View(bid);
        }

        // GET: Bid/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bids == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids
                .Include(p => p.BidStaffs).ThenInclude(p => p.Staff)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (bid == null)
            {
                return NotFound();
            }
            PopulateAssignedStaffData(bid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "Site", bid.ProjectID);
            return View(bid);
        }

        // POST: Bid/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedOptions)
        {
            var bidToUpdate = await _context.Bids
               .Include(p => p.BidStaffs).ThenInclude(p => p.Staff)
                .FirstOrDefaultAsync(p => p.ID == id); 

            if (bidToUpdate == null)
            {
                return NotFound();
            }

            UpdateBidStaffs(selectedOptions, bidToUpdate);

            if (await TryUpdateModelAsync<Bid>(bidToUpdate, "", b => b.BidDate, b => b.BidAmount, b => b.BidNBDApproved, b => b.BidNBDApprovalNotes, b => b.BidClientApproved, b => b.BidClientApprovalNotes
                    , b => b.BidMarkForRedesign, b => b.ReviewDate, b => b.ReviewedBy, b => b.ProjectID))
            {
                try
                {
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
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "Site", bidToUpdate.ProjectID);
            return View(bidToUpdate);
        }
        // GET: Bid/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bids == null)
            {
                return Problem("Entity set 'NBDContext.Bids'  is null.");
            }
            var bid = await _context.Bids
                .Include(p => p.BidInventories).ThenInclude(p => p.Inventory)
                .Include(b => b.BidStaffs).ThenInclude(p => p.Staff)
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
        private void UpdateBidStaffs(string[] selectedOptions, Bid bidToUpdate)
        {
            if (selectedOptions == null)
            {
                bidToUpdate.BidStaffs = new List<BidStaff>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var bidOptionsHS = new HashSet<int>
                (bidToUpdate.BidStaffs.Select(c => c.StaffID));//IDs of the currently selected staffs
            foreach (var option in _context.Staffs)
            {
                if (selectedOptionsHS.Contains(option.ID.ToString())) //It is checked
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
        private bool BidExists(int id)
        {
          return _context.Bids.Any(e => e.ID == id);
        }
    }
}

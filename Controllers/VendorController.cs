using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Data;
using Inventory_M.Models.vendor;

namespace Inventory_M.Controllers
{
    public class VendorController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly InventoryDbContext _context;

        public VendorController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: Vendor
       
        public async Task<IActionResult> Index(string SearchString)
        {

            ViewData["CurrentFilter"] = SearchString;
            
            var Vendors = from b in _context.Vendor  
                          select b ;

            if (!String.IsNullOrEmpty(SearchString))
            {
                Vendors = Vendors.Where ( b => b.VendorName.Contains(SearchString)|| b.Email.Contains(SearchString));
            }             

            return View(await Vendors.AsNoTracking().ToListAsync());
        }

        // GET: Vendor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vendor == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendor
                .FirstOrDefaultAsync(m => m.VendorId == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        // GET: Vendor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vendor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendorId,VendorName,Address,ContactPerson,Telephone,Email,Fax")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendor);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = " Vendor Created Successfully....!";
                return RedirectToAction(nameof(Index));
            }
            return View(vendor);
        }

        // GET: Vendor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vendor == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendor.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            return View(vendor);
        }

        // POST: Vendor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VendorId,VendorName,Address,ContactPerson,Telephone,Email,Fax")] Vendor vendor)
        {
            if (id != vendor.VendorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorExists(vendor.VendorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["AlertMessage"] = " Vendor Updated Successfully....!";
                return RedirectToAction(nameof(Index));
            }
            return View(vendor);
        }

        // GET: Vendor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vendor == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendor
                .FirstOrDefaultAsync(m => m.VendorId == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        // POST: Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vendor == null)
            {
                return Problem("Entity set 'InventoryDbContext.Vendor'  is null.");
            }
            var vendor = await _context.Vendor.FindAsync(id);
            if (vendor != null)
            {
                _context.Vendor.Remove(vendor);

            }
            
            await _context.SaveChangesAsync();
            TempData["AlertMessage"] = " Vendor Deleted Successfully....!";
            return RedirectToAction(nameof(Index));
        }

        private bool VendorExists(int id)
        {
          return (_context.Vendor?.Any(e => e.VendorId == id)).GetValueOrDefault();
        }
    }
}

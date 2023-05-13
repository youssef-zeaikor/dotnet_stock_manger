using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Data;
using Inventory_M.Models.adjustement;

namespace Inventory_M.Controllers
{
    public class CancelledorderController : Controller
    {
        private readonly InventoryDbContext _context;

        public CancelledorderController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: Cancelledorder
        public async Task<IActionResult> Index()
        {
            var inventoryDbContext = _context.Adjustements.Include(a => a.Products);
            return View(await inventoryDbContext.ToListAsync());
        }

        // GET: Cancelledorder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Adjustements == null)
            {
                return NotFound();
            }

            var adjustement = await _context.Adjustements
                .Include(a => a.Products)
                .FirstOrDefaultAsync(m => m.AdjId == id);
            if (adjustement == null)
            {
                return NotFound();
            }

            return View(adjustement);
        }

        // GET: Cancelledorder/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: Cancelledorder/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdjId,Reference,ProductId,Qty,Action,Remarks,UserAuth,DateStamp")] Adjustement adjustement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adjustement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", adjustement.ProductId);
            return View(adjustement);
        }

        // GET: Cancelledorder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Adjustements == null)
            {
                return NotFound();
            }

            var adjustement = await _context.Adjustements.FindAsync(id);
            if (adjustement == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", adjustement.ProductId);
            return View(adjustement);
        }

        // POST: Cancelledorder/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdjId,Reference,ProductId,Qty,Action,Remarks,UserAuth,DateStamp")] Adjustement adjustement)
        {
            if (id != adjustement.AdjId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adjustement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdjustementExists(adjustement.AdjId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", adjustement.ProductId);
            return View(adjustement);
        }

        // GET: Cancelledorder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Adjustements == null)
            {
                return NotFound();
            }

            var adjustement = await _context.Adjustements
                .Include(a => a.Products)
                .FirstOrDefaultAsync(m => m.AdjId == id);
            if (adjustement == null)
            {
                return NotFound();
            }

            return View(adjustement);
        }

        // POST: Cancelledorder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Adjustements == null)
            {
                return Problem("Entity set 'InventoryDbContext.Adjustements'  is null.");
            }
            var adjustement = await _context.Adjustements.FindAsync(id);
            if (adjustement != null)
            {
                _context.Adjustements.Remove(adjustement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdjustementExists(int id)
        {
          return (_context.Adjustements?.Any(e => e.AdjId == id)).GetValueOrDefault();
        }
    }
}

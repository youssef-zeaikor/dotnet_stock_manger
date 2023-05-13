using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Data;
using Inventory_M.Models.stockin;

namespace Inventory_M.Controllers
{
    public class StockHisController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly InventoryDbContext _context;

        public StockHisController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: StockHis
        public async Task<IActionResult> Index()
        {
            var inventoryDbContext = _context.Stockins.Include(s => s.products).Include(s => s.venders);
            return View(await inventoryDbContext.ToListAsync());
        }

        // GET: StockHis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stockins == null)
            {
                return NotFound();
            }

            var stockIn = await _context.Stockins
                .Include(s => s.products)
                .Include(s => s.venders)
                .FirstOrDefaultAsync(m => m.StockInId == id);
            if (stockIn == null)
            {
                return NotFound();
            }

            return View(stockIn);
        }

        // GET: StockHis/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "VendorId");
            return View();
        }

        // POST: StockHis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockInId,Reference,Date,stockeby,status,VendorId,ProductId,qtynew")] StockIn stockIn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockIn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", stockIn.ProductId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "VendorId", stockIn.VendorId);
            return View(stockIn);
        }

        // GET: StockHis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stockins == null)
            {
                return NotFound();
            }

            var stockIn = await _context.Stockins.FindAsync(id);
            if (stockIn == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", stockIn.ProductId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "VendorId", stockIn.VendorId);
            return View(stockIn);
        }

        // POST: StockHis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StockInId,Reference,qtynew,Date,stockeby,status,VendorId,ProductId")] StockIn stockIn)
        {
            if (id != stockIn.StockInId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockIn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockInExists(stockIn.StockInId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", stockIn.ProductId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "VendorId", stockIn.VendorId);
            return View(stockIn);
        }

        // GET: StockHis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stockins == null)
            {
                return NotFound();
            }

            var stockIn = await _context.Stockins
                .Include(s => s.products)
                .Include(s => s.venders)
                .FirstOrDefaultAsync(m => m.StockInId == id);
            if (stockIn == null)
            {
                return NotFound();
            }

            return View(stockIn);
        }

        // POST: StockHis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stockins == null)
            {
                return Problem("Entity set 'InventoryDbContext.Stockins'  is null.");
            }
            var stockIn = await _context.Stockins.FindAsync(id);
            if (stockIn != null)
            {
                _context.Stockins.Remove(stockIn);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockInExists(int id)
        {
          return (_context.Stockins?.Any(e => e.StockInId == id)).GetValueOrDefault();
        }
    }
}

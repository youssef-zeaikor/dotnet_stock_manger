using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Data;
using Inventory_M.Models.recod;

namespace Inventory_M.Controllers
{
    public class RecodssController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly InventoryDbContext _context;

        public RecodssController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: Recodss
        public async Task<IActionResult> Index()
        {
            var inventoryDbContext = _context.records.Include(r => r.Trades).Include(r => r.adj).Include(r => r.bran).Include(r => r.categ).Include(r => r.produits).Include(r => r.sal).Include(r => r.stock).Include(r => r.vend);
            return View(await inventoryDbContext.ToListAsync());
        }

        // GET: Recodss/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.records == null)
            {
                return NotFound();
            }

            var recod = await _context.records
                .Include(r => r.Trades)
                .Include(r => r.adj)
                .Include(r => r.bran)
                .Include(r => r.categ)
                .Include(r => r.produits)
                .Include(r => r.sal)
                .Include(r => r.stock)
                .Include(r => r.vend)
                .FirstOrDefaultAsync(m => m.RecordId == id);
            if (recod == null)
            {
                return NotFound();
            }

            return View(recod);
        }

        // GET: Recodss/Create
        public IActionResult Create()
        {
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "TradesId");
            ViewData["AdjId"] = new SelectList(_context.Adjustements, "AdjId", "AdjId");
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            ViewData["saleId"] = new SelectList(_context.Sales, "saleId", "transno");
            ViewData["StockInId"] = new SelectList(_context.Stockins, "StockInId", "StockInId");
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "VendorId");
            return View();
        }

        // POST: Recodss/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecordId,TradesId,ProductId,CategoryId,BrandId,saleId,StockInId,VendorId,AdjId")] Recod recod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "TradesId", recod.TradesId);
            ViewData["AdjId"] = new SelectList(_context.Adjustements, "AdjId", "AdjId", recod.AdjId);
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", recod.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", recod.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", recod.ProductId);
            ViewData["saleId"] = new SelectList(_context.Sales, "saleId", "transno", recod.saleId);
            ViewData["StockInId"] = new SelectList(_context.Stockins, "StockInId", "StockInId", recod.StockInId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "VendorId", recod.VendorId);
            return View(recod);
        }

        // GET: Recodss/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.records == null)
            {
                return NotFound();
            }

            var recod = await _context.records.FindAsync(id);
            if (recod == null)
            {
                return NotFound();
            }
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "TradesId", recod.TradesId);
            ViewData["AdjId"] = new SelectList(_context.Adjustements, "AdjId", "AdjId", recod.AdjId);
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", recod.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", recod.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", recod.ProductId);
            ViewData["saleId"] = new SelectList(_context.Sales, "saleId", "transno", recod.saleId);
            ViewData["StockInId"] = new SelectList(_context.Stockins, "StockInId", "StockInId", recod.StockInId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "VendorId", recod.VendorId);
            return View(recod);
        }

        // POST: Recodss/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecordId,TradesId,ProductId,CategoryId,BrandId,saleId,StockInId,VendorId,AdjId")] Recod recod)
        {
            if (id != recod.RecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecodExists(recod.RecordId))
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
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "TradesId", recod.TradesId);
            ViewData["AdjId"] = new SelectList(_context.Adjustements, "AdjId", "AdjId", recod.AdjId);
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", recod.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", recod.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", recod.ProductId);
            ViewData["saleId"] = new SelectList(_context.Sales, "saleId", "transno", recod.saleId);
            ViewData["StockInId"] = new SelectList(_context.Stockins, "StockInId", "StockInId", recod.StockInId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "VendorId", recod.VendorId);
            return View(recod);
        }

        // GET: Recodss/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.records == null)
            {
                return NotFound();
            }

            var recod = await _context.records
                .Include(r => r.Trades)
                .Include(r => r.adj)
                .Include(r => r.bran)
                .Include(r => r.categ)
                .Include(r => r.produits)
                .Include(r => r.sal)
                .Include(r => r.stock)
                .Include(r => r.vend)
                .FirstOrDefaultAsync(m => m.RecordId == id);
            if (recod == null)
            {
                return NotFound();
            }

            return View(recod);
        }

        // POST: Recodss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.records == null)
            {
                return Problem("Entity set 'InventoryDbContext.records'  is null.");
            }
            var recod = await _context.records.FindAsync(id);
            if (recod != null)
            {
                _context.records.Remove(recod);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecodExists(int id)
        {
          return (_context.records?.Any(e => e.RecordId == id)).GetValueOrDefault();
        }
    }
}

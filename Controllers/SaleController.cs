using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Data;
using Inventory_M.Models.sale;
using Newtonsoft.Json;
using System.Globalization;

namespace Inventory_M.Controllers
{
    public class SaleController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly InventoryDbContext _context;

        public SaleController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: Sale
        public async Task<IActionResult> Index()
        {
           var trades = await _context.Tradespeoples.ToListAsync();
            var tradesList = trades.Select(t => new SelectListItem
            {
                
                Text = t.FullName
            }).ToList();

            ViewBag.Trades_list = tradesList;


            var InventoryDbContext = _context.Sales.Include(s => s.Trades).Include(s => s.produits);
            
            return View(await InventoryDbContext.ToListAsync());
        }

        // GET: Sale/Details/5



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Trades)
                .Include(s => s.produits)
                .FirstOrDefaultAsync(m => m.saleId == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sale/Create
        public IActionResult Create()
        {
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "FullName");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Product_Name");
            return View();
        }

        // POST: Sale/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("saleId,transno,Disc,Price,Qty,Date,status,TradesId,ProductId")] Sale sale)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "FullName", sale.TradesId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Product_Name", sale.ProductId);
            return View(sale);
        }

        // GET: Sale/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "FullName", sale.TradesId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Product_Name", sale.ProductId);
            return View(sale);
        }

        // POST: Sale/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("saleId,transno,Disc,Price,Qty,Date,status,TradesId,ProductId")] Sale sale)
        {
            if (id != sale.saleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.saleId))
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
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "FullName", sale.TradesId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Product_Name", sale.ProductId);
            return View(sale);
        }

        // GET: Sale/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Trades)
                .Include(s => s.produits)
                .FirstOrDefaultAsync(m => m.saleId == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sales == null)
            {
                return Problem("Entity set 'InventoryDbContext.Sales'  is null.");
            }
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
          return (_context.Sales?.Any(e => e.saleId == id)).GetValueOrDefault();
        }

      
    }
}

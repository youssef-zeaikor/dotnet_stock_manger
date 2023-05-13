using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Data;
using Inventory_M.Models.sale;
using System.Globalization;
using Newtonsoft.Json;
namespace Inventory_M.Controllers
{
    public class SellingController : Controller
    {
        private readonly InventoryDbContext _context;

        public SellingController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: Selling
        public async Task<IActionResult> Index()
        {
            var inventoryDbContext = _context.Sales.Include(s => s.Trades).Include(s => s.produits);
            return View(await inventoryDbContext.ToListAsync());
        }

        // GET: Selling/Details/5
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

        // GET: Selling/Create
        public IActionResult Create()
        {
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "TradesId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }





        // POST: Selling/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("saleId,transno,Disc,Price,Qty,Date,status,TradesId,ProductId")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "TradesId", sale.TradesId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", sale.ProductId);
            return View(sale);
        }

        // GET: Selling/Edit/5
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
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "TradesId", sale.TradesId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", sale.ProductId);
            return View(sale);
        }

        // POST: Selling/Edit/5
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
            ViewData["TradesId"] = new SelectList(_context.Tradespeoples, "TradesId", "TradesId", sale.TradesId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", sale.ProductId);
            return View(sale);
        }

        // GET: Selling/Delete/5
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

        // POST: Selling/Delete/5
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


        public async Task<IActionResult> Topsalling()
        {
            var sales = await _context.Sales
                 .Include(s => s.Trades)
                .Include(s => s.produits)
                .OrderByDescending(s => s.Qty)
                .ToListAsync();
            return View(sales);
        }

        public async Task<IActionResult> SalesStatistics()
        {
            // Récupérer les données de vente
            var sales = await _context.Sales
                .GroupBy(s => s.Date.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(s => s.Price * s.Qty) })
                .OrderBy(g => g.Month)
                .ToListAsync();

            // Préparer les données pour le graphique
            var labels = sales.Select(s => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(s.Month)).ToArray();
            var data = sales.Select(s => s.Total).ToArray();

            // Créer le graphique
            ViewBag.Chart = new
            {
                Type = "bar",
                Data = new
                {
                    Labels = labels,
                    Datasets = new[]
                    {
                new { Label = "Ventes totales", Data = data, BackgroundColor = "#3e95cd" }
            }
                },
                Options = new
                {
                    Scales = new
                    {
                        YAxes = new[] { new { Ticks = new { BeginAtZero = true } } }
                    }
                }
            };

            return View();
        }


        // GET: Selling/TradespeopleSales




    }
}
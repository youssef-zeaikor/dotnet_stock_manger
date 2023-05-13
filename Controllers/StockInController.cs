using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Data;
using Inventory_M.Models.stockin;
using System.Drawing.Printing;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Inventory_M.Models.vendor;

namespace Inventory_M.Controllers
{
    public class StockInController : Controller
    {
        private readonly InventoryDbContext _context;

        public StockInController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: StockIn
        public async Task<IActionResult> Index()
        {
            var inventoryDbContext = _context.Stockins.Include(s => s.products).Include(s => s.venders);

            return View(await inventoryDbContext.ToListAsync());
        }





        // GET: StockIn/Details/5
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

        // GET: StockIn/Create      

        public IActionResult Create()
        {
            ViewBag.VendorId = new SelectList(_context.Vendor, "VendorId", "VendorName");
            ViewBag.ProductId = new SelectList(_context.Products, "ProductId", "Product_Name");

            ViewBag.VendorList = _context.Vendor.Select(v => new SelectListItem
            {
                Text = v.VendorName,
                Value = v.VendorId.ToString()
            }).ToList();

            ViewBag.ProductList = _context.Products.Select(p => new SelectListItem
            {
                Text = p.Product_Name,
                Value = p.ProductId.ToString()
            }).ToList();




            return View();
        }





        // POST: StockIn/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        public IActionResult create(StockIn stockIn)
        {
            if (!ModelState.IsValid)
            {
                // Ajouter l'objet StockIn à la base de données
                _context.Add(stockIn);
                _context.SaveChanges();

                // Renvoyer une vue partielle qui contient la nouvelle ligne HTML de la table
                return PartialView("_SaveValuesTableRow", stockIn);
            }

            return View(stockIn);
        }


        [HttpGet]
        public IActionResult GetVendorInfo(int vendorId)
        {
            var vendor = _context.Vendor.FirstOrDefault(v => v.VendorId == vendorId);
            if (vendor == null)
            {
                return NotFound();
            }

            return Json(new { address = vendor.Address, contactPerson = vendor.ContactPerson });
        }

        // GET: StockIn/Edit/5
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Product_Name", stockIn.ProductId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "VendorName", stockIn.VendorId);
            return View(stockIn);
        }

        // POST: StockIn/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StockInId,Reference,Quantite,Date,stockeby,status,VendorId,ProductId,VendorName,Adress,ContactPerson,rolename,Product_Name,Lname,qtynew")] StockIn stockIn)
        {
            if (id != stockIn.StockInId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Product_Name", stockIn.ProductId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "VendorName", stockIn.VendorId);
            return View(stockIn);
        }

        // GET: StockIn/Delete/5
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

        // POST: StockIn/Delete/5
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

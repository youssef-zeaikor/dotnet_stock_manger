using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Data;
using Inventory_M.Models.product;
using Inventory_M.Models.adjustement;


namespace Inventory_M.Controllers
{
    public class AdjustementController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly InventoryDbContext _context;

        //public int Quantity { get; private set; }

        public AdjustementController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: Product


        public async Task<IActionResult> Index(string searchString)
        {
            ViewBag.UserName = HttpContext.Session.GetString("username");

            var inventoryDbContext = _context.Products.Include(p => p.Brands).Include(p => p.Categories);

            if (!String.IsNullOrEmpty(searchString))
            {
                var products = from b in _context.Products
                               where b.Product_Name.Contains(searchString) || b.Barcode.Contains(searchString)
                               select b;

                // Include related Brands and Categories
                products = products.Include(p => p.Brands).Include(p => p.Categories);

                ViewData["CurrentFilter"] = searchString;

                return View(await products.AsNoTracking().ToListAsync());
            }
            else
            {
                return View(await inventoryDbContext.ToListAsync());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string productId, string Reference, string Pcode, string description, string quantity, string action, string Remarks, string user)
        {
            // Create a new instance of the Adjustment model and set its properties
            var adjustment = new Adjustement
            {
                ProductId = int.Parse(productId),
                Reference = int.Parse(Reference),
                Qty = int.Parse(quantity),
                Action = action,
                Remarks = Remarks,
                UserAuth = user,
                DateStamp = DateTime.Now // set the date to the current date and time
            };

            // Retrieve the product object from the database
            var product = await _context.Products.FindAsync(int.Parse(productId));

            // Adjust the product quantity based on the adjustment action
            if (action == "add")
            {
                product.Quantity += int.Parse(quantity);
            }
            else if (action == "remove")
            {
                product.Quantity -= int.Parse(quantity);
            }

            // Add the new Adjustment to the database and save changes
            _context.Adjustements.Add(adjustment);
            await _context.SaveChangesAsync();

            // Redirect to the Index page and display a success message
            TempData["AlertMessage"] = "Adjustment added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Recall(string searchString)
        {

            var adjustements = from a in _context.Adjustements
                               select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                adjustements = adjustements.Where(a => a.Products.Product_Name.Contains(searchString) || a.Products.Barcode.Contains(searchString));
                ViewData["CurrentFilter"] = searchString;
            }

            adjustements = adjustements.Include(a => a.Products); // include related Product

            return View(await adjustements.AsNoTracking().ToListAsync());


        }

        private async Task<Dictionary<string, int>> GetAdjustmentStats()
        {
            var stats = new Dictionary<string, int>();

            var addCount = await _context.Adjustements.CountAsync(a => a.Action == "add");
            var removeCount = await _context.Adjustements.CountAsync(a => a.Action == "remove");

            stats.Add("Add", addCount);
            stats.Add("Remove", removeCount);

            return stats;
        }
        public async Task<IActionResult> Index1(string searchString)
        {


            var stats = await GetAdjustmentStats();
            ViewBag.AdjustmentStats = stats;

            return View();
        }







    }
}
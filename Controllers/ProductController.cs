using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Data;
using Inventory_M.Models.product;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
namespace Inventory_M.Controllers
{
    public class ProductController : Controller
    {
        private readonly InventoryDbContext _context;

        public ProductController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: Product


        public async Task<IActionResult> Index(string searchString)
        {
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

        //renvoyer les informations du  vendor  en fonction de l'ID
        [HttpGet]
        public IActionResult GetVendorInfo(int vendorId)
        {
            var vendor = _context.Vendor.FirstOrDefault(v => v.VendorId == vendorId);
            if (vendor == null)
            {
                return NotFound();
            }

            var data = new
            {
                address = vendor.Address,
                contactPerson = vendor.ContactPerson
            };

            return Json(data);
        }




        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brands)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Brand_Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Category_Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Product_Name,Barcode,description,Price,Quantity,Recorder,BrandId,Brand_Name,CategoryId,Category_Name")] Product product)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = " Product Created Successfully....!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Brand_Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Category_Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Brand_Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Category_Name", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Product_Name,Barcode,description,Price,Quantity,Recorder,BrandId,CategoryId,Category_Name,Brand_Name")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["AlertMessage"] = " Product Updated Successfully....!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Brand_Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Category_Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brands)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'InventoryDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            TempData["AlertMessage"] = " Product Deleted Successfully....!";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        // GET: Product/AdministrativeList
        public IActionResult AdministrativeList()
        {
            // Get the list of products from the database
            var products = _context.Products.ToList();

            // Create a new PDF document
            var document = new PdfDocument();

            // Create a new page in the document
            var page = document.AddPage();

            // Create a new graphics object for drawing on the page
            var gfx = XGraphics.FromPdfPage(page);

            // Set up the font and font size for the text
            var font = new XFont("Arial", 10, XFontStyle.Regular);

            // Set up the brush for the text color
            var brush = XBrushes.DarkBlue;

            // Draw the title of the document
            gfx.DrawString("List of Products", font, XBrushes.Black, new XRect(0, 0, page.Width.Point, 50), XStringFormats.Center);

            // Define the table layout
            var tableX = 0;
            var tableY = 50;
            var columnWidths = new double[] { 200, 150, 100, 100 };
            var rowHeight = 20;

            // Draw the table headers
            for (int i = 0; i < columnWidths.Length; i++)
            {
                var headerRect = new XRect(tableX, tableY, columnWidths[i], rowHeight);
                gfx.DrawRectangle(XBrushes.LightGray, headerRect);
                gfx.DrawString(GetHeaderLabel(i), font, brush, headerRect, XStringFormats.Center);
                tableX += (int)columnWidths[i];
            }

            tableX = 0;
            tableY += rowHeight;

            // Draw the table rows
            foreach (var product in products)
            {
                for (int i = 0; i < columnWidths.Length; i++)
                {
                    var cellRect = new XRect(tableX, tableY, columnWidths[i], rowHeight);
                    gfx.DrawRectangle(XBrushes.White, cellRect);
                    gfx.DrawString(GetCellValue(product, i), font, XBrushes.Black, cellRect, XStringFormats.Center);
                    tableX += (int)columnWidths[i];
                }

                tableX = 0;
                tableY += rowHeight;
            }

            // Save the document to a memory stream
            var stream = new MemoryStream();
            document.Save(stream);

            // Set the position of the stream to the beginning
            stream.Position = 0;

            // Set the content type to PDF
            var contentType = "application/pdf";

            // Return the PDF file as a file result
            return File(stream, contentType, "ProductList.pdf");
        }

        // Helper method to get the header label based on the column index
        private string GetHeaderLabel(int columnIndex)
        {
            switch (columnIndex)
            {
                case 0:
                    return "Product Name";
                case 1:
                    return "Barcode";
                case 2:
                    return "Price";
                case 3:
                    return "Quantity";
                default:
                    return string.Empty;
            }
        }

        // Helper method to get the cell value based on the column index
        private string GetCellValue(Product product, int columnIndex)
        {
            switch (columnIndex)
            {
                case 0:
                    return product.Product_Name;
                case 1:
                    return product.Barcode;
                case 2:
                    return product.Price.ToString();
                case 3:
                    return product.Quantity.ToString();
                default:
                    return "";
            }
        }


    }
}
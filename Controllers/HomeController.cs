using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inventory_M.Models;
using Inventory_M.Models.vendor;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Inventory_M.Data;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Models.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace Inventory_M.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly InventoryDbContext _context;

    public HomeController(ILogger<HomeController> logger, InventoryDbContext context)
    {
        _logger = logger;
        _context = context;
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



    public async Task<IActionResult> Index()
    {
        ViewBag.UserName = HttpContext.Session.GetString("username");
        ViewBag.UserLname = HttpContext.Session.GetString("userlname");
        //===========================================================
        //============================================================
             // Récupérer les données de stockin à afficher dans le graphique, en incluant les données de product
            var stockins = _context.Stockins.Include(s => s.products).ToList();
            //$$$$$$$$$$$$$$$$$$$
           
            var vendors = from b in _context.Vendor 
                          select b;

            //$$$$$$$$$$$$$$$$$$$
            var products = await _context.Products.ToListAsync();
        //============================================================



            //$$$$$$$$$$$$$$$$$$$
             // Créer une liste pour stocker les noms des produits
            var productNames = new List<string>();

            // Créer une liste pour stocker les quantités de stockin
            var quantities = new List<int>();

            // Remplir les listes à partir des données de stockin
            foreach (var stockin in stockins)
            {
                productNames.Add(stockin.products.Product_Name);
                quantities.Add(stockin.qtynew);
            }

            //$$$$$$$$$$$$$$$$$$$

            // Count the number of vendors
            var vendorCount = await vendors.CountAsync();

            //$$$$$$$$$$$$$$$$$$$
             // Count the number of products in each category
            var categoryCounts = await _context.Products
                .Include(p => p.Categories)
                .GroupBy(p => p.CategoryId)
                .Select(g => new {
                    CategoryName = g.First().Categories.Category_Name,
                    Count = g.Sum(p => p.Quantity)
                })
                .OrderByDescending(c => c.Count)
                .ToListAsync();

            //$$$$$$$$$$$$$$$$$$$

            
        //=============================================================

            // Créer un objet de données pour le graphique
            var chartData_stockIn = new
            {
                labels = productNames,
                datasets = new[] {
            new { label = "Quantités de stockin", data = quantities, backgroundColor = "#3e95cd" }
            }
            };

            //$$$$$$$$$$$$$$$$$$$


            // Prepare the data for the chart vendor
            var chartData_Vendor = new
            {
                labels = new[] { "Vendors" },
                datasets = new List<object>
                {
                    new
                    {
                        label = "Number of Vendors",
                        data = new[] { vendorCount },
                        backgroundColor = new[] { "#3e95cd" }
                    }
                }
            };

            //$$$$$$$$$$$$$$$$$$$



        // Prepare the data for the chart Producr per category

        var chartData_ProductPerCategory = new
            {
                labels = categoryCounts.Select(c => c.CategoryName).ToList(),
                datasets = new List<object>
                {
                    new
                    {
                        label = "Number of Products",
                        data = categoryCounts.Select(c => c.Count).ToList(),
                        backgroundColor = new List<string>
                        {
                            "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850",
                            "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850",
                            "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"
                        }
                    }
                }
            };

           


            //===============================================

           

           // Convertir l'objet de données en JSON
            var chartDataJson = JsonConvert.SerializeObject(chartData_stockIn);


            //=================================================
            var stats = await GetAdjustmentStats();
            ViewData["AdjustmentStats"] = stats;


            ViewData["StockInchart"] = chartDataJson;

            ViewData["Vendorchart"] = chartData_Vendor;

            ViewData["ChartDataProductPerCategory"] = chartData_ProductPerCategory;

            var count = await _context.Sales.CountAsync();

            ViewBag.Count = count;

            var mycount = await _context.Stockins.SumAsync(s => s.qtynew);

            ViewBag.MyCount = mycount;

            var countpro = await _context.Products.CountAsync();

            ViewBag.Countpro = countpro;

        return View();
    }




    public IActionResult Privacy()
    {
        return View();
    }



    public async Task<IActionResult> LogOut()
    {
        // Get the UserId from the session
        var userId = HttpContext.Session.GetInt32("UserId");

        // Retrieve the user from the database
        var user = _context.Users.SingleOrDefault(u => u.UserId == userId);


        if (user != null)
        {
            // Update the user's status
            user.status = false;
            _context.SaveChanges();
        }

        // Remove the session
        HttpContext.Session.Clear();

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Auth");

    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }




}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inventory_M.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Inventory_M.Data;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Models.ViewModels;
using Inventory_M.Models.users_management;

namespace Inventory_M.Controllers;

public class EmployeeController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly InventoryDbContext _context;

    public EmployeeController(ILogger<EmployeeController> logger, InventoryDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var employees = _context.Users
        .Include(u => u.UsersToRoles)
        .ThenInclude(ur => ur.Role)
        .Select(u => new EmployeeViewModel
        {
            UserId = u.UserId,
            Name = u.Name,
            Lname = u.Lname,
            Email = u.Email,
            PassWord = u.PassWord,
            Status = u.status,
            RoleName = u.UsersToRoles.FirstOrDefault().Role.rolename
        })
        .ToList();

        ViewBag.Employees = employees;


        return View();
    }

    public IActionResult Profil()
    {

        // Get the user id from session
        var userId = HttpContext.Session.GetString("UserId");

        // Convert the user id to int
        int userIdInt = int.Parse(userId);

        var employee = _context.Users.Include(u => u.UsersToRoles)
                              .ThenInclude(ur => ur.Role)
                              .FirstOrDefault(e => e.UserId == userIdInt);

        // Get the list of available roles
        var roles = _context.Roles.ToList();

        // Set the ViewBag.Roles to contain the list of available roles
        ViewBag.Roles = roles;

        var model = new EmployeeViewModel
        {
            UserId = employee.UserId,
            Name = employee.Name,
            Lname = employee.Lname,
            Email = employee.Email,
            PassWord = employee.PassWord,
            Status = employee.status,
            RoleName = employee.UsersToRoles.FirstOrDefault()?.Role?.rolename, // Assuming a user can have only one role
            // other properties
        };

        return View(model);

    }




    [HttpPost]
    public IActionResult EditUser(EmployeeViewModel model)
    {

        // Retrieve the user ID value from the hidden input field
        int userIdInt = int.Parse(Request.Form["UserId"]);

        // Get the employee from the database using the user id
        var employee = _context.Users.Include(u => u.UsersToRoles)
                                  .ThenInclude(ur => ur.Role)
                                  .FirstOrDefault(e => e.UserId == userIdInt);

        // Update the employee data with the data from the view model
        employee.Name = model.Name;
        employee.Lname = model.Lname;
        employee.Email = model.Email;
        employee.PassWord = model.PassWord;
        employee.status = model.Status;

        // Get the role from the database using the role name
        var role = _context.Roles.FirstOrDefault(r => r.rolename == model.RoleName);

        // Update the user's role
        employee.UsersToRoles = new List<UserToRole>()
    {
        new UserToRole { UserId = employee.UserId, RoleId = role.RoleId }
    };

        // Save changes to the database
        _context.SaveChanges();

        ViewData["ValidateMessage"] = "User Edited With Success";


        // Redirect to the profile page
        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult DeleteUser(EmployeeViewModel model)
    {
        // Retrieve the user ID value from the hidden input field
        int userId = int.Parse(Request.Form["UId"]);
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            ViewData["FoundMessage"] = "The User You want to delete do Not Existe ";
            return RedirectToAction("Index");
        }
        _context.Users.Remove(user);
        _context.SaveChanges();

        return RedirectToAction("Index");

    }



    [HttpGet]
    public IActionResult GetUser(int id)
    {
        // Get the employee from the database using the user id
        var employee = _context.Users.Include(u => u.UsersToRoles)
                                    .ThenInclude(ur => ur.Role)
                                    .FirstOrDefault(e => e.UserId == id);

        // Create a new object to hold the employee data
        var employeeData = new
        {
            name = employee.Name,
            lname = employee.Lname,
            email = employee.Email,
            roleName = employee.UsersToRoles.FirstOrDefault().Role.rolename,
            userId = employee.UserId
        };

        // Return the employee data as JSON
        return Json(employeeData);
    }



}







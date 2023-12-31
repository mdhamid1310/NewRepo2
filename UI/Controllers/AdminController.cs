using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;

namespace UI.Controllers
{
    public class AdminController : Controller
    {
        private InsuranceDbContext dbContext;
        public AdminController()
        {
            dbContext = new InsuranceDbContext(); // Initialize your DbContext
        }
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult GetAllCustomers()
        {
            var customers = dbContext.Customers.ToList();
            return View(customers);
        }

        // Action method to get all users
        public ActionResult GetAllUsers()
        {
            var users = dbContext.Customers.ToList(); // Assuming users are stored in the same table as customers
            return View(users);
        }
        public ActionResult PoliciesListed()
        {
            var policies = dbContext.Policies.ToList();
            return View(policies);
        }
        public ActionResult Categories()
        {
            var categories = dbContext.Categories.ToList();
            return View(categories);
        }






        public ActionResult Policy()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;

namespace UI.Controllers
{
    public class CustomerController : Controller
    {
        private InsuranceDbContext dbContext;
        public CustomerController()
        {
            dbContext = new InsuranceDbContext(); // Initialize your DbContext
        }
        // GET: Customer
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
















    }
}
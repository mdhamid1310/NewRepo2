using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;

namespace UI.Controllers
{
    public class PolicyController : Controller
    {
        private InsuranceDbContext dbContext; // Replace YourDbContext with the actual name of your DbContext class

        public PolicyController()
        {
            dbContext = new InsuranceDbContext(); // Initialize your DbContext
        }

        // Action method to show all policies
        public ActionResult ShowAllPolicy()
        {
            var policies = dbContext.Policies.ToList();

            return View(policies);
        }


        public ActionResult AddPolicy()
        {
            return View();
        }

        // Action method to handle the form submission and add a new policy
        [HttpPost]
        public ActionResult AddPolicy(Policy newPolicy)
        {
            if (ModelState.IsValid)
            {
                // Add logic to save the new policy to the database
                dbContext.Policies.Add(newPolicy);
                dbContext.SaveChanges();

                // Redirect to a success view or another action
                return RedirectToAction("AddPolicySuccess");
            }

            // If the model is not valid, redisplay the form with validation errors
            return View(newPolicy);
        }

        // Action method to display a success message after adding a policy
        public ActionResult AddPolicySuccess()
        {
            return View();
        }



        public ActionResult UpdatePolicy(int id)
        {
            // Retrieve the policy from the database based on the provided id
            var policy = dbContext.Policies.Find(id);

            if (policy == null)
            {
                // Handle case where policy with given ID is not found
                return HttpNotFound();
            }

            return View(policy);
        }

        // Action method to handle the form submission and update an existing policy
        [HttpPost]
        public ActionResult UpdatePolicy(Policy updatedPolicy)
        {
            if (ModelState.IsValid)
            {
                // Attach the updated policy to the DbContext and mark it as modified
                dbContext.Entry(updatedPolicy).State = EntityState.Modified;
                dbContext.SaveChanges();

                // Redirect to a success view or another action
                return RedirectToAction("UpdatePolicySuccess");
            }

            // If the model is not valid, redisplay the form with validation errors
            return View(updatedPolicy);
        }

        // Action method to display a success message after updating a policy
        public ActionResult UpdatePolicySuccess()
        {
            return View();
        }
    }
}
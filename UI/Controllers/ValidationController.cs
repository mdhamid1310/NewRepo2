using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNetCore.Identity;

using DAL.Service;
using DAL;

using UI.Models;


namespace UILayer.Controllers
{
    public class ValidationController : Controller
    {
        private readonly InsuranceDbContext context;

        public ValidationController()
        {
            this.context = new InsuranceDbContext();
        }


        // GET: Validation
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(UserView user)
        {
            if (context.Admins.Any(a => a.Email == user.Email) || context.Customers.Any(e => e.Email == user.Email))
            {
                // Email already registered
                ModelState.AddModelError("Email", "Email already registered with us.");
                return View("Registration", user);
            }
            else if (context.Admins.Any(a => a.UserName == user.UserName) || context.Customers.Any(e => e.UserName == user.UserName))
            {
                // Username already registered
                ModelState.AddModelError("UserName", "Username already registered with us.");

                return View("Registration", user);
            }
            if (user.UserType == 2)
            {
                Customer employee = new Customer
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.UserType
                };
                var passwordHash = new PasswordHasher<Customer>();
                employee.Password = passwordHash.HashPassword(employee, user.Password);
                context.Customers.Add(employee);
                context.SaveChanges();

                return RedirectToAction("Index", "Customer");
            }
            else
            {
                Admin newadmin = new Admin
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.UserType
                };
                var passwordHash = new PasswordHasher<Admin>();
                newadmin.Password = passwordHash.HashPassword(newadmin, user.Password);
                context.Admins.Add(newadmin);
                context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
        }
        // GET: Account
        public ActionResult CustomerLogin()
        {


            return View();
        }

        public ActionResult AdminLogin()
        {


            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(LoginView loginView)
        {
            //var isAdmin = Authentication.VerifyAdminCredentials(loginView.UserName, loginView.Password);
             var isAdmin = AuthenticateAdmin(loginView.UserName, loginView.Password);

            if (isAdmin)
            {
                FormsAuthentication.SetAuthCookie(loginView.UserName, false);
                return RedirectToAction("Dashboard", "Admin");

            }
            else
            {
                // If authentication fails, you may want to show an error message.
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(loginView);
            }
        }
        private bool AuthenticateAdmin(string username, string password)
        {

            var admin = context.Admins.SingleOrDefault(a => a.UserName == username && a.Password == password);

            // Return true if an admin is found, otherwise false.
            return admin != null;
        }
        [HttpPost]
        public ActionResult CustomerLogin(LoginView loginView)
        {
            var isAdmin = Authentication.VerifyCustomerCredentials(loginView.UserName, loginView.Password);

            if (isAdmin)
            {
                var user = context.Customers.FirstOrDefault(x => x.UserName == loginView.UserName);
                Session["UserId"] = user.Id;
                Session["UserName"] = user.UserName;
                FormsAuthentication.SetAuthCookie(loginView.UserName, false);
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                // If authentication fails, you may want to show an error message.
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(loginView);
            }
        }
        public ActionResult ForgetPassword()
        {


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the username and email combination exists in Admins
                var admin = context.Admins.FirstOrDefault(a => a.UserName == model.Username && a.Email == model.Email);

                if (admin != null)
                {
                    // This is an admin
                    return ProcessForgetPasswordForAdmin(admin, model);
                }

                // Check if the username and email combination exists in Customers
                var customer = context.Customers.FirstOrDefault(c => c.UserName == model.Username && c.Email == model.Email);

                if (customer != null)
                {
                    // This is a customer
                    return ProcessForgetPasswordForCustomer(customer, model);
                }

                // Username and email combination not found
                ModelState.AddModelError(string.Empty, "Invalid username or email");
            }

            return View(model);
        }

        private ActionResult ProcessForgetPasswordForAdmin(Admin admin, ForgetPasswordViewModel model)
        {
            // Update the password to the new password
            var passwordHasher = new PasswordHasher<Admin>();
            var newPasswordHash = passwordHasher.HashPassword(admin, model.NewPassword);
            admin.Password = newPasswordHash;

            context.SaveChanges();

            TempData["SuccessMessage"] = "Password changed successfully!";

            // You might want to redirect to a success page or login page
            return RedirectToAction("AdminLogin", "Account");
        }

        private ActionResult ProcessForgetPasswordForCustomer(Customer customer, ForgetPasswordViewModel model)
        {
            // Update the password to the new password
            var passwordHasher = new PasswordHasher<Customer>();
            var newPasswordHash = passwordHasher.HashPassword(customer, model.NewPassword);
            customer.Password = newPasswordHash;

            context.SaveChanges();

            TempData["SuccessMessage"] = "Password changed successfully!";

            // You might want to redirect to a success page or login page
            return RedirectToAction("CustomerLogin", "Account");
        }
    }
}
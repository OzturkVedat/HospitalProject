using HospitalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace HospitalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public UserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ApplicationDbContext context){
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "A user with the same email address already exists.");
                return View(registerModel);
            }
            if (ModelState.IsValid)
            {
                var user = new Patient  
                {             
                    UserName = registerModel.Email,
                    Email = registerModel.Email,
                    Name = registerModel.Name,
                    Surname = registerModel.Surname,
                };
                var result = await _userManager.CreateAsync(user, registerModel.Password);    // create a user(patient)

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Patient"); // give role(Authorization)
                    Console.WriteLine("REGISTRATION SUCCESSFUL");
                    // Sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Creating a related patient record
                    _context.Patients.Add(user);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("User/patient saved to db");
                    return RedirectToAction("Login"); // redirect to login page
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(registerModel);     // if there's errors, return to view with error message
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    loginModel.Email,
                    loginModel.Password,
                    loginModel.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginModel.Email);
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("Admin"))        // Authentication
                    {
                        return RedirectToAction("AdminPanel");
                    }
                    else if (roles.Contains("Patient"))
                    {
                        return RedirectToAction("PatientPanel");
                    }
                    else if (roles.Contains("Doctor"))
                    {
                        return RedirectToAction("DoctorPanel");
                    }
                    Console.WriteLine("User is not authenticated..");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your username and password.");
                }
            }

            // If ModelState is not valid or there are errors, return to the login view with errors
            return View(loginModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return RedirectToAction("AdminPanel", "Admin");
        }
        [Authorize(Roles = "Patient")]
        public IActionResult PatientPanel()
        {
            return View();
        }
        [Authorize(Roles = "Doctor")]
        public IActionResult DoctorPanel()
        {
            return View();
        }
    }
}

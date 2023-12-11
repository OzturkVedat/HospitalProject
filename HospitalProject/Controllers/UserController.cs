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
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public UserController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager, ApplicationDbContext context)
        {
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
            if (ModelState.IsValid)
            {
                var user = new CustomUser
                {             // inherited from IdentityUser
                    UserName = registerModel.Email,
                    Email = registerModel.Email,
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    Password = registerModel.Password
                };
                var result = await _userManager.CreateAsync(user, user.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Patient");
                    Console.WriteLine("REGISTRATION SUCCESSFUL");
                    // Sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Creating a related patient record
                    var patient = new Patient
                    {
                        PatientFirstName = user.FirstName,
                        PatientLastName = user.LastName,
                        PatientId = user.Id,
                        Appointments = new List<Appointment>(),
                        MedicalRecords = new List<MedicalRecord>()
                    };
                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("User and patient saved to db");
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

                    if (roles.Contains("Admin"))
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
                    else if (roles.Contains("Scheduler"))
                    {
                        return RedirectToAction("SchedulerPanel");
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
        [Authorize(Roles = "Scheduler")]
        public IActionResult SchedulerPanel()
        {
            return View();
        }
        
    }
}

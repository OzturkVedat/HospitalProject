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
        private readonly ILogger<UserController> _logger;
        public UserController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
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
            try
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

                    var result = await _userManager.CreateAsync(user, registerModel.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Patient");
                        _context.Patients.Add(user);
                        await _context.SaveChangesAsync();
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }

                        ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during registration.");

                ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
            }
            return View(registerModel);
        }

        [HttpGet]
        [AllowAnonymous]
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
                        return RedirectToAction("AdminPanel","Admin");
                    }
                    else if (roles.Contains("Patient"))
                    {
                        return RedirectToAction("PatientPanel","Patient");
                    }
                    Console.WriteLine("User is not authenticated..");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your username and password.");
                }
            }
            return View(loginModel);
        }
    }
}

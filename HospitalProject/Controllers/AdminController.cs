using HospitalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace HospitalProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult LoadSection(string section)
        {
            if (section == "_section1")
            {
                var doctors = _context.Doctors.ToList();  // LINQ
                var viewModel = new AdminDocViewModel { Doctors = doctors };
                return View(section, viewModel);
            }
            else if (section == "_section2")
            {
                var departments = _context.Departments.ToList();
                var viewModel = new AdminDepViewModel { Departments = departments };
                return View(section, viewModel);
            }
            return View(section);
        }
        public IActionResult AdminPanel()
        {
            return RedirectToAction("LoadSection", new { section = "_section1" });
        }

        [HttpPost]
        public IActionResult DepartmentRegister(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the department already exists in the database
                    var departmentExist = _context.Departments.Any(d => d.DepartmentId == department.DepartmentId);

                    if (departmentExist)
                    {
                        var existingDepartment = _context.Departments.Find(department.DepartmentId);
                        _context.Entry(existingDepartment).CurrentValues.SetValues(department);
                        _context.Entry(existingDepartment).State = EntityState.Modified;
                        Console.WriteLine("department modified");
                    }
                    else
                    {
                        _context.Departments.Add(department);
                        Console.WriteLine("department added");
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the department. Please try again.");
                }
            }
            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        public IActionResult DepartmentRemove(int id)
        {
            var department = _context.Departments.Include(d => d.Doctors).FirstOrDefault(d => d.DepartmentId == id);

            if (department == null)
                return NotFound();
            try
            {
                if (department.Doctors != null && department.Doctors.Any())
                {
                    // Set the Doctors property to null for associated doctors
                    foreach (var doctor in department.Doctors)
                        doctor.Department = null;
                }
                _context.Departments.Remove(department);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while trying to remove the department.");
            }
            return RedirectToAction("AdminPanel");
        }


        [HttpPost]
        public IActionResult DoctorRegister(Doctor newDoctor)
        {
            if (ModelState.IsValid)
            {
                var department = _context.Departments.FirstOrDefault(d => d.DepartmentId == newDoctor.DepartmentId);
                if (department != null)
                    department.Doctors.Add(newDoctor);

                _context.Doctors.Add(newDoctor);
                _context.SaveChanges();
            }
            else
                Console.WriteLine("INVALID MODEL STATE");
            return RedirectToAction("LoadSection", new { section = "_section1" });
        }

        public IActionResult DoctorView(int id)
        {
            // Retrieve the doctor from the database
            var doctor = _context.Doctors.Find(id);

            if (doctor == null)
            {
                return NotFound();
            }

            // Perform any necessary logic for viewing
            // ...

            return View(doctor);
        }

        public IActionResult DoctorRemove(int id)
        {
            // Retrieve the doctor from the database
            var doctor = _context.Doctors.Find(id);

            if (doctor == null)
            {
                return NotFound();
            }

            // Perform any necessary logic for removal
            // ...

            _context.Doctors.Remove(doctor);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index)); // Redirect to the list of doctors after removal
        }
    }
}

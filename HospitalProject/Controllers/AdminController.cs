using HospitalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult AdminPanel()
        {
            var departments = _context.Departments.OrderBy(d => d.DepartmentName).ToList();

            var departmentSelectList = new SelectList(departments, "DepartmentId", "DepartmentName");
            ViewBag.AvailableDepartments = departmentSelectList;    // Viewbag for doctor registration

            var adminViewModel = new AdminViewModel
            {
                Doctors = _context.Doctors.OrderBy(d => d.Name).ToList(),
                Departments = _context.Departments.OrderBy(dep => dep.DepartmentName).ToList(),
                Patients = _context.Patients.OrderBy(p => p.Name).ToList(),
                Appointments = _context.Appointments.OrderBy(app => app.Id).ToList()
            };
            return View("~/Views/User/AdminPanel.cshtml", adminViewModel); // send the data to adminPanel
        }

        [HttpPost]
        public IActionResult DepartmentRegister(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Departments.Add(department);
                    _context.SaveChanges();
                    return RedirectToAction("AdminPanel", "User");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the department. Please try again.");
                    return RedirectToAction("AdminPanel");
                }
            }
            var dep = new AdminViewModel();
            dep.department = department;

            return View("~/Views/User/AdminPanel.cshtml", dep);      // invalid model
        }

        [HttpPost]
        public async Task<IActionResult> DoctorRegister(DocRegisterViewModel newDoctor)
        {
            if (ModelState.IsValid)
            {
                var user = new Doctor
                {
                    UserName = newDoctor.Email,
                    Email = newDoctor.Email,
                    Name = newDoctor.Name,
                    Surname = newDoctor.Surname,
                    DepartmentId = newDoctor.DepartmentId,
                    WorkingHours = newDoctor.WorkingHoursPerWeek
                };
                var result = await _userManager.CreateAsync(user, newDoctor.Password);    // create a user(doctor)
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Doctor"); // give the doctor role
                    var selectedDepartment = _context.Departments
                        .Where(d => d.DepartmentId == newDoctor.DepartmentId)
                        .FirstOrDefault();
                    if (selectedDepartment != null)
                    {
                        selectedDepartment.Doctors.Add(user);       // to the department's doctors collection
                        _context.Doctors.Add(user);
                        _context.SaveChanges();
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Selected department not found.");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                return RedirectToAction("AdminPanel", "User");      // if registered
            }
            var doc = new AdminViewModel();
            doc.doctor = newDoctor;
            return View("~/Views/User/AdminPanel.cshtml", doc); // invalid model
        }


        public IActionResult DoctorEdit(int id)
        {
            // Retrieve the doctor from the database
            var doctor = _context.Doctors.Find(id);

            if (doctor == null)
            {
                return NotFound();
            }

            // Perform any necessary logic for editing
            // ...

            return View(doctor);
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

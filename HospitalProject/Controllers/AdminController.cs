using HospitalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;

namespace HospitalProject.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult AdminPanel()
        {
            var departments = _context.Departments.OrderBy(d => d.DepartmentName).ToList();

            // Create a SelectList for the dropdown
            var departmentSelectList = new SelectList(departments, "DepartmentId", "DepartmentName");

            // Assign the SelectList to ViewBag
            ViewBag.AvailableDepartments = departmentSelectList;

            var adminViewModel = new AdminViewModel
            {
                Doctors = _context.Doctors.OrderBy(d => d.DoctorFirstName).ToList(),
                Departments = _context.Departments.OrderBy(dep => dep.DepartmentName).ToList(),
                Patients = _context.Patients.OrderBy(p => p.PatientFirstName).ToList(),
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
                    // Handle any exceptions that might occur during database interaction
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the department. Please try again.");
                    // Log the exception for debugging purposes if needed

                    // Return the view with the same model to display validation errors
                    return PartialView("~/Views/PartialViews/_DepartmentRegisterPartialView.cshtml", department);
                }
            }
            return PartialView("~/Views/PartialViews/_DepartmentRegisterPartialView.csthml", department);
        }

        [HttpPost]
        public IActionResult DoctorRegister(Doctor doctor)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the selected department from the database
                    var selectedDepartment = _context.Departments
                        .Where(d => d.DepartmentId == doctor.DepartmentId)
                        .FirstOrDefault();


                    if (selectedDepartment != null)
                    {
                        // Associate the doctor with the selected department
                        doctor.Department = selectedDepartment;

                        // Add the doctor to the department's Doctors collection
                        selectedDepartment.Doctors.Add(doctor);

                        // Add the doctor to the Doctors DbSet and save changes
                        _context.Doctors.Add(doctor);
                        _context.SaveChanges();

                        // Redirect to a success page or perform other actions
                        return RedirectToAction("AdminPanel", "User");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Selected department not found.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that might occur during database interaction
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the doctor. Please try again.");
                    // Log the exception for debugging purposes if needed

                    // Return the view with the same model to display validation errors
                    return View("~/Views/PartialViews/_DoctorRegisterPartialView.cshtml", doctor);
                }
            }

            // If the model is not valid, return the view with validation errors
            // This will happen if the user submits the form with invalid data
            return View("~/Views/PartialViews/_DoctorRegisterPartialView.cshtml", doctor);
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

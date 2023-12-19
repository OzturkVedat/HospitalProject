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
                if (_context.Doctors.Any(d => d.Name == null || d.Surname == null))
                    Console.WriteLine("DOCTORS TABLE CONTAINS NULL");

                else
                {
                    var viewModel = new AdminDocViewModel { 
                        Doctors = _context.Doctors.ToList(),
                        doctor=new Doctor(),
                    };
                    return View(section, viewModel);
                }
            }
            else if (section == "_section2")
            {
                if (_context.Departments.Any(d => d.DepartmentName == null || d.DepartmentId == null))
                    Console.WriteLine("DEPARTMENTS TABLE CONTAINS NULL");
                else
                {
                    var departments = _context.Departments.ToList();
                    var viewModel = new AdminDepViewModel { Departments = departments };
                    return View(section, viewModel);
                }
            }
            else if(section == "_section3")
            {
                if (_context.Patients.Any(p => p.Id == null || p.UserName == null))
                    Console.WriteLine("PATIENTS TABLE CONTAINS NULL");
                else
                {
                    var patients = _context.Patients.ToList();
                    return View(section, patients);
                }
            }
            else if (section == "_section4")
            {
                if(_context.Appointments.Any(a => a.Id == null || a.Date == null))
                    Console.WriteLine("APPOINTMENTS TABLE CONTAINS NULL");
                else
                {
                    var appointments = _context.Appointments.ToList();
                    return View(section, appointments);
                }
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
                return RedirectToAction("AdminPanel");
            }
            else
            {
                AdminDepViewModel viewModel = new AdminDepViewModel();
                viewModel.department = department;
                return View("~/Views/Admin/_section2.cshtml", viewModel);
            }
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
                    //foreach (var doctor in department.Doctors)
                    //    doctor.Department = null;
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
        [ValidateAntiForgeryToken]
        public IActionResult RegisterDoctor(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the department already exists in the database
                    var doctorExist = _context.Doctors.Any(d => d.DoctorId == doctor.DoctorId);

                    if (doctorExist)
                    {
                        var existingDoctor = _context.Doctors.Find(doctor.DoctorId);
                        _context.Entry(existingDoctor).CurrentValues.SetValues(doctor);
                        _context.Entry(existingDoctor).State = EntityState.Modified;
                        Console.WriteLine("doctor modified");
                    }
                    else
                    {
                        _context.Doctors.Add(doctor);
                        Console.WriteLine("doctor added");
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the doctor. Please try again.");
                }
                return RedirectToAction("AdminPanel");
            }
            else
            {
                AdminDocViewModel viewModel = new AdminDocViewModel();
                viewModel.doctor = doctor;
                return View("~/Views/Admin/_section1.cshtml", viewModel);
            }
        }

        [HttpPost]
        public IActionResult DoctorRemove(int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (doctor == null)
                return NotFound();
            
            _context.Doctors.Remove(doctor);
            _context.SaveChanges();
            return RedirectToAction("AdminPanel"); 
        }
    }
}

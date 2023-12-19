using HospitalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HospitalProject.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PatientController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult PatientPanel()
        {
            return RedirectToAction("LoadSection", new { section = "_section1" });
        }
        public async Task<IActionResult> LoadSection(string section)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
            if (section == "_section1")
            {
                var patient = _context.Patients
            .Include(p => p.Appointments) // Include the Appointments navigation property
            .FirstOrDefault(p => p.Id == userId);

                if (patient != null)
                {
                    var patientAppointments = patient.Appointments;
                    return View(section, patientAppointments);
                }
                else
                    return NotFound();

            }
            else if (section == "_section2")
            {
                var patient = _context.Patients.FirstOrDefault(p => p.Id == userId);
                if (patient != null)
                {
                    var ViewModel = new PatientViewModel
                    {
                        Departments = _context.Departments.ToList(),
                        Doctors = _context.Doctors.ToList(),
                        Patient = patient
                    };
                    return View(section, ViewModel);
                }
                else { Console.WriteLine("Couldn't fetch the data"); }
            }


            else if (section == "_section3")
            {
                var patient = _context.Patients.FirstOrDefault(p => p.Id == userId);
                if (patient != null)
                {
                    return View(section, patient);
                }
                else
                    return NotFound();
            }
            return View(section);
        }
    }
}

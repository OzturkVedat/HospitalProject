using HospitalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                var patient = await _context.Patients       // exlicitly loading
                  .Include(p => p.Appointments)
                      .ThenInclude(a => a.Doctor)
                  .Include(p => p.Appointments)
                      .ThenInclude(a => a.Department)
                  .FirstOrDefaultAsync(p => p.Id == userId);

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

                    var viewModel = new PatientViewModel
                    {
                        Patient = patient,
                        Appointment = new Appointment(),
                        Departments = _context.Departments.ToList() ?? new List<Department>(),
                        Doctors = _context.Doctors.ToList() ?? new List<Doctor>()
                    };
                    return View(section, viewModel);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Department department = _context.Departments.Find(appointment.DepartmentId);
                    Doctor doctor = _context.Doctors.Find(appointment.DoctorId);
                    Patient patient = _context.Patients.Find(appointment.PatientId);

                    if (department != null && doctor != null && patient != null)
                    {
                        appointment.Department = department;
                        appointment.Doctor = doctor;
                        appointment.Patient = patient;

                        // Add the appointment to the related collections
                        department.Appointments.Add(appointment);
                        doctor.Appointments.Add(appointment);
                        patient.Appointments.Add(appointment);

                        // Add the appointment to the main Appointments DbSet
                        _context.Appointments.Add(appointment);

                        // Save changes
                        _context.SaveChanges();

                        Console.WriteLine("Appointment added");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Department, Doctor, or Patient not found.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred while saving the appointment: {ex.Message}");
                    Console.WriteLine($"Error: {ex.Message}");
                }
                return RedirectToAction("PatientPanel");
            }
            else
            {
                PatientViewModel viewModel = new PatientViewModel();
                viewModel.Appointment = appointment;
                return View("_section2", viewModel);
            }
        }
        [HttpPost]
        public IActionResult CancelAppointment(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);

            if (appointment == null)
                return NotFound();
            try
            {
                if (appointment.Department != null && appointment.Doctor != null && appointment.Patient != null)
                {
                    appointment.Department = null; appointment.Patient=null;appointment.Doctor = null;
                    _context.SaveChanges();
                }
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while trying to remove the department.");
            }
            return View("_section1");
        }
    }
}

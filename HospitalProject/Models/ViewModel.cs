using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        [MaxLength(16)]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        [MaxLength(16)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class AdminDocViewModel
    {
        public Doctor doctor { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
    public class AdminDepViewModel
    {
        public Department department { get; set; }
        public List<Department> Departments { get; set; }
    }
    public class PatientViewModel
    {
        public Patient patient { get; set; }    
        public List<Patient> Patients { get; set; }
    }
    public class AppointmentViewModel
    {
        public Appointment appointment { get; set; }    
        public List<Appointment> Appointments { get; set; }

    }
}

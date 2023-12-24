using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "User Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        [MaxLength(16)]
        [Display(Name = "Your name")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        [MaxLength(16)]
        [Display(Name = "Your surname")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class AdminDocViewModel
    {
        public Doctor doctor { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Department>? Departments { get; set; } = new List<Department>();

    }
    public class AdminDepViewModel
    {
        public Department department { get; set; }
        public List<Department> Departments { get; set; }
    }
<<<<<<< Updated upstream

    public class PatientViewModel :Appointment         // for new appointments
=======
    public class PatientViewModel        // for new appointments
>>>>>>> Stashed changes
    {
        public Patient? Patient { get; set; }
        public Appointment? Appointment { get; set; }
        public List<Department>? Departments { get; set; } = new List<Department>();
        public List<Doctor>? Doctors { get; set; } = new List<Doctor>();
    }
}

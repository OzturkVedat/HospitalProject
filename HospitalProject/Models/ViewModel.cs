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
    public class DocRegisterViewModel: RegisterViewModel
    {
        public int DepartmentId { get; set; }
        public ICollection <WorkingHour> WorkingHoursPerWeek { get; set; }
        public bool IsValid { get; set; } // Indicates whether the model is valid
        public List<string> Errors { get; set; } // List to store error messages

        public DocRegisterViewModel()
        {
            IsValid = true; // Initialize as valid by default
            Errors = new List<string>();
        }
    }

    public class AdminViewModel
    {
        public DocRegisterViewModel doctor { get; set; }
        public List<Doctor> Doctors { get; set; }
        public Department department { get; set; }
        public List<Department> Departments { get; set; }
        public List<Patient> Patients { get; set; }
        public List<Appointment>Appointments { get; set; }
    }
    public class PatientViewModel
    {

    }
    public class DoctorViewModel
    {

    }
}

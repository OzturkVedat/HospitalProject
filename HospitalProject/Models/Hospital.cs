using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string PhoneNumber {  get; set; }
        public ICollection<Doctor>? Doctors { get; set; }

    }

    public class Doctor
    {
       // [RegularExpression(@"^\d{11}$", ErrorMessage = "Doctor's Id must be 11 digits.")]
        public int DoctorId { get; set; }
        public Department? Department { get; set; }     // navigation property (one-to-many relation)
        public int? DepartmentId { get; set; }       // foreign key

        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "docName must contain only letters.")]
        public string DoctorFirstName { get; set; }

        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "docSurname must contain only letters.")]
        public string DoctorLastName { get; set; }


        public string FullName => $"{DoctorFirstName} {DoctorLastName}";

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        public ICollection<WorkingHour> WorkingHours { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

    }


    public class WorkingHour
    {
        public int Id { get; set; }
        public DateTime WorkHour { get; set; }
        public Doctor Doctor { get; set; }
        public int DoctorId {  set; get; }
    }

}

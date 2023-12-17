using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{
   public class Doctor      // inherits IdentityUser
    {
        public int? DoctorId {  get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? FullName => $"{Name} {Surname}";
        public Department? Department { get; set; }     // navigation property (one-to-many relation)
        public int? DepartmentId { get; set; }       // foreign key
        public TimeSpan? StartHour { get; set; }
        public TimeSpan? EndHour { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }

}

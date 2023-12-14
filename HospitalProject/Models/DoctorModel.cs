using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{
   public class Doctor: ApplicationUser      // inherits IdentityUser
    {
        public Department? Department { get; set; }     // navigation property (one-to-many relation)
        public int? DepartmentId { get; set; }       // foreign key
        public ICollection<WorkingHour>? WorkingHours { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }

    public class WorkingHour
    {
        public int Id { get; set; }
        public DateTime WorkHour { get; set; }
        public Doctor Doctor { get; set; }
        public string DoctorId {  set; get; }
    }

}

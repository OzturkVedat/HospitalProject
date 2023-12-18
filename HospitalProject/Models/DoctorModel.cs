using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{
   public class Doctor     
    {
        public int? DoctorId {  get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Department? Department { get; set; }     // navigation property (one-to-many relation)
        public int? DepartmentId { get; set; }       // foreign key
        public TimeSpan? StartHour { get; set; }
        public TimeSpan? EndHour { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}

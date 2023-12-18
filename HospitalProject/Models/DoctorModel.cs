using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{
   public class Doctor     
    {
        public int DoctorId {  get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Department? Department { get; set; }     
        public int? DepartmentId { get; set; }    
        public int? StartHour { get; set; }
        public int? EndHour { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}

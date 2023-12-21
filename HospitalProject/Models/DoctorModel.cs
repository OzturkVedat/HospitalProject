using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{
   public class Doctor     
    {
        [Key]
        public int? DoctorId {  get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName => $"{Name} {Surname}";
        [DataType(DataType.Time)]
        [Display(Name = "Start Hour")]
        public DateTime? StartHour { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "End Hour")]
        public DateTime? EndHour { get; set; }
        public string Shift => $"{StartHour?.ToString("HH:mm")} - {EndHour?.ToString("HH:mm")}";

        public int? DepartmentId {  get; set; }
        public Department? Department { get; set; }

        public ICollection <Appointment>? Appointments { get; set; } = new List<Appointment>();
    }
}

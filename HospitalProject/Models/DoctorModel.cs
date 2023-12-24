using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{
   public class Doctor     
    {
<<<<<<< HEAD
<<<<<<< Updated upstream
        public int DoctorId { get; set; }
=======
        [Key]
        public int? DoctorId { get; set; }
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name must contain only letters.")]
>>>>>>> Stashed changes
=======
        [Key]
        public int? DoctorId {  get; set; }
>>>>>>> 70bb60e2bba537100c2b20f3ee40149dd90b72a1
        public string Name { get; set; }
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Surname must contain only letters.")]
        public string Surname { get; set; }
        public string FullName => $"{Name} {Surname}";
<<<<<<< HEAD
<<<<<<< Updated upstream

=======
        private DateTime? startHour;
>>>>>>> Stashed changes
=======
>>>>>>> 70bb60e2bba537100c2b20f3ee40149dd90b72a1
        [DataType(DataType.Time)]
        [Display(Name = "Start Hour")]
        public DateTime? StartHour
        {
            get => startHour;
            set
            {
                startHour = value;
                UpdateShift();
            }
        }

        private DateTime? endHour;
        [DataType(DataType.Time)]
        [Display(Name = "End Hour")]
<<<<<<< Updated upstream
        public DateTime? EndHour { get; set; }
        public string Shift => $"{StartHour?.ToString("HH:mm")} - {EndHour?.ToString("HH:mm")}";

        public int? DepartmentId {  get; set; }
        public Department? Department { get; set; }
<<<<<<< HEAD
        public int? DepartmentId { get; set; }
        public int? StartHour { get; set; }
        public int? EndHour { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
=======
        public DateTime? EndHour
        {
            get => endHour;
            set
            {
                endHour = value;
                UpdateShift();
            }
        }

        public string Shift { get; private set; }

        public void UpdateShift()
        {
            Shift = $"{StartHour?.ToString("HH:mm")} - {EndHour?.ToString("HH:mm")}";
        }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
>>>>>>> Stashed changes
=======

        public ICollection <Appointment>? Appointments { get; set; } = new List<Appointment>();
>>>>>>> 70bb60e2bba537100c2b20f3ee40149dd90b72a1
    }
}

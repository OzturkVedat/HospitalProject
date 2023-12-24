using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{
    public class Doctor
    {
<<<<<<< Updated upstream
        public int DoctorId { get; set; }
=======
        [Key]
        public int? DoctorId { get; set; }
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name must contain only letters.")]
>>>>>>> Stashed changes
        public string Name { get; set; }
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Surname must contain only letters.")]
        public string Surname { get; set; }

        // Keep the changes from addedPatientPanel branch
        public string FullName => $"{Name} {Surname}";
<<<<<<< Updated upstream

=======
        private DateTime? startHour;
>>>>>>> Stashed changes
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

        // Keep the changes from master branch
        public Department? Department { get; set; }
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
    }
}

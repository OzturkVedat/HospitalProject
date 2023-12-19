using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        // Keep the changes from addedPatientPanel branch
        public string FullName => $"{Name} {Surname}";

        [DataType(DataType.Time)]
        [Display(Name = "Start Hour")]
        public DateTime? StartHour { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "End Hour")]
        public DateTime? EndHour { get; set; }

        // Keep the changes from master branch
        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }
        public int? StartHour { get; set; }
        public int? EndHour { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}

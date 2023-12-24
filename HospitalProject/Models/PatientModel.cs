using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HospitalProject.Models
{

    public class Patient : ApplicationUser   // inherits IdentityUser
    {
        public ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
    }

    
}

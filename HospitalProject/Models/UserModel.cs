using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{
    public class ApplicationUser: IdentityUser
    {
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; }

        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Surname must contain only letters.")]
        public string Surname { get; set; }
        public string FullName => $"{Name} {Surname}";

        // email, password, etc from identity
    }
}

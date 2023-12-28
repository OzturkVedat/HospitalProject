using Microsoft.AspNetCore.Mvc;
using HospitalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HospitalProject.Controllers
{
    [Produces("application/json")]
    [Route("api/doctor")]
    [ApiController]
    public class DoctorAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public DoctorAPIController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/<DoctorAPIController>
        [HttpGet]
        public IEnumerable<Doctor> Get()
        {
            return _context.Doctors.ToList();
        }

        // GET api/<DoctorAPIController>/5
        [HttpGet("{id}")]
        public Doctor Get(int id)
        {
            return _context.Doctors.FirstOrDefault(item => item.DoctorId == id);
        }

    }
}

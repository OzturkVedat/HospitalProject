namespace HospitalProject.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Doctor Doctor { get; set; }
        public int? DoctorId {  get; set; }   // foreign key
        public Department Department { get; set; }
        public int? DepartmentId { get; set; }       // foreign key
        public Patient Patient { get; set; }
        public string PatientId { get; set; }   // foreign key

    }
}

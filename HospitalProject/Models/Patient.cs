namespace HospitalProject.Models
{
    public class Patient
    {
        public string PatientId { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Doctor Doctor { get; set; }
        public Department Department { get; set; }
        public Patient Patient { get; set; }
    }
    public class MedicalRecord
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }

        public ICollection<Allergy> Allergies { get; set; }
        public ICollection<Medication> Medications { get; set; }
        public ICollection<Diagnosis> Diagnoses { get; set; }
    }

    public class Allergy
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Foreign key to MedicalRecord
        public int MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
    }

    public class Medication
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
    }

    public class Diagnosis
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
    }
}

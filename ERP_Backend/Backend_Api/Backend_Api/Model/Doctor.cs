namespace Backend_Api.Model
{
    public class Doctor
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; }
        public DateTime HireDate { get; set; }
    }
}

namespace Backend_Api.Model
{
    public class PatientRegistration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class PatientFollowupRequest
    {
        public string MobileNumber { get; set; }
    }
}

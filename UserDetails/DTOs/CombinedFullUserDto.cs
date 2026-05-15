namespace UserDetails.DTOs
{
    public class CombinedFullUserDto
    {
        public Guid UserId { get; set; }

        // Service1
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string EmploymentID { get; set; }
        public string PanCard { get; set; }

        // UserDetails
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Field { get; set; }

        // Employment
        public DateTime DOJ { get; set; }
        public string AadharCard { get; set; }
    }
}
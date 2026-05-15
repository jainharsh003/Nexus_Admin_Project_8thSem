namespace UserDetails.DTOs
{
    public class AuthUserDto
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        public string EmploymentID { get; set; }

        public string PanCard { get; set; }
    }
}
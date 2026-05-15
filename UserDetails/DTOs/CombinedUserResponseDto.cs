namespace UserDetails.DTOs
{
    public class CombinedUserResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }

        public string Name { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Field { get; set; }
    }
}
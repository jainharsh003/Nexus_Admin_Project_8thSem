namespace UserDetails.DTOs
{
    public class CreateUserDetailsDto
    {
        public string Name { get; set; } = null!;
        public string FatherName { get; set; } = null!;
        public string MotherName { get; set; } = null!;
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public string Field { get; set; } = null!;
    }
}
namespace UserDetails.Models
{
    public class UserDetailsEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }

        public string Name { get; set; } = null!;
        public string FatherName { get; set; } = null!;
        public string MotherName { get; set; } = null!;

        public DateTime DOB { get; set; }
        public int Age { get; set; }

        public string Gender { get; set; } = null!;
        public string Field { get; set; } = null!;
    }
}

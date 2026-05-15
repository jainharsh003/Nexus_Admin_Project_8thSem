namespace UserDetails.DTOs
{
    public class CreateEmploymentDto
    {
        public DateTime DOJ { get; set; }

        public string EmploymentID { get; set; }

        public string PanCard { get; set; }

        public string? AadharCard { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace UserDetails.Models
{
    public class EmploymentDetails
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime DOJ { get; set; }

        public string EmploymentID { get; set; }

        public string PanCard { get; set; }

        public string AadharCard { get; set; }
    }
}

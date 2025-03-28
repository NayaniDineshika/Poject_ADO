using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_ado.Models

{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }
        [DisplayName("Date Of Birth")]
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Salary")]
        [Required]
        public double Salary { get; set; }
        [NotMapped]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

    }
}

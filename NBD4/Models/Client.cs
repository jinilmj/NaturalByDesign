using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
    public class Client
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Client Name is required.")]
        [StringLength(100, ErrorMessage = "Client Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "Phone")]
        public string PhoneFormatted
        {
            get
            {
                return "(" + Phone.Substring(0, 3) + ") " + Phone.Substring(3, 3) + "-" + Phone[6..];
            }
        }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number (no spaces).")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string Phone { get; set; }

        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();

    }
}

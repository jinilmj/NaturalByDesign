using System.ComponentModel.DataAnnotations;
using System.IO;

namespace NBD4.Models
{
    public class Staff
    {
       
        public int ID { get; set; }
        [Display(Name = "Contact")]
        public string FullName
        {
            get
            {
                return StaffFirstName
                    + (string.IsNullOrEmpty(StaffMiddleName) ? " " :
                        (" " + (char?)StaffMiddleName[0] + ". ").ToUpper())
                    + StaffLastName;
            }
        }
        [Display(Name = "Phone")]
        public string PhoneFormatted
        {
            get
            {
                return "(" + Phone.Substring(0, 3) + ") " + Phone.Substring(3, 3) + "-" + Phone[6..];
            }
        }


        [Display(Name = "Staff First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters long.")]
        public string StaffFirstName { get; set; }

        [Display(Name = "Staff Name")]
        [StringLength(50, ErrorMessage = "Middle name cannot be more than 50 characters long.")]
        public string StaffMiddleName { get; set; }

        [Display(Name = "Staff LastName")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(100, ErrorMessage = "Last name cannot be more than 100 characters long.")]
        public string StaffLastName { get; set; }

        [Required(ErrorMessage = "Email Address is required.e.g. johndoe@example.com")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
        + "@"
        + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
        ErrorMessage = "Enter a valid email address.e.g. johndoe@example.com")]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
       
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number (no spaces).")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "You must select a Staff Role.")]
        [Display(Name = "Staff Role")]
        public int StaffRoleID { get; set; }

        [Display(Name = "Staff Role")]
        public StaffRole StaffRole { get; set; }


        public ICollection<BidStaff>BidStaffs { get; set; } = new HashSet<BidStaff>();
    }
}

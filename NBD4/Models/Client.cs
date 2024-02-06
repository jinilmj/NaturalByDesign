using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace NBD4.Models
{
    public class Client
    {
        public int ID { get; set; }

        [Display(Name = "Contact")]
        public string FullName
        {
            get
            {
                return ContactFirstName
                    + (string.IsNullOrEmpty(ContactMiddleName) ? " " :
                        (" " + (char?)ContactMiddleName[0] + ". ").ToUpper())
                    + ContactLastName;
            }
        }


        [Display(Name = "Client Name")]
        [Required(ErrorMessage = "You cannot leave the Client Name blank.")]
        [StringLength(100, ErrorMessage = "Client Name cannot exceed 100 characters.")]
        public string Name { get; set; }


        [Display(Name = "Contact First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters long.")]
        public string ContactFirstName { get; set; }

        [Display(Name = "Contact Middle Name")]
        [StringLength(50, ErrorMessage = "Middle name cannot be more than 50 characters long.")]
        public string ContactMiddleName { get; set; }

        [Display(Name = "Contact LastName")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(100, ErrorMessage = "Last name cannot be more than 100 characters long.")]
        public string ContactLastName { get; set; }

        [Required(ErrorMessage = "Email Address is required.e.g. johndoe@example.com")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
        + "@"
        + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
        ErrorMessage = "Enter a valid email address.e.g. johndoe@example.com")]
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

        public string PostalCodeFormatted
        {
            get
            {
                string formattedPostalCode = PostalCode.ToUpper(); 
                return formattedPostalCode.Substring(0, 3) + " " + formattedPostalCode[3..];
            }
        }

        [Display(Name = "Address")]
        public string Summary
        {
            get
            {
                return $"{Street}, {City?.Summary}, {PostalCodeFormatted}";
            }
        }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number (no spaces).")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        [MaxLength(255, ErrorMessage = "Street cannot exceed 255 characters.")]
        public string Street { get; set; }


        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "Postal Code is required.e.g. A1B1C1")]
        [RegularExpression(@"^[a-zA-Z]\d[a-zA-Z]\d[a-zA-Z]\d$",
         ErrorMessage = "Enter Postal Code In the correct format.e.g. A1B1C1 (No Spaces)")]
     
        public string PostalCode { get; set; }

        [Display(Name = "City")]
        [Range(1, int.MaxValue)]
        public int CityID { get; set; }
        public City City { get; set; }

        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();

    }
}

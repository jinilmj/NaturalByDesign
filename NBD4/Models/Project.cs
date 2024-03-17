using System.ComponentModel.DataAnnotations;
using System;

namespace NBD4.Models
{
    public class Project :Auditable,IValidatableObject
    {
        public int ID { get; set; }

        [Display(Name = "Est.Start Date")]
        [Required(ErrorMessage = "Estimated Start Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }


        [Display(Name = "Est.Completion Date")]
        [Required(ErrorMessage = "Estimated Completion Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }


        [Display(Name = "Project Site")]
        [Required(ErrorMessage = "Site Name is required.")]
        [StringLength(150, ErrorMessage = "Site name cannot exceed 150 characters.")]
        public string Site { get; set; }

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Amount is required.")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; } = 0d;

        [Required(ErrorMessage = "You must select a Client.")]
        [Display(Name = "Client")]
        public int ClientID { get; set; }

        [Display(Name = "Client")]
        public Client Client { get; set; }

        public ICollection<Bid> Bids { get; set; } = new HashSet<Bid>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult("Project cannot end before it starts.", new[] { "EndDate" });
            }
        }
    }
}

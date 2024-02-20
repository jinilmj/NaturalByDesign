using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
    public class Bid : IValidatableObject
    {
        public int ID { get; set; }

        [Display(Name = "Bid Date")]
        [Required(ErrorMessage = "Bid Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BidDate { get; set; }

        [Display(Name = "Bid Amount")]
        [Required(ErrorMessage = "Bid Amount is required.")]
        public decimal BidAmount { get; set; }

        [Display(Name = "NBD Approved")]
        public bool BidNBDApproved { get; set; }

        [Display(Name = "NBD Approval Notes")]
        public string BidNBDApprovalNotes { get; set; }

        [Display(Name = "Client Approved")]
        public bool BidClientApproved { get; set; }

        [Display(Name = "Client Approval Notes")]
        public string BidClientApprovalNotes { get; set; }

        [Display(Name = "Mark for Redesign/Review")]
        public bool BidMarkForRedesign { get; set; }

        [Display(Name = "Review Date")]
        public DateTime? ReviewDate { get; set; }

        [Display(Name = "Reviewed By")]
        public string ReviewedBy { get; set; }

        // Relationship with Project
        [Required(ErrorMessage = "You must select a Project.")]
        [Display(Name = "Project")]
        public int ProjectID { get; set; }

        [Display(Name = "Project")]
        public Project Project { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Your custom validation logic, if needed
            // For example, ensuring that BidAmount is positive, etc.

            return new List<ValidationResult>();
        }
    }
}

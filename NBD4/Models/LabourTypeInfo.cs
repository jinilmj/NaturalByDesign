using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
    public class LabourTypeInfo
    {
        //[Display(Name = "Four Letter Labour Code")]
        //[Required(ErrorMessage = "You cannot leave the Labour code blank.")]
        //[RegularExpression("^[A-Z]{4}$", ErrorMessage = "Please enter four capital letters for the Labour Code.")]
        //public string ID { get; set; }
      
        public string ID { get; set; }

        [Required(ErrorMessage = "You must Enter Labour Type Name")]
        [Display(Name = "Labour Type Name")]
        [StringLength(50, ErrorMessage = "Labour Type Name cannot be more than 50 characters long.")]
        public string LabourTypeName { get; set; }

        [Required(ErrorMessage = "You must enter an amount for the Labour Price Per Hour.")]
        [Display(Name = "Labour Price/Hour")]
        [DataType(DataType.Currency)]
        public double PricePerHour { get; set; }

        [Required(ErrorMessage = "You must enter an amount for the Labour Cost Per Hour.")]
        [Display(Name = "Labour Cost/Hour")]
        [DataType(DataType.Currency)]
        public double CostPerHour {  get; set; }

        public ICollection<Labour> Labours { get; set; } = new HashSet<Labour>();
    }
}

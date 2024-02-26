using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
    public class Labour
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the labour Hours Blank")]
        [Display(Name = "Labour Hours")]
        public int Hours { get; set; }

        [Required(ErrorMessage = "You must select a Labour type.")]
        [Display(Name = "Labour Type")]
        public int LabourTypeInfoID { get; set; }

        [Display(Name = "Labour Type")]
        public LabourTypeInfo LabourTypeInfo { get; set; }

        public double LabourCharge { get; set; }
        public int BidID { get; set; }

        public Bid Bid { get; set; }

        public void CalculateExtendPrice()
        {
            LabourCharge = Math.Round(Hours * LabourTypeInfo.PricePerHour,2);
        }


    }
}

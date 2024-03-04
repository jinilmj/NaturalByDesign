using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
	public class BidLabourTypeInfo
	{
		public int BidID { get; set; }
		public Bid Bid { get; set; }

		public int LabourTypeInfoID { get; set; }
		public LabourTypeInfo LabourTypeInfo { get; set; }

		[Required(ErrorMessage = "You cannot leave the labour Hours Blank")]
		[Display(Name = "Labour Hours")]
        private int _hours;
        public int Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                CalculateLabourCharge();
            }
        }

        public double LabourCharge { get; set; }

        public void CalculateLabourCharge()
        {
            if (LabourTypeInfo != null && Hours > 0)
            {
                LabourCharge = Math.Round(Hours * LabourTypeInfo.PricePerHour, 2);
            }
            else
            {
                LabourCharge = 0; // Default value if Hours or LabourTypeInfo is null
            }
        }


    }
}

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
		public int Hours { get; set; }

		public double LabourCharge { get; set; }

		public void CalculateLabourCharge()
		{
			LabourCharge = Math.Round(Hours * LabourTypeInfo.PricePerHour, 2);
		}
	}
}

using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
    public class BidInventory
    {
		public int BidID { get; set; }
		public Bid Bid { get; set; }
		public string InventoryID { get; set; }
		public Inventory Inventory { get; set; }

		[Display(Name = "Material Quantity")]
        public int MaterialQuantity { get; set; }

        [Display(Name = "Material Extend Price")]
        public double MaterialExtendPrice { get; set; }

        public void CalculateExtendPrice()
        {
            MaterialExtendPrice = Math.Round(MaterialQuantity * Inventory.ListCost,2);
        }
    }
}

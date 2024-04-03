using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
    public class BidInventory
    {
		public int BidID { get; set; }
		public Bid Bid { get; set; }
		public int InventoryID { get; set; }
		public Inventory Inventory { get; set; }

        [Required(ErrorMessage = "You cannot leave the labour Hours Blank")]
        [Display(Name = "Labour Hours")]
        private int _materialQuantity;

        [Display(Name = "Material Quantity")]
        public int MaterialQuantity 
        {
            get { return _materialQuantity; }
            set
            {
                _materialQuantity = value;
                CalculateExtendPrice();
            }
        }

       // [Display(Name = "Material Extend Price")]
        public double MaterialExtendPrice { get; set; }

        public void CalculateExtendPrice()
        {
            if (Inventory != null && MaterialQuantity > 0)
            {
                MaterialExtendPrice = Math.Round(MaterialQuantity * Inventory.ListCost, 2);
            }
            else
            {
               MaterialExtendPrice = 0; 
            }
            
        }
    }
}

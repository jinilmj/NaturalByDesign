using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
    public class Material
    {
        public int ID { get; set; }


        [Required(ErrorMessage = "You must select a Material.")]   //inventory item give inventory description in the field
        [Display(Name = "Material")]
        public int InventoryID { get; set; }

        [Display(Name = "Material")]
        public Inventory Inventory { get; set; }

        [Display(Name = "Material Quantity")]
        public int MaterialQuantity { get; set; }

        [Display(Name = "Material Extend Price")]
        public double MaterialExtendPrice { get; private set; } = 0; 

        public int BidID { get; set; }
        public Bid Bid { get; set; }


        public void CalculateExtendPrice()
        {
            MaterialExtendPrice = MaterialQuantity * Inventory.ListCost;
        }
    }
}

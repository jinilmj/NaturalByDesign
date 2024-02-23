using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
    public class Material
    {
        public int ID { get; set; }


        [Display(Name = "Material Quantity")]
        public int MaterialQuantity { get; set; }

        [Display(Name = "Material Extend Price")]
        public double MaterialExtendPrice { get; private set; } = 0; 

        public int BidID { get; set; }
        public Bid Bid { get; set; }


        [Required(ErrorMessage = "You must select a Material.")]   //inventory item give inventory description in the field
        [Display(Name = "Inventory Item")]
        public string InventoryID { get; set; }

        [Display(Name = "Inventory Item")]
        public Inventory Inventory { get; set; }


        public void CalculateExtendPrice()
        {
            MaterialExtendPrice = MaterialQuantity * Inventory.ListCost;
        }
    }
}

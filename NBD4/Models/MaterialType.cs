using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
    public class MaterialType
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "You must Enter Material Type Name")]
        [Display(Name = "Material Type Name")]
        [StringLength(50, ErrorMessage = "Material Type Name cannot be more than 50 characters long.")]
        public string MaterialTypeName { get; set; }

        public ICollection<Inventory> Inventories { get; set; } = new HashSet<Inventory>();




    }
}

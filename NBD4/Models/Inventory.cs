﻿using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
    public class Inventory
    {
        public int ID { get; set; } 

        [Display(Name = "Four Letter Inventory Code")]
        [Required(ErrorMessage = "You cannot leave the Inventory code blank.")]
        [RegularExpression("^[A-Za-z0-9]{4,7}$", ErrorMessage = "Please enter a code Between 4-7 You can use Letters and Numbers.")]
        public string Code { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "You must enter a description.")]
        [StringLength(255, ErrorMessage = "Description cannot be more than 255 characters long.")]
        public string Description { get; set; }

        [Display(Name = "Size")]
        [Required(ErrorMessage = "You must enter a size.")]
        [StringLength(15, ErrorMessage = "Size cannot be more than 15 characters long.")]
        public string Size { get; set; }

        [Display(Name = "List Cost")]
        [Required(ErrorMessage = "You must enter a list cost.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "List cost must be a positive number.")]
        [DataType(DataType.Currency)]
        public double ListCost { get; set; }

        [Required(ErrorMessage = "You must select a Material type.")]
        [Display(Name = "Material Type")]
        public int MaterialTypeID { get; set; }

        [Display(Name = "Material Type")]
        public MaterialType MaterialType { get; set; }

        public ICollection<BidInventory> BidInventories { get; set; } = new HashSet<BidInventory>();


    }
}

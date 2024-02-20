using System.ComponentModel.DataAnnotations;

namespace NBD4.Models
{
    public class StaffRole
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "You must Enter Staff  Name")]
        [Display(Name = "Staff Role")]
        [StringLength(50, ErrorMessage = "Staff Role cannot be more than 50 characters long.")]
        public string StaffRoleName { get; set; }

        public ICollection<Staff> Staffs { get; set; } = new HashSet<Staff>();

    }
}

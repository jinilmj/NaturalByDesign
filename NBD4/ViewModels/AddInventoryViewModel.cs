using NBD4.Models;

namespace NBD4.ViewModels
{
    public class AddInventoryViewModel
    {
        public int BidId { get; set; }
        public List<Inventory> Inventories { get; set; }
        public List<int> SelectedInventoryIds { get; set; }
        public List<int> Quantities { get; set; }
    }
}

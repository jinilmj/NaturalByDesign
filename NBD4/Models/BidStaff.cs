namespace NBD4.Models
{
    public class BidStaff
    {
        public int StaffID { get; set; }
        public Staff Staff { get; set; }

        public int BidID { get; set; }
        public Bid Bid { get; set; }
    }
}

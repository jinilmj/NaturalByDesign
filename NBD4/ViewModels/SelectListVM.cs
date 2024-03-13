namespace NBD4.ViewModels
{
    public class SelectListVM
    {
        public int ID { get; set; }
        public string DisplayText { get; set; }
        public bool Assigned { get; set; }    
        public int Quantities { get; set; }
        public double MaterialExtendPrice { get; set; }
        public int MaterialTypeID { get; set; }
    }
}

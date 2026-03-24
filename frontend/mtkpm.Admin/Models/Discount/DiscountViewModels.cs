namespace mtkpm.Admin.Models.Discount
{
    public class DiscountViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal DiscountAmount { get; set; }
        public int DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUses { get; set; }
        public int CurrentUses { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateDiscountViewModel
    {
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal DiscountAmount { get; set; }
        public int DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUses { get; set; }
    }

    public class UpdateDiscountViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
        public DateTime EndDate { get; set; }
        public int MaxUses { get; set; }
        public bool IsActive { get; set; }
    }
}

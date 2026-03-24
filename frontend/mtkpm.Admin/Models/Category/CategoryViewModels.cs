namespace mtkpm.Admin.Models.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int ProductCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateCategoryViewModel
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
    }

    public class UpdateCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
    }
}

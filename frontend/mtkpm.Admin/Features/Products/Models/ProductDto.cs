namespace mtkpm.Admin.Features.Products.Models
{
    /// <summary>
    /// Product list item DTO
    /// </summary>
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// Product detail DTO
    /// </summary>
    public class ProductDetailDto : ProductDto
    {
        public decimal CostPrice { get; set; }
        public double Discount { get; set; }
        public int Views { get; set; }
        public int Purchases { get; set; }
        public double Rating { get; set; }
    }
}

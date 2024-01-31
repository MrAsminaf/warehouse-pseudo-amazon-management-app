namespace Warehouse.Application.Models;

public class CreateProductModel
{
    public int? CartId { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public int? Count { get; set; }
    public decimal? Price { get; set; }
    public string? Dimensions { get; set; }
    public string? Location { get; set; }
}
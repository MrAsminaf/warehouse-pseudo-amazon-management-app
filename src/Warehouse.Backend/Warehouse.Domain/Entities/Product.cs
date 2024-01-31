namespace Warehouse.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public int? CartId { get; set; }
    public Cart? Cart { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public int? Count { get; set; }
    public decimal? Price { get; set; }
    public string? Dimensions { get; set; }
    public string? Location { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
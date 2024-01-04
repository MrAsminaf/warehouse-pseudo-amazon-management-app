namespace Warehouse.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public ICollection<Product> Products = new List<Product>();
}
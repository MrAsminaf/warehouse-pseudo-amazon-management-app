namespace Warehouse.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int? WorkerId { get; set; }
    public Worker? Worker { get; set; }
    public string? Status { get; set; }
    public string? Comment { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
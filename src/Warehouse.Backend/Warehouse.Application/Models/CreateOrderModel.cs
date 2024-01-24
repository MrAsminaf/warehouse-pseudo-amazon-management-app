namespace Warehouse.Application.Models;

public class CreateOrderModel
{
    public int? WorkerId { get; set; }
    public string? Status { get; set; }
    public string? Comment { get; set; }
}
namespace Warehouse.Application.Models;

public class ReturnOrderModel
{
    public int Id { get; set; }
    public int? WorkerId { get; set; }
    public string? Status { get; set; }
    public string? Comment { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
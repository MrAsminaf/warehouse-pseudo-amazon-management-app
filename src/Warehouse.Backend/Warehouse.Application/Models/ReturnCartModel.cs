namespace Warehouse.Application.Models;

public class ReturnCartModel
{
    public int Id { get; set; }
    public int? ClientId { get; set; }
    public ICollection<ReturnProductModel> Products { get; set; } = new List<ReturnProductModel>();
}
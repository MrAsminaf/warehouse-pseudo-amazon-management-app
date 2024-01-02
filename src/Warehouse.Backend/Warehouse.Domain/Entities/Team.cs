namespace Warehouse.Domain.Entities;

public class Team
{
    public int Id { get; set; }
    public int? ManagerId { get; set; }
    public Worker? Manager { get; set; }
    public string? Shift { get; set; }
}
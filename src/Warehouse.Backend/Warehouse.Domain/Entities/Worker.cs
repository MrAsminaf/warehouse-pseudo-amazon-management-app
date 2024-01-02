namespace Warehouse.Domain.Entities;

public class Worker
{
    public int? Id { get; set; }
    public ICollection<Team> ManagedTeams { get; set; } = new List<Team>();
    public string? Position { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public DateTime? CreatedAt { get; set; }
}
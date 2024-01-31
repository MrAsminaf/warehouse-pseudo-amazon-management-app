namespace Warehouse.Domain.Entities;

public class Worker : ApplicationUser
{
    public int? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    
    public ICollection<Team> ManagedTeams { get; set; } = new List<Team>();
    public string? Position { get; set; }
}
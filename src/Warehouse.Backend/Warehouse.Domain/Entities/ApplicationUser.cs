using Microsoft.AspNetCore.Identity;

namespace Warehouse.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime? CreatedAt { get; set; }
    
    public Worker? Worker { get; set; }
    public Client? Client { get; set; }
}
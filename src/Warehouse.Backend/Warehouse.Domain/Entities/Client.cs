namespace Warehouse.Domain.Entities;

public class Client : ApplicationUser
{
    public int? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    
    public string? Address { get; set; }
    public string? PaymentMethod { get; set; }
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
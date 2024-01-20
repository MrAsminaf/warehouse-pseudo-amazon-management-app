namespace Warehouse.Application.Models;

public class SignUpModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Address { get; set; }
    public string? PaymentMethod { get; set; }
}
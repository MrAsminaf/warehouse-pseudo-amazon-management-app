using Warehouse.Domain.Entities;

namespace Warehouse.Application.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(ApplicationUser user);
}
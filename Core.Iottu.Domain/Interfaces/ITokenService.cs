using Core.Iottu.Domain.Entities;

namespace Core.Iottu.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string subject, IEnumerable<KeyValuePair<string, string>>? claims = null);
    }
}


using ToDo.Domain.Entities;

namespace ToDo.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}

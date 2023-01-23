using ApiToDatabase.Models.RequestModels;

namespace ApiToDatabase.Services
{
    public interface IJwtManager
    {
        string CreateToken(UserRequest userRequest);
    }
}

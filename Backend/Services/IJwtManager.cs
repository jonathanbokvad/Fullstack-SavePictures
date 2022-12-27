namespace ApiToDatabase.Services
{
    public interface IJwtManager
    {
        string CreateToken();
        bool ValidateToken(string token);
    }
}

namespace todoapi.Services
{
    public interface IPasswordHasher
    {
         string Hash(string password);

         (bool verified, bool needUpdate) Check(string hash, string password);
    }
}
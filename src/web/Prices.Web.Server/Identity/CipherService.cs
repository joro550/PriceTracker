using SimpleCrypto;

namespace Prices.Web.Server.Identity
{
    public interface ICipherService
    {
        bool ValidatePasswordAgainstHash(string password, string salt, string knownHash);
    }

    public class CipherService : ICipherService
    {
        private readonly ICryptoService _cryptoService;

        public CipherService() 
            => _cryptoService = new PBKDF2();

        public bool ValidatePasswordAgainstHash(string password, string salt, string knownHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt))
                return false;

            var passwordHash = _cryptoService.Compute(password, salt);
            return _cryptoService.Compare(passwordHash, knownHash);
        }
    }
}
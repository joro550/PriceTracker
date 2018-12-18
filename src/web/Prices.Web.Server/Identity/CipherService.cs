using SimpleCrypto;

namespace Prices.Web.Server.Identity
{
    public interface ICipherService
    {
        bool ValidatePasswordAgainstHash(string password, string salt, string knownHash);
        PasswordResult GeneratePassword(string password);
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

        public PasswordResult GeneratePassword(string password)
        {
            var passwordHash = _cryptoService.Compute(password);
            return new PasswordResult
            {
                HashedPassword = passwordHash,
                PasswordSalt = _cryptoService.Salt
            };
        }
    }

    public class PasswordResult
    {
        public string HashedPassword { get; set; }
        public string PasswordSalt { get; set; }
    }
}
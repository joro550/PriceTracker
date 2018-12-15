using SimpleCrypto;

namespace Prices.Web.Server.Identity
{
    public interface ICipherService
    {
        PasswordResult Encrypt(string input);
    }

    public class CipherService : ICipherService
    {
        private readonly CipherServiceConfig _cipherConfig;
        private readonly ICryptoService _cryptoService;

        public CipherService(CipherServiceConfig cipherConfig)
        {
            _cipherConfig = cipherConfig;
            _cryptoService = new PBKDF2();
        }

        public PasswordResult Encrypt(string input)
        {
            return new PasswordResult
            {
                Hash = _cryptoService.Compute(input, _cipherConfig.Salt),
                Salt = _cryptoService.Salt
            };
        }
    }

    public class PasswordResult
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}
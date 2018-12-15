using Microsoft.Extensions.Configuration;

namespace Prices.Web.Server.Identity
{
    public class CipherServiceConfig
    {
        public string Salt { get; private set; }

        public static CipherServiceConfig FromConfiguration(IConfiguration config)
        {
            return new CipherServiceConfig
            {
                Salt = config["Cipher:Salt"]
            };
        }
    }
}
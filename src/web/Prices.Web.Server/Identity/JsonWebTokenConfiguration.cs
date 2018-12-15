using Microsoft.Extensions.Configuration;

namespace Prices.Web.Server.Identity
{
    public class JsonWebTokenConfiguration
    {
        public string Key { get; private set; }
        public string Issuer { get; private set; }
        public string Audience { get; private set; }
        public double ExpiryMinutesToAdd { get; private set; }

        public static JsonWebTokenConfiguration FromConfiguration(IConfiguration config)
        {
            return new JsonWebTokenConfiguration
            {
                Key = config["Jwt:Key"],
                Issuer = config["Jwt:Issuer"],
                Audience = config["Jwt:Audience"],
                ExpiryMinutesToAdd = double.Parse(config["Jwt:ExpireTime"])
            };
        }
    }
}
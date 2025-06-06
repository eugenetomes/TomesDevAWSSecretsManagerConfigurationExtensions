using EasyCompany.Extensions.Configuration.AWSSecretsManager;
using Microsoft.Extensions.Configuration;

namespace Sampe1;

internal class Program
{


    static void Main(string[] args)
    {
        var customData = new Dictionary<string, string>
        {
            { "PostgreSQLCredentials:Username", "CustomValue" },
            { "PostgreSQLCredentials:Password", "AnotherValue" }
        };

        var secretName = "dev/PostgreSQLCredentials";
        var regionName = "eu-west-1";


        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //.AddSecretsManagerConfiguration(regionName, secretName)
            .AddSecretsManagerJsonConfiguration(regionName, secretName, "PostgreSQLCredentials", optional: true)
            .Build();
    }
}

using EasyCompany.Extensions.Configuration.AWSSecretsManager;
using EasyCompany.Extensions.Configuration.AWSSecretsManager.Internal;
using EasyCompany.Extensions.Configuration.AWSSecretsManager.Internal.SecretsManagerJson;

namespace Microsoft.Extensions.Configuration;

public static class SecretsManagerExtensions
{
    public static IConfigurationBuilder AddSecretsManagerConfiguration(this IConfigurationBuilder builder, string regionName, string secretName)
    {
        return builder.Add(new SecretsManagerConfigurationSource(regionName, secretName));
    }

    public static IConfigurationBuilder AddSecretsManagerJsonConfiguration(this IConfigurationBuilder builder, string regionName, string secretName, string jsonObjectName, bool optional)
    {
        return builder.Add(new SecretsManagerJsonConfigurationSource(regionName, secretName, jsonObjectName, optional));
    }
}

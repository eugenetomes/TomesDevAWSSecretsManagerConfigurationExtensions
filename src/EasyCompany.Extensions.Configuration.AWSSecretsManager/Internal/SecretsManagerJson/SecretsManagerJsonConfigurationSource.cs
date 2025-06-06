using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCompany.Extensions.Configuration.AWSSecretsManager.Internal.SecretsManagerJson;
internal class SecretsManagerJsonConfigurationSource : IConfigurationSource
{
    private readonly string _region;
    private readonly string _secretName;
    private readonly string _jsonObjectName;
    private readonly bool _optional;

    public SecretsManagerJsonConfigurationSource(string region, string secretName, string jsonObjectName, bool optional)
    {
        _region = region;
        _secretName = secretName;
        _jsonObjectName = jsonObjectName;
        _optional = optional;
    }



    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new SecretsManagerJsonConfigurationProvider(_region, _secretName, _jsonObjectName, _optional);
    }
}

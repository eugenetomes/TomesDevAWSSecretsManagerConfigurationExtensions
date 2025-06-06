using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCompany.Extensions.Configuration.AWSSecretsManager.Internal.SecretsManagerJson;
internal class SecretsManagerJsonConfigurationProvider : ConfigurationProvider
{
    private readonly string _region;
    private readonly string _secretName;
    private readonly string _jsonObjectName;
    private readonly bool _optional;

    public SecretsManagerJsonConfigurationProvider(string region, string secretName, string jsonObjectName, bool optional)
    {
        _region = region;
        _secretName = secretName;
        _jsonObjectName = jsonObjectName;
        _optional = optional;
    }

    public override void Load()
    {
        try
        {
            var secret = GetSecret();
            if (string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException($"Secret '{_secretName}' not found or is empty.");
            }

            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(secret);
            var prefix = string.Empty;
            if (!string.IsNullOrEmpty(_jsonObjectName))
            {
                prefix = _jsonObjectName + ":";
            }

            if(jsonData is null)
            {
                return;
            }

            foreach (var kvp in jsonData)
            {
                var key = $"{prefix}{kvp.Key}";
                Data[key] = kvp.Value;
            }
        }
        catch (Exception)
        {
            if (_optional == false)
            {
                throw;
            }
            
        }
    }

    private string GetSecret()
    {
        var request = new GetSecretValueRequest
        {
            SecretId = _secretName,
            VersionStage = "AWSCURRENT" // VersionStage defaults to AWSCURRENT if unspecified.
        };

        using (var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(_region)))
        {
            var response = client.GetSecretValueAsync(request).Result;

            if (response.SecretString != null)
            {
                return response.SecretString;
            }
            else
            {
                using var memoryStream = response.SecretBinary;
                using var reader = new StreamReader(memoryStream);
                return Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }
        }
    }
}

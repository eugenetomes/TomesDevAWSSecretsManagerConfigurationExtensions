﻿using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCompany.Extensions.Configuration.AWSSecretsManager.Internal;
internal class SecretsManagerConfigurationProvider : ConfigurationProvider
{
    //private readonly IDictionary<string, string> _data;
    private readonly string _region;
    private readonly string _secretName;

    public SecretsManagerConfigurationProvider(string region, string secretName)
    {
        _region = region;
        _secretName = secretName;
    }

    public override void Load()
    {
        var secret = GetSecret();
        //foreach (var kvp in _data)
        //{
        //    Data[kvp.Key] = kvp.Value;
        //}
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

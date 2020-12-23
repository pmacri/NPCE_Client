using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPCE_Client.Test
{
    public class TestHelper
    {
        public class TestConfiguration
        {
            public string PostaEvoConnectionString { get; set; }

        }

        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets("e3dfcccf-0cb3-423a-b302-e3e92e95c128")
                .AddEnvironmentVariables()
                .Build();
        }

        public static TestConfiguration GetApplicationConfiguration(string outputPath)
        {
            var configuration = new TestConfiguration();

            var iConfig = GetIConfigurationRoot(outputPath);

            iConfig
                .GetSection("KavaDocs")
                .Bind(configuration);

            return configuration;
        }
    }
}

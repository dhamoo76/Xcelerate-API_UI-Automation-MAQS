using System;
using System.Net.Http.Headers;
using Idm.Core.Authentication.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSM.Core.Configuration;
using RSM.Xcelerate.CDS.Service.Client;
using RSM.Xcelerate.CEM.Service.Client;
using RSM.Xcelerate.ETL.Service.Client;

namespace Tests.API.Utilities.Common.TestServerSetup
{
    public static class CommonWebHostBuilder
    {
        public static IWebHostBuilder CreateWebHostBuilder(string serviceNameInUpperCase)
        {
            var partialConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.local.json", false)
                // this will require for run on CI
          //      .ConfigureRsmAppConfiguration(serviceNameInUpperCase, true)
                .Build();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.local.json", false)
                .AddJsonFile($"appsettings.{serviceNameInUpperCase}.json", false)
                .AddJsonFile($"appsettings.{partialConfig["RSM_ENVIRONMENT_TYPE"]}.json")
                .Build();

            return new WebHostBuilder()
                    .UseConfiguration(configuration)
                    .UseStartup<TestStartup>()
                    .ConfigureServices(services =>
                    {
                        services.AddIdmAuthentication(options =>
                        {
                            options.EnableIdmAuthorizationClaimInjection = true;
                        }).AddIdmClientCredentialsTokenFactory("Okta");

                        services.AddCdsServices(configuration);
                        services.AddEtlServices(configuration);
                        services.AddCemServices(configuration);

                 //       services.AddAzureAppConfiguration();
                        
                        services.AddHttpClient(Constants.WithUserToken)
                            .ConfigureHttpClient(o =>
                            {
                                o.BaseAddress = new Uri(configuration[$"Services:{serviceNameInUpperCase}:BaseUrl"]);
                                o.DefaultRequestHeaders.Authorization =
                                    new AuthenticationHeaderValue("Bearer", configuration[$"{serviceNameInUpperCase}_USER_TOKEN"]);
                            });
                        services.AddHttpClient(Constants.ExternalUserToken)
                            .ConfigureHttpClient(o =>
                            {
                                o.BaseAddress = new Uri(configuration[$"Services:{serviceNameInUpperCase}:BaseUrl"]);
                                o.DefaultRequestHeaders.Authorization =
                                    new AuthenticationHeaderValue("Bearer", configuration[$"{serviceNameInUpperCase}_EXTERNAL_USER_TOKEN"]);
                            });
                        services.AddHttpClient(Constants.WithoutUserToken)
                            .ConfigureHttpClient(o =>
                            {
                                o.BaseAddress = new Uri(configuration[$"Services:{serviceNameInUpperCase}:BaseUrl"]);
                            });
                    })
                ;
        }
    }
}

using System.Net.Http;
using Magenic.Maqs.BaseSeleniumTest;
using Microsoft.Extensions.DependencyInjection;
using RSM.Xcelerate.ETL.Service.Client;
using RSM.Xcelerate.ETL.Test.AzureStorageBlob;
using Tests.API.Utilities;
using Tests.API.Utilities.Common.TestServerSetup;
using Tests.Common.TestServerSetup;
using AutomationCore;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using System;
using System.Threading;
using CognizantSoftvision.Maqs.Utilities.Helper;
using RSM.Xcelerate.Test.Shared.ETL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Common.Etl
{
    public abstract class EtlUserTokenBaseClass : BaseSeleniumTest //TODO: BaseRsmTestCase uses MAQS 8, project currently uses MAQS 7. 
    {
        public IEtlServiceClient EtlUserClient { get; }
        public IEtlServiceClient EtlNoTokenClient { get; }

        protected EtlApiTestObject Etl;

        protected AzureStorageTestObject Azure;

        protected EtlRequestFactory EtlRequestFactory;
        protected EtlUserTokenBaseClass()
        {
            var factory = new EtlTestWebApplicationFactory<TestStartup>();

            var serviceProvider = factory.Server.Host.Services;
            
            var httpFactory = serviceProvider.GetService<IHttpClientFactory>();

            var etlAuthClient  = httpFactory.CreateClient(Constants.WithUserToken);

            var etlUnauthClient = httpFactory.CreateClient(Constants.WithoutUserToken);

            EtlUserClient = new EtlServiceClient(etlAuthClient);

            EtlNoTokenClient = new EtlServiceClient(etlUnauthClient);

            // Set up test object for interacting with ETL Api
            Etl = new EtlApiTestObject(
                etlAuthClient,
                etlUnauthClient,
                Log,
                GetFullyQualifiedTestClassName()
            );

            // Set up test object for interacting with Azure Storage
            Azure = new AzureStorageTestObject(
                Config.GetValueForSection("AzureStorageMaqs", "Connection"), 
                Config.GetValueForSection("AzureStorageMaqs", "BlobContainerName"), 
                Log, 
                GetFullyQualifiedTestClassName()
            );

            EtlRequestFactory = new EtlRequestFactory(new Data.API.Config());
        }

        [TestCleanup]
        public new async Task Teardown()
        {
            base.Teardown();

            await Etl.AuthenticatedManager.CleanupTestData();
            await Etl.UnauthenticatedManager.CleanupTestData();
        }

    }
}

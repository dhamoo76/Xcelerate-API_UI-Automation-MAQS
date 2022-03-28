using System;
using System.Net.Http;
using Magenic.Maqs.BaseSeleniumTest;
using Microsoft.Extensions.DependencyInjection;
using RSM.Xcelerate.CDS.Service.Client;
using RSM.Xcelerate.CEM.Service.Client;
using RSM.Xcelerate.ETL.Service.Client;
using Tests.API.Utilities.Common.TestServerSetup;
using Tests.Common.TestServerSetup;

namespace Tests.API.Utilities.Common
{
    public class CommonTokenBaseClass : BaseSeleniumTest
    {
        private static readonly Lazy<CdsTestWebApplicationFactory<TestStartup>> cdsFactory = new();
        private static readonly Lazy<CemTestWebApplicationFactory<TestStartup>> cemFactory = new();
        private static readonly Lazy<EtlTestWebApplicationFactory<TestStartup>> etlFactory = new();

        #region CDS fields

        private static Lazy<ICdsServiceClient> _cdsUserClient { get; set; } = new(InitializeCdsWithUserToken());

        public static ICdsServiceClient CdsUserClient => _cdsUserClient.Value;

        private static Lazy<ICdsServiceClient> _cdsUserClientWithoutToken { get; set; } = new(InitializeCdsWithoutToken());

        public static ICdsServiceClient CdsNoTokenClient => _cdsUserClientWithoutToken.Value;

        private static Lazy<ICdsServiceClient> _cdsServerClient { get; set; } = new(InitializeCdsServer());

        public static ICdsServiceClient CdsServerClient => _cdsServerClient.Value;

        private static Lazy<ICdsServiceClient> _cdsExternalUserClient { get; set; } = new(InitializeCdsExternalUserToken());

        public static ICdsServiceClient CdsExternalUserClient => _cdsExternalUserClient.Value;

        #endregion

        #region ETL fields

        private static Lazy<IEtlServiceClient> _etlUserClient { get; set; } = new(InitializeEtlWithUserToken());

        public static IEtlServiceClient EtlUserClient => _etlUserClient.Value;

        private static Lazy<IEtlServiceClient> _etlUserClientWithoutToken { get; set; } = new(InitializeEtlWithoutToken());

        public static IEtlServiceClient EtlNoTokenClient => _etlUserClientWithoutToken.Value;

        private static Lazy<IEtlServiceClient> _etlServerClient { get; set; } = new(InitializeEtlServer());

        public static IEtlServiceClient EtlServerClient => _etlServerClient.Value;

        private static Lazy<IEtlServiceClient> _etlExternalUserClient { get; set; } = new(InitializeEtlExternalUserToken());

        public static IEtlServiceClient EtlExternalUserClient => _etlExternalUserClient.Value;

        #endregion

        #region CEM fields

        private static Lazy<ICemServiceClient> _cemUserClient { get; set; } = new(InitializeCemWithUserToken());

        public static ICemServiceClient CemUserClient => _cemUserClient.Value;

        private static Lazy<ICemServiceClient> _cemUserClientWithoutToken { get; set; } = new(InitializeCemWithoutToken());

        public static ICemServiceClient CemNoTokenClient => _cemUserClientWithoutToken.Value;

        private static Lazy<ICemServiceClient> _cemServerClient { get; set; } = new(InitializeCemServer());

        public static ICemServiceClient CemServerClient => _cemServerClient.Value;

        private static Lazy<ICemServiceClient> _cemExternalUserClient { get; set; } = new(InitializeCemExternalUserToken());

        public static ICemServiceClient CemExternalUserClient => _cemExternalUserClient.Value;

        #endregion


        #region CDS methods

        private static ICdsServiceClient InitializeCdsServer()
        {
            var serviceProvider = cdsFactory.Value.Server.Host.Services;

            var cdsResolver = serviceProvider.GetService<ICdsServiceResolver>();

            _cdsServerClient = new Lazy<ICdsServiceClient>(cdsResolver.GetCdsServiceClientAsServer());

            return _cdsServerClient.Value;
        }

        private static ICdsServiceClient InitializeCdsWithUserToken()
        {
            var serviceProvider = cdsFactory.Value.Server.Host.Services;

            var httpFactory = serviceProvider.GetService<IHttpClientFactory>();

            _cdsUserClient = new Lazy<ICdsServiceClient>(new CdsServiceClient(httpFactory.CreateClient(Constants.WithUserToken)));

            return _cdsUserClient.Value;
        }

        private static ICdsServiceClient InitializeCdsExternalUserToken()
        {
            var serviceProvider = cdsFactory.Value.Server.Host.Services;

            var httpFactory = serviceProvider.GetService<IHttpClientFactory>();

            _cdsExternalUserClient = new Lazy<ICdsServiceClient>(new CdsServiceClient(httpFactory.CreateClient(Constants.ExternalUserToken)));

            return _cdsExternalUserClient.Value;
        }

        private static ICdsServiceClient InitializeCdsWithoutToken()
        {
            var serviceProvider = cdsFactory.Value.Server.Host.Services;

            var httpFactory = serviceProvider.GetService<IHttpClientFactory>();

            _cdsUserClientWithoutToken = new Lazy<ICdsServiceClient>(new CdsServiceClient(httpFactory.CreateClient(Constants.WithoutUserToken)));

            return _cdsUserClientWithoutToken.Value;
        }

        #endregion

        #region ETL methods

        private static IEtlServiceClient InitializeEtlServer()
        {
            var serviceProvider = etlFactory.Value.Server.Host.Services;

            var etlResolver = serviceProvider.GetService<IEtlServiceResolver>();

            _etlServerClient = new Lazy<IEtlServiceClient>(etlResolver.GetEtlServiceClientAsServer());

            return _etlServerClient.Value;
        }

        private static IEtlServiceClient InitializeEtlWithUserToken()
        {
            var serviceProvider = etlFactory.Value.Server.Host.Services;

            var httpFactory = serviceProvider.GetService<IHttpClientFactory>();

            _etlUserClient = new Lazy<IEtlServiceClient>(new EtlServiceClient(httpFactory.CreateClient(Constants.WithUserToken)));

            return _etlUserClient.Value;
        }

        private static IEtlServiceClient InitializeEtlExternalUserToken()
        {
            var serviceProvider = etlFactory.Value.Server.Host.Services;

            var httpFactory = serviceProvider.GetService<IHttpClientFactory>();

            _etlExternalUserClient = new Lazy<IEtlServiceClient>(new EtlServiceClient(httpFactory.CreateClient(Constants.ExternalUserToken)));

            return _etlExternalUserClient.Value;
        }

        private static IEtlServiceClient InitializeEtlWithoutToken()
        {
            var serviceProvider = etlFactory.Value.Server.Host.Services;

            var httpFactory = serviceProvider.GetService<IHttpClientFactory>();

            _etlUserClientWithoutToken = new Lazy<IEtlServiceClient>(new EtlServiceClient(httpFactory.CreateClient(Constants.WithoutUserToken)));

            return _etlUserClientWithoutToken.Value;
        }

        #endregion

        #region CEM methods

        private static ICemServiceClient InitializeCemServer()
        {
            var serviceProvider = cemFactory.Value.Server.Host.Services;

            var cemResolver = serviceProvider.GetService<ICemServiceResolver>();

            _cemServerClient = new Lazy<ICemServiceClient>(cemResolver.GetCemServiceClientAsServer());

            return _cemServerClient.Value;
        }

        private static ICemServiceClient InitializeCemWithUserToken()
        {
            var serviceProvider = cemFactory.Value.Server.Host.Services;

            var httpFactory = serviceProvider.GetService<IHttpClientFactory>();

            _cemUserClient = new Lazy<ICemServiceClient>(new CemServiceClient(httpFactory.CreateClient(Constants.WithUserToken)));

            return _cemUserClient.Value;
        }

        private static ICemServiceClient InitializeCemExternalUserToken()
        {
            var serviceProvider = cemFactory.Value.Server.Host.Services;

            var httpFactory = serviceProvider.GetService<IHttpClientFactory>();

            _cemExternalUserClient = new Lazy<ICemServiceClient>(new CemServiceClient(httpFactory.CreateClient(Constants.ExternalUserToken)));

            return _cemExternalUserClient.Value;
        }

        private static ICemServiceClient InitializeCemWithoutToken()
        {
            var serviceProvider = cemFactory.Value.Server.Host.Services;

            var httpFactory = serviceProvider.GetService<IHttpClientFactory>();

            _cemUserClientWithoutToken = new Lazy<ICemServiceClient>(new CemServiceClient(httpFactory.CreateClient(Constants.WithoutUserToken)));

            return _cemUserClientWithoutToken.Value;
        }

        #endregion
    }
}

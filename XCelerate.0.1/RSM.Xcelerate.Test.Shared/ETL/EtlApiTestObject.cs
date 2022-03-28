using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using RSM.Xcelerate.ETL.Service.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace RSM.Xcelerate.Test.Shared.ETL
{
    /// <summary>
    /// Test object for managing both the authenticated and unautheticated ETL api clients.
    /// </summary>
    /// <remarks>
    /// The assembly that contains the <see cref="EtlServiceClient"/> is generated from the API assembly,
    /// providing an interface for the API with already typed DTO objects. This is used over the MAQS native
    /// WebDriver to reduce test case automation implemenation time. The <see cref="EtlApiManager"/>
    /// and <see cref="EtlApiTestObject"/> classes provide a layer of indirection to take advantage of MAQS features
    /// like the lazy loading of <see cref="EtlServiceClient"/>, while also providing a layer of 
    /// abstraction that seperates test case logic from API call implementation details.
    /// </remarks>
    public class EtlApiTestObject: BaseTestObject
    {
        // Key for the EtlApiManager that has a bearer token
        private static readonly string authManagerKey = typeof(EtlApiManager).FullName + "Authenticated";
        // Key for the EtlApiManager that has no bearer token
        private static readonly string unauthManagerKey = typeof(EtlApiManager).FullName + "Unauthenticated";
        
        public EtlApiTestObject(HttpClient authClient, HttpClient anonClient, ITestObject baseTestObject) : base(baseTestObject)
        {
            ManagerStore.Add(authManagerKey, new EtlApiManager(authClient, this));
            ManagerStore.Add(unauthManagerKey, new EtlApiManager(anonClient, this));
        }

        public EtlApiTestObject(HttpClient authClient, HttpClient anonClient, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            ManagerStore.Add(authManagerKey, new EtlApiManager(authClient, this));
            ManagerStore.Add(unauthManagerKey, new EtlApiManager(anonClient, this));
        }

        public EtlApiTestObject(HttpClient authClient, HttpClient anonClient, ILogger logger, ISoftAssert softAssert, IPerfTimerCollection collection, string fullyQualifiedTestName) 
            : base(logger, softAssert, collection, fullyQualifiedTestName)
        {
            ManagerStore.Add(authManagerKey, new EtlApiManager(authClient, this));
            ManagerStore.Add(unauthManagerKey, new EtlApiManager(anonClient, this));
        }

        /// <summary>
        /// Creates the protocol in ETL through the API, and then deletes the protocol when object is deallocated.
        /// </summary>
        /// <param name="request">The request body to send to ETL</param>
        /// <returns>The response from the create protocol request</returns>
        public async Task<CreateProtocolCommandResponse> AddProtocol(CreateProtocolCommandRequest request)
        {
            var result = await AuthenticatedManager.GetClient().Protocols_CreateProtocolAsync(request);

            AuthenticatedManager.CleanupFunctions.Add(async (client) => await client.Protocols_DeletedByIdAsync(result.Id));

            return result;
        }

        /// <summary>
        /// Creates the payloads in ETL through the API, and then deletes the payloads when object is deallocated.
        /// </summary>
        /// <param name="request">The request body to send to ETL</param>
        /// <returns>The response from the create payload request</returns>
        public async Task<CreatePayloadDefinitionCommandResponse> AddPayload(CreatePayloadDefinitionCommandRequest request)
        {
            var result = await AuthenticatedManager.GetClient().PayloadDefinitions_CreateAsync(request);

            AuthenticatedManager.CleanupFunctions.Add(async (client) => {
                foreach (var id in result.Ids)
                {
                    await client.PayloadDefinitions_DeletedByIdAsync(id);
                }
            });

            return result;
        }

        public async Task<GetTransactionWithPayloadDefinitionResponse> GetPayloadDefinition(int clientId, string payloadName = null)
        {
            var result = await AuthenticatedManager.GetClient().Transactions_WithPayloadDefinitionAsync(filter: $"mdmClientId eq '{clientId}'");

            if (payloadName != null)
            {
                result.Results = result.Results.Where(transaction => transaction.PayloadDefinitionName == payloadName).ToList();
            }

            return result;
        }

        /// <summary>
        /// Gets the 'preferred_username' field for the current user.
        /// </summary>
        /// <returns>
        /// The 'perferred_username' for the current user.
        /// </returns>
        /// <remarks>
        /// This call should likely be refactored once the logic to fetch the Okta
        /// token is implemented.
        /// </remarks>
        public async Task<string> GetUsername()
        {
            var result = await AuthenticatedManager.HttpClient.GetAsync("https://preview.rsmidentity.com/oauth2/ausqht9pJOBv5Bc6r1d5/v1/userinfo");

            var seralizedResult = await JsonSerializer.DeserializeAsync<dynamic>(result.Content.ReadAsStream());

            return seralizedResult.GetProperty("preferred_username").GetString();
        }

        /// <summary>
        /// Manager for the ETL Client with a bearer token.
        /// </summary>
        public EtlApiManager AuthenticatedManager
        {
            get
            {
                return ManagerStore.GetManager<EtlApiManager>(authManagerKey);
            }
        }

        /// <summary>
        /// Manager for the ETL Client without a bearer token.
        /// </summary>
        public EtlApiManager UnauthenticatedManager
        {
            get
            {
                return ManagerStore.GetManager<EtlApiManager>(unauthManagerKey);
            }
        }
    }
}

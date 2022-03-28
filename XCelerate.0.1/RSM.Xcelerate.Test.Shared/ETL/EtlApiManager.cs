using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Magenic.Maqs.BaseTest;
using RSM.Xcelerate.ETL.Service.Client;

namespace RSM.Xcelerate.Test.Shared.ETL
{
    /// <summary>
    /// Wrapper around the EtlServiceClient, to allow MAQS to manage object initialization and deinitialization.
    /// </summary>
    /// <remarks>
    /// The assembly that contains the <see cref="EtlServiceClient"/> is generated from the API assembly,
    /// providing an interface for the ETL API with already typed DTO objects. This is used over the MAQS native
    /// WebDriver to reduce test case automation implemenation time. The <see cref="EtlApiManager"/>
    /// and <see cref="EtlApiTestObject"/> classes provide a layer of indirection to take advantage of MAQS features
    /// like the lazy loading of <see cref="EtlServiceClient"/>, while also providing a layer of 
    /// abstraction that seperates test case logic from API call implementation details.
    /// </remarks>
    public class EtlApiManager : DriverManager
    {
        /// <summary>
        /// A list of cleanup funcitons that is executed in reverse order on test cleanup. The cleanup will be executed
        /// in reverse order (last in, first out) to reduce dependency issues.
        /// </summary>
        internal List<Func<IEtlServiceClient, Task>> CleanupFunctions = new List<Func<IEtlServiceClient, Task>>();

        /// <summary>
        /// Underlying <see cref="HttpClient"/> used by the ETL Service Client.
        /// </summary>
        private HttpClient _httpClient;
       
        /// <summary>
        /// Underlying <see cref="HttpClient"/> used by the ETL Service Client.
        /// </summary>
        public HttpClient HttpClient
        {
            get
            {
                return _httpClient;
            }
        }

        public EtlApiManager(HttpClient client, ITestObject testObject) 
            : base(() => new EtlServiceClient(client), testObject)
        {
            _httpClient = client;
        }

        public override object Get()
        {
            return GetClient();
        }

        public IEtlServiceClient GetClient()
        {
            return GetBase() as IEtlServiceClient;
        }

        /// <summary>
        /// Execute all stored cleanup functions.
        /// </summary>
        public async Task CleanupTestData()
        {
            foreach (var cleanup in CleanupFunctions.Reverse<Func<IEtlServiceClient, Task>>())
            {
                await cleanup(GetClient());
            }
        }
        
        protected override void DriverDispose()
        {
        }
    }
}

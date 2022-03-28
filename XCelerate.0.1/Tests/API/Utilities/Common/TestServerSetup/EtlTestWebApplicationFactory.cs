using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Tests.API.Utilities.Common.TestServerSetup;

namespace Tests.Common.TestServerSetup
{
    public class EtlTestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return CommonWebHostBuilder.CreateWebHostBuilder("ETL");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseTestServer();
        }
    }
}

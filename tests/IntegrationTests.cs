using System.Threading.Tasks;
using SodaPop.ConfigExplorer.Sample;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SodaPop.ConfigExplorer.Tests
{
    public class IntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public IntegrationTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ConfirmCanGetResponseFromOptions()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/config");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains("Tier1:Tier2:Tier3:Level3Option", responseString);
        }
    }
}

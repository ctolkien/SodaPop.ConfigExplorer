using System;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetOptionsExplorer.Sample;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace AspNetOptionsExplorer.Tests
{
    public class IntergrationTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public IntergrationTest()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ConfirmCanGetResponseFromOptions()
        {
            var response = await _client.GetAsync("/options");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal($"Key: 'Tier1:Tier2:Tier3:Level3Option', Value: 'And it's value' {Environment.NewLine}Key: 'Tier1:SomeKey', Value: 'Some Value' {Environment.NewLine}",
                responseString);
        }
    }
}

using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using SecureServer;

namespace AttackPlatform {
    public class ApiTester {
        public static Url BASE_URL = "http://localhost:5000";

        public TestServer Server { get; }
        public FlurlClient Client { get; }

        public ApiTester() {
            var webHostBuilder = new WebHostBuilder().UseStartup<Startup>();
            Server = new TestServer(webHostBuilder);
            Client = new FlurlClient(Server.CreateClient());
        }

        public async Task<string> GetAsString(Url url) => await RequestBuilder(url).GetStringAsync();

        public async Task<HttpResponseMessage> PostJson<T>(Url url, T body) => await RequestBuilder(url).PostJsonAsync(body);
        public async Task<string> PostJsonAsString<T>(Url url, T body) => await RequestBuilder(url).PostJsonAsync(body).ReceiveString();

        public async Task<HttpResponseMessage> PostForm<T>(Url url, T body) => await RequestBuilder(url).PostUrlEncodedAsync(body);

        private IFlurlRequest RequestBuilder(Url url) => Client.Request(BASE_URL, url).AllowAnyHttpStatus();
    }
}
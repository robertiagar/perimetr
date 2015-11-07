using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Perimetr.WindowsUniversal.Services
{
    public class LoginService : ILoginService
    {
        private HttpClient client;
        private readonly string tokenLocation = "/token";

        public LoginService(string baseAddress)
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(baseAddress);
        }

        public async Task<string> GetAccessTokenAsync(string username, string password)
        {
            var stuffs = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("Username", username),
                new KeyValuePair<string, string>("Password", password),
                new KeyValuePair<string, string>("Grant_type", "password")
            });

            var response = await this.client.PostAsync(tokenLocation, stuffs);

            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
            }

            return string.Empty;
        }
    }
}

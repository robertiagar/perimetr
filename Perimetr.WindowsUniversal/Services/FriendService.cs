using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Perimetr.WindowsUniversal.Services
{
    public class FriendService : IFriendService
    {
        private HttpClient client;
        private ISettingsService settingsService;
        private readonly string apiLocation = "api/Friends";
        private readonly string baseAddress;

        public FriendService(ISettingsService settingsService, string baseAddress)
        {
            this.settingsService = settingsService;
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(baseAddress);
        }

        public async Task AddFriendAsync(string friendId)
        {
            await AddAuthorizationHeadersAsync();
            var response = await this.client.PostAsync(string.Format("{0}/AddFriend?friendId={1}", apiLocation, friendId), null);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch(Exception ex)
            {
                //upsie
            }
        }

        public async Task<IEnumerable<ContactView>> FindFriendsAsync(IEnumerable<ContactBinding> contacts)
        {
            await AddAuthorizationHeadersAsync();
            var content = JsonConvert.SerializeObject(contacts);
            var stringContent = new StringContent(content);
            stringContent.Headers.Clear();
            stringContent.Headers.Add("Content-type", "application/json");

            var response = await this.client.PostAsync(string.Format("{0}/FindFriends", apiLocation), stringContent);
            try
            {
                if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var contactViews = JsonConvert.DeserializeObject<IEnumerable<ContactView>>(responseJson);
                    return contactViews;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public async Task<IEnumerable<FriendView>> GetFriendsAsync()
        {
            await AddAuthorizationHeadersAsync();
            var jsonResponse = await this.client.GetStringAsync(apiLocation);
            return JsonConvert.DeserializeObject<IEnumerable<FriendView>>(jsonResponse);
        }

        private async Task AddAuthorizationHeadersAsync()
        {
            var access_token = await settingsService.ReadItemAsync<string>("access_token");
            this.client.DefaultRequestHeaders.Clear();
            this.client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", access_token));
        }
    }
}

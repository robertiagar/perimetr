using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace Perimetr.WindowsUniversal.Services
{
    public class SettingsService : ISettingsService
    {
        private StorageFile settingsFile;
        private Strategy strategy;

        public SettingsService(Strategy strategy = Strategy.Local)
        {
            this.strategy = strategy;
        }

        public async Task SaveItemAsync<T>(T value, string key)
        {
            var file = await GetStorageFileAsync();
            var fileText = await FileIO.ReadTextAsync(file);
            dynamic json = JsonConvert.DeserializeObject(fileText);
            if (json == null)
            {
                json = new ExpandoObject();
            }
            json[key] = value;
            var text = (string)JsonConvert.SerializeObject(json);
            await FileIO.WriteTextAsync(file, text);
        }

        public async Task<T> ReadItemAsync<T>(string key) where T : class
        {
            var file = await GetStorageFileAsync();
            var fileText = await FileIO.ReadTextAsync(file);
            var json = JObject.Parse(fileText);
            try
            {
                return json[key].ToObject<T>();
            }
            catch
            {
                return null;
            }
        }

        private async Task<StorageFile> GetStorageFileAsync()
        {
            return await ApplicationData.Current.LocalFolder.CreateFileAsync("settings.json", CreationCollisionOption.OpenIfExists);
        }
    }

    public enum Strategy
    {
        Local,
        Roaming
    }
}

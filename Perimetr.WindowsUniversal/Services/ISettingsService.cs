using System.Threading.Tasks;

namespace Perimetr.WindowsUniversal.Services
{
    public interface ISettingsService
    {
        Task<T> ReadItemAsync<T>(string key) where T : class;
        Task SaveItemAsync<T>(T value, string key);
    }
}
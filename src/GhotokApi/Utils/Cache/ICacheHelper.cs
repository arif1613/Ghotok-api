using System.Threading.Tasks;

namespace GhotokApi.Utils.Cache
{
    public interface ICacheHelper
    {
        Task Add<T>(T o, string key,int minutes) where T : class;
        Task Clear(string key);
        Task<bool> Exists(string key);
        Task<T> Get<T>(string key) where T : class;
        Task Update<T>(T NewObject, string key) where T : class;
    }
}

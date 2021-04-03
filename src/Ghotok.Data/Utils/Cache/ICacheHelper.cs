

namespace Ghotok.Data.Utils.Cache
{
    public interface ICacheHelper
    {
        void Add<T>(T o, string key,int minutes) where T : class;
        void Clear(string key);
        bool Exists(string key);
        T Get<T>(string key) where T : class;
        void Update<T>(T NewObject, string key) where T : class;
    }
}

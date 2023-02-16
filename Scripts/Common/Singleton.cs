
namespace Frame.Common
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T instance;

        public static T Instance => instance ?? (instance = new T());
    }
}
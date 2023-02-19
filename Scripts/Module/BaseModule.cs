using Frame.Common;

namespace Frame.Module
{
    public class BaseModule<T> : Singleton<T>, IModule where T : BaseModule<T>, new()
    {
        public virtual void _Ready()
        {
        }

        public virtual void _Process()
        {
        }

        public virtual void _Dispose()
        {
        }
    }
}
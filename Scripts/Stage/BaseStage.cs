using Frame.Common;

namespace Frame.Stage
{
    public class BaseStage<T> : Singleton<T>, IStage where T : Singleton<T>, new()
    {
        
        public virtual void OnEnter()
        {
        }
        
        public virtual void OnExit()
        {
        }

        public virtual void OnProcess()
        {
        }
    }
}
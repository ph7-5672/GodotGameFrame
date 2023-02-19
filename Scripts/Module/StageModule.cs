using Frame.Common;
using Frame.Stage;

namespace Frame.Module
{
    /// <summary>
    /// 阶段模块。
    /// </summary>
    public class StageModule : Singleton<StageModule>
    {

        private IStage current;

        public static IStage Current
        {
            get => Instance.current;
            private set => Instance.current = value;
        }

        public static void ChangeStage<T>() where T : BaseStage<T>, new()
        {
            Current?.OnExit();
            Current = BaseStage<T>.Instance;
            Current.OnEnter();
        }

    }
}
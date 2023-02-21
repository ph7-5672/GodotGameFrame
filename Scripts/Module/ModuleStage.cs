using Frame.Common;
using Frame.Stage;

namespace Frame.Module
{
    /// <summary>
    /// 阶段模块。
    /// </summary>
    public class ModuleStage : Singleton<ModuleStage>
    {

        private IStage current;

        public static IStage Current
        {
            get => Instance.current;
            private set => Instance.current = value;
        }

        public static void ChangeStage<T>() where T : StageBase<T>, new()
        {
            Current?.OnExit();
            Current = StageBase<T>.Instance;
            Current.OnEnter();
        }

    }
}
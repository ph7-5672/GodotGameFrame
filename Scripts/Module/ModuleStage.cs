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

        public IStage Current
        {
            get => current;
            private set => current = value;
        }

        public void ChangeStage<T>() where T : StageBase<T>, new()
        {
            Current?.OnExit();
            Current = StageBase<T>.Instance;
            Current.OnEnter();
        }

    }
}
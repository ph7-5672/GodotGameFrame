using Frame.Common;
using Frame.Module;

namespace Frame.Stage
{
    public class StagePreload : StageBase<StagePreload>
    {
        
        public override void OnEnter()
        {
            ModuleScene.LoadScene(SceneType.Test);
            ModuleEntity.Spawn(EntityType.Player);
        }

    }
}
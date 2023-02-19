using Frame.Common;
using Frame.Module;

namespace Frame.Stage
{
    public class StagePreload : BaseStage<StagePreload>
    {
        
        public override void OnEnter()
        {
            SceneModule.LoadScene(SceneType.Test);
            EntityModule.Spawn(EntityType.Player);
        }

    }
}
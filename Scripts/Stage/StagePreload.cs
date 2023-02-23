using Frame.Common;
using Frame.Form;
using Frame.Module;
using Godot;

namespace Frame.Stage
{
    public class StagePreload : StageBase<StagePreload>
    {
        
        public override void OnEnter()
        {
            ModuleScene.LoadScene(SceneType.Test);
            var player = ModuleEntity.Spawn(EntityType.Player);
            var form = (FormPlayerInfo) ModuleForm.Open(FormType.PlayerInfo);
            form.Player = player;
        }

    }
}
using Frame.Common;
using Frame.Entity;
using Frame.Form;
using Frame.Module;
using Godot;

namespace Frame.Stage
{
    public class StageTest : StageBase<StageTest>
    {
        private FormPlayerInfo playerInfoForm;
        private Node playerEntity;
        
        public override void OnEnter()
        {
            
            ModuleScene.LoadScene(SceneType.Test);
            
            playerEntity = ModuleEntity.Spawn(EntityType.Police);
            playerEntity.SetValue(ValueType.Move2DPlatform, new ValueMove2DPlatform()
            {
                speed = new Value(10f),
                bounce = new Value(2f)
            });
            
            playerEntity.SetValue(ValueType.Hero, new ValueHero());

            var datatable = ModuleDatatable.GetDatatable(DatatableType.Guns, 2);
            playerEntity.SetValue(ValueType.Shooter, new ValueShooter(datatable)
            {
                bulletCount = 30
            });

            /*playerEntity.AddLogic(LogicType.Player, new LogicPlayer());
            playerEntity.AddLogic(LogicType.Move2D, new LogicMove2D());
            
            
            playerInfoForm = (FormPlayerInfo) ModuleForm.Open(FormType.PlayerInfo);
            playerEntity.Behave(new BehaviorChangeGun(2));*/
        }

        [Event(EventType.EntitySetValue)]
        public static void OnEntitySetValue(Node entity, string valueName, float value)
        {
            if (Instance.playerInfoForm == null || !entity.Equals(Instance.playerEntity))
            {
                return;
            }

            if ("bulletCount".Equals(valueName))
            {
                Instance.playerInfoForm.bulletCount = (int) value;
                Instance.playerInfoForm.Refresh();
            }
            
            if ("magazine".Equals(valueName))
            {
                Instance.playerInfoForm.magazine = (int) value;
                Instance.playerInfoForm.Refresh();
            }


        }
    }
}
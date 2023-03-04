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
            
            playerInfoForm = (FormPlayerInfo) ModuleForm.Open(FormType.PlayerInfo);
            
            playerEntity = ModuleEntity.Spawn(EntityType.Police);
            playerEntity.SetValue(ValueType.Move2DPlatform, new ValueMove2DPlatform()
            {
                speed = new Value(10f),
                bounce = new Value(2f)
            });
            
            playerEntity.SetValue(ValueType.Hero, new ValueHero());

            var datatable = ModuleDatatable.GetDatatable(DatatableType.Shooter, 2);
            playerEntity.SetValue(ValueType.Shooter, new ValueShooter(datatable)
            {
                bulletCount = 30
            });
            
        }



        [Event(EventType.EntitySetValue)]
        public static void OnEntitySetValue(Node entity, ValueType valueType, IEntityValue value)
        {
            if (Instance.playerInfoForm == null || !entity.Equals(Instance.playerEntity))
            {
                return;
            }

            if (valueType == ValueType.Shooter && value is ValueShooter shooter)
            {
                Instance.playerInfoForm.Refresh(shooter.bulletCount, shooter.magazine.intFinal);
            }
        }
    }
}
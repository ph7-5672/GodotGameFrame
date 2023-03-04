using Frame.Common;
using Frame.Entity;
using Frame.Form;
using Godot;

namespace Frame.Stage
{
    public class StageTest : StageBase<StageTest>
    {
        private FormPlayerInfo playerInfoForm;
        private Node playerEntity;
        
        public override void OnEnter()
        {
            
            GameFrame.Scene.LoadScene(SceneType.Test);
            
            playerInfoForm = (FormPlayerInfo) GameFrame.Form.Open(FormType.PlayerInfo);
            
            playerEntity = GameFrame.Entity.Spawn(EntityType.Police);
            playerEntity.SetValue(new ValueMove2DPlatform()
            {
                speed = new Value(10f),
                bounce = new Value(2f)
            });
            
            playerEntity.SetValue(new ValueHero());

            var datatable = GameFrame.Datatable.GetDatatable(DatatableType.Shooter, 2);
            playerEntity.SetValue(new ValueShooter(datatable)
            {
                bulletCount = 30
            });
            
        }


        [Event(EventType.EntitySetValue)]
        public static void OnEntitySetValue(Node entity, IEntityValue value)
        {
            if (Instance.playerInfoForm == null || !entity.Equals(Instance.playerEntity))
            {
                return;
            }

            if (value is ValueShooter shooter)
            {
                Instance.playerInfoForm.Refresh(shooter.bulletCount, shooter.magazine.intFinal);
            }
        }
    }
}
using Frame.Common;
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

            playerEntity.SetValue(new ValueBuff());
            playerEntity.SetValue(new ValueStunByObstacle());
            playerEntity.SetValue(new ValueHealth());
            
            
            var datatable = GameFrame.Datatable.GetDatatable(DatatableType.Shooter, 2);
            playerEntity.SetValue(new ValueShooter(datatable)
            {
                bulletCount = 30
            });
            
            
            

            var zombie = (Node2D)GameFrame.Entity.Spawn(EntityType.Zombie);
            zombie.SetValue(new ValueMove2DPlatform());
            zombie.Position = new Vector2(200f, 0);
            

            
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
using System;
using System.Reflection;
using Frame.Common;
using Frame.Module;

namespace Frame.Stage
{
    public class StagePreload : StageBase<StagePreload>
    {
        
        public override void OnEnter()
        {
            // 订阅所有事件。
            SubscribeAssemblies();
            // 加载所有数据表。
            LoadAllDatabase();
            
            
            ModuleScene.LoadScene(SceneType.Test);
            
            ModuleEntity.Spawn2D(EntityType.Police);
            /*var player = ModuleEntity.Spawn2DByDatabase(DatabaseType.Heroes, (int) HeroType.Police);
            //var player = ModuleEntity.Spawn2D(EntityType.Police);
            var form = (FormPlayerInfo) ModuleForm.Open(FormType.PlayerInfo);
            form.Player = player;*/
        }


        void SubscribeAssemblies()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                ScanAssemblies(assembly);
            }
        }
        
        void ScanAssemblies(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                ScanMethods(type);
            }
        }

        void ScanMethods(IReflect type)
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (method.TryGetAttribute<EventAttribute>(out var attribute))
                {
                    ModuleEvent.Subscribe(attribute.eventType, method);
                }
            }
        }

        void LoadAllDatabase()
        {
            foreach (var value in Enum.GetValues(typeof(DatabaseType)))
            {
                ModuleDatabase.Load((DatabaseType) value);
            }
        }

    }
}
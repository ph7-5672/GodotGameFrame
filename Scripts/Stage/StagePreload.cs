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
            LoadAllDatatable();
            // 填充实体对象池。
            FillEntityPools();
            
            GameFrame.Stage.ChangeStage<StageTest>();
        }


        void SubscribeAssemblies()
        {
            foreach (var assembly in UtilityType.Assemblies)
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
                    GameFrame.Event.Subscribe(attribute.eventType, method);
                }
            }
        }

        void LoadAllDatatable()
        {
            foreach (var datatableType in Constants.datatableTypeArray)
            {
                GameFrame.Datatable.Load(datatableType);
            }
        }


        void FillEntityPools()
        {
            GameFrame.Entity.FillPool(EntityType.Bullet, 10);
            GameFrame.Entity.FillPool(EntityType.Zombie, 10);
        }

    }
}
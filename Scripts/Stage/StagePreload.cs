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
            

            ModuleStage.ChangeStage<StageTest>();
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
                    ModuleEvent.Subscribe(attribute.eventType, method);
                }
            }
        }

        void LoadAllDatatable()
        {
            foreach (var datatableType in Constants.datatableTypeArray)
            {
                ModuleDatatable.Load(datatableType);
            }
        }


        void FillEntityPools()
        {
            ModuleEntity.FillPool(EntityType.Bullet, 10);
            ModuleEntity.FillPool(EntityType.Zombie, 10);
        }

    }
}
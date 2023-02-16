using Frame.Common;
using Godot;

namespace Frame.Module
{
    public class EntityModule : Singleton<EntityModule>
    {
        //private readonly EntityDB[] database = DatabaseModule.Load<EntityDB>();
        
        public static Node2D Spawn(EntityType entityType)
        {
            //var name = Instance.database[type].FileName;
            var name = entityType.ToString();
            var instance = SceneModule.LoadInstance<Node2D>($"Entities/{name}", GameFrame.EntityRoot);
            return instance;
        }


    }
}
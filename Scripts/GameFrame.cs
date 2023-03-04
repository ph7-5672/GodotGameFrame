using System;
using System.Collections.Generic;
using System.Linq;
using Frame.Common;
using Frame.Module;
using Frame.Stage;
using Godot;

namespace Frame
{
    public class GameFrame : Node
    {
        /// <summary>
        /// GUI根节点。
        /// </summary>
        public static CanvasLayer FormRoot { get; private set; }

        public static Node EntityRoot { get; private set; }

        public static Node SceneRoot { get; private set; }

        public static List<IEntityLogic> Logics { get; } = new List<IEntityLogic>();

        public static ModuleBehavior Behavior { get; private set; }
        public static ModuleDatatable Datatable { get; private set; }
        public static ModuleEntity Entity { get; private set; }
        public static ModuleEvent Event { get; private set; }
        public static ModuleForm Form { get; private set; }
        public static ModuleScene Scene { get; private set; }
        public static ModuleStage Stage { get; private set; }
        public static ModuleTimer Timer { get; private set; }

        public override void _Ready()
        {
            FormRoot = GetNode<CanvasLayer>(nameof(FormRoot));
            EntityRoot = GetNode<Node>(nameof(EntityRoot));
            SceneRoot = GetNode<Node>(nameof(SceneRoot));

            Behavior = ModuleBehavior.Instance;
            Datatable = ModuleDatatable.Instance;
            Entity = ModuleEntity.Instance;
            Event = ModuleEvent.Instance;
            Form = ModuleForm.Instance;
            Scene = ModuleScene.Instance;
            Stage = ModuleStage.Instance;
            Timer = ModuleTimer.Instance;
            AddLogics();
            
            Stage.ChangeStage<StagePreload>();
        }


        void AddLogics()
        {
            foreach (var assembly in UtilityType.Assemblies)
            {
                foreach (var type in assembly.GetTypes()
                    .Where(t => t.GetInterface(nameof(IEntityLogic)) != null && !t.IsAbstract))
                {
                    var instance = (IEntityLogic) Activator.CreateInstance(type);
                    Logics.Add(instance);
                }
            }
        }

        public static void ForeachLogics(Action<IEntityLogic> action)
        {
            foreach (var logic in Logics.AsParallel())
            {
                action(logic);
            }
        }

        public static void ForeachEntities(Action<Node> action)
        {
            foreach (Node entity in EntityRoot.GetChildren().AsParallel())
            {
                action(entity);
            }
        }

        public override void _Process(float delta)
        {
            ForeachLogics(logic => ForeachEntities(entity => logic.Process(entity, delta)));
        }

        public override void _PhysicsProcess(float delta)
        {
            ForeachLogics(logic => ForeachEntities(entity => logic.PhysicsProcess(entity, delta)));
        }
    }
}
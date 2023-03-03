using System;
using System.Collections.Generic;
using System.Linq;
using Frame.Common;
using Frame.Logic;
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


        public override void _Ready()
        {
            FormRoot = GetNode<CanvasLayer>(nameof(FormRoot));
            EntityRoot = GetNode<Node>(nameof(EntityRoot));
            SceneRoot = GetNode<Node>(nameof(SceneRoot));
            
            AddLogics();
            
            ModuleStage.ChangeStage<StagePreload>();
        }


        void AddLogics()
        {
            foreach (var assembly in UtilityType.Assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(t => t.GetInterface(nameof(IEntityLogic)) != null && !t.IsAbstract))
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
            /*foreach (var logic in Logics)
            {
                foreach (Node entity in EntityRoot.GetChildren())
                {
                    logic.Process(entity, delta);
                }
            }*/
        }

        public override void _PhysicsProcess(float delta)
        {
            ForeachLogics(logic => ForeachEntities(entity => logic.PhysicsProcess(entity, delta)));
            /*foreach (var logic in Logics)
            {
                foreach (Node entity in EntityRoot.GetChildren())
                {
                    logic.PhysicsProcess(entity, delta);
                }
            }*/
            
            /*foreach (Node entity in EntityRoot.GetChildren())
            {
                ModuleEntity.LogicForeach(entity, logic => logic.PhysicsProcess(delta));
            }*/
        }
    }
}



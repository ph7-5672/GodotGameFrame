using System;
using Frame.Common;
using Godot;
using Godot.Collections;

namespace Frame.Module
{
    public class ModuleScene : Singleton<ModuleScene>
    {
    
        private readonly Dictionary<string, PackedScene> scenesCache = new Dictionary<string, PackedScene>();
    
        /// <summary>
        /// 加载一个指定路径下的场景。
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static PackedScene Load(string path)
        {
            if (!Instance.scenesCache.TryGetValue(path, out var packedScene))
            {
                packedScene = GD.Load<PackedScene>($"{Constants.resourceRoot}{path}{Constants.sceneSuffix}");
                if (packedScene == null)
                {
                    throw new Exception($"指定路径下没有场景。{path}");
                }
                Instance.scenesCache.Add(path, packedScene);
            }
            return packedScene;
        }

        /// <summary>
        /// 获取指定路径下的场景实例。
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="parent">指定父节点</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadInstance<T>(string path, Node parent = null) where T : Node
        {
            var packedScene = Load(path);
            if (!packedScene.CanInstance())
            {
                GD.PrintErr($"指定场景无法实例化。{path}");
            }

            var instance = packedScene.Instance<T>();
            parent?.AddChild(instance);
            return instance;
        }

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneType"></param>
        /// <returns></returns>
        public static Node LoadScene(SceneType sceneType)
        {
            return LoadInstance<Node>($"Scenes/{sceneType.ToString()}", GameFrame.SceneRoot);
        }


    }
}


using System;
using System.Collections.Generic;
using Frame.Module;
using Godot;

namespace Frame.Common
{
    public static class ExtensionUtility
    {
        public static List<Node> GetComponents(this Node2D entity)
        {
            var result = new List<Node>();
            foreach (var child in entity.GetChildren())
            {
                if (child is IEntityComponent && child is Node node)
                {
                    result.Add(node);
                }
            }
            return result;
        }

    }
}
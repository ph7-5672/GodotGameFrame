using Frame.Common;
using Godot;

namespace Frame.Module
{
    public class FormModule : Singleton<FormModule>
    {
        /// <summary>
        /// 打开指定类型的用户界面。
        /// </summary>
        /// <param name="formType"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Node Open<T>(FormType formType) where T : CanvasItem
        {
            var name = formType.ToString();
            var instance = SceneModule.LoadInstance<T>($"Forms/{name}", GameFrame.FormRoot);
            return instance;
        }

    }
}


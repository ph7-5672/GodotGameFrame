using Frame.Common;
using Godot;

namespace Frame.Module
{
    public class ModuleForm : Singleton<ModuleForm>
    {
        /// <summary>
        /// 打开指定类型的用户界面。
        /// </summary>
        /// <param name="formType"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static CanvasItem Open(FormType formType)
        {
            var name = formType.ToString();
            var instance = ModuleScene.LoadInstance<CanvasItem>($"Forms/{name}", GameFrame.FormRoot);
            return instance;
        }

    }
}


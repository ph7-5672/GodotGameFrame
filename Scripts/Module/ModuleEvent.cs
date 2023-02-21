using System;
using Frame.Common;
using Godot;

namespace Frame.Module
{
    
    public class ModuleEvent : Singleton<ModuleEvent>
    {
        /// <summary>
        /// 订阅事件和发布者。
        /// </summary>
        /// <param name="handler">处理器</param>
        /// <param name="sender">发布者，如果不指定则接受所有发布者的事件。</param>
        /// <typeparam name="T">事件类型必须是结构体，且实现IEventArgs接口</typeparam>
        public static void Subscribe<T>(EventHandler<T> handler, object sender = null) where T : struct, IEventArgs
        {
            EventManger<T>.Subscribe(handler, sender);
        }

        /// <summary>
        /// 发送事件。
        /// </summary>
        /// <param name="eventArgs">事件数据</param>
        /// <param name="sender">发布者</param>
        /// <typeparam name="T">事件类型必须是结构体，且实现IEventArgs接口</typeparam>
        public static void Send<T>(T eventArgs, object sender) where T : struct, IEventArgs
        {
            EventManger<T>.Send(eventArgs, sender);
        }
    }


    internal static class EventManger<T> where T : struct, IEventArgs
    {
        public static event EventHandler<T> e = delegate(object sender, T args) {};

        public static void Subscribe(EventHandler<T> handler, object sender = null)
        {
            e += (o, args) =>
            {
                if (sender == null || sender.Equals(o))
                {
                    handler.Invoke(o, args);
                }
            };
        }

        public static void Send(T eventArgs, object sender)
        {
            foreach (var handler in e.GetInvocationList())
            {
                try
                {
                    handler.DynamicInvoke(sender, eventArgs);
                }
                catch (Exception exception)
                {
                    GD.PrintErr(exception);
                }
            }
        }

    }
}
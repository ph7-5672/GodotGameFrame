using System;
using System.Collections.Generic;
using System.Reflection;
using Frame.Common;

namespace Frame.Module
{
    
    public class ModuleEvent : Singleton<ModuleEvent>
    {
        private readonly Dictionary<EventType, List<MethodInfo>> eventMethods = new Dictionary<EventType, List<MethodInfo>>();
        

        public void Subscribe(EventType eventType, MethodInfo method)
        {
            if (!eventMethods.TryGetValue(eventType, out var methods))
            {
                methods = new List<MethodInfo>();
                eventMethods.Add(eventType, methods);
            }

            methods.Add(method);
        }

        /// <summary>
        /// 发送事件。
        /// 发送的参数要和接受方法一致。
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="args"></param>
        public void Send(EventType eventType, params object[] args)
        {
            if (eventMethods.TryGetValue(eventType, out var methods))
            {
                foreach (var method in methods)
                {
                    try
                    {
                        method.FastInvoke(null, args);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }

    }
}
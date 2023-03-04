using System.Collections.Generic;
using System.Threading;
using Frame.Common;

namespace Frame.Module
{
    /// <summary>
    /// 计时器模块。
    /// 每个指定间隔发送事件。
    /// </summary>
    public class ModuleTimer : Singleton<ModuleTimer>
    {
        private readonly Dictionary<string, Timer> timers = new Dictionary<string, Timer>();

        /// <summary>
        /// 开启一个新的计时器。
        /// 如果是重复计时器，请注意手动释放资源。
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="interval"></param>
        /// <param name="timerName"></param>
        /// <param name="isRepeat">是否重复计时</param>
        public static Timer StartNew(object owner, float interval, string timerName, bool isRepeat = false)
        {
            var dueTime = (long) (interval * 1000);
            var timer = new Timer(o => ModuleEvent.Send(EventType.Timeout, o, timerName, isRepeat), owner, dueTime, dueTime);
            Instance.timers.Add(timerName, timer);
            return timer;
        }


        public static bool HasTimer(string name)
        {
            return Instance.timers.TryGetValue(name, out _);
        }

        [Event(EventType.Timeout)]
        public static void OnTimeout(object owner, string timerName, bool isRepeat)
        {
            if (!isRepeat && Instance.timers.TryGetValue(timerName, out var timer))
            {
                timer.Dispose();
                Instance.timers.Remove(timerName);
            }
        }

    }
}
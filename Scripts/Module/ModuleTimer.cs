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
            // 参数解读：
            // 回调事件；
            // 回调事件里的参数，这里用计时器的持有者，方便发送事件。
            // 第一次回调间隔时间。
            // 之后每次的回调间隔时间。
            var timer = new Timer(o => o.SendEvent(new EventTimeout(timerName)), owner, dueTime, dueTime);
            
            if (isRepeat)
            {
                return timer;
            }
            
            // 如果不是重复计时，订阅事件，并在回调中释放计时器。
            ModuleEvent.Subscribe<EventTimeout>((sender, timeout) =>
            {
                if (timerName.Equals(timeout.timerName))
                {
                    timer.Dispose();
                }
            });

            return timer;
        }
    }
}
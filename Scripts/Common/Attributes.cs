using System;

namespace Frame.Common
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventAttribute : Attribute
    {
        public readonly EventType eventType;

        public EventAttribute(EventType eventType)
        {
            this.eventType = eventType;
        }
    }
}
using System;
using Frame.Common;
using Godot;
using ValueType = Frame.Common.ValueType;

namespace Frame.Logic
{
    public class LogicBuff : LogicBase<Node>
    {
        public override ValueType ValueType => ValueType.Buff;


        protected override void Ready(Node entity)
        {
            entity.LoginBehaviorCondition<BehaviorTranslate>(CanTranslate);
            
            entity.LoginBehaviorExecutor<BehaviorAddBuff>(AddBuff);
            
        }

        protected override void Dispose(Node entity)
        {
            entity.LogoutBehaviorCondition<BehaviorTranslate>(CanTranslate);
            
            entity.LogoutBehaviorExecutor<BehaviorAddBuff>(AddBuff);
        }
        
        private void AddBuff(Node entity, BehaviorAddBuff behavior)
        {
            // 添加计时。
            if (behavior.interval > 0)
            {
                var timerName = GetBuffTimerName(entity, behavior.buffType, true);
                GameFrame.Timer.StartNew(entity, behavior.interval, timerName, true);
            }

            if (behavior.duration > 0)
            {
                var timerName = GetBuffTimerName(entity, behavior.buffType, false);
                GameFrame.Timer.StartNew(entity, behavior.duration, timerName, false);
            }
            
        }

        private bool CanTranslate(Node entity, BehaviorTranslate behavior)
        {
            return !HasBuff(entity, BuffType.Stun) && !HasBuff(entity, BuffType.Freeze);
        }

        public static bool HasBuff(Node entity, BuffType type, bool isRepeat = false)
        {
            var name = GetBuffTimerName(entity, type, isRepeat);
            return GameFrame.Timer.HasTimer(name);
        }

        public static string GetBuffTimerName(Node entity, BuffType type, bool isRepeat = false)
        {
            var expand = isRepeat ? "interval" : "duration";
            return $"{entity.GetInstanceId()}_{type}_{expand}";
        }
        
        [Event(EventType.Timeout)]
        public static void OnTimeout(object owner, string timerName, bool isRepeat)
        {
            if (!(owner is Node entity))
            {
                return;
            }

            var split = timerName.Split('_');
            if (split.Length < 3)
            {
                return;
            }
            
            if (!Enum.TryParse(split[1], out BuffType buffType))
            {
                return;
            }

            switch (buffType)
            {
                case BuffType.Stun:
                    break;
                case BuffType.Burn:
                    break;
                case BuffType.Freeze:
                    break;
                case BuffType.Poison:
                    break;
            }
            
        }

    }
}
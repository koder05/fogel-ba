using System;

namespace RF.Common.UI
{
    public static class UIWaitOverdoor
    {
        private static readonly object sync = new object();
        private static IUIWaitOverdoorBehavior _behavior;
        private static int counter = 0;

        public static void SetBehavior(IUIWaitOverdoorBehavior behavior)
        {
            lock (sync)
                _behavior = behavior;
        }

        public static void On()
        {
            if (_behavior == null)
                throw new InvalidOperationException("Не задана реализация UI ширмы.");

            lock (sync)
            {
                _behavior.OverdoorOn();
                counter++;
            }
        }

        public static void Off()
        {
            if (_behavior == null)
                throw new InvalidOperationException("Не задана реализация UI ширмы.");
            
            lock (sync)
            {
                counter--;
                if (counter <= 0)
                {
                    counter = 0;
                    _behavior.OverdoorOff();
                }
            }
        }
    }
}

using System;

namespace RF.Common.UI
{
    public static class UIErrorOverdoor
    {
        private static readonly object sync = new object();
        private static IUIErrorOverdoorBehavior _behavior;

        public static void SetBehavior(IUIErrorOverdoorBehavior behavior)
        {
            lock (sync)
                _behavior = behavior;
        }

        public static void Show(Exception ex)
        {
            if (_behavior == null)
                throw new InvalidOperationException("Не задана реализация UI обработчика ошибок.");
            _behavior.ShowError(ex);
        }
    }
}

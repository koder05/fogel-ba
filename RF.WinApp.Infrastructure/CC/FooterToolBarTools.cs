using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace RF.WinApp
{
    public class EditFormButtonIsEnabledConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var editFormIsNotEmpty = (bool)values[0];
            var insertFormIsOpened = (bool)values[1];
            return editFormIsNotEmpty && !insertFormIsOpened;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

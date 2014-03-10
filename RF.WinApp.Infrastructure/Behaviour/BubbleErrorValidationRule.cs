using System.Globalization;
using System.Windows.Controls;

namespace RF.WinApp.Infrastructure.Behaviour
{
    public class BubbleErrorValidationRule : ValidationRule
    {
        private ValidationError _error;
        public BubbleErrorValidationRule(ValidationError error)
        {
            _error = error;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (_error != null)
                return _error.RuleInError.Validate(_error.BindingInError, cultureInfo);
            return ValidationResult.ValidResult;
        }
    }
}

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using DA = System.ComponentModel.DataAnnotations;

namespace RF.WinApp
{
    public class DataAnnotationsValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            BindingExpression be = value as BindingExpression;
            if (be == null)
                return ValidationResult.ValidResult;

            Binding b = be.ParentBinding;

            string propertyName = b.Path.Path.Replace("Model.", "");

            var model = be.DataItem;
            var dobj = model as RF.WinApp.JIT.DataObj;
            if (dobj != null)
                model = dobj.Model;

            string error = string.Empty;
            error = Validate(model, propertyName);

            if (string.IsNullOrEmpty(error) == false)
                return new ValidationResult(false, error);

            return ValidationResult.ValidResult;
        }

        protected virtual string Validate(object model, string propertyName)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(propertyName))
                return error;

            string curPropName = propertyName;
            object curmodel = model;
            object value = curmodel;
            

            string[] propTree = propertyName.Split('.');
            foreach (string prop in propTree)
            {
                var pi = value.GetType().GetProperty(prop);
                if (pi == null)
                    break;

                curPropName = prop;
                curmodel = value;
                value = pi.GetValue(value, null);
            }

            var results = new List<DA.ValidationResult>(1);
            var validCtx = new DA.ValidationContext(curmodel, null, null) { MemberName = curPropName };
            bool result = DA.Validator.TryValidateProperty(value, validCtx, results);
            if (!result)
            {
                DA.ValidationResult validationResult = results.First();
                error = validationResult.ErrorMessage;
            }

            return error;
        }
    }
}

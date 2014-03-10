//-----------------------------
//--------------- http://www.codeproject.com/Articles/456589/Bindable-Converter-Parameter
//-----------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows;
using System.Text.RegularExpressions;

namespace RF.WinApp
{
    [ContentProperty("Bindings")]
    public class BcpBinding : MarkupExtension
    {
        private const string BINDING_EXTRACT_PATH_REGEX_PATTERN = @"Path=(?<path>\(?([A-Za-z0-9]+:)?([A-Za-z0-9]+)?(\[[0-9]+\])?(\.)?([A-Za-z0-9]+)?\)?)";
        private const string ENUM_REGEX_PATTERN = @"Enum [A-Za-z0-9\.]+";
        private const string ENUM_EXTRACT_ENUM_TYPE_NAME_REGEX_PATTERN = @"Enum (?<typename>[A-Za-z0-9\.]+)";
        private const string BINDING_OR_ENUM_REGEX_PATTERN = "(" + "Binding" + ")?" + "(" + ENUM_REGEX_PATTERN + ")?";

        private const string BINDING_EXTRACT_ELEMENT_NAME_REGEX_PATTERN = @"ElementName=(?<elementname>([A-Za-z0-9]+))";

        public BcpBinding()
        {
            Mode = BindingMode.OneWay;
        }

        private static Dictionary<DependencyObject, List<BcpBinding>> dictAllDependencyObjectsBcpBindings = new Dictionary<DependencyObject, List<BcpBinding>>();

        public string Bindings { get; set; }
        public string ConverterParameters { get; set; }
        public object Converter { get; set; }
        public string Path { get; set; }
        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }
        public BindingMode Mode { get; set; }
        public string ElementName { get; set; }

        public DependencyProperty TargetDependencyProperty { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            FrameworkElement TargetObject = pvt.TargetObject as FrameworkElement;
            if (!dictAllDependencyObjectsBcpBindings.ContainsKey(TargetObject)) // this should allow us to register to Initialized only one time per object
            {
                TargetObject.Initialized += TargetObject_Initialized;
                TargetObject.Unloaded += TargetObject_Unloaded;
                dictAllDependencyObjectsBcpBindings.Add(TargetObject, new List<BcpBinding>());
            }
            this.TargetDependencyProperty = pvt.TargetProperty as DependencyProperty;
            dictAllDependencyObjectsBcpBindings[TargetObject].Add(this);


            return null;// there is no actual need to return value, as we will change this dependency-property
        }

        void TargetObject_Initialized(object sender, EventArgs e)
        {
            DependencyObject TargetDependencyObject = sender as DependencyObject;

            foreach (var objBcpBinding in dictAllDependencyObjectsBcpBindings[TargetDependencyObject])
            {
                UpdateDependencyObjectDependencyPropertyWithAttachedPropertiesBasedReplacement(TargetDependencyObject, objBcpBinding);
            }
        }

        void TargetObject_Unloaded(object sender, RoutedEventArgs e)
        {
            ////SOME MEMORY CLEANUP
            (sender as FrameworkElement).Initialized -= TargetObject_Initialized;
            (sender as FrameworkElement).Unloaded -= TargetObject_Unloaded;
            dictAllDependencyObjectsBcpBindings.Remove(sender as DependencyObject);
        }

        private void UpdateDependencyObjectDependencyPropertyWithAttachedPropertiesBasedReplacement(DependencyObject item, BcpBinding BcpBindingN)
        {
            DependencyProperty dp = BcpBindingN.TargetDependencyProperty;

            List<DependencyProperty> ConverterParameters = new List<DependencyProperty>();
            List<DependencyProperty> Bindings = new List<DependencyProperty>();

            //attached prop for two-way binding operations
            DependencyProperty apIsSourceChanged = GetOrCreateAttachedProperty(dp.Name + "IsSourceChanged", typeof(bool), false);

            //attached prop that stores all ConverterParameters-bindings
            DependencyProperty apConverterParameters = GetOrCreateAttachedProperty(dp.Name + "ConverterParameters", typeof(List<DependencyProperty>), new List<DependencyProperty>());
            //attached prop that stores all MultiBinding-bindings
            DependencyProperty apBindings = GetOrCreateAttachedProperty(dp.Name + "Bindings", typeof(List<DependencyProperty>), new List<DependencyProperty>());

            DependencyProperty apConverter = GetOrCreateAttachedProperty(dp.Name + "Converter", typeof(object), null);
            item.SetValue(apConverter, BcpBindingN.Converter);
            DependencyProperty apEvaluatedResult =
                GetOrCreateAttachedProperty(dp.Name + "EvaluatedResult", typeof(object), null, apEvaluatedResultChanged);

            Binding bindingOrigDpToEvaluatedResult = new Binding("(" + typeof(BindingBase).Name + "." + dp.Name + "EvaluatedResult)");
            bindingOrigDpToEvaluatedResult.Source = item;
            bindingOrigDpToEvaluatedResult.Mode = BcpBindingN.Mode;
            bindingOrigDpToEvaluatedResult.UpdateSourceTrigger = this.UpdateSourceTrigger;
            BindingOperations.SetBinding(item, dp, bindingOrigDpToEvaluatedResult);
            
            string[] arrConverterParameters;
            if (BcpBindingN.ConverterParameters.Contains(','))
            {
                arrConverterParameters = BcpBindingN.ConverterParameters.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                var bindings = Regex.Split(BcpBindingN.ConverterParameters, @"Binding ").Where(s => s != "").Select(s => "Binding " + s);
                arrConverterParameters = bindings.ToArray();
            }

            for (int i = 0; i < arrConverterParameters.Length; i++)
            {
                DependencyProperty apConverterParameterBindingSource =
                        GetOrCreateAttachedProperty(dp.Name + "_ConverterParameterBindingSource" + i.ToString(), typeof(object), null, apConverterParameterSourceChanged);
                string spath = Regex.Match(arrConverterParameters[i], BINDING_EXTRACT_PATH_REGEX_PATTERN).Groups["path"].Value;
                string selementname = Regex.Match(arrConverterParameters[i], BINDING_EXTRACT_ELEMENT_NAME_REGEX_PATTERN).Groups["elementname"].Value;
                Binding b = new Binding(spath);
                if (selementname == "Self")
                    b.Source = item;
                else if (selementname == "TemplatedParent")
                    b.RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
                else if (selementname != "")
                    b.ElementName = selementname;
                BindingOperations.SetBinding(item, apConverterParameterBindingSource, b);
                ConverterParameters.Add(apConverterParameterBindingSource);
            }

            item.SetValue(apConverterParameters, ConverterParameters);

            // for single-binding scenarios >>> make it similar to multibinding syntax(with single binding)
            if (BcpBindingN.Path != null)
            {
                BcpBindingN.Bindings = "Binding " + (BcpBindingN.ElementName != "" ? "ElementName=" + BcpBindingN.ElementName : "") + " Path=" + BcpBindingN.Path;
            }
            else
            {
                if (!BcpBindingN.Bindings.Contains("Binding"))
                {
                    BcpBindingN.Bindings = "Binding Path=" + BcpBindingN.Bindings;
                }
            }

            //bind each Binding in the MultiBinding to special dp (instead of the original dp)
            var binds = Regex.Split(BcpBindingN.Bindings, @"Binding ").Where(s => s != "");
            int i1 = 0;
            foreach (var binding in binds)
            {
                string spath = Regex.Match(binding, BINDING_EXTRACT_PATH_REGEX_PATTERN).Groups["path"].Value;
                string selementname = Regex.Match(binding, BINDING_EXTRACT_ELEMENT_NAME_REGEX_PATTERN).Groups["elementname"].Value;
                DependencyProperty apBinding = GetOrCreateAttachedProperty(dp.Name + "_Binding" + i1.ToString(), typeof(object), null, apMultiBindingAnySourceChanged);
                Binding NewBindingToSource = new Binding(spath);
                if (selementname == "Self")
                    NewBindingToSource.Source = item;
                else if (selementname == "TemplatedParent")
                    NewBindingToSource.RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
                else if (selementname != "")
                    NewBindingToSource.ElementName = selementname;

                NewBindingToSource.Mode = BcpBindingN.Mode;
                BindingOperations.SetBinding(item, apBinding, NewBindingToSource);
                Bindings.Add(apBinding);
                i1++;
            }
            //save all bindings-dps into apBindings
            item.SetValue(apBindings, Bindings);
        }

        private static void apMultiBindingAnySourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            string dpName = e.Property.Name.Split('_')[0];
            DependencyProperty apIsSourceChanged = GetOrCreateAttachedProperty(dpName + "IsSourceChanged");
            obj.SetValue(apIsSourceChanged, true);
            DependencyProperty apConverterParameters = GetOrCreateAttachedProperty(dpName + "ConverterParameters");
            DependencyProperty apBindings = GetOrCreateAttachedProperty(dpName + "Bindings");
            List<DependencyProperty> ConverterParameters = (List<DependencyProperty>)obj.GetValue(apConverterParameters);// new List<DependencyProperty>();
            List<DependencyProperty> Bindings = (List<DependencyProperty>)obj.GetValue(apBindings);//


            IEnumerable<object> v1 = ConverterParameters.Select(dpcp => { return obj.GetValue(dpcp); });
            IEnumerable<object> v2 = Bindings.Select(dpcp => { return obj.GetValue(dpcp); });

            DependencyProperty apConverter = GetOrCreateAttachedProperty(dpName + "Converter");
            DependencyProperty apEvaluatedResult = GetOrCreateAttachedProperty(dpName + "EvaluatedResult");

            object converter = obj.GetValue(apConverter);
            if (converter is IMultiValueConverter)
            {
                obj.SetValue(apEvaluatedResult, (converter as IMultiValueConverter).Convert(v2.ToArray(), null, v1.ToArray(), null));
            }
            else
            {
                obj.SetValue(apEvaluatedResult, (converter as IValueConverter).Convert(e.NewValue, null, v1.ToArray(), null));
            }
            obj.SetValue(apIsSourceChanged, false);
        }

        private static void apConverterParameterSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            string dpName = e.Property.Name.Split('_')[0];
            DependencyProperty apIsSourceChanged = GetOrCreateAttachedProperty(dpName + "IsSourceChanged");
            obj.SetValue(apIsSourceChanged, true);
            DependencyProperty apConverterParameters = GetOrCreateAttachedProperty(dpName + "ConverterParameters");
            DependencyProperty apBindings = GetOrCreateAttachedProperty(dpName + "Bindings");
            List<DependencyProperty> ConverterParameters = (List<DependencyProperty>)obj.GetValue(apConverterParameters);// new List<DependencyProperty>();
            List<DependencyProperty> Bindings = (List<DependencyProperty>)obj.GetValue(apBindings);//

            IEnumerable<object> v1 = ConverterParameters.Select(dpcp => { return obj.GetValue(dpcp); });
            IEnumerable<object> v2 = Bindings.Select(dpcp => { return obj.GetValue(dpcp); });

            DependencyProperty apConverter = GetOrCreateAttachedProperty(dpName + "Converter");
            DependencyProperty apEvaluatedResult = GetOrCreateAttachedProperty(dpName + "EvaluatedResult");

            object converter = obj.GetValue(apConverter);
            if (converter is IMultiValueConverter)
            {
                obj.SetValue(apEvaluatedResult, (converter as IMultiValueConverter).Convert(v2.ToArray(), null, v1.ToArray(), null));
            }
            else if (v2.Count() > 0)
            {
                obj.SetValue(apEvaluatedResult, (converter as IValueConverter).Convert(v2.ElementAt(0), null, v1.ToArray(), null));
            }
            else
            {
                obj.SetValue(apEvaluatedResult, (converter as IValueConverter).Convert(null, null, v1.ToArray(), null));
            }
            obj.SetValue(apIsSourceChanged, false);
        }

        private static void apEvaluatedResultChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            string dpName = e.Property.Name.Replace("EvaluatedResult", "");
            DependencyProperty apIsSourceChanged = GetOrCreateAttachedProperty(dpName + "IsSourceChanged");
            if (!(bool)obj.GetValue(apIsSourceChanged))
            {
                // change didn't come from source>>> target got changed in two-way binding
                // change source via convert back
                DependencyProperty apConverterParameters = GetOrCreateAttachedProperty(dpName + "ConverterParameters");
                List<DependencyProperty> ConverterParameters = (List<DependencyProperty>)obj.GetValue(apConverterParameters);// new List<DependencyProperty>();
                IEnumerable<object> v1 = ConverterParameters.Select(dpcp => { return obj.GetValue(dpcp); });
                DependencyProperty apBindings = GetOrCreateAttachedProperty(dpName + "Bindings");
                List<DependencyProperty> Bindings = (List<DependencyProperty>)obj.GetValue(apBindings);//

                DependencyProperty apConverter = GetOrCreateAttachedProperty(dpName + "Converter");
                object converter = obj.GetValue(apConverter);
                if (converter is IValueConverter)
                {
                    object ret = (obj.GetValue(apConverter) as IValueConverter).ConvertBack(e.NewValue, null, v1.ToArray(), null);
                    obj.SetValue(Bindings[0], ret);
                }
                else
                {
                    object[] ret = (obj.GetValue(apConverter) as IMultiValueConverter).ConvertBack(e.NewValue, null, v1.ToArray(), null);
                    for (int i = 0; i < ret.Length; i++)
                    {
                        obj.SetValue(Bindings[i], ret[i]);
                    }
                }
            }
        }

        // this Dictionary holds every Attached-Property we've created, so we can use it later
        private static Dictionary<string, DependencyProperty> RegistredDependencyProperties = new Dictionary<string, DependencyProperty>();// = new KeyValuePair<string, DependencyProperty>();
        private static DependencyProperty GetOrCreateAttachedProperty(string sDpName, Type t = null, object defaulevalue = null
                , PropertyChangedCallback changedCallback = null, CoerceValueCallback coerceCallBack = null)
        {
            if (RegistredDependencyProperties.ContainsKey(sDpName))
            {
                return RegistredDependencyProperties[sDpName];
            }
            else
            {
                DependencyProperty dp = DependencyProperty.RegisterAttached(sDpName, t, typeof(BindingBase), new PropertyMetadata(defaulevalue, changedCallback, coerceCallBack));
                RegistredDependencyProperties.Add(sDpName, dp);
                return dp;
            }
        }
    }
}

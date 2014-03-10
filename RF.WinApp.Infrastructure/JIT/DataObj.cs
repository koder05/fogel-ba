using System;
using System.ComponentModel;

namespace RF.WinApp.JIT
{
    public class DataObj : INotifyPropertyChanged, IDataErrorInfo
    {
        private object _model;
        public object Model 
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Model"));
            }
        }

        public Guid StaticStateVersion { get; set; }

        private bool _isEditing = false;
        public bool IsEditing
        {
            get
            {
                return _isEditing;
            }
            set
            {
                _isEditing = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditing"));
            }
        }

        //public static readonly DependencyProperty IDProperty = DependencyProperty.Register("ID", typeof(int), typeof(DataObj), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        //public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(DataObj), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        //public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register("IsEditing", typeof(bool), typeof(DataObj), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get 
            { 
                throw new NotImplementedException(); 
            }
        }
    }
}

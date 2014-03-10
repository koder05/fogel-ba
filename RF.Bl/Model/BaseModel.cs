using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RF.BL.Model
{
    public class BaseModel : INotifyPropertyChanged
    {
        public BaseModel(Guid id)
        {
            this.Id = id;
        }

        public BaseModel()
            : this(Guid.NewGuid())
        {
        }

        private Guid _id;
        public virtual Guid Id 
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public virtual BaseModel ShallowCopy()
        {
            return (BaseModel)this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            var g = obj as BaseModel;
            return g != null && this.Id == g.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}

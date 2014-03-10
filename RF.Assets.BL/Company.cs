using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using RF.Common;
using RF.BL.Model.Enums;

namespace RF.BL.Model
{
	public class Company : BaseModel
	{
        private string _name;
        [Required(ErrorMessage = "Требуется название УК")]
        public string Name 
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                this.OnPropertyChanged("Name");
            }
        }

        private byte _lawFormValue;
		public byte lawFormValue
        {
            get
            {
                return _lawFormValue;
            }
            protected set
            {
                _lawFormValue = value;
                this.OnPropertyChanged("lawFormValue");
            }
        }

		public LawFormType LawForm 
		{
			get
			{
				return (LawFormType)lawFormValue;
			}
			set
			{
				lawFormValue = (byte)value;
			}
		}

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", Utils.GetEnumDescription<LawFormType>(this.LawForm), Name).Trim();
            }
        }
	}
}

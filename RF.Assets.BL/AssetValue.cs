using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using RF.Common;
using RF.BL.Model.Enums;

namespace RF.BL.Model
{
    public class AssetValue : BaseModel
    {
        private DateTime _takingDate;
        public DateTime TakingDate
        {
            get
            {
                return _takingDate;
            }
            set
            {
                _takingDate = value;
                this.OnPropertyChanged("TakingDate");
            }
        }

        private decimal _value;
        public decimal Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                this.OnPropertyChanged("Value");
            }
        }

        private decimal _cashFlow;
        public decimal CashFlow
        {
            get
            {
                return _cashFlow;
            }
            set
            {
                _cashFlow = value;
                this.OnPropertyChanged("CashFlow");
            }
        }

        private Guid _governorId;
        public Guid GovernorId
        {
            get
            {
                return _governorId;
            }
            set
            {
                _governorId = value;
                this.OnPropertyChanged("GovernorId");
            }
        }

        private Governor _governor;
        [Required(ErrorMessage="Требуется указать УК")]
        public virtual Governor Governor
        {
            get
            {
                return _governor;
            }
            set
            {
                _governor = value;
                this.OnPropertyChanged("Governor");
            }
        }

        private byte _insuranceTypeValue;
        public byte InsuranceTypeValue 
        {
            get
            {
                return _insuranceTypeValue;
            }
            set
            {
                _insuranceTypeValue = value;
                this.OnPropertyChanged("InsuranceTypeValue");
            }
        }

        public InsuranceType InsuranceType
        {
            get
            {
                return (InsuranceType)InsuranceTypeValue;
            }
            set
            {
                InsuranceTypeValue = (byte)value;
            }
        }

        public string InsuranceTypeString
        {
            get
            {
                return Utils.GetEnumDescription<InsuranceType>(this.InsuranceType);
            }
        }

        public override BaseModel ShallowCopy()
        {
            var ret = new AssetValue();
            ret.Id = this.Id;
            ret.TakingDate = this.TakingDate;
            ret.Value = this.Value;
            ret.CashFlow = this.CashFlow;
            ret.InsuranceTypeValue = this.InsuranceTypeValue;
            ret.GovernorId = this.GovernorId;
            ret.Governor = this.Governor;
            return ret;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RF.BL.Model
{
    public class Governor : BaseModel
    {
        private Guid _companyId;
        public Guid CompanyId
        {
            get
            {
                return _companyId;
            }
            set
            {
                _companyId = value;
                this.OnPropertyChanged("CompanyId");
            }
        }

        public virtual Company Company { get; set; }

        private string _shortName;
        [Required(ErrorMessage = "Требуется короткое имя УК")]
        public string ShortName
        {
            get
            {
                return _shortName;
            }
            set
            {
                _shortName = value;
                this.OnPropertyChanged("ShortName");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.BL.Model
{
    public class Zl : BaseModel
    {
        public virtual int KodPs { get; private set; }
        public virtual string InsCert { get; private set; }
        public virtual string LN { get; private set; }
        public virtual string FN { get; private set; }
        public virtual string SN { get; private set; }
        public virtual string GenderName { get; private set; }
        public virtual Person Person { get; set; }
    }
}

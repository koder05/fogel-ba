using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.BL.Model
{
    public class Person : BaseModel
    {
        public virtual string LN { get; private set; }
        public virtual string FN { get; private set; }
        public virtual string SN { get; private set; }

        public string ImpersonalFullName
        {
            get
            {
                return string.Format("{0} {1} {2}.", this.FN, this.SN, this.LN[0]); 
            }
        }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1} {2}.", this.LN, this.FN, this.SN);
            }
        }

        public Person()
            : base()
        {
        }

        public Person(string ln, string fn, string sn)
            : base()
        {
            this.LN = ln;
            this.FN = fn;
            this.SN = sn;
        }
    }
}

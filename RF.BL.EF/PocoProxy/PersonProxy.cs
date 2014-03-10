using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.BL.Model;
using EF.Poco;

namespace RF.BL.EF.PocoProxy
{
    public class PersonProxy : Person, IPocoProxy
    {
        public BasePoco Poco
        {
            get { return _poco; }
        }

        private PersonPoco _poco;

        public PersonProxy(PersonPoco poco)
            : base()
        {
            if (poco == null)
                throw new InvalidOperationException("poco");

            if (poco.Id == Guid.Empty) poco.Id = this.Id; else this.Id = poco.Id;

            this._poco = poco;
        }

        public override string FN
        {
            get
            {
                return _poco.FN;
            }
        }

        public override string LN
        {
            get
            {
                return _poco.LN;
            }
        }

        public override string SN
        {
            get
            {
                return _poco.SN;
            }
        }
    }
}

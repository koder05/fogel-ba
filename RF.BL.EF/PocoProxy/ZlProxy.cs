using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.BL.Model;
using EF.Poco;

namespace RF.BL.EF.PocoProxy
{
    public class ZlProxy : Zl, IPocoProxy
    {
        public BasePoco Poco
        {
            get { return _poco; }
        }

        private ZlPoco _poco;

        public ZlProxy(ZlPoco poco)
			: base()
		{
			if (poco == null)
				throw new InvalidOperationException("poco");

			this._poco = poco;
		}

        public override int KodPs
        {
            get
            {
                return _poco.KodPs;
            }
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

        public override string GenderName
        {
            get
            {
                return _poco.GenderName;
            }
        }

        public override string InsCert
        {
            get
            {
                return _poco.InsCert;
            }
        }

        private Person _p = null;
        public override Person Person
        {
            get
            {
                if (_p == null && _poco.Persons.Count > 0)
                    _p = ProxyActivator.CreateProxy<PersonPoco, Person>(_poco.Persons.ElementAt(0));
                return _p;
            }
            set
            {
                _p = value;

                IPocoProxy proxy = value as IPocoProxy;
                if (proxy == null)
                    throw new InvalidOperationException("Raw reference model is not supported, only proxy instance.");
                _poco.Persons.Clear();
                _poco.Persons.Add((PersonPoco)proxy.Poco); 
            }
        }
    }
}

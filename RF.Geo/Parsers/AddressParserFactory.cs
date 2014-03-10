using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Geo.Parsers
{
	public class AddressParserFactory
	{
		public IAddressParser GetParser(string initString)
		{
			if(KozedubAddressParser.KozedubAddressRx.IsMatch(initString))
				return new KozedubAddressParser(initString);

			return new AddressParser(initString);
            
		}
	}
}

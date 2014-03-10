using System.Collections.Generic;

using RF.Geo.BL;

namespace RF.Geo.Parsers
{
	public interface IAddressParser
	{
		Addr Parse();
		IEnumerable<Addr> AddressFindList { get; }
		string SourceAddressString { get; }
	}
}

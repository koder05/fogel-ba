using System;
using System.Text;
using System.Security.Cryptography;

namespace RF.Common.Security
{
	/// <summary>
	/// Summary description for hash.
	/// </summary>
	public class Hash
	{
		static public byte[] GenerateHash(string password, byte[] salt)
		{
			byte[] pwdBytes = Encoding.Unicode.GetBytes(password);
			byte[] res = pwdBytes;

			if (salt != null && salt.Length > 0)
			{
				res = new byte[salt.Length + pwdBytes.Length];
				salt.CopyTo(res, 0);
				pwdBytes.CopyTo(res, salt.Length);
			}

			byte[] hashBytes = new SHA1Managed().ComputeHash(res);
			return hashBytes;
		}
	}
}
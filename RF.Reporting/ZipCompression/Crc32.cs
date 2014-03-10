// Crc32.cs
//
// Implements the CRC algorithm, which is used in zip files.  The zip format calls for
// the zipfile to contain a CRC for the unencrypted byte stream of each file.
//
// It is based on example source code published at
//    http://www.vbaccelerator.com/home/net/code/libraries/CRC32/Crc32_zip_CRC32_CRC32_cs.asp
//
// This implementation adds a tweak of that code for use within zip creation.  While
// computing the CRC we also compress the byte stream, in the same read loop. This
// avoids the need to read through the uncompressed stream twice - once to computer CRC
// and another time to compress.
//
//
// Thu, 30 Mar 2006  13:58
// 

using System;
using System.IO;

namespace RF.ZipCompression
{
	/// <summary>
	/// Calculates a 32bit Cyclic Redundancy Checksum (CRC) using the
	/// same polynomial used by Zip.
	/// </summary>
	internal class CRC32
	{
		private const int BufferSize = 8192;

		private Int32 m_TotalBytesRead = 0;
		private UInt32[] m_crc32Table;

		public Int32 TotalBytesRead
		{
			get
			{
				return m_TotalBytesRead;
			}
		}

		/// <summary>
		/// Returns the CRC32 for the specified stream.
		/// </summary>
		/// <param name="input">The stream over which to calculate the CRC32</param>
		/// <returns>the CRC32 calculation</returns>
		public UInt32 GetCrc32(Stream input)
		{
			return GetCrc32AndCopy(input, null);
		}

		/// <summary>
		/// Returns the CRC32 for the specified stream, and writes the input into the output stream.
		/// </summary>
		/// <param name="input">The stream over which to calculate the CRC32</param>
		/// <param name="output">The stream into which to deflate the input</param>
		/// <returns>the CRC32 calculation</returns>
		public UInt32 GetCrc32AndCopy(Stream input, Stream output)
		{
			unchecked
			{
				UInt32 crc32Result;
				crc32Result = 0xFFFFFFFF;
				byte[] buffer = new byte[BufferSize];
				int readSize = BufferSize;

				m_TotalBytesRead = 0;
				int count = input.Read(buffer, 0, readSize);
				if (output != null) output.Write(buffer, 0, count);
				m_TotalBytesRead += count;
				while (count > 0)
				{
					for (int i = 0; i < count; i++)
					{
						crc32Result = ((crc32Result) >> 8) ^ m_crc32Table[(buffer[i]) ^ ((crc32Result) & 0x000000FF)];
					}
					count = input.Read(buffer, 0, readSize);
					if (output != null) output.Write(buffer, 0, count);
					m_TotalBytesRead += count;

				}

				return ~crc32Result;
			}
		}


		/// <summary>
		/// Construct an instance of the CRC32 class, pre-initialising the table
		/// for speed of lookup.
		/// </summary>
		public CRC32()
		{
			unchecked
			{
				// This is the official polynomial used by CRC32 in PKZip.
				// Often the polynomial is shown reversed as 0x04C11DB7.
				UInt32 dwPolynomial = 0xEDB88320;
				UInt32 i, j;

				m_crc32Table = new UInt32[256];

				UInt32 dwCrc;
				for (i = 0; i < 256; i++)
				{
					dwCrc = i;
					for (j = 8; j > 0; j--)
					{
						if ((dwCrc & 1) == 1)
						{
							dwCrc = (dwCrc >> 1) ^ dwPolynomial;
						}
						else
						{
							dwCrc >>= 1;
						}
					}
					m_crc32Table[i] = dwCrc;
				}
			}
		}
	}

}

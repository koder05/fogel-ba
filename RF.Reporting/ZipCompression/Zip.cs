/*
Zip.cs

This class reads and writes zip files, according to the format
described by pkware, at:
http://www.pkware.com/business_and_developers/developer/popups/appnote.txt

This implementation is based on the
System.IO.Compression.DeflateStream base class in the .NET Framework
v2.0 base class library.

There are other Zip class libraries available.  For example, it is
possible to read and write zip files within .NET via the J# runtime.
But some people don't like to install the extra DLL.  Also, there is
a 3rd party LGPL-based (or is it GPL?) library called SharpZipLib,
which works, in both .NET 1.1 and .NET 2.0.  But some people don't
like the GPL. Finally, there are commercial tools (From ComponentOne,
XCeed, etc).  But some people don't want to incur the cost.

This alternative implementation is not GPL licensed, is free of cost,
and does not require J#.

It does require .NET 2.0 (for the DeflateStream class).  

Notes:
This is at best a cripppled and naive implementation. 



Bugs:
1. does not do 0..9 compression levels (not supported by DeflateStream)
2. does not do encryption
3. no support for reading or writing multi-disk zip archives
4. no support for file comments or archive comments
5. does not stream as it compresses; all compressed data is kept in memory.
6. no support for double-byte chars in filenames
7. no support for asynchronous operation

But it does read and write basic zip files, and it gets reasonable compression. 

NB: PKWare's zip specification states: 

----------------------
  PKWARE is committed to the interoperability and advancement of the
  .ZIP format.  PKWARE offers a free license for certain technological
  aspects described above under certain restrictions and conditions.
  However, the use or implementation in a product of certain technological
  aspects set forth in the current APPNOTE, including those with regard to
  strong encryption or patching, requires a license from PKWARE.  Please 
  contact PKWARE with regard to acquiring a license.
----------------------
   
Fri, 31 Mar 2006  14:43

*/

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using RF.Reporting;
using RF.Common;

namespace RF.ZipCompression
{
	internal static class Shared
	{
		private const char s_Backslash = '\\';
		public static string ReplaceInvalidFileChars(string s)
		{
			if (s == null)
				throw new ArgumentNullException("s");

			s = s.Replace('"', "'"[0]);

			Encoding oem = GetOemEncoding();
			byte[] bytes = oem.GetBytes(s);
			s = oem.GetString(bytes);
			s = s.Replace('?', '-');
			s = s.Replace('<', "'"[0]);
			s = s.Replace('>', "'"[0]);

			List<char> chars = new List<char>(Path.GetInvalidFileNameChars());
			if (chars.Contains(s_Backslash))
				chars.Remove(s_Backslash);

			foreach (char c in chars)
				s = s.Replace(c, '-');

			return s;
		}
		public static string StringFromBuffer(byte[] buf)
		{
			return GetOemEncoding().GetString(buf);
		}

		public static byte[] StringToBytes(string s)
		{
			return GetOemEncoding().GetBytes(s);
		}

		private static Encoding GetOemEncoding()
		{
			return Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);
		}

		public static int ReadSignature(Stream s)
		{
			BinaryReader br = new BinaryReader(s);
			int signature = br.ReadInt32();
			return signature;
		}

		public static DateTime PackedToDateTime(Int32 packedDateTime)
		{
			Int16 packedTime = (Int16)(packedDateTime & 0x0000ffff);
			Int16 packedDate = (Int16)((packedDateTime & 0xffff0000) >> 16);

			int year = 1980 + ((packedDate & 0xFE00) >> 9);
			int month = (packedDate & 0x01E0) >> 5;
			int day = packedDate & 0x001F;

			int hour = (packedTime & 0xF800) >> 11;
			int minute = (packedTime & 0x07E0) >> 5;
			int second = packedTime & 0x001F;

			DateTime dt = DateTime.MinValue;
			try
			{
				dt = new DateTime(year, month, day, hour, minute, second, 0);
			}
			catch
			{
				dt = DateTime.Now;
			}

			return dt;
		}

		public static Int32 DateTimeToPacked(DateTime dt)
		{
			UInt16 packedDate = (UInt16)((dt.Day & 0x0000001F) | ((dt.Month << 5) & 0x000001E0) | (((dt.Year - 1980) << 9) & 0x0000FE00));
			UInt16 packedTime = (UInt16)((dt.Second & 0x0000001F) | ((dt.Minute << 5) & 0x000007E0) | ((dt.Hour << 11) & 0x0000F800));
			return (Int32)(((UInt32)(packedDate << 16)) | packedTime);
		}

		public static string RelativePathTo(string fromDirectory, string toPath)
		{
			if (fromDirectory == null)
				throw new ArgumentNullException("fromDirectory");

			if (toPath == null)
				throw new ArgumentNullException("toPath");

			if (Path.IsPathRooted(fromDirectory) && Path.IsPathRooted(toPath))
			{
				if (string.Compare(Path.GetPathRoot(fromDirectory), Path.GetPathRoot(toPath), true) != 0)
					throw new ArgumentException(string.Format("The paths '{0} and '{1}' have different path roots.", fromDirectory, toPath));
			}

			StringCollection relativePath = new StringCollection();
			string[] fromDirectories = fromDirectory.Split(System.IO.Path.DirectorySeparatorChar);
			string[] toDirectories = toPath.Split(System.IO.Path.DirectorySeparatorChar);
			int length = Math.Min(fromDirectories.Length, toDirectories.Length);
			int lastCommonRoot = -1;
			// find common root
			for (int x = 0; x < length; x++)
			{
				if (string.Compare(fromDirectories[x], toDirectories[x], true) != 0)
					break;
				lastCommonRoot = x;
			}
			if (lastCommonRoot == -1)
				throw new ArgumentException(string.Format("The paths '{0} and '{1}' do not have a common prefix path.", fromDirectory, toPath));

			// add relative folders in from path
			for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)
				if (fromDirectories[x].Length > 0)
					relativePath.Add("..");

			// add to folders to path
			for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)
				relativePath.Add(toDirectories[x]);

			// create relative path
			string[] relativeParts = new string[relativePath.Count];
			relativePath.CopyTo(relativeParts, 0);
			string newPath = string.Join(Path.DirectorySeparatorChar.ToString(), relativeParts);
			return newPath;
		}
	}

	internal class ZipDirEntry
	{
		internal const int ZipDirEntrySignature = 0x02014b50;

		private DateTime m_LastModified;
		private string m_FileName;
		private string m_Comment;
		private Int16 m_VersionMadeBy;
		private Int16 m_VersionNeeded;
		private Int16 m_CompressionMethod;
		private int m_CompressedSize;
		private int m_UncompressedSize;
		private Int16 m_BitField;
		private int m_LastModDateTime;

		private int m_Crc32;
		private byte[] m_Extra;

		private ZipDirEntry()
		{
		}

		public DateTime LastModified
		{
			get { return m_LastModified; }
		}

		public string FileName
		{
			get { return m_FileName; }
		}

		public string Comment
		{
			get { return m_Comment; }
		}

		public Int16 VersionMadeBy
		{
			get { return m_VersionMadeBy; }
		}

		public Int16 VersionNeeded
		{
			get { return m_VersionNeeded; }
		}

		public Int16 CompressionMethod
		{
			get { return m_CompressionMethod; }
		}

		public int CompressedSize
		{
			get { return m_CompressedSize; }
		}

		public int UncompressedSize
		{
			get { return m_UncompressedSize; }
		}

		internal ZipDirEntry(ZipEntry ze) { }

		public static ZipDirEntry Read(Stream s)
		{
			int signature = Shared.ReadSignature(s);
			// return null if this is not a local file header signature
			if (SignatureIsNotValid(signature))
			{
				s.Seek(-4, SeekOrigin.Current);
				return null;
			}

			ZipDirEntry zde = new ZipDirEntry();

			BinaryReader br = new BinaryReader(s);

			zde.m_VersionMadeBy = br.ReadInt16();
			zde.m_VersionNeeded = br.ReadInt16();
			zde.m_BitField = br.ReadInt16();
			zde.m_CompressionMethod = br.ReadInt16();
			zde.m_LastModDateTime = br.ReadInt32();
			zde.m_Crc32 = br.ReadInt32();
			zde.m_CompressedSize = br.ReadInt32();
			zde.m_UncompressedSize = br.ReadInt32();

			zde.m_LastModified = Shared.PackedToDateTime(zde.m_LastModDateTime);

			Int16 fileNameLength = br.ReadInt16();
			Int16 extraFieldLength = br.ReadInt16();
			Int16 commentLength = br.ReadInt16();
			Int16 diskNumber = br.ReadInt16();
			Int16 internalFileAttrs = br.ReadInt16();
			Int32 externalFileAttrs = br.ReadInt32();
			Int32 offset = br.ReadInt32();

			byte[] block = br.ReadBytes(fileNameLength);
			if (block.Length != fileNameLength)
				throw new Exception("Invalid filename block.");

			zde.m_FileName = Shared.StringFromBuffer(block);

			zde.m_Extra = br.ReadBytes(extraFieldLength);
			if (zde.m_Extra.Length != extraFieldLength)
				throw new Exception("Invalid extra block.");

			block = br.ReadBytes(commentLength);
			if (block.Length != commentLength)
				throw new Exception("Invalid comment block.");

			zde.m_Comment = Shared.StringFromBuffer(block);
			return zde;
		}

		private static bool SignatureIsNotValid(int signature)
		{
			return (signature != ZipDirEntrySignature);
		}
	}

	public class ZipEntry
	{
		private const int ZipEntrySignature = 0x04034b50;
		private const Int16 FixedVersionNeeded = 0x14;// from examining existing zip files
		private const Int16 DeflateCompressionMethod = 0x08; // 0x08 = Deflate			

		private DateTime m_LastModified = DateTime.Now;
		private string m_FileName;
		private string m_PhysicalFileName;
		private Int16 m_VersionNeeded;
		private Int16 m_BitField;
		private Int16 m_CompressionMethod;
		private Int32 m_CompressedSize;
		private Int32 m_UncompressedSize;
		private Int32 m_Crc32;
		private byte[] m_Extra;
		private byte[] m_FileData;
		private byte[] m_Header;
		private int m_RelativeOffsetOfHeader;

		private MemoryStream m_UnderlyingMemoryStream;
		private Stream m_InputStream;

		public ZipEntry()
		{
		}

		public long Size
		{
			get { return m_InputStream != null ? m_InputStream.Length : 0; }
		}

		public DateTime LastModified
		{
			get { return m_LastModified; }
		}

		public string FileName
		{
			get { return m_FileName; }
		}

		public Int16 VersionNeeded
		{
			get { return m_VersionNeeded; }
		}

		public Int16 BitField
		{
			get { return m_BitField; }
		}

		public Int16 CompressionMethod
		{
			get { return m_CompressionMethod; }
		}

		public Int32 CompressedSize
		{
			get { return m_CompressedSize; }
		}

		public Int32 UncompressedSize
		{
			get { return m_UncompressedSize; }
		}


		private byte[] FileData
		{
			get
			{
				return m_FileData;
			}
		}

		internal byte[] Header
		{
			get
			{
				return m_Header;
			}
		}

		private static bool ReadHeader(Stream s, ZipEntry ze)
		{
			int signature = Shared.ReadSignature(s);

			// return null if this is not a local file header signature
			if (SignatureIsNotValid(signature))
			{
				s.Seek(-4, SeekOrigin.Current);
				return false;
			}

			BinaryReader br = new BinaryReader(s);

			ze.m_VersionNeeded = br.ReadInt16();
			ze.m_BitField = br.ReadInt16();
			ze.m_CompressionMethod = br.ReadInt16();
			ze.m_LastModified = Shared.PackedToDateTime(br.ReadInt32());
			ze.m_Crc32 = br.ReadInt32();
			ze.m_CompressedSize = br.ReadInt32();
			ze.m_UncompressedSize = br.ReadInt32();

			Int16 filenameLength = br.ReadInt16();
			Int16 extraFieldLength = br.ReadInt16();

			byte[] block = br.ReadBytes(filenameLength);
			if (block.Length != filenameLength)
				throw new Exception("Invalid fileName block.");

			ze.m_FileName = Shared.StringFromBuffer(block);

			ze.m_Extra = br.ReadBytes(extraFieldLength);
			if (ze.m_Extra.Length != extraFieldLength)
				throw new Exception("Invalid extra block.");

			return true;
		}

		private static bool SignatureIsNotValid(int signature)
		{
			return signature != ZipEntry.ZipEntrySignature;
		}

		public static ZipEntry Read(Stream s)
		{
			ZipEntry entry = new ZipEntry();
			if (!ReadHeader(s, entry))
				return null;

			entry.m_FileData = new byte[entry.CompressedSize];
			int n = s.Read(entry.FileData, 0, entry.FileData.Length);
			if (n != entry.FileData.Length)
				throw new Exception("Badly formed zip file.");

			return entry;
		}

		internal static ZipEntry Create(string[] fileNameParts, string physicalFileName, Stream inputStream)
		{
			if (fileNameParts == null)
				throw new ArgumentNullException("fileNameParts");

			if (fileNameParts.Length == 0)
				throw new ArgumentException("fileNameParts length must be > 0.", "fileNameParts");

			string fileName = "";
			foreach (string part in fileNameParts)
			{
				if (part == null)
					throw new ArgumentException("fileNameParts array cannot contain null values", "fileNameParts");

				fileName += Shared.ReplaceInvalidFileChars(part) + "/";
			}

			fileName = fileName.Substring(0, fileName.Length - 1);

			ZipEntry entry = new ZipEntry();
			entry.m_FileName = fileName;
			entry.m_PhysicalFileName = physicalFileName;

			if (inputStream == null)
			{
				if (physicalFileName == null)
					throw new ArgumentNullException("physicalFileName");

				entry.m_LastModified = File.GetLastWriteTime(physicalFileName);
			}
			else
			{
				entry.m_InputStream = inputStream;
			}

			return entry;
		}

		internal static ZipEntry Create(string fileName, string physicalFileName, Stream inputStream)
		{
			if (fileName == null)
				throw new ArgumentNullException("fileName");

			return Create(new string[] { fileName }, physicalFileName, inputStream);
		}

		public void Extract()
		{
			Extract(".");
		}

		public void Extract(string baseDirectory)
		{
			string targetFileName = Path.Combine(baseDirectory, this.FileName);

			using (MemoryStream ms = new MemoryStream(this.FileData))
			{
				using (DeflateStream input = new DeflateStream(ms, CompressionMode.Decompress))
				{
					// ensure the target path exists
					if (!Directory.Exists(Path.GetDirectoryName(targetFileName)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(targetFileName));
					}

					using (FileStream output = new FileStream(targetFileName, FileMode.CreateNew))
					{
						byte[] bytes = new byte[4096];
						while (true)
						{
							int bytesRead = input.Read(bytes, 0, bytes.Length);
							if (bytesRead > 0)
								output.Write(bytes, 0, bytesRead);

							else
								break;
						}
					}

					// We may have to adjust the last modified time to compensate
					// for differences in how the .NET Base Class Library deals
					// with daylight saving time (DST) versus how the Windows
					// filesystem deals with daylight saving time.
					if (LastModified.IsDaylightSavingTime())
					{
						DateTime AdjustedLastModified = LastModified + new TimeSpan(1, 0, 0);
						File.SetLastWriteTime(targetFileName, AdjustedLastModified);
					}
					else
						File.SetLastWriteTime(targetFileName, LastModified);
				}
			}
		}

		internal void WriteCentralDirectoryEntry(Stream s)
		{
			BinaryWriter bw = new BinaryWriter(s);

			// signature
			bw.Write(ZipDirEntry.ZipDirEntrySignature);

			// Version Made By
			bw.Write(ZipEntry.FixedVersionNeeded);

			// Version Needed, Bitfield, compression method, lastmod,
			// crc, sizes, filename length and extra field length -
			// are all the same as the local file header. So just copy them
			for (int j = 0; j < 26; j++)
				bw.Write(Header[4 + j]);

			// File Comment Length
			bw.Write((ushort)0);

			// Disk number start
			bw.Write((ushort)0);

			// internal file attrs
			// TODO: figure out what is required here.
			bw.Write((byte)1);
			bw.Write((byte)0);

			// external file attrs
			// TODO: figure out what is required here. 
			bw.Write((byte)0x20);
			bw.Write((byte)0);
			bw.Write((byte)0xb6);
			bw.Write((byte)0x81);

			// relative offset of local header (I think this can be zero)
			bw.Write(m_RelativeOffsetOfHeader);

			// actual filename (starts at offset 34 in header)
			bw.Write(Shared.StringToBytes(this.FileName));
		}

		private Stream GetInputStream()
		{
			if (this.m_InputStream != null)
				return this.m_InputStream;

			return File.OpenRead(this.m_PhysicalFileName);
		}

		private void WriteHeader(Stream s)
		{
			MemoryStream ms = new MemoryStream();
			// write the header info
			BinaryWriter bw = new BinaryWriter(ms);

			// signature
			bw.Write(ZipEntry.ZipEntrySignature);

			// version needed
			bw.Write(ZipEntry.FixedVersionNeeded);

			// bitfield
			Int16 bitField = 0x00; // from examining existing zip files
			bw.Write(bitField);

			// compression method
			bw.Write(ZipEntry.DeflateCompressionMethod);

			// LastMod
			bw.Write(Shared.DateTimeToPacked(m_LastModified));

			// CRC32 (Int32)
			CRC32 crc32 = new CRC32();
			UInt32 crc = 0;
			this.m_UnderlyingMemoryStream = new MemoryStream();
			using (DeflateStream comprStream = new DeflateStream(this.m_UnderlyingMemoryStream, CompressionMode.Compress, true))
			{
				using (BufferedStream wbs = new BufferedStream(comprStream, 65536))
				{
					using (Stream input = GetInputStream())
						crc = crc32.GetCrc32AndCopy(input, wbs);
				}
			}

			bw.Write(crc);

			// CompressedSize (Int32)
			Int32 compressedSize = (Int32)m_UnderlyingMemoryStream.Length;
			bw.Write(compressedSize);

			// UncompressedSize (Int32)
			bw.Write(crc32.TotalBytesRead);

			// filename length (Int16)
			Int16 length = (Int16)FileName.Length;
			bw.Write(length);

			// extra field length (short)
			Int16 extraFieldLength = 0x00;
			bw.Write(extraFieldLength);

			// actual filename
			byte[] fileNameBuffer = Shared.StringToBytes(this.FileName);
			bw.Write(fileNameBuffer, 0, fileNameBuffer.Length);

			// extra field (we always write null in this implementation)
			// ;;

			// remember the file offset of this header
			m_RelativeOffsetOfHeader = (int)s.Position;

			byte[] bytes = ms.ToArray();

			// finally, write the header to the stream
			s.Write(bytes, 0, bytes.Length);

			// preserve this header data for use with the central directory structure.
			m_Header = bytes;
		}

		internal void Write(Stream s)
		{
			byte[] bytes = new byte[4096];
			int n;

			// write the header:
			this.WriteHeader(s);

			// write the actual file data: 
			this.m_UnderlyingMemoryStream.Position = 0;

			while ((n = m_UnderlyingMemoryStream.Read(bytes, 0, bytes.Length)) != 0)
			{
				s.Write(bytes, 0, n);
			}

			m_UnderlyingMemoryStream = null;
			m_InputStream = null;
		}
	}

	internal class ZipEntryCollection : KeyedCollection<string, ZipEntry>
	{
		protected override string GetKeyForItem(ZipEntry item)
		{
			return item.FileName;
		}
	}

	public class ZipFile : IEnumerable<ZipEntry>
	{
		public static string ZipContentType = Utils.GetEnumDescription<ContentType>(ContentType.Zip);

		private string m_FileName;
		private ZipEntryCollection m_Entries = new ZipEntryCollection();
		private List<ZipDirEntry> m_DirEntries = new List<ZipDirEntry>();

		public string FileName
		{
			get { return m_FileName; }
		}

		public ZipFile()
		{
		}

		#region For Writing Zip Files

		public ZipFile(string newZipFileName)
		{
			// create a new zipfile
			this.m_FileName = newZipFileName;
			if (File.Exists(m_FileName))
				throw new IOException(string.Format("File \"{0}\" already exists.", newZipFileName));
		}

		public void AddFile(string fileName)
		{
			FileInfo fi = new FileInfo(fileName);
			if (fi.Exists == false)
				throw new FileNotFoundException(string.Format("File \"{0}\" not found.", fileName));

			this.AddFile(fi.Name, fi.FullName);
		}

		private void AddFile(string fileName, string physicalFileName)
		{
			ZipEntry ze = ZipEntry.Create(fileName, physicalFileName, null);
			this.m_Entries.Add(ze);
		}

		public void AddFileWithContent(string fileNameInZip, string content)
		{
			AddFileWithContent(new string[] { fileNameInZip }, content);
		}

		public void AddFileWithContent(string[] fileNameInZipPathParts, string content)
		{
			AddFileWithContent(fileNameInZipPathParts, content, new UTF8Encoding(true));
		}

		public void AddFileWithContent(string[] fileNameInZipPathParts, string content, Encoding encoding)
		{
			if (encoding == null)
				throw new ArgumentNullException("encoding");

			MemoryStream ms = new MemoryStream();
			byte[] bom = encoding.GetPreamble();
			ms.Write(bom, 0, bom.Length);
			byte[] contentBytes = encoding.GetBytes(content);
			ms.Write(contentBytes, 0, contentBytes.Length);
			ms.Position = 0;
			this.AddFileWithContent(fileNameInZipPathParts, ms);
		}

		public void AddFileWithContent(string fileNameInZip, byte[] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");

			AddFileWithContent(new string[] { fileNameInZip }, buffer);
		}

		public void AddFileWithContent(string[] fileNameInZipPathParts, byte[] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");

			MemoryStream ms = new MemoryStream(buffer);
			this.AddFileWithContent(fileNameInZipPathParts, ms);
		}

		public void AddFileWithContent(string fileNameInZip, Stream inputStream)
		{
			if (fileNameInZip == null)
				throw new ArgumentNullException("fileNameInZip");

			AddFileWithContent(new string[] { fileNameInZip }, inputStream);
		}

		public void AddFileWithContent(string[] fileNameInZipPathParts, Stream inputStream)
		{
			if (fileNameInZipPathParts == null)
				throw new ArgumentNullException("fileNameInZipPathParts");

			ZipEntry ze = ZipEntry.Create(fileNameInZipPathParts, null, inputStream);
			this.m_Entries.Add(ze);
		}

		public void AddDirectory(string directoryName)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(directoryName);
			string parentDir = (dirInfo.Parent != null) ? dirInfo.Parent.FullName : directoryName;
			this.AddDirectory(dirInfo.Parent.FullName, directoryName);
		}

		private void AddDirectory(string baseDirectory, string directoryName)
		{
			string[] filenames = Directory.GetFiles(directoryName);
			foreach (string filename in filenames)
			{
				string relativeFilePath = Shared.RelativePathTo(baseDirectory, filename);
				this.AddFile(relativeFilePath, filename);
			}

			string[] childDirs = Directory.GetDirectories(directoryName);
			foreach (string childDir in childDirs)
				this.AddDirectory(baseDirectory, childDir);
		}

		public void Save()
		{
			if (this.m_FileName == null)
				throw new InvalidOperationException("FileName must be provided in ZipFile constructor.");

			Stream writeStream = new FileStream(m_FileName, FileMode.CreateNew);
			this.Save(writeStream);
		}

		public void Save(Stream writeStream)
		{
			if (writeStream == null)
				throw new ArgumentNullException("writeStream");

			using (writeStream)
			{
				// an entry for each file
				foreach (ZipEntry e in this.m_Entries)
					e.Write(writeStream);

				this.WriteCentralDirectoryStructure(writeStream);
			}
		}

		public byte[] SaveToBytes()
		{
			MemoryStream ms = new MemoryStream();
			this.Save(ms);
			return ms.ToArray();
		}

		private void WriteCentralDirectoryStructure(Stream writeStream)
		{
			// the central directory structure
			long start = writeStream.Position;
			foreach (ZipEntry e in m_Entries)
				e.WriteCentralDirectoryEntry(writeStream);

			long finish = writeStream.Position;

			// now, the footer
			WriteCentralDirectoryFooter(writeStream, start, finish - start);
		}

		private void WriteCentralDirectoryFooter(Stream writeStream, long startOfCentralDirectory, long centralDirectoryByteCount)
		{
			// signature
			const uint EndOfCentralDirectorySignature = 0x06054b50;
			BinaryWriter bw = new BinaryWriter(writeStream);
			bw.Write(EndOfCentralDirectorySignature);

			// number of this disk
			bw.Write((ushort)0);

			// number of the disk with the start of the central directory
			bw.Write((ushort)0);
			// total number of entries in the central dir on this disk
			bw.Write((ushort)m_Entries.Count);

			// total number of entries in the central directory
			bw.Write((ushort)m_Entries.Count);

			// size of the central directory
			uint sizeOfCentralDirectory = (uint)centralDirectoryByteCount;
			bw.Write(sizeOfCentralDirectory);

			// offset of the start of the central directory 
			uint startOffset = (uint)startOfCentralDirectory;  // cast down from Long
			bw.Write(startOffset);

			// zip comment length
			bw.Write((ushort)0);
		}

		#endregion

		#region For Reading Zip Files

		/// <summary>
		/// This will throw if the zipfile does not exist. 
		/// </summary>
		public static ZipFile Read(string zipFileName)
		{
			if (zipFileName == null)
				throw new ArgumentNullException("zipFileName");

			ZipFile zf = new ZipFile();
			zf.m_FileName = zipFileName;
			using (Stream readStream = File.OpenRead(zipFileName))
			{
				ZipEntry e;
				while ((e = ZipEntry.Read(readStream)) != null)
				{
					zf.m_Entries.Add(e);
				}

				// read the zipfile's central directory structure here.

				ZipDirEntry de;
				while ((de = ZipDirEntry.Read(readStream)) != null)
				{
					zf.m_DirEntries.Add(de);
				}
			}

			return zf;
		}

		public IEnumerator<ZipEntry> GetEnumerator()
		{
			foreach (ZipEntry e in m_Entries)
				yield return e;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void ExtractAll(string path)
		{
			ExtractAll(path, false);
		}

		public void ExtractAll(string path, bool wantVerbose)
		{
			bool header = wantVerbose;
			foreach (ZipEntry e in m_Entries)
			{
				if (header)
				{
					Console.WriteLine("\n{0,-20} {1,-22} {2,-6} {3,4}   {4,-8}", "Name", "Modified", "Size", "Ratio", "Packed");
					Console.WriteLine(new System.String('-', 72));
					header = false;
				}
				if (wantVerbose)
					Console.WriteLine("{0,-20} {1,-22} {2,-6} {3,4:F0}%   {4,-8}",
								 e.FileName,
								 e.LastModified.ToString("yyyy-MM-dd HH:mm:ss"),
								 e.UncompressedSize,
								 100 * (1.0 - (1.0 * e.CompressedSize) / (1.0 * e.UncompressedSize)),
								 e.CompressedSize);
				e.Extract(path);
			}
		}

		public void Extract(string fileName)
		{
			if (fileName == null)
				throw new ArgumentNullException("fileName");

			fileName = Shared.ReplaceInvalidFileChars(fileName);
			this.m_Entries[fileName].Extract();
		}

		public ZipEntry this[string fileName]
		{
			get
			{
				if (fileName == null)
					throw new ArgumentNullException("fileName");

				fileName = Shared.ReplaceInvalidFileChars(fileName);

				if (this.m_Entries.Contains(fileName))
					return this.m_Entries[fileName];

				return null;
			}
		}

		#endregion

	}
}

/*
 Example usage: 
 1. Extracting all files from a Zip file: 

     try 
     {
       ZipFile zip= ZipFile.Read(ZipFile); 
       zip.ExtractAll(TargetDirectory, true);
     }
     catch (System.Exception ex1)
     {
       System.Console.Error.WriteLine("exception: " + ex1);
     }

 2. Extracting files from a zip individually:

     try 
     {
       ZipFile zip= ZipFile.Read(ZipFile); 
       foreach (ZipEntry e in zip) 
       {
         e.Extract(TargetDirectory);
       }
     }
     catch (System.Exception ex1)
     {
       System.Console.Error.WriteLine("exception: " + ex1);
     }

 3. Creating a zip archive: 

     try 
     {
       ZipFile zip= new ZipFile(NewZipFile); 

       String[] filenames= System.IO.Directory.GetFiles(Directory); 
       foreach (String filename in filenames) {
         zip.Add(filename);
       }

       zip.Save(); 

     }
     catch (System.Exception ex1)
     {
       System.Console.Error.WriteLine("exception: " + ex1);
     }


 ==================================================================



 Information on the ZIP format:

 From
 http://www.pkware.com/business_and_developers/developer/popups/appnote.txt

  Overall .ZIP file format:

     [local file header 1]
     [file data 1]
     [data descriptor 1]  ** sometimes
     . 
     .
     .
     [local file header n]
     [file data n]
     [data descriptor n]   ** sometimes
     [archive decryption header] 
     [archive extra data record] 
     [central directory]
     [zip64 end of central directory record]
     [zip64 end of central directory locator] 
     [end of central directory record]

 Local File Header format:
         local file header signature     4 bytes  (0x04034b50)
         version needed to extract       2 bytes
         general purpose bit flag        2 bytes
         compression method              2 bytes
         last mod file time              2 bytes
         last mod file date              2 bytes
         crc-32                          4 bytes
         compressed size                 4 bytes
         uncompressed size               4 bytes
         file name length                2 bytes
         extra field length              2 bytes
         file name                       varies
         extra field                     varies


 Data descriptor:  (used only when bit 3 of the general purpose bitfield is set)
         crc-32                          4 bytes
         compressed size                 4 bytes
         uncompressed size               4 bytes


   Central directory structure:

       [file header 1]
       .
       .
       . 
       [file header n]
       [digital signature] 


       File header:
         central file header signature   4 bytes  (0x02014b50)
         version made by                 2 bytes
         version needed to extract       2 bytes
         general purpose bit flag        2 bytes
         compression method              2 bytes
         last mod file time              2 bytes
         last mod file date              2 bytes
         crc-32                          4 bytes
         compressed size                 4 bytes
         uncompressed size               4 bytes
         file name length                2 bytes
         extra field length              2 bytes
         file comment length             2 bytes
         disk number start               2 bytes
         internal file attributes        2 bytes
         external file attributes        4 bytes
         relative offset of local header 4 bytes
         file name (variable size)
         extra field (variable size)
         file comment (variable size)

 End of central directory record:

         end of central dir signature    4 bytes  (0x06054b50)
         number of this disk             2 bytes
         number of the disk with the
         start of the central directory  2 bytes
         total number of entries in the
         central directory on this disk  2 bytes
         total number of entries in
         the central directory           2 bytes
         size of the central directory   4 bytes
         offset of start of central
         directory with respect to
         the starting disk number        4 bytes
         .ZIP file comment length        2 bytes
         .ZIP file comment       (variable size)

 date and time are packed values, as MSDOS did them
 time: bits 0-4 : second
            5-10: minute
            11-15: hour
 date  bits 0-4 : day
            5-8: month
            9-15 year (since 1980)

 see http://www.vsft.com/hal/dostime.htm

*/
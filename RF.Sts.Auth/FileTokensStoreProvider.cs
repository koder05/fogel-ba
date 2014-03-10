using System;
using System.IO;
using System.Web;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace RF.Sts.Auth
{
    public class FileTokensStoreProvider : ITokensStoreProvider
    {
        public T TakeToken<T>(Uri realm)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "Atlas2dot0", "OAuth");
            string filePath = Path.Combine(tempDir, string.Format("tkn_{0}.dat", HttpUtility.UrlEncode(realm.ToString())));

            try
            {
                if (File.Exists(filePath))
                {
                    using (var fileStream = WaitForStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if (fileStream != null)
                        {
                            var formatter = new BinaryFormatter();
                            fileStream.Position = 0;
                            if (fileStream.Length > 0)
                                return (T)formatter.Deserialize(fileStream);
                        }
                    }
                }
            }
            catch
            {
                return default(T);
            }

            return default(T);
        }

        public void PutToken<T>(Uri realm, T rawToken)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "Atlas2dot0", "OAuth");
            string filePath = Path.Combine(tempDir, string.Format("tkn_{0}.dat", HttpUtility.UrlEncode(realm.ToString())));
            Directory.CreateDirectory(tempDir);

            try
            {
                using (FileStream fileStream = WaitForStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    if (fileStream != null)
                    {
                        var formatter = new BinaryFormatter();
                        fileStream.Position = 0;
                        formatter.Serialize(fileStream, rawToken);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Ошибка при кэшировании билета безопасности." + Environment.NewLine + "Ошибка: {0}", ex.Message), ex);
            }
        }

        private FileStream WaitForStream(string filename, FileMode mode, FileAccess fileAccess, FileShare fileShare)
        {
            for (var i = 0; i < 300; i++)
            {
                try
                {
                    return new FileStream(filename, mode, fileAccess, fileShare);
                }
                catch (IOException)
                {
                    Thread.Sleep(50);
                }
            }

            return null;
        }
    }
}

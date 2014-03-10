using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace RF.WinApp.Infrastructure.UIS
{
    public class UISettingsStoreInBinaryFile : IUISettingsStoreProviderAgent
    {
        public Dictionary<string, object> GetSettings(string controlUid)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "Atlas2dot0", "UIS");
            string filePath = Path.Combine(tempDir, string.Format("uis_{0}.dat", controlUid));

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
                                return (Dictionary<string, object>)formatter.Deserialize(fileStream);
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public void PutSettings(string controlUid, Dictionary<string, object> settings)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "Atlas2dot0", "UIS");
            string filePath = Path.Combine(tempDir, string.Format("uis_{0}.dat", controlUid));
            Directory.CreateDirectory(tempDir);

            try
            {
                using (FileStream fileStream = WaitForStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    if (fileStream != null)
                    {
                        var formatter = new BinaryFormatter();
                        fileStream.Position = 0;
                        formatter.Serialize(fileStream, settings);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Ошибка при кэшировании настроек клиента." + Environment.NewLine + "Ошибка: {0}", ex.Message), ex);
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

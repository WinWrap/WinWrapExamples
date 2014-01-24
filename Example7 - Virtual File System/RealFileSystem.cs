using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example7
{
    public class RealFileSystem : WinWrap.Basic.IVirtualFileSystem
    {
        public string Combine(string baseScriptPath, string name)
        {
            if (string.IsNullOrEmpty(baseScriptPath))
                baseScriptPath = Directory.GetCurrentDirectory();
            else
                baseScriptPath = Path.GetDirectoryName(baseScriptPath);

            return Path.GetFullPath(Path.Combine(baseScriptPath, name));
        }

        public void Delete(string scriptPath)
        {
            File.Delete(scriptPath);
        }

        public bool Exists(string scriptPath)
        {
            return File.Exists(scriptPath);
        }

        public string GetCaption(string scriptPath)
        {
            return Path.GetFileName(scriptPath);
        }

        public DateTime GetTimeStamp(string scriptPath)
        {
            return File.GetLastWriteTimeUtc(scriptPath);
        }

        public string Read(string scriptPath)
        {
            return File.ReadAllText(scriptPath);
        }

        public void Write(string scriptPath, string text)
        {
            File.WriteAllText(scriptPath, text, System.Text.Encoding.UTF8);
        }
    }
}

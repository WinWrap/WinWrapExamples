using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//------------------------------------------------------------------------------
// <copyright from='2013' to='2014' company='Polar Engineering and Consulting'>
//    Copyright (c) Polar Engineering and Consulting. All Rights Reserved.
//
//    This file contains confidential material.
//
// </copyright>
//------------------------------------------------------------------------------

namespace Example7
{
    public class MyFileSystem : WinWrap.Basic.IVirtualFileSystem
    {
        private string rootDir = Path.GetFullPath(@"..\..\..\Scripts");

        public string Combine(string baseScriptPath, string name)
        {
            // ignore baseScriptPath in this example
            // all scripts are in the same "directory"
            return name;
        }

        public void Delete(string scriptPath)
        {
            File.Delete(ActualFileName(scriptPath));
        }

        public bool Exists(string scriptPath)
        {
            return File.Exists(ActualFileName(scriptPath));
        }

        public string GetCaption(string scriptPath)
        {
            return Path.GetFileName(ActualFileName(scriptPath));
        }

        public DateTime GetTimeStamp(string scriptPath)
        {
            return File.GetLastWriteTimeUtc(ActualFileName(scriptPath));
        }

        public string Read(string scriptPath)
        {
            return File.ReadAllText(ActualFileName(scriptPath));
        }

        public void Write(string scriptPath, string text)
        {
            File.WriteAllText(ActualFileName(scriptPath), text, System.Text.Encoding.UTF8);
        }

        private string ActualFileName(string scriptPath)
        {
            return Path.Combine(rootDir, scriptPath);
        }
    }
}

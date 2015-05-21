using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Example
{
    public class GlobalQueue
    {
        private const string prefix_ = "Example 9 - AZW Debug - ";
        private string filename_;
        private Mutex mutex_;

        public string Name { get; private set; }

        public GlobalQueue(string name)
        {
            Name = name;
        }

        public void Append(string text)
        {
            Wait();
            File.AppendAllText(filename_, text);
            mutex_.ReleaseMutex();
        }

        public string ReadAll()
        {
            Wait();
            string data = null;
            if (File.Exists(filename_))
            {
                data = File.ReadAllText(filename_);
                File.WriteAllText(filename_, "");
            }

            mutex_.ReleaseMutex();
            return data;
        }

        private void Wait()
        {
            if (mutex_ == null)
            {
                mutex_ = new Mutex(false, prefix_ + Name);
                filename_ = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + prefix_ + Name + ".dat";
            }

            mutex_.WaitOne(Timeout.Infinite);
        }
    }
}
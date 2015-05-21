using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Example
{
    public class ApplicationQueue
    {
        public string Name { get; private set; }

        public ApplicationQueue(string name)
        {
            Name = "ApplicationQueue/" + name;
        }

        public void Append(string text)
        {
            HttpApplicationState appstate = HttpContext.Current.Application;
            appstate.Lock();
            appstate[Name] = (string)appstate[Name] + text;
            appstate.UnLock();
        }

        public string ReadAll()
        {
            HttpApplicationState appstate = HttpContext.Current.Application;
            appstate.Lock();
            string text = (string)appstate[Name];
            appstate[Name] = null;
            appstate.UnLock();
            return text;
        }
    }
}
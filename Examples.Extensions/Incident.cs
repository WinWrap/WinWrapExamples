﻿using System;
using System.Reflection;
using System.Text;

namespace Examples.Extensions
{
    [Scriptable] public class Incident
    {
        private DateTime Datetime { get; set; }

        [Scriptable] public event Action Started;

        [Scriptable] public string FiredBy { private get; set; }
        [Scriptable] public string FilledInBy { private get; set; }
        [Scriptable] public string Data { private get; set; }

        [Scriptable] public void LogMe()
        {
            Datetime = DateTime.Now;
            ScriptingLanguage.Host.Log(ToString());
        }

        /// no [Scriptable] attribute
        /// allows host to fire the event, but not the script
        public void Start(string firedby)
        {
            FiredBy = firedby;
            if (Started != null)
                Started();
        }

        [Scriptable] public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(@"Incident at {0:dddd, MMMM yyyy HH:mm:ss tt zzz}", Datetime));
            sb.AppendLine("  FiredBy: " + FiredBy);
            sb.AppendLine("  FilledInBy: " + FilledInBy);
            if (Data != null)
                sb.AppendLine("  Data: " + Data);
            return sb.ToString();
        }
    }
}

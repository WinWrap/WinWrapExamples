using System;
using System.Reflection;
using System.Text;

//------------------------------------------------------------------------------
// <copyright from='2013' to='2014' company='Polar Engineering and Consulting'>
//    Copyright (c) Polar Engineering and Consulting. All Rights Reserved.
//
//    This file contains confidential material.
//
// </copyright>
//------------------------------------------------------------------------------

namespace Examples.Extensions
{
    [Scriptable]
    public class Incident
    {
        private DateTime Datetime { get; set; }

        [Scriptable]
        public string FiredBy { private get; set; }
        [Scriptable]
        public string FilledInBy { private get; set; }
        [Scriptable]
        public string Data { private get; set; }

        [Scriptable]
        public void LogMe()
        {
            Datetime = DateTime.Now;
            ScriptingLanguage.Host.Log(ToString());
        }

        // no [Scriptable] attribute
        public void Start(IIncidentAction action, string firedby)
        {
            FiredBy = firedby;
            action.Started(this);
        }

        [Scriptable]
        public override string ToString()
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

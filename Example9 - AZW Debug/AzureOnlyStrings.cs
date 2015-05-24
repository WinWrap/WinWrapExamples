using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Example
{
    public static class AzureOnlyStrings
    {
        public static string GetNamedString(string name, string defaultValue = null)
        {
            return GetPatternString(name + @"\(""(.*?)""\)", defaultValue);
        }

        public static string GetPatternString(string pattern, string defaultValue = null)
        {
            // put GitHub\Working-business\WinWrapExamples\examples\examples-string-a675bb8c.txt
            //   in "C:\Users\Public\Public Documents\examples"
            // download Application-a675bb8c.htm to C:\Polar Engineering\WinWrap Basic\Certificates
            string path = DataDirectory() + @"\WinWrapBasic10\examples-strings-a675bb8c.txt";
            if (!File.Exists(path)) return defaultValue;
            string strings = File.ReadAllText(path);
            Regex rgx = new Regex(pattern, RegexOptions.Multiline);
            MatchCollection matches = rgx.Matches(strings);
            Match match = rgx.Match(strings);
            if (match.Groups.Count < 2) return defaultValue;
            string smatch = match.Groups[1].Value.ToString();
            return smatch;
        }

        private static string DataDirectory()
        {
            return AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
        }

    }
}
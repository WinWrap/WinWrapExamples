using System.IO;
using System.Text;

namespace Examples.SharedSource
{
    public static class WinWrapBasic
    {
        public static string FormatError(WinWrap.Basic.Error error)
        {
            var sb = new StringBuilder();
            if (error != null)
            {
                sb.AppendLine(error.File);
                if (File.Exists(error.File))
                {
                    string[] lines = File.ReadAllLines(error.File);
                    string line = lines[error.Line - 1];
                    if (error.Offset >= 0)
                        line = line.Insert(error.Offset, "<here>");
                    line = string.Format(@"{0:00}:{1}", error.Line, line);
                    sb.AppendLine(line);
                }
                sb.AppendLine(error.Text);
                sb.AppendLine("");
            }
            return sb.ToString();
        }

        public static string FormatTimeoutError(WinWrap.Basic.BasicNoUIObj basicNoUIObj, bool timedout)
        {
            // get the line that's executing right now
            var sb = new StringBuilder();
            sb.AppendLine((string)basicNoUIObj.Query("GetStack")["Caller[0]"]);
            sb.AppendLine(timedout ? "time exceeded" : "paused" + ", terminating script.");
            sb.AppendLine("");
            return sb.ToString();
        }
    }
}

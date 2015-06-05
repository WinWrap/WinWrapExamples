using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Example
{
    /// <summary>
    /// Summary description for DebugPortal
    /// </summary>
    public class DebugPortal : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            ApplicationQueue responses = null;

            // read the request
            context.Request.InputStream.Seek(0, SeekOrigin.Begin);
            using (StreamReader sr = new StreamReader(context.Request.InputStream))
            {
                string text = sr.ReadToEnd();
                //Debug.Write(text);
                if (text.StartsWith("Commands:"))
                {
                    int x = text.IndexOf("\r\n");
                    if (x > 0)
                    {
                        int target = 0;
                        if (int.TryParse(text.Substring(9, x - 9), out target))
                        {
                            text = text.Substring(x + 2);
                            ApplicationQueue commands = ApplicationQueue.Create("commands", target.ToString());
                            commands.Append(text);
                            string[] parts = text.Split(new char[] { ' ' }, 2);
                            if (parts.Length == 2)
                            {

                                int id = 0;
                                if (int.TryParse(parts[0], out id))
                                    responses = ApplicationQueue.Create("responses", target.ToString(), id.ToString());
                            }
                        }
                    }
                }
            }

            // send the response
            {
                // Access-Control-Allow-Origin: *
                // Access-Control-Allow-Headers: X-Requested-With
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.AddHeader("Access-Control-Allow-Headers", "X-Requested-With");
                context.Response.ContentType = "text/plain";
                context.Response.ContentEncoding = Encoding.UTF8;
                string text = responses != null ? responses.ReadAll() : null;
                //Debug.Write(text);
                context.Response.Write("Responses:\r\n" + text);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
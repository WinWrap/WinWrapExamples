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
        private GlobalQueue commands_ = new GlobalQueue("commands");
        private GlobalQueue responses_ = new GlobalQueue("responses");

        public void ProcessRequest(HttpContext context)
        {
            // read the request
            context.Request.InputStream.Seek(0, SeekOrigin.Begin);
            using (StreamReader sr = new StreamReader(context.Request.InputStream))
            {
                string text = sr.ReadToEnd();
                //Debug.Write(text);
                if (text.StartsWith("{\"Param\":\"?attach"))
                {
                    // reset queues
                    commands_.ReadAll();
                    responses_.ReadAll();
                }

                commands_.Append(text);
            }

            // send the response
            {
                context.Response.ContentType = "text/plain";
                context.Response.ContentEncoding = Encoding.UTF8;
                string text = responses_.ReadAll();
                //Debug.Write(text);
                context.Response.Write(text);
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
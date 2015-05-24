using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example
{
    /// <summary>
    /// Summary description for Test
    /// </summary>
    public class Test : IHttpHandler
    {
        private ApplicationQueue queue_ = ApplicationQueue.Create("test");
        private ApplicationQueue commands_ = ApplicationQueue.Create("commands");
        private ApplicationQueue responses_ = ApplicationQueue.Create("responses");

        // {"Param":"?attach \"10.30.069/32W{1600} - 5/21/2015 10:09:20 AM\"","Id":-2},
        private const string attach = @"{""Param"":""?attach \""10.30.069/32W{1600} - 5/21/2015 10:09:20 AM\"""",""Id"":-2},";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string data = context.Request["data"];
            string action = context.Request["action"];
            if (action != null)
            {
                switch (action.ToLowerInvariant())
                {
                    case "append":
                        queue_.Append(data);
                        context.Response.Write("Append: '" + data + "'");
                        break;
                    case "readall":
                        data = queue_.ReadAll();
                        context.Response.Write("ReadAll: '" + data + "'");
                        break;
                    case "attach":
                        data = attach + "\r\n";
                        commands_.Append(data);
                        context.Response.Write("attach: data='" + data + "'");
                        break;
#if false
                    case "info":
                        data = "Secret=" + Util.GetNamedString("Secret") + "\r\n" +
                            "QueueConnection=" + Util.GetNamedString("QueueConnection");
                        context.Response.Write(data);
                        break;
#endif
                    default:
                        data = commands_.ReadAll();
                        context.Response.Write("commands: data='" + data + "'");
                        data = responses_.ReadAll();
                        context.Response.Write("responses: data='" + data + "'");
                        break;
                }
            }
            else
            {
                context.Response.Write("data='" + data + "'");
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
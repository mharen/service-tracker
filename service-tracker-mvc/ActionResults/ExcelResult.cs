using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;

namespace service_tracker_mvc.ActionResults
{
    public class ExcelResult : ActionResult
    {

        private Stream ExcelStream;
        private string Filename;
        public ExcelResult(byte[] excel, string filename)
        {
            ExcelStream = new MemoryStream(excel);
            Filename = filename;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/vnd.ms-excel";

            if (!string.IsNullOrEmpty(Filename))
            {
                response.AppendHeader("content-disposition", string.Format("attachment;filename=\"{0}.xls\"", Filename));
            }

            byte[] buffer = new byte[4096];

            while (true)
            {
                int read = this.ExcelStream.Read(buffer, 0, buffer.Length);
                if (read == 0)
                {
                    break;
                }

                response.OutputStream.Write(buffer, 0, read);
            }
            response.End();
        }
    }
}
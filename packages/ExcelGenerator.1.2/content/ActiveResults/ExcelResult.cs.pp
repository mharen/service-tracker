using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace $rootnamespace$.ActionResults
{
    /// <summary>
    /// Custom ActionResult for saving excel files
    /// </summary>
    public class ExcelResult : ActionResult
    {
        private Stream excelStream;
        private String fileName;
        
        /// <summary>
        /// Creates a new ActionResult for saving excel files
        /// </summary>
        /// <param name="excel">byte array from excel workbook</param>
        /// <param name="fileName">string defining file name</param>
        public ExcelResult(byte[] excel, String fileName)
        {
            excelStream = new MemoryStream(excel);
            this.fileName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("content-disposition", "attachment; filename=" + fileName);

            byte[] buffer = new byte[4096];

            while (true)
            {
                int read = this.excelStream.Read(buffer, 0, buffer.Length);
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
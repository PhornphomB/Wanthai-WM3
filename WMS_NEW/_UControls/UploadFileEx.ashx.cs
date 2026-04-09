using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IMS.Web
{
    /// <summary>
    /// Summary description for _UploadImage
    /// </summary>
    public class _UploadImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request.Files.Count == 0)
                    return;

                string savepath = string.Empty;
                string filename = string.Empty;
                string folder_id = string.Empty;

                // Test pass floder_id from client
                if (context.Request.Form["folder_id"] != null)
                {
                    folder_id = context.Request.Form["folder_id"].ToString();
                }

                HttpPostedFile postedFile = context.Request.Files[0];
               
                //savepath = context.Server.MapPath(FieldsStatic.FolderImages + folder_id);
                savepath = context.Server.MapPath(folder_id);

                filename = postedFile.FileName;

                if (!Directory.Exists(savepath))
                    Directory.CreateDirectory(savepath);

                postedFile.SaveAs(savepath + "\\" + filename);

            }
            catch (Exception ex)
            {

                throw ex;
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
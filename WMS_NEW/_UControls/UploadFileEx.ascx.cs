using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls
{
    public enum FileType
    {
        Image,
        File
    }

    public partial class UploadFileEx : UserControl
    {
        string urlUpload = "~/_UControls/UploadFileEx.ashx";
        public string SysID
        {
            get
            {
                if (ViewState["SysID"] == null)
                    ViewState["SysID"] = string.Empty;

                return (string)ViewState["SysID"];
            }
            set
            {
                ViewState["SysID"] = value;
            }
        }

        public string FuncName
        {
            get
            {
                if (ViewState["FuncName"] == null)
                    ViewState["FuncName"] = string.Empty;

                return (string)ViewState["FuncName"];
            }
            set
            {
                ViewState["FuncName"] = value;
            }
        }

        public string ExtensionName
        {
            get
            {
                if (ViewState["ExtensionName"] == null)
                    ViewState["ExtensionName"] = string.Empty;
                //jpg, jpeg, png

                return (string)ViewState["ExtensionName"];
            }
            set
            {
                ViewState["ExtensionName"] = value;
            }
        }

        public string FolderRoot
        {
            get
            {
                if (ViewState["FolderRoot"] == null)
                {
                    var folder_root = FieldsStatic.FolderUpload + this.FuncName + "/";
                    ViewState["FolderRoot"] = folder_root;
                }
                //jpg, jpeg, png

                return (string)ViewState["FolderRoot"];
            }
        }

        public FileType FileTypes { get; set; }



        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    switch (FileTypes)
            //    {
            //        case FileType.Image:
            //            Refresh();
            //            break;
            //        case FileType.File:
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        protected void Page_PreRender(object sender, EventArgs e) //Page Stat working after Load Control events
        {
            try
            {
                if (!string.IsNullOrEmpty(SysID))
                {
                    if (FileTypes == FileType.Image)
                    {
                        Page.ScriptPageRegister("$(document).ready(function(){ bindUpload('" + panelUploader.ClientID + "','" + ResolveUrl(this.urlUpload) + "','" + FolderRoot + "/" + this.SysID + "','" + btRefreshGrid.ClientID + "','" + this.ExtensionName + "'); });", "js_bindUpload");
                    }
                    else
                    {
                        Page.ScriptPageRegister("$(document).ready(function(){ bindUploadFile('" + panelUploader.ClientID + "','" + ResolveUrl(this.urlUpload) + "','" + FolderRoot + "/" + this.SysID + "','" + btRefreshGrid.ClientID + "','" + this.ExtensionName + "'); });", "js_bindUpload");
                    }

                }

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BindingData(string _id)
        {
            this.SysID = _id;
            switch (FileTypes)
            {
                case FileType.Image:
                    break;
                case FileType.File:
                    break;
                default:
                    break;
            }
            Refresh();


        }

        #region Product File,Image Function

        string[] ext = new string[] { ".jpg", ".jpeg", ".png" };
        string dirPath = "";

        List<DTO_UploadFile> ImageList
        {
            get
            {
                if (ViewState["ImageList"] == null)
                    ViewState["ImageList"] = new DTO_UploadFile[0];
                return new List<DTO_UploadFile>((DTO_UploadFile[])ViewState["ImageList"]);
            }
            set
            {
                ViewState["ImageList"] = value.ToArray();
            }
        }

        public void ViewListImage()
        {
            dirPath = Server.MapPath(FolderRoot + this.SysID);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            var fileInfo = new DirectoryInfo(dirPath).GetFiles();

            var files = (from file in fileInfo
                             //where ext.Contains(file.Extension.ToLower())
                         orderby file.LastWriteTime descending
                         select new DTO_UploadFile
                         {
                             FileUrl = (file.Extension.ToLower() == ".xls" || file.Extension.ToLower() == ".xlsx") ? ResolveUrl("~/_images/ico_excel.png") :
                            (file.Extension.ToLower() == ".doc" || file.Extension.ToLower() == ".docx") ? ResolveUrl("~/_images/ico_word.png") :
                            (file.Extension.ToLower() == ".pdf") ? ResolveUrl("~/_images/ico_pdf.png") :
                             ResolveUrl(FolderRoot + this.SysID + "/" + file.Name),
                             LinkUrl = ResolveUrl(FolderRoot + this.SysID + "/" + file.Name),
                             Name = file.Name,
                             Size = Extensions.SizeSuffix(file.Length),
                             Extension = file.Extension.ToLower()

                         }).ToList();

            ImageList = files;
            BindListView();
        }

        void ClearListImage()
        {
            SysID = string.Empty;
            ImageList = (new DTO_UploadFile[0]).ToList();
            BindListView();
        }

        void BindListView()
        {
            listGallery.DataSource = ImageList;
            listGallery.DataBind();
        }

        void Refresh()
        {
            ViewListImage();
            UpdatePanel1.Update();
        }

        protected void btRefreshGrid_Click(object sender, EventArgs e)
        {
            try
            {
                switch (FileTypes)
                {
                    case FileType.Image:
                        break;
                    case FileType.File:
                        break;
                    default:
                        break;
                }
                Refresh();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void listGallery_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            try
            {
                string key = this.listGallery.DataKeys[e.ItemIndex].Value.ToString();
                dirPath = Server.MapPath(FolderRoot + this.SysID);
                File.Delete(dirPath + "\\" + key);

                ViewListImage();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }

    [Serializable()]
    public class DTO_UploadFile
    {
        public string FileUrl { get; set; }
        public string LinkUrl { get; set; }

        public string Name { get; set; }
        public string Size { get; set; }
        public string Extension { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Prototype.Providers.Controls;
using ConfigGlobal.Interface;

namespace _UControls
{
    public partial class PagerGridViewExt : UControlCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.Browser.IsMobileDevice)
            //{
            //    Panel1.Attributes["style"] = "margin-top: 10px;margin-left:15px;";
            //}
            //else if (!Request.Browser.IsMobileDevice)
            //{
            //    Panel1.Attributes["style"] = "margin-top: 0px;margin-left:0px;";
            //}
        }


        #region Custom Property

        public string PagedControlID
        {
            set
            {
                DataPagerExt1.PagedControlID = value;
                DataPagerExt2.PagedControlID = value;
            }
            get
            {
                return DataPagerExt1.PagedControlID;
            }
        }

        public HorizontalAlign Align
        {
            set
            {
                Panel1.HorizontalAlign = value;
            }
            get
            {
                return Panel1.HorizontalAlign;
            }
        }

        #endregion


        protected void LabelTo_DataBinding(object sender, EventArgs e)
        {

        }
        protected void LabelOf_DataBinding(object sender, EventArgs e)
        {

        }
        protected void LabelRows_DataBinding(object sender, EventArgs e)
        {

        }


        public event EventHandler RefreshClick;

        protected void btGridRefresh_Click(object sender, EventArgs e)
        {
            if (RefreshClick != null)
                RefreshClick(sender, e);
        }
    }
}
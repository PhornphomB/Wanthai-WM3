using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace _UControls
{
    public partial class LabelExt : System.Web.UI.UserControl, ConfigGlobal.Interface.IResource
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            labResource.DataBinding += btResource_DataBinding;

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.CssClass))
                    labResource.Attributes.Add("class", this.CssClass);

                if (!string.IsNullOrEmpty(this.AttibuteFor))
                    labResource.Attributes.Add("for", this.AttibuteFor);


                if (!string.IsNullOrEmpty(this.ResourceValue))
                {
                    this.InnerText = this.ResourceValue;
                }
                else if (!string.IsNullOrEmpty(this.InnerText))
                {
                    this.InnerText = this.InnerText;
                }
                else
                {
                    var _text = this.DefaultText;

                    this.InnerText = _text.GetTextAutoCreate();
                }
            }
        }

        void btResource_DataBinding(object sender, EventArgs e)
        {
            if (DataBinding != null)
                DataBinding(sender, e);
        }

        public new event EventHandler DataBinding;

        public string CssClass
        {
            get
            {
                if (ViewState["CssClass"] == null)
                    ViewState["CssClass"] = string.Empty;

                return (string)ViewState["CssClass"];
            }
            set
            {
                ViewState["CssClass"] = value;
            }
        }

        public string InnerText
        {
            get
            {
                if (ViewState["Text"] == null)
                    ViewState["Text"] = string.Empty;

                return (string)ViewState["Text"];
            }
            set
            {
                ViewState["Text"] = value;

                if (labResource != null && labResource.InnerText != value)
                {
                    if (IsPrimary)
                    {
                        labResource.InnerText = value;
                        labResource.Attributes.Add("class", "uk-text-danger uk-text-bold");
                    }
                    else
                    {
                        labResource.InnerText = value;
                    }
                }
            }
        }

        public string AttibuteFor
        {
            get
            {
                if (ViewState["AttibuteFor"] == null)
                    ViewState["AttibuteFor"] = string.Empty;

                return (string)ViewState["AttibuteFor"];
            }
            set
            {
                ViewState["AttibuteFor"] = value;

                if (labResource != null)
                {
                    labResource.Attributes.Add("for", value);
                }
            }
        }

        public string DefaultText
        {
            get
            {
                if (ViewState["DefaultText"] == null)
                    ViewState["DefaultText"] = string.Empty;

                return (string)ViewState["DefaultText"];
            }
            set
            {
                ViewState["DefaultText"] = value;
            }
        }

        public bool IsPrimary
        {
            get
            {
                if (ViewState["IsPrimary"] == null)
                    ViewState["IsPrimary"] = false;

                return (bool)ViewState["IsPrimary"];
            }
            set
            {
                ViewState["IsPrimary"] = value;

                //if (Page.IsPostBack)
                //{
                if (value)
                {
                    labResource.InnerText = InnerText;
                    labResource.Attributes.Add("class", "uk-text-danger uk-text-bold");
                }
                else
                {
                    labResource.InnerText = InnerText;
                    labResource.Attributes.Remove("class");
                }
                //}
            }
        }

        #region Interface Resource

        public string ResourceGroup
        {
            get
            {
                if (ViewState["ResourceGroup"] == null)
                    ViewState["ResourceGroup"] = string.Empty;

                return (string)ViewState["ResourceGroup"];
            }
            set
            {
                ViewState["ResourceGroup"] = value;
            }
        }

        public string ResourceName
        {
            get
            {
                if (ViewState["ResourceName"] == null)
                    ViewState["ResourceName"] = string.Empty;

                return (string)ViewState["ResourceName"];
            }
            set
            {
                ViewState["ResourceName"] = value;
            }
        }

        public string ResourceValue
        {
            get
            {
                if (ViewState["ResourceValue"] == null)
                    ViewState["ResourceValue"] = string.Empty;

                return (string)ViewState["ResourceValue"];
            }
            set
            {
                ViewState["ResourceValue"] = value;

                if (!string.IsNullOrEmpty(value))
                    this.InnerText = value;
            }
        }
        #endregion
    }
}
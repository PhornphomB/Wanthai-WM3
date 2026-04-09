using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls
{
    public partial class ButtonExt : System.Web.UI.UserControl, ConfigGlobal.Interface.IResource
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btResource.DataBinding += btResource_DataBinding;
            btResource.Click += btResource_Click;

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.ResourceValue))
                { btResource.Text = this.ResourceValue; }
                else if (!string.IsNullOrEmpty(this.Text))
                {
                    btResource.Text = this.Text;
                }
                else
                {
                    btResource.Text = this.ResourceName;
                }

                if (!string.IsNullOrEmpty(this.ValidationGroup))
                    btResource.ValidationGroup = this.ValidationGroup;
            }
        }

        void btResource_DataBinding(object sender, EventArgs e)
        {
            if (DataBinding != null)
                DataBinding(sender, e);
        }

        public new event EventHandler DataBinding;
        public event EventHandler Click;

        public string CssClass
        {
            get
            {
                return btResource.CssClass;
            }
            set
            {
                btResource.CssClass = value;
            }
        }
        public string Text
        {
            get
            {
                return btResource.Text;
            }
            set
            {
                btResource.Text = value;
            }
        }

        public string CommandArgument
        {
            get
            {
                return btResource.CommandArgument;
            }
            set
            {
                btResource.CommandArgument = value;
            }
        }
        public string CommandName
        {
            get
            {
                return btResource.CommandName;
            }
            set
            {
                btResource.CommandName = value;
            }
        }

        public Unit Width
        {
            get
            {
                return btResource.Width;
            }
            set
            {
                btResource.Width = value;
            }
        }
        public Unit Height
        {
            get
            {
                return btResource.Height;
            }
            set
            {
                btResource.Height = value;
            }
        }

        public string ValidationGroup
        {
            get
            {
                return btResource.ValidationGroup;
            }
            set
            {
                btResource.ValidationGroup = value;
            }
        }
        public bool CausesValidation
        {
            get
            {
                return btResource.CausesValidation;
            }
            set
            {
                btResource.CausesValidation = value;
            }
        }
        public bool UseSubmitBehavior
        {
            get
            {
                return btResource.UseSubmitBehavior;
            }
            set
            {
                btResource.UseSubmitBehavior = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return btResource.Enabled;
            }
            set
            {
                btResource.Enabled = value;
            }
        }

        public string OnClientClick
        {
            get
            {
                return btResource.OnClientClick;
            }
            set
            {
                btResource.OnClientClick = value;
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
                btResource.Text = value;
            }
        }

        public string DataFieldValue { get ; set; }

        #endregion

        protected void btResource_Click(object sender, EventArgs e)
        {
            if (Click != null)
                Click(sender, e);
        }
    }
}
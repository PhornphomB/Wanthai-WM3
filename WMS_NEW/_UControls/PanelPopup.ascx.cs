using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

using Prototype.Providers;

namespace _UControls
{
    public partial class PanelPopup : UserControl
    {
        #region Template

        private ITemplate _template1 = null;

        [TemplateContainer(typeof(TemplateControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate DataTemplate
        {
            get
            {
                return _template1;
            }
            set
            {
                _template1 = value;
            }
        }

        private ITemplate _template2 = null;

        [TemplateContainer(typeof(TemplateControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate CommandTemplate
        {
            get
            {
                return _template2;
            }
            set
            {
                _template2 = value;
            }
        }

        #endregion


        void Page_Init()
        {
            if (_template1 != null)
            {
                _template1.InstantiateIn(PlaceHolderTemp);
            }

            if (_template2 != null)
            {
                _template2.InstantiateIn(PlaceHolderCommand);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopupInit();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e) //Page Stat working after Load Control events
        {
            if (IsDraggable)
            {
                var clientScript = @" $('document').ready(function () {";
                clientScript += "$('#" + panelContent.ClientID + "').draggable({";
                clientScript += "handle: '#" + contentHead.ClientID + "',";
                clientScript += "containment: 'document'";
                clientScript += "});";
                clientScript += "});";

                Page.ScriptPageRegister(clientScript, panelContent.ClientID + "_Draggable");
            }
        }


        public void PopupInit()
        {

            if (Request.Browser.IsMobileDevice)
                StyleSize = _StyleSize.Large;

            if (StyleSize != _StyleSize.Default)
            {
                var size = StyleSize.GetAttrEntry();
                container.CssClass += " modal-" + size.Name;
            }

            var color = StyleColor.GetAttrEntry();
            container.CssClass += " modal-" + color.Name;

            if (this.ShowDialogNow)
                this.ShowDialog();
            else
                this.HideDialog(false);

            contentCommand.Visible = this.VisibleCommand;

            if (IsDraggable)
            {
                contentHead.Attributes.Add("style", "cursor: move;");
            }
        }

        public PlaceHolder ContentTemplate
        {
            get
            {
                return PlaceHolderTemp;
            }
        }

        public bool VisibleMinimize
        {
            get
            {
                return btMini.Visible;
            }
            set
            {
                btMini.Visible = value;
            }
        }

        #region Optional Propertise

        public string HeaderText
        {
            get
            {
                return labTitle.Text;
            }
            set
            {
                labTitle.Text = value;
                updateTitle.Update();
            }
        }

        public _StyleColor StyleColor { get; set; }
        public _StyleSize StyleSize { get; set; }

        public void UpdateContent()
        {
            updateContent.Update();
        }
        public void UpdateCommand()
        {
            updateCommand.Update();
        }

        public bool ShowDialogNow
        {
            get
            {
                if (ViewState["ShowDialogNow"] == null)
                    ViewState["ShowDialogNow"] = false;

                return (bool)ViewState["ShowDialogNow"];
            }
            set
            {
                ViewState["ShowDialogNow"] = value;
            }
        }
        public bool IsShowDialog
        {
            get
            {
                if (ViewState["IsShowDialog"] == null)
                    ViewState["IsShowDialog"] = false;

                return (bool)ViewState["IsShowDialog"];
            }
            set
            {
                ViewState["IsShowDialog"] = value;
            }
        }
        public bool validateOnCloseClick
        {
            get
            {
                if (ViewState["validateOnCloseClick"] == null)
                    ViewState["validateOnCloseClick"] = false;
                return (bool)ViewState["validateOnCloseClick"];
            }
            set
            {
                ViewState["validateOnCloseClick"] = value;
            }
        }

        public void ShowDialog()
        {
            if (panelTemp.Visible)
                panelTemp.Visible = false;

            DisplayDialog(true);

            IsShowDialog = true;
        }

        public void HideDialog()
        {
            HideDialog(true);
        }
        public void HideDialog(bool _is_close_click)
        {
            if (_is_close_click)
            {
                if (CloseClick != null)
                    CloseClick(null, EventArgs.Empty);
            }
            if (!validateOnCloseClick)
            {
                if (panelTemp.Visible)
                    panelTemp.Visible = false;

                DisplayDialog(false);

                IsShowDialog = false;

            }
        }

        private void DisplayDialog(bool _visible)
        {
            if (myModal.Visible != _visible)
            {
                var page_ms = (Page.Master as WMS_NEW.Layout);

                page_ms.CountPopupShow += _visible ? 1 : -1;

                if (page_ms.CountPopupShow < 0)
                    page_ms.CountPopupShow = 0;

                page_ms.IsTriggerHideScroll = true;
            }

            viewContent.Visible = _visible;
            myModal.Visible = _visible;

            updateFrom.Update();
        }

        public event System.EventHandler CloseClick;

        public bool VisibleCommand
        {
            get
            {
                if (ViewState["VisibleCommand"] == null)
                    ViewState["VisibleCommand"] = true;

                return (bool)ViewState["VisibleCommand"];
            }
            set
            {
                ViewState["VisibleCommand"] = value;
            }
        }

        public bool IsDraggable
        {
            get
            {
                if (ViewState["IsDraggable"] == null)
                    ViewState["IsDraggable"] = true;

                return (bool)ViewState["IsDraggable"];
            }
            set
            {
                ViewState["IsDraggable"] = value;
            }
        }

        #endregion


        protected void btResize_Click(object sender, EventArgs e)
        {
            labTempTitle.Text = labTitle.Text;

            if (!panelTemp.Visible)
            {
                panelTemp.Visible = true;

                DisplayDialog(false);
            }
            else
            {
                panelTemp.Visible = false;

                DisplayDialog(true);
            }
        }

        protected void btClose_Click(object sender, EventArgs e)
        {
            HideDialog();
        }

        protected void btCloseCommand_Click(object sender, EventArgs e)
        {
            HideDialog();
        }
    }
}
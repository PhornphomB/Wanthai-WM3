using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls
{
    public partial class BaseInputControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            filterStrip.FilterChangedValue += filterStrip_FilterChangedValue;
        }

        protected void Page_PreRender(object sender, EventArgs e) //Page Stat working after Load Control events
        {
            if (updateBaseContent != null)
            {
                string css = updateBaseContent.Attributes["class"];

                if (!string.IsNullOrEmpty(css) && !css.Contains("col-lg"))
                {
                    var css_new = css.Replace("col-sm", "col-lg");
                    css_new = "col-sm-6 col-md-4 " + css_new;

                    updateBaseContent.Attributes["class"] = css_new;
                }
            }
        }

        #region Create ITemplate

        void Page_Init()
        {
            if (_template1 != null)
            {
                _template1.InstantiateIn(placeHolderControl);
            }
        }

        private ITemplate _template1 = null;

        [TemplateContainer(typeof(TemplateControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate ControlTemplate
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

        #endregion

        #region Filter Properties

        public event FilterChangedHandler BaseFilterChangedValue;

        void filterStrip_FilterChangedValue(Prototype.Providers.FilterAt _filterAt)
        {
            if (BaseFilterChangedValue != null)
            {
                BaseFilterChangedValue(_filterAt);
                UpdateContent();
            }
        }

        public void SetEnableFilter(Prototype.Providers.FilterAt filter)
        {
            filterStrip.SetEnableFilter(filter);
        }

        public void SetDisableFilter(Prototype.Providers.FilterAt filter)
        {
            filterStrip.SetDisableFilter(filter);
        }

        public global::Prototype.Providers.FilterAt DefaultFilter
        {
            get { return filterStrip.Filter.FilterAt; }
            set
            {
                filterStrip.Filter = new Prototype.Providers.Filter() { FilterAt = value };
            }
        }

        public global::Prototype.Providers.Filter Filter
        {
            get { return filterStrip.Filter; }
            set { filterStrip.Filter = value; }
        }

        public bool FixFilter
        {
            get { return !filterStrip.Enabled; }
            set
            {
                filterStrip.Enabled = !value;
            }
        }

        public bool Filterable
        {
            get { return filterStrip.Visibled; }
            set
            {
                filterStrip.Visibled = value;
                panelFilter.Visible = value;

                if (value)
                {
                    DefaultFilter = Prototype.Providers.FilterAt.Equal;
                }
            }
        }

        public bool IsFilter
        {
            get { return filterStrip.Filter.IsFilter; }
        }

        public void ClearFilter()
        {
            filterStrip.ClearFilter();
        }

        #endregion

        public void UpdateAttributeAdd(string _attrKey, string _attrValue)
        {
            UpdateAttributeRemove(_attrKey);

            updateBaseContent.Attributes.Add(_attrKey, _attrValue);
        }

        public void UpdateAttributeRemove(string _attrKey)
        {
            updateBaseContent.Attributes.Remove(_attrKey);
        }

        public string UpdatePanelID
        {
            get
            {
                return updateBaseContent.ID;
            }
        }
        public string UpdatePanelClientID
        {
            get
            {
                return updateBaseContent.ClientID;
            }
        }

        public void UpdateContent()
        {
            updateBaseContent.Update();
        }

        public LabelExt LabelText
        {
            get
            {
                return lblText;
            }
            set
            {
                lblText = value;
            }
        }

        public bool TextInLine
        {
            get
            {
                if (ViewState["TextInLine"] == null)
                    ViewState["TextInLine"] = false;

                return (bool)ViewState["TextInLine"];
            }
            set
            {
                panelLabel.Attributes.Remove("style");
                panelInput.Attributes.Remove("style");

                if (value && panelLabel != null)
                {
                    panelLabel.Attributes.Add("style", "float:left; margin-right:6px; margin-top:4px;");
                    panelInput.Attributes.Add("style", "float:left;");
                }

                ViewState["TextInLine"] = value;
            }
        }

        public void AddAtributeBaseContent(string _key, string _value)
        {
            updateBaseContent.Attributes.Remove(_key);
            updateBaseContent.Attributes.Add(_key, _value);
        }

        public string BaseContentCss
        {
            get
            {
                return updateBaseContent.Attributes["class"];
            }
            set
            {
                AddAtributeBaseContent("class", value);
            }
        }

        public bool VisibleLabel
        {
            set
            {
                if (value)
                    panelLabel.Attributes.Add("style", "display:none");
                else
                    panelLabel.Attributes.Add("style", "display:block");
            }
        }

        public bool VisibleBaseContent
        {
            get
            {
                return panelBaseContent.Visible;
            }
            set
            {
                panelBaseContent.Visible = value;

                //updateBaseContent.Attributes.Remove("style");
                //updateBaseContent.Attributes.Add("style", !value ? "display:none;" : "display:block;");
                updateBaseContent.Visible = value;
                updateBaseContent.Update();
            }
        }
    }
}
using _UControls.PanelCustom;
using ConfigGlobal.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Prototype.Providers;

namespace _UControls
{
    public delegate void PanelTabIndexHandler(int _index);

    public partial class PanelTab : UControlCustom
    {
        #region Template

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

        void Page_Init()
        {
            if (_template1 != null)
            {
                _template1.InstantiateIn(PlaceHolderControl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                foreach (var panel in PleaceHolderTemplate.Controls.OfType<PanelControlTab>())
                {
                    if (!PanelControls.Any(x => x.ControlID == panel.ControlID))
                        AddPanel((IPanelControlTab)panel);
                }

                #region Initial Resource Language

                var _iResourceList = PanelControls.OfType<IResource>()
                             .Where(qry => !string.IsNullOrEmpty(qry.ResourceGroup) && !string.IsNullOrEmpty(qry.ResourceName)).ToList();

                var _resourceTab = (Page.Master as WMS_NEW.Layout).GetResourceByI(_iResourceList);

                foreach (var res in _resourceTab)
                {
                    var _iResource = _iResourceList.First(qry => qry.ResourceGroup.ToLower() == res.ResourceGroup.ToLower() && qry.ResourceName == res.ResourceName);
                    _iResource.ResourceValue = res.ResourceValue;
                }

                #endregion
            }

            InitialPanel();
        }

        protected void Page_PreRender(object sender, EventArgs e) //Page Stat working after Load Control events
        {
            if (!Page.IsPostBack)
            {

            }
            else //Manual Event Postback of Trigger Control
            {
                var eventControl = Request.Params.Get("__EVENTTARGET");
                var eventArgument = Request.Params.Get("__EVENTARGUMENT");

                if (eventControl == navUcPage.ClientID && eventArgument != string.Empty) // Nav ChangePage
                {
                    ChangeActivePanel(Convert.ToInt32(eventArgument));
                }
            }

            LoadStatePanel();
        }

        public void UpdateContent()
        {
            updateContent.Update();
        }

        public event PanelTabIndexHandler TabIndexChanged;

        public void ChangeActivePanel(int _tabIndex)
        {
            _panelControls = PanelControls;

            var _panelActived = _panelControls.FirstOrDefault(qry => qry.IsActive == true);
            if (_panelActived != null)
            {
                _panelActived.IsActive = false;
            }

            var _panelIsActive = _panelControls.FirstOrDefault(qry => qry.Index == _tabIndex);
            if (_panelIsActive != null)
            {
                _panelIsActive.IsActive = true;
            }

            PanelControls = _panelControls;

            if (TabIndexChanged != null)
                TabIndexChanged(_tabIndex);
        }

        public void VisiblePanel(int _tabIndex, bool _visible)
        {
            var _panel = PanelControls.FirstOrDefault(qry => qry.Index == _tabIndex);
            if (_panel != null)
            {
                var ctrl = PlaceHolderControl.FindControl(_panel.ControlID);

                if (ctrl == null)
                {
                    foreach (Control ctl in PlaceHolderControl.Controls)
                    {
                        if (ctl.ID == _panel.ControlID)
                        {
                            ctrl = ctl;
                            break;
                        }
                    }
                }

                if (ctrl != null)
                {
                    ctrl.Visible = _visible;
                }

                var li = (HtmlGenericControl)navUcPage.FindControl("liNav_" + _panel.Index);
                if (li != null)
                {
                    li.Visible = _visible;
                }
            }
        }

        public void BindingAgain()
        {
            InitialPanel();
        }

        void InitialPanel()
        {
            HtmlGenericControl li;
            HtmlAnchor link;

            int inx = 1;

            _panelControls = PanelControls;
            foreach (var panel in _panelControls)
            {
                if (panel.Index == null) //Initial Value
                {
                    panel.Index = inx;

                    if (inx == 1)
                    {
                        panel.IsActive = true;
                    }
                }

                li = new HtmlGenericControl("li");
                li.ID = "liNav_" + panel.Index;


                link = new HtmlAnchor();
                link.InnerText = (!string.IsNullOrEmpty(panel.ResourceValue) ? panel.ResourceValue : panel.PanelName);

                li.Controls.Add(link);
                navUcPage.Controls.Add(li);

                inx++;
            }

            PanelControls = _panelControls;
        }

        void LoadStatePanel()
        {
            _panelControls = PanelControls;

            foreach (var panel in _panelControls)
            {
                var li = (HtmlGenericControl)navUcPage.FindControl("liNav_" + panel.Index);
                if (li != null)
                {
                    li.Attributes.Remove("class");
                    li.Attributes.Add("class", "nav-item");


                    var link = li.Controls.OfType<HtmlAnchor>().First();
                    link.HRef = "javascript:__doPostBack('" + navUcPage.ClientID + "','" + panel.Index + "')";
                    link.Attributes.Add("class", "nav-link");

                    if (panel.IsActive)
                    {
                        li.Attributes.Add("class", "nav-item active");
                        link.HRef = string.Empty;
                        link.Attributes.Add("class", "nav-link active");

                    }
                }

                var ctrl = PlaceHolderControl.FindControl(panel.ControlID);
                if (ctrl == null)
                {
                    foreach (Control ctl in PlaceHolderControl.Controls)
                    {
                        if (ctl.ID == panel.ControlID)
                        {
                            ctrl = ctl;
                            break;
                        }
                    }
                }

                if (ctrl != null)
                {
                    if (ctrl.Visible != panel.IsActive)
                        ctrl.Visible = panel.IsActive;
                }
            }
        }

        public void AddPanel(PanelControlTab _panel)
        {
            PlaceHolderControl.Controls.Add(_panel);
            AddPanel((IPanelControlTab)_panel);
        }
        public void AddPanel(IPanelControlTab _iPanel)
        {
            if (_panelControls == null)
                _panelControls = new List<PanelTabChild>();
            else
                _panelControls = PanelControls;

            var child = new PanelTabChild();
            var iChild = (IPanelControlTab)child;
            iChild.CloneObjectByInterface(_iPanel, false);

            _panelControls.Add(child);
            PanelControls = _panelControls;
        }

        public void InsertPanel(IPanelControlTab _iPanel, int _index)
        {
            if (_panelControls == null)
                _panelControls = new List<PanelTabChild>();
            else
                _panelControls = PanelControls;

            var child = new PanelTabChild();
            var iChild = (IPanelControlTab)child;
            iChild.CloneObjectByInterface(_iPanel, false);

            _panelControls.Insert(_index, child);
            PanelControls = _panelControls;
        }

        public void Clear()
        {
            PlaceHolderControl.Controls.Clear();

            navUcPage.Controls.Clear();

            _panelControls = new List<PanelTabChild>();
            PanelControls = _panelControls;
        }

        public IEnumerable<IPanelControlTab> Panels
        {
            get
            {
                return PlaceHolderControl.Controls.OfType<IPanelControlTab>();
            }
        }

        public int TabIndexActive
        {
            get
            {
                var _panelActived = PanelControls.FirstOrDefault(qry => qry.IsActive == true);
                if (_panelActived != null)
                {
                    return _panelActived.Index.Value;
                }
                else
                {
                    return 1;
                }
            }
        }

        private List<PanelTabChild> _panelControls = null;
        public List<PanelTabChild> PanelControls
        {
            get
            {
                if (ViewState["PanelControls"] == null)
                    ViewState["PanelControls"] = new PanelTabChild[0];

                return new List<PanelTabChild>((PanelTabChild[])ViewState["PanelControls"]);
            }
            set
            {
                ViewState["PanelControls"] = value.ToArray();
            }
        }

        public PlaceHolder PleaceHolderTemplate
        {
            get
            {
                return PlaceHolderControl;
            }
        }
    }

    [Serializable()]
    public class PanelTabChild : IPanelControlTab
    {
        public PanelTabChild()
        {

        }
        public PanelTabChild(string _panelName, string _controlId)
        {
            PanelName = _panelName;
            ControlID = _controlId;
            IsActive = false;
        }


        public string PanelName { get; set; }
        public int? Index { get; set; }
        public string ControlID { get; set; }
        public bool IsActive { get; set; }

        public string ResourceGroup { get; set; }

        public string ResourceName { get; set; }

        public string ResourceValue { get; set; }
        public string DataFieldValue { get ; set; }
    }
}
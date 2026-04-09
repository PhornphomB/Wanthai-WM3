using _UControls.PanelCustom;
using SecurityM.Access.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Authen
{
    public partial class AssignMenu : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddGroupUser.PostValueChanged += ddGroupUser_PostValueChanged;
            ddPlatform.PostValueChanged += ddPlatform_PostValueChanged;

            try
            {
                if (!Page.IsPostBack)
                {
                    ddGroupUser.AutoPostBack = true;
                    ddGroupUser.MethodQueryProperty = delegate () { return SecurityM.Access.PropertyCollection.UserGroup.Instance.GetQueryProperty(_SessionVals.AppID); };
                    ddGroupUser.BindDataSource();

                    ddPlatform.AutoPostBack = true;
                    ddPlatform.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("platform"); };
                    ddPlatform.BindDataSource();

                    EnableActionSave(false);
                }
                else
                {
                    if (MenuViewRoleList.Count() > 0)
                    {
                        _entMenuRole = MenuViewRoleList;
                        InitialTreeViewMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
            }
        }

        void ddGroupUser_PostValueChanged(dynamic _value)
        {
            EnableActionSave(false);
        }
        void ddPlatform_PostValueChanged(dynamic _value)
        {
            EnableActionSave(false);
        }

        void EnableActionSave(bool _enabled)
        {
            if (btSave.Enabled != _enabled)
            {
                btSave.Enabled = _enabled;
                updateContentSave.Update();
            }
        }

        protected List<string> _listTempMenu = null;
        protected List<MenuKey> _listEntMenu = null;

        protected List<MenuViewRole> _entMenuRole = null;
        public List<MenuViewRole> MenuViewRoleList
        {
            get
            {
                if (ViewState["MenuViewRoleList"] == null)
                    ViewState["MenuViewRoleList"] = new MenuViewRole[0];

                return new List<MenuViewRole>((MenuViewRole[])ViewState["MenuViewRoleList"]);
            }
            set
            {
                ViewState["MenuViewRoleList"] = value.ToArray();
            }
        }


        private void NewBindingTreeView()
        {
            using (var _menu = new global::SecurityM.Access.Config.AssignMenu())
            {
                this.PlugEventResult(_menu);

                _entMenuRole = _menu.GetListMenuByRole(_SessionVals.AppID, ddGroupUser.GetValue(), ddPlatform.GetValue(), _SessionVals.LocaleID);
                MenuViewRoleList = _entMenuRole;
            }

            PanelTab1.Clear();
            InitialTreeViewMenu();
            PanelTab1.BindingAgain();

            if (MenuViewRoleList.Count() > 0)
            {
                panelMsg.Visible = false;
            }
            else
            {
                labMsg.Text = "Not found list menu " + ddPlatform.GetText() + " Application";
                panelMsg.Visible = true;
            }
        }

        #region Set Menu to Entity Data

        private void Save()
        {
            SetEntityMenu();

            using (var _menu = new global::SecurityM.Access.Config.AssignMenu())
            {
                this.PlugEventResult(_menu);

                if (_menu.Save(_listEntMenu, ddGroupUser.GetValue(), _SessionVals.UserName))
                {
                    NewBindingTreeView();
                }
            }
        }

        private void SetEntityMenu()
        {
            _listTempMenu = new List<string>();
            _listEntMenu = new List<MenuKey>();

            var entBase = _entMenuRole.Where(wh => string.IsNullOrEmpty(wh.MenuParentKey)).OrderBy(or => or.MenuGroupIndex);
            int inx = 0;

            PanelControlTab panel = null;
            TreeView treeview = null;

            var panels = PanelTab1.Panels;

            foreach (var menu in entBase)
            {
                inx++;

                panel = (PanelControlTab)panels.FirstOrDefault(wh => wh.ControlID == "panelMenu_" + inx);
                if (panel == null)
                    continue;

                treeview = panel.Controls.OfType<TreeView>().FirstOrDefault(wh => wh.ID == "treeview_" + inx);
                if (treeview == null)
                    continue;

                SetMenuSelected(treeview.Nodes);
            }

            var entNewSelect = _entMenuRole.Where(qry => qry.MenuUse == false && _listTempMenu.Contains(qry.MenuKey))
                                .Select(se => new MenuKey { MenuId = se.MenuKey, IsSelected = true });

            var entUnselect = _entMenuRole.Where(qry => qry.MenuUse == true && !_listTempMenu.Contains(qry.MenuKey))
                                .Select(se => new MenuKey { MenuId = se.MenuKey, IsSelected = false });

            _listEntMenu.AddRange(entNewSelect);
            _listEntMenu.AddRange(entUnselect);
        }

        private void SetMenuSelected(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                {
                    _listTempMenu.Add(node.Value);

                    if (node.ChildNodes != null && node.ChildNodes.Count > 0)
                    {
                        SetMenuSelected(node.ChildNodes);
                    }
                }
            }
        }

        #endregion


        #region Function TreeView Menu

        private void InitialTreeViewMenu()
        {
            try
            {
                //var entBase = _entMenuRole.Where(wh => string.IsNullOrEmpty(wh.MenuParentKey)).OrderBy(or => new { or.MenuGroupIndex, or.MenuIndex });
                var entBase = from rows in _entMenuRole
                              where string.IsNullOrEmpty(rows.MenuParentKey)
                              orderby rows.MenuGroupIndex, rows.MenuIndex
                              select rows;

                int inx = 1;

                PanelControlTab panel = null;
                TreeView treeview = null;

                foreach (var menu in entBase)
                {
                    treeview = new TreeView();
                    treeview.ID = "treeview_" + inx;
                    treeview.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                    treeview.CssClass = "control-treeview";
                    treeview.ShowLines = true;
                    treeview.ShowCheckBoxes = TreeNodeTypes.All;
                    treeview.BackColor = System.Drawing.Color.Transparent;
                    treeview.NodeStyle.NodeSpacing = 2;
                    treeview.NodeStyle.BackColor = System.Drawing.Color.Transparent;

                    treeview.Nodes.Clear();
                    BindParentMenu(ref treeview, menu);
                    treeview.DataBind();

                    panel = new PanelControlTab();
                    panel.ID = "panelMenu_" + inx;
                    panel.Attributes.Add("class", "panel-treeview");
                    panel.Controls.Add(treeview);
                    panel.PanelName = menu.MenuName;
                    panel.ControlID = panel.ID;

                    PanelTab1.AddPanel(panel);

                    inx++;
                }
            }
            catch (Exception ex)
            {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
            }
        }

        private void BindParentMenu(ref TreeView treeView, MenuViewRole _parent_menu)
        {
            try
            {
                TreeNode treeNode = null;

                if (_parent_menu != null)
                {
                    treeNode = new TreeNode(_parent_menu.MenuName, _parent_menu.MenuKey);
                    treeNode.Checked = _parent_menu.MenuUse;

                    BindSubMenu(ref treeNode, _parent_menu.MenuKey);

                    if (treeNode != null)
                        treeView.Nodes.Add(treeNode);
                }
            }
            catch (Exception ex)
            {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
            }
        }
        private void BindSubMenu(ref TreeNode treeNode, string MenuKey)
        {
            try
            {
                TreeNode childNode = null;

                var listMenu = this._entMenuRole.Where(qry => qry.MenuParentKey == MenuKey).OrderBy(or => or.MenuIndex);
                foreach (var menu in listMenu)
                {
                    childNode = new TreeNode(menu.MenuName, menu.MenuKey);
                    childNode.Checked = menu.MenuUse;

                    BindSubMenu(ref childNode, menu.MenuKey);// Check for children of the new node

                    if (childNode != null)
                        treeNode.ChildNodes.Add(childNode);
                }
            }
            catch (Exception ex)
            {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
            }
        }

        #endregion

        protected void btSearch_Click(object sender, EventArgs e)
        {
            try
            {
                NewBindingTreeView();
                btSave.Enabled = (MenuViewRoleList.Count() > 0);
            }
            catch (Exception ex)
            {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
            }
        }
        protected void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
            }
        }

    }
}
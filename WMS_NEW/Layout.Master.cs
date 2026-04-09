using ConfigGlobal.Interface;
using Prototype.Providers.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using WebSecurityApi.DTO;

using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Web.UI.WebControls;
using Prototype.Providers;

namespace WMS_NEW
{
    public partial class Layout : MasterPage
    {
        #region Action Logging

        public global::Prototype.Providers.Logging Logging;
        public event global::Prototype.Providers.EventHandler EventResulted;

        public void RaiseLogging()
        {
            this.Logging.Raise(EventResulted);
        }

        public void PlugEventResult(dynamic _objectEventResulted)
        {
            _objectEventResulted.EventResulted += new Prototype.Providers.EventHandler(ContentPage_EventResulted);
        }

        void ContentPage_EventResulted(object sender, Prototype.Providers.EventArgsCustom e)
        {
            Page.ShowEventLogging(e);
        }

        #endregion


        #region Binding Menu

        protected List<ConfigGlobal.DTO.Authen.MenuView> _entMenuView = new List<ConfigGlobal.DTO.Authen.MenuView>();
        protected List<ConfigGlobal.DTO.Authen.MenuView> MenuActiveRelation = null;
        protected string MenuCurrentView = string.Empty;
        protected string MenuCurrentName = string.Empty;

        protected string MenuKeyActive
        {
            get
            {
                if (ViewState["MenuKeyActive"] == null)
                    ViewState["MenuKeyActive"] = string.Empty;

                return ViewState["MenuKeyActive"].ToString();
            }
            set
            {
                ViewState["MenuKeyActive"] = value;
            }
        }


        protected void SetMenuActiveRelation(string _lastMenuKey)
        {
            var menuCurrent = this._entMenuView.First(qry => qry.MenuKey == _lastMenuKey);
            MenuActiveRelation.Add(menuCurrent);

            if (!string.IsNullOrEmpty(menuCurrent.MenuParentKey))
            {
                SetMenuActiveRelation(menuCurrent.MenuParentKey);
            }
        }

        protected void SetMenuName()
        {
            if (MenuActiveRelation == null)
            {
                MenuActiveRelation = new List<ConfigGlobal.DTO.Authen.MenuView>();

                var menuKey = this._entMenuView.FirstOrDefault(qry => qry.MenuCode == this.MenuKeyActive);
                if (menuKey != null)
                    SetMenuActiveRelation(menuKey.MenuKey);
            }

            if (MenuActiveRelation.Count > 0)
            {
                MenuCurrentView = "<ul class=\"breadcrumb\">";

                var sortMenu = MenuActiveRelation.OrderBy(or => or.MenuGroupIndex).ThenBy(or => or.MenuIndex);
                //var sortMenu = MenuActiveRelation.OrderBy(or => new { or.MenuGroupIndex ,or.MenuIndex});
                foreach (var menu in sortMenu)
                {
                    if (menu.MenuCode == this.MenuKeyActive)
                    {
                        MenuCurrentName = menu.MenuName;
                        MenuCurrentView += "<li class=\"breadcrumb-item text-uppercase active\">" + menu.MenuName + "</li>";
                    }
                    else
                    {
                        MenuCurrentView += "<li class=\"breadcrumb-item text-uppercase\">" + menu.MenuName + "</li>";
                    }
                }

                MenuCurrentView += "</ul>";
            }
        }

        protected void RenderMenus(string _menuParentKey)
        {
            var result = this._entMenuView.AsEnumerable();

            if (!string.IsNullOrEmpty(_menuParentKey))
                result = result.Where(qry => qry.MenuParentKey == _menuParentKey);
            else
                result = result.Where(qry => string.IsNullOrEmpty(qry.MenuParentKey));

            var listMenu = result.OrderBy(or => or.MenuGroupIndex).ThenBy(or => or.MenuIndex);

            foreach (var _menu in listMenu)
            {
                var _symbolKey = _menu.MenuUrl.Contains("?") ? "&" : "?";

                var prmMenuKey = _symbolKey + "mkey=" + _menu.MenuCode;
                var prmUrl = string.Empty;
                var target = string.Empty;

                if (_menu.MenuUrl.Contains("~/"))
                {
                    prmUrl = ResolveUrl(_menu.MenuUrl.Replace("|", "/")) + prmMenuKey;
                }
                else
                {
                    prmUrl = _menu.MenuUrl;
                    target = @"target=""_blank""";
                }


                if (this._entMenuView.Any(qry => qry.MenuParentKey == _menu.MenuKey)) //Has Sub Menu
                {
                    if (MenuActiveRelation.Any(qry => qry.MenuKey == _menu.MenuKey))
                        Response.Write("<li class=\"nav-item nav-dropdown open\">");
                    else
                        Response.Write("<li class=\"nav-item nav-dropdown\">");

                    string icon = string.IsNullOrEmpty(_menu.MenuParentKey) ? "icon-list" : "icon-folder";

                    Response.Write("<a class=\"nav-link nav-dropdown-toggle\" href=\"#\"><i class=\"" + icon + "\"></i>" + _menu.MenuName + "</a>");
                    Response.Write("<ul class=\"nav-dropdown-items\">");

                    RenderMenus(_menu.MenuKey);

                    Response.Write("</ul>");
                    Response.Write("</li>");
                }
                else
                {
                    var a_active = string.Empty;

                    if (MenuActiveRelation.Any(qry => qry.MenuKey == _menu.MenuKey))
                    {
                        a_active = " active";
                        Response.Write("<li class=\"nav-item open\">");
                    }
                    else
                        Response.Write("<li class=\"nav-item\">");

                    Response.Write("<a class=\"nav-link nav-item-child" + a_active + "\" " + target + " href=\"" + prmUrl + "\">" + _menu.MenuName + "</a>");
                    Response.Write("</li>");
                }
            }
        }

        #endregion


        #region Properties

        protected string DefaultPage
        {
            get
            {
                if (ViewState["DefaultPage"] == null)
                    ViewState["DefaultPage"] = string.Empty;

                return ViewState["DefaultPage"].ToString();
            }
            set
            {
                ViewState["DefaultPage"] = value;
            }
        }

        protected string HostUrl
        {
            get
            {
                return ViewState["HostUrl"].ToString();
            }
            set
            {
                ViewState["HostUrl"] = value;
            }
        }

        public string SiteName
        {
            get
            {
                if (ViewState["SiteName"] == null)
                    ViewState["SiteName"] = string.Empty;

                return ViewState["SiteName"].ToString();
            }
            set
            {
                ViewState["SiteName"] = value;
            }
        }

        protected string CssBackground
        {
            get
            {
                var page = "~/" + DefaultPage.ToLower();
                if ((page == PageGlobal.SYS_LOGIN.ToLower()) || (page == PageGlobal.SYS_INDEX.ToLower()))
                {
                    return "content-bg";
                }
                else
                {
                    return "";
                }
            }
        }

        protected string SidebarFixed
        {
            get
            {
                if (!string.IsNullOrEmpty(_SessionVals.UserName))
                    return "sidebar-fixed aside-menu-fixed";
                else
                    return string.Empty;
            }
        }

        public int CountPopupShow
        {
            get
            {
                if (ViewState["CountPopupShow"] == null)
                    ViewState["CountPopupShow"] = 0;

                return (int)ViewState["CountPopupShow"];
            }
            set
            {
                ViewState["CountPopupShow"] = value;
            }
        }

        public bool IsTriggerHideScroll { get; set; }

        #endregion


        public List<IResource> iResourceList = new List<IResource>();

        void Page_Init()
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    AccessSystem.ClearUsersTimeout();
                }


                DefaultPage = System.IO.Path.GetFileName(Request.Path ?? string.Empty);

                DateTime last_logon_date = DateTime.Now;

                if (!string.IsNullOrEmpty(_SessionVals.UserName))
                {
                    using (var _model = new SecurityM.Source.SecurityM_Entities())
                    {
                        var ent_device = _model.t_com_user_device.FirstOrDefault(x => x.app_id == _SessionVals.AppID && x.user_id == _SessionVals.UserName && x.device == _SessionVals.DeviceID && x.is_active == "YES");
                        if (ent_device != null)
                        {
                            last_logon_date = ent_device.logon_datetime;

                            ent_device.last_alive_time = DateTime.Now;
                            _model.SaveChanges();
                        }
                    }

                    if (Request.AppRelativeCurrentExecutionFilePath == "~/default.aspx")
                    {
                        Response.Redirect(PageGlobal.SYS_INDEX);
                    }
                }
                else if ((string.IsNullOrEmpty(_SessionVals.UserName) || (last_logon_date != (DateTime)Session["LOGON_DATE"]))
                     && ("~/" + DefaultPage.ToLower() != PageGlobal.SYS_LOGIN.ToLower()))
                {
                    var any_dev_logon = !string.IsNullOrEmpty(_SessionVals.UserName) ? "?usrdev_any_log=true" : "";

                    Response.Redirect(PageGlobal.SYS_LOGIN + any_dev_logon);
                }



                if (Session["MenuView"] != null)
                {
                    this._entMenuView = (List<ConfigGlobal.DTO.Authen.MenuView>)Session["MenuView"];
                }

                if (!Page.IsPostBack)
                {
                    if (Session["GridKeepField"] == null)
                        Session["GridKeepField"] = new List<global::_UControls.GridKeepField>();
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(System.Threading.ThreadAbortException))
                {
                    this.Logging = new Prototype.Providers.Logging(this, ex);
                    this.RaiseLogging();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Plug Event Logging

                this.EventResulted += new Prototype.Providers.EventHandler(ContentPage_EventResulted);

                #endregion


                MenuKeyActive = Request.QueryString["mkey"] ?? string.Empty;
                this.SetMenuName();

                if (!Page.IsPostBack)
                {
                    ThemeLoad();

                    if (string.IsNullOrEmpty(SiteName))
                        SiteName = System.Configuration.ConfigurationManager.AppSettings["WEB_NAME"];

                    HostUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";


                    var _resourceGeneral = new List<Resource>();

                    if (_SessionVals.UserName != string.Empty)
                    {
                        if (_SessionVals.User != null && !string.IsNullOrEmpty(_SessionVals.User.first_name))
                            labUser.Text = _SessionVals.User.first_name + " " + _SessionVals.User.last_name;
                        else
                            labUser.Text = _SessionVals.UserName;


                        #region Initial Resource Language

                        #region Resource General All Page

                        if (Session["Resource_General"] == null)
                        {
                            _resourceGeneral = new List<Resource>()
                            {
                                new Resource(){ResourceGroup ="General", ResourceName="Filter" },
                                new Resource(){ResourceGroup ="General", ResourceName="Hide Filter" },
                                new Resource(){ResourceGroup ="General", ResourceName="Limit" },
                                new Resource(){ResourceGroup ="General", ResourceName="Export to" },
                                new Resource(){ResourceGroup ="General", ResourceName="Search" },
                                new Resource(){ResourceGroup ="General", ResourceName="Hide" },
                                new Resource(){ResourceGroup ="General", ResourceName="View" },
                                new Resource(){ResourceGroup ="General", ResourceName="Edit" },
                                new Resource(){ResourceGroup ="General", ResourceName="Delete" },
                                new Resource(){ResourceGroup ="General", ResourceName="to" },
                                new Resource(){ResourceGroup ="General", ResourceName="of" },
                                new Resource(){ResourceGroup ="General", ResourceName="rows" },
                                //Button
                                new Resource(){ResourceGroup ="General", ResourceName="Save" },
                                new Resource(){ResourceGroup ="General", ResourceName="Clear" },
                                new Resource(){ResourceGroup ="General", ResourceName="Close" },
                                //Popup
                                new Resource(){ResourceGroup ="General", ResourceName="EditData" },
                                new Resource(){ResourceGroup ="General", ResourceName="NewData" },
                                //Common
                                new Resource(){ResourceGroup ="General", ResourceName="Active" },
                                new Resource(){ResourceGroup ="General", ResourceName="create_by" },
                                new Resource(){ResourceGroup ="General", ResourceName="create_date" },
                                new Resource(){ResourceGroup ="General", ResourceName="Update By" },
                                new Resource(){ResourceGroup ="General", ResourceName="Update Date" }
                            };

                            iResourceList.AddRange(_resourceGeneral);
                        }

                        #endregion


                        ContentPlaceHolder1.Controls.FindControlsDeepByType(ref iResourceList);


                        #region  Manual Set IResource

                        var _iResManual = iResourceList.Where(qry => !string.IsNullOrEmpty(qry.ResourceGroup) && !string.IsNullOrEmpty(qry.ResourceName));

                        var _resourceManual = GetResourceByI(_iResManual);
                        foreach (var res in _resourceManual)
                        {
                            var iResRefs = _iResManual.Where(qry => qry.ResourceGroup.ToLower() == res.ResourceGroup.ToLower() && qry.ResourceName.ToLower() == res.ResourceName.ToLower());

                            foreach (var iRes in iResRefs)
                                iRes.ResourceValue = res.ResourceValue;
                        }

                        #endregion


                        this.GetResourceByAutoI();

                        #endregion
                    }


                    if (Session["Resource_General"] == null) Session["Resource_General"] = _resourceGeneral;
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(System.Threading.ThreadAbortException))
                {
                    this.Logging = new Prototype.Providers.Logging(this, ex);
                    this.RaiseLogging();
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e) //Page Stat working after Load Control events
        {
            if (Page.IsPostBack && IsTriggerHideScroll)
            {
                var script = "";

                if (CountPopupShow > 0)
                    script = "$('body').addClass('hide-scroll');";
                else
                    script = "$('body').removeClass('hide-scroll');";

                Page.ScriptPageRegister(" $('document').ready(function () { " + script + " });", "Body_HideScroll");
            }
        }

        public void GetResourceByAutoI()
        {
            #region  Auto Set IResourceInput

            var resCommonLink = new ResourceAutoLinks();
            string page_name = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);

            var _iResAuto = (from rows in iResourceList.OfType<IResourceInput>()
                             where string.IsNullOrEmpty(rows.ResourceValue)
                             select new Resource
                             {
                                 ResourceGroup = page_name,
                                 ResourceName = rows.DataFieldValue,
                             }).ToArray();

            foreach (var iRes in _iResAuto.Where(x => resCommonLink.Any(any => any.resource_name_db == x.ResourceName.ToLower())))
            {
                iRes.ResourceGroup = "General";
                iRes.ResourceName = resCommonLink.First(x => x.resource_name_db == iRes.ResourceName).resource_name_alias;
            }

            var _resourceAuto = GetResourceByI(_iResAuto);
            foreach (var res in _resourceAuto)
            {
                var resComm = resCommonLink.FirstOrDefault(x => x.resource_name_alias.ToLower() == res.ResourceName.ToLower());
                if (resComm != null)
                    res.ResourceName = resComm.resource_name_db;

                var iResRefs = iResourceList.OfType<IResourceInput>().Where(qry => !string.IsNullOrEmpty(qry.DataFieldValue)
                                                                         && qry.DataFieldValue.ToLower() == res.ResourceName.ToLower() && string.IsNullOrEmpty(qry.ResourceValue));

                foreach (var iRes in iResRefs)
                    iRes.ResourceValue = res.ResourceValue;
            }

            #endregion
        }

        public Resource[] GetResourceByI(IEnumerable<IResource> _iResourceList)
        {
            using (var acc = new Access.Configuration.ResourceMaster())
            {
                this.PlugEventResult(acc);

                var _resourceList = acc.Get_Resource(_iResourceList);
                return _resourceList;
            }
        }

        protected string HeaderCss
        {
            get
            {
                if (ViewState["HeaderFix"] == null)
                    ViewState["HeaderFix"] = "header-fixed";

                return ViewState["HeaderFix"].ToString();
            }
            set
            {
                ViewState["HeaderFix"] = value;
            }
        }

        public bool HeaderVisible
        {
            get
            {
                return header.Visible;
            }
            set
            {
                header.Visible = value;

                this.HeaderCss = !value ? string.Empty : "header-fixed";
            }
        }


        public bool MenuTogglerHide
        {
            get
            {
                if (ViewState["MenuTogglerHide"] == null)
                    ViewState["MenuTogglerHide"] = false;

                return (bool)ViewState["MenuTogglerHide"];
            }
            set
            {
                ViewState["MenuTogglerHide"] = value;
            }
        }

        protected void btnMenuToggler_Click(object sender, EventArgs e)
        {
            MenuTogglerHide = !MenuTogglerHide;
        }

        protected void linkLogout_Click(object sender, EventArgs e)
        {
            try
            {
                AccessSystem.Logout();

                Access.Transaction.OutbndManifest.OutbndManifestExt.Instance.ClearTransportData(Session.SessionID);

                try
                {
                    Response.Redirect(PageGlobal.SYS_LOGIN);
                }
                catch (System.Threading.ThreadAbortException ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(System.Threading.ThreadAbortException))
                {
                    this.Logging = new Prototype.Providers.Logging(this, ex);
                    this.RaiseLogging();
                }
            }
        }

        protected void btThemeDark_Click(object sender, EventArgs e)
        {
            ThemeSave("DARK");
        }

        protected void btThemeLight_Click(object sender, EventArgs e)
        {
            ThemeSave("LIGHT");
        }


        public string ThemeName
        {
            get
            {
                if (ViewState["ThemeName"] == null)
                    ViewState["ThemeName"] = "LIGHT";

                return ViewState["ThemeName"].ToString();
            }
            set
            {
                ViewState["ThemeName"] = value;
            }
        }

        public void ThemeSave(string _theme)
        {
            ThemeName = _theme;

            var cookInfo = new HttpCookie("WMS_CFG");
            cookInfo.HttpOnly = false;
            cookInfo.Secure = false;
            cookInfo.Expires = DateTime.Now.AddDays(30);
            cookInfo["THEME_NAME"] = _theme;

            Response.Cookies.Add(cookInfo);
        }

        public string ThemeLoad()
        {
            var name = "";

            var cookInfo = Request.Cookies["WMS_CFG"];
            if (cookInfo != null)
            {
                name = cookInfo["THEME_NAME"].ToString();
                ThemeName = name;
            }

            return name;
        }
        public string GetLanguage()
        {
            var vReturn = string.Empty;
            using (var model = new SecurityM.Source.SecurityM_Entities())
            {
                var dtoUser = model.t_com_user.Where(qry => qry.user_id == _SessionVals.User.user_id).FirstOrDefault();
                switch (_SessionVals.LocaleID)
                {
                    case "1033":
                        vReturn = "us";
                        dtoUser.locale_id = "1033";
                        break;
                    case "1054":
                        vReturn = "th";
                        dtoUser.locale_id = "1054";
                        break;
                }
                model.SaveChanges();
            }
            return vReturn;
        }
        protected void linkLanguage_Click(object sender, EventArgs e)
        {
            try
            {
                using (var _model = new SecurityM.Source.SecurityM_Entities())
                {
                    LinkButton lbn = (LinkButton)sender;
                    _SessionVals.LocaleID = lbn.CommandArgument;
                    var listMenuView = new List<ConfigGlobal.DTO.Authen.MenuView>();
                    //Reload Menu
                    var WEB_SECURE_API_URL = ConfigurationManager.AppSettings["WEB_SECURE_API_URL"];
                    var FUNC_GETMENU = ConfigurationManager.AppSettings["WEB_SECURE_API_FUNC_GETMENU"];

                    string app_id = _SessionVals.AppID;
                    string plat_form = _SessionVals.Platform;

                    var req_menu = new RequestMenu();
                    req_menu.app_id = app_id;
                    req_menu.platform = plat_form;
                    req_menu.locale_id = _SessionVals.LocaleID == null ? "1033" : _SessionVals.LocaleID;
                    req_menu.user_group_id = _SessionVals.User.user_group_id;

                    var _client = new HttpClientExtension();

                    _client.CreateHttpClient(WEB_SECURE_API_URL);

                    var res_menus = _client.PostByJson<RequestMenu, ResultMenu[]>(FUNC_GETMENU, req_menu);
                    if (res_menus != null && res_menus.Count() > 0)
                    {
                        global::ConfigGlobal.DTO.Authen.MenuView menu = null;
                        foreach (ResultMenu rows in res_menus)
                        {
                            menu = new ConfigGlobal.DTO.Authen.MenuView();
                            menu.MenuCode = rows.menu_id.GetPasswordMD5().ToLower();
                            menu.MenuName = rows.menu_name;
                            menu.MenuKey = rows.menu_id;
                            menu.MenuParentKey = rows.parent_menu_id;
                            menu.MenuGroupIndex = rows.menu_group_sequence.Value;
                            menu.MenuIndex = rows.menu_sequence;
                            menu.MenuUrl = !string.IsNullOrEmpty(rows.process) ? rows.process : "";

                            listMenuView.Add(menu);
                        }
                    }

                    Session["MenuView"] = listMenuView;
                    Session["Resource_General"] = null;

                    try
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    catch (System.Threading.ThreadAbortException ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(System.Threading.ThreadAbortException))
                {
                    this.Logging = new Prototype.Providers.Logging(this, ex);
                    this.RaiseLogging();
                }
            }
        }
    }



    public static class AccessSystem
    {
        public static void ClearUsersTimeout()
        {
            try
            {
                var app_id = ConfigurationManager.AppSettings["APP_ID"];
                var date_now = DateTime.Now;
                var sess_timeout = (FieldsStatic.SessionTimeout - 2);

                using (var _model = new SecurityM.Source.SecurityM_Entities())
                {
                    var devices = _model.t_com_user_device.Where(x => x.app_id == app_id && x.is_active == "YES" && SqlFunctions.DateDiff("minute", (x.last_alive_time ?? date_now), date_now) > sess_timeout).ToArray();

                    foreach (var dev in devices)
                    {
                        dev.is_active = "NO";
                        dev.logout_datetime = date_now;

                        var ent_logon = _model.t_com_user_logon.Where(x => x.app_id == dev.app_id && x.logon_end_datetime == null
                                   && x.user_id == dev.user_id && x.device.ToUpper() == dev.device.ToUpper()).OrderByDescending(or => or.logon_start_datetime).FirstOrDefault();

                        if (ent_logon != null)
                        {
                            ent_logon.logon_end_datetime = dev.logout_datetime;
                            ent_logon.logon_time = Convert.ToInt32((ent_logon.logon_end_datetime.Value - ent_logon.logon_start_datetime).TotalSeconds);
                        }
                    }

                    if (devices.Count() > 0)
                        _model.SaveChanges();

                    if (!string.IsNullOrEmpty(_SessionVals.UserName) && devices.Any(x => x.user_id == _SessionVals.UserName))
                    {
                        HttpContext.Current.Session["LOGON_DATE"] = DateTime.MinValue;
                        _SessionVals.InitialSession();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Logout()
        {
            try
            {
                using (var _client = new HttpClientExtension())
                {
                    var WEB_SECURE_API_URL = ConfigurationManager.AppSettings["WEB_SECURE_API_URL"];
                    var FUNC_LOGOUT = ConfigurationManager.AppSettings["WEB_SECURE_API_FUNC_LOGOUT"];

                    _client.CreateHttpClient(WEB_SECURE_API_URL);

                    var req_logout = new RequestLogin();
                    req_logout.app_id = ConfigurationManager.AppSettings["APP_ID"];
                    req_logout.platform = ConfigurationManager.AppSettings["PLATFORM"];
                    req_logout.device_or_ip = _SessionVals.DeviceID;
                    string user_name = _SessionVals.UserName;
                    req_logout.username = user_name;

                    var res_login = _client.PostByJson<RequestLogin, ResultLogin>(FUNC_LOGOUT, req_logout);
                    if (res_login.is_success == true)
                    {
                        HttpContext.Current.Session["LOGON_DATE"] = DateTime.MinValue;
                        _SessionVals.InitialSession();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
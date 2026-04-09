using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using WebSecurityApi.DTO;

namespace WMS_NEW
{
    public partial class Login : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (!string.IsNullOrEmpty(_SessionVals.UserName))
                    {
                        Session["LOGON_DATE"] = DateTime.MinValue;

                        _SessionVals.InitialSession();
                    }

                    if (!string.IsNullOrEmpty(Request.QueryString["usrdev_any_log"]))
                    {
                        Page.MessageWarning("! There are other users logged in.");
                    }

                    Access.Transaction.OutbndManifest.OutbndManifestExt.Instance.ClearTransportData(Session.SessionID);

                    txtUsername.Focus();
                }
                else //Manual Event Postback of Trigger Control
                {
                    var eventControl = Request.Params.Get("__EVENTTARGET");
                    var eventArgument = Request.Params.Get("__EVENTARGUMENT");

                    if (eventControl == "CHECK_LOGIN" && !string.IsNullOrEmpty(eventArgument))
                    {
                        CheckValidate();
                    }
                }

                string version = FieldsStatic.ApplicationVersion;
                lblVersion.Text = version;

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btLogin_Click(object sender, EventArgs e)
        {            
            CheckValidate();
        }        

        void CheckValidate()
        {
            if (!string.IsNullOrEmpty(txtUsername.Text.Trim()) && !string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                CheckLogin();
            }
            else
            {
                Page.MessageWarning("! Please Input Username and Password.");
            }
        }


        void CheckLogin()
        {
            var _client = new HttpClientExtension();

            try
            {
                var WEB_SECURE_API_URL = ConfigurationManager.AppSettings["WEB_SECURE_API_URL"];
                var FUNC_LOGIN = ConfigurationManager.AppSettings["WEB_SECURE_API_FUNC_LOGIN"];
                var FUNC_GETMENU = ConfigurationManager.AppSettings["WEB_SECURE_API_FUNC_GETMENU"];

                _client.CreateHttpClient(WEB_SECURE_API_URL);

                var req_login = new RequestLogin();
                req_login.app_id = ConfigurationManager.AppSettings["APP_ID"];
                req_login.platform = ConfigurationManager.AppSettings["PLATFORM"];
                req_login.username = txtUsername.Text.Trim();
                req_login.password = txtPassword.Text.Trim();
                req_login.device_or_ip = _SessionVals.DeviceID;

                var res_login = _client.PostByJson<RequestLogin, ResultLogin>(FUNC_LOGIN, req_login);
                if (res_login.is_success == true)
                {
                    var dto_user = res_login.user;

                    _SessionVals.User = dto_user;
                    _SessionVals.UserName = dto_user.username;
                    _SessionVals.GroupID = dto_user.user_group_id;
                    _SessionVals.IsAdmin = false;
                    _SessionVals.LocaleID = dto_user.locale_id;

                    var logon_datetime = DateTime.Now;

                    //2024-03-06 : Kritsada : Check last login ถ้าเป็นค่า null หรือน้อยกว่า 90 วันบังคับให้เปลี่ยน Password
                    //ถ้า result_code = -5 แสดงว่า เป็นค่า null หรือน้อยกว่า 90 วันบังคับให้เปลี่ยน Password
                    if (res_login.result_code == "-5") {
                        popupChangePassword.HeaderText = res_login.result_msg;
                        popupChangePassword.ShowDialog();
                        return;
                    }

                    //Extensions.FormatDecimal =
                    using (var model = new WMS_NEW.Source.WMSEntities())
                    {
                        string digit = model.t_wms_rule.Where(w => w.rule_code == "RULE_DISPLAY_DECIMAL_SCALE" && w.is_active == "YES").FirstOrDefault()?.value ?? "2";
                        int iDigit = Convert.ToInt32(digit);
                        Extensions.Digit_INT = iDigit;
                        Extensions.FormatDecimal = iDigit <= 0 ? "#,##0" : "#,##0." + "0".PadLeft(iDigit, '0');
                    }


                    using (var _model = new SecurityM.Source.SecurityM_Entities())
                    {
                        var ent_device = _model.t_com_user_device.FirstOrDefault(x => x.app_id == req_login.app_id && x.user_id == req_login.username && x.device == req_login.device_or_ip && x.is_active == "YES");
                        if (ent_device != null)
                        {
                            logon_datetime = ent_device.logon_datetime;

                            ent_device.last_alive_time = DateTime.Now;
                            _model.SaveChanges();
                        }
                        //2024-03-06 : Kritsada : Check last login ถ้าเป็นค่า null หรือน้อยกว่า 90 วันบังคับให้เปลี่ยน Pasword
                        //ยกเลิกไปเช็คที่ security แทน
                        //var ent_lass_password = _model.t_com_user.FirstOrDefault(x => x.user_id == req_login.username);
                        //if (ent_lass_password.last_change_password == null || ent_lass_password.last_change_password < DateTime.Now.AddDays(-90)) {                            
                        //    popupChangePassword.ShowDialog();
                        //    return;
                        //}
                    }

                    Session["LOGON_DATE"] = logon_datetime;

                    var listMenuView = new List<ConfigGlobal.DTO.Authen.MenuView>();
                    string app_id = _SessionVals.AppID;
                    string plat_form = _SessionVals.Platform;

                    var req_menu = new RequestMenu();
                    req_menu.app_id = app_id;
                    req_menu.platform = plat_form;
                    req_menu.locale_id = dto_user.locale_id;
                    req_menu.user_group_id = dto_user.user_group_id;

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
                        Response.Redirect(PageGlobal.SYS_INDEX);
                    }
                    catch (System.Threading.ThreadAbortException ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    Page.MessageWarning(res_login.result_msg);
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
            finally
            {
                _client.Dispose();
            }
        }

        protected void btThemeDark_Click(object sender, EventArgs e)
        {
            SetTheme("DARK");
        }

        protected void btThemeLight_Click(object sender, EventArgs e)
        {
            SetTheme("LIGHT");
        }

        void SetTheme(string _theme)
        {
            var page_ms = (this.Master as Layout);
            page_ms.ThemeSave(_theme);
        }

        #region Change password

        protected void btComfirmChangePass_Click(object sender, EventArgs e) {
            this.ChangePassValidate();
        }

        public void ChangePassValidate() {
            if (string.IsNullOrEmpty(txtNewPasswords.Text.Trim()) || string.IsNullOrEmpty(txtReNewPasswords.Text.Trim())) {
                Page.MessageWarning("! Please input Password and Confirm Password.");
                return;
            } else if (!txtNewPasswords.Text.Trim().Equals(txtReNewPasswords.Text.Trim())) {
                Page.MessageWarning("! New Password and Confirm Password not match.");
                return;
            }

            if (txtNewPasswords.Text.Trim().Count() < 8) {
                Page.MessageWarning("! New Password must more then 8 characters");
                return;
            }
            //
            int countPasswordPolicy = 0;
            // Define the password policy using a regular expression
            string passwordPattern1 = @"[a-z]";
            if (Regex.IsMatch(txtNewPasswords.Text.Trim(), passwordPattern1)) {
                countPasswordPolicy++;
            }
            string passwordPattern2 = @"[A-Z]";
            if (Regex.IsMatch(txtNewPasswords.Text.Trim(), passwordPattern2)) {
                countPasswordPolicy++;
            }
            string passwordPattern3 = @"[0-9]";
            if (Regex.IsMatch(txtNewPasswords.Text.Trim(), passwordPattern3)) {
                countPasswordPolicy++;
            }
            string passwordPattern4 = @"[!@#$%*(){}+=-]";
            if (Regex.IsMatch(txtNewPasswords.Text.Trim(), passwordPattern4) || txtNewPasswords.Text.Trim().Contains("[") || txtNewPasswords.Text.Trim().Contains("]")) {
                countPasswordPolicy++;
            }
            if (countPasswordPolicy < 4) {
                Page.MessageWarning("! Password is not valid. Please check your policy password must contains (A-Z, a-z, 0-9, !@#$%*(){}+=-[]) with more then 8 characters");
                return;
            }
            //
            this.ChangePassword(txtUsername.Text.Trim(), txtNewPasswords.Text.Trim());
        }

        public void ChangePassword(string _username, string _password) {
            try {
                using (var _acc = new SecurityM.Access.Master.User()) {
                    // ดึงรายการ Password เก่า
                    List<string> ListPasswrodLog = _acc.getPasswordLog(_username);
                    var passwordLog = ListPasswrodLog.Where(q => q == (_password)).ToList();
                    if (passwordLog.Count() >= 1) {
                        Page.MessageWarning("! Can not update password with your old password");
                        return;
                    }
                    // เปลี่ยน Password
                    var is_success = _acc.ChangePassword(_username, _password);
                    if (is_success) {
                        Page.MessageSuccess("Update Password Success");
                        popupChangePassword.HideDialog();
                    } else {
                        Page.MessageWarning("! Update password fail, Please check with your Admin.");
                    }
                }
            } catch (Exception ex) {
                if (ex.GetType() != typeof(System.Threading.ThreadAbortException)) {
                    this.Logging = new Prototype.Providers.Logging(this, ex);
                    this.RaiseLogging();
                }
            }
        }
        #endregion Change password
    }
}
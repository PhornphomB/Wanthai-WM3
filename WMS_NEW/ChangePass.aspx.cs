using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace WMS_NEW
{
    public partial class ChangePass : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack)
                {
                    txtNewPass.Attributes.Add("value", txtNewPass.Text);
                    txtNewPassConf.Attributes.Add("value", txtNewPassConf.Text);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        #region Button Event

        protected void btChange_Click(object sender, EventArgs e)
        {
            try
            {
                ChangePassValidate();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void txtNewPassConf_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ChangePassValidate();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        #endregion

        void ChangePassValidate()
        {
            if (string.IsNullOrEmpty(txtNewPass.Text.Trim()) || string.IsNullOrEmpty(txtNewPassConf.Text.Trim()))
            {
                Page.MessageWarning("! Please input Password and Confirm Password.");
                return;
            }
            else if (!txtNewPass.Text.Trim().Equals(txtNewPassConf.Text.Trim()))
            {
                Page.MessageWarning("! New Password and Confirm Password not match.");
                return;
            }

            if (txtNewPass.Text.Trim().Count() < 8) {
                Page.MessageWarning("! New Password must more then 8 characters");
                return;
            }

            int countPasswordPolicy = 0;
            // Define the password policy using a regular expression
            string passwordPattern1 = @"[a-z]";            
            if(Regex.IsMatch(txtNewPass.Text.Trim(), passwordPattern1)) {
                countPasswordPolicy++;
            }
            string passwordPattern2 = @"[A-Z]";
            if (Regex.IsMatch(txtNewPass.Text.Trim(), passwordPattern2)) {
                countPasswordPolicy++;
            }
            string passwordPattern3 = @"[0-9]";
            if (Regex.IsMatch(txtNewPass.Text.Trim(), passwordPattern3)) {
                countPasswordPolicy++;
            }
            string passwordPattern4 = @"[!@#$%*(){}+=-]";
            if (Regex.IsMatch(txtNewPass.Text.Trim(), passwordPattern4) || txtNewPass.Text.Trim().Contains("[") || txtNewPass.Text.Trim().Contains("]")) {
                countPasswordPolicy++;
            }

            if (countPasswordPolicy < 4) {
                Page.MessageWarning("! Password is not valid. Please check your policy password must contains (A-Z, a-z, 0-9, !@#$%*(){}+=-[]) with more then 8 characters");
                return;
            }
            
            ChangePassword();
        }

        void ChangePassword()
        {
            try
            {
                using (var _acc = new SecurityM.Access.Master.User())
                {
                    this.PlugEventResult(_acc);
                    // ดึงรายการ Password เก่า
                    List<string> ListPasswrodLog = _acc.getPasswordLog(_SessionVals.UserName);
                    var passwordLog = ListPasswrodLog.Where(q => q == (txtNewPass.Text.Trim())).ToList();
                    if (passwordLog.Count() >= 1) {
                        Page.MessageWarning("! Can not update password with your old password");
                        return;
                    }
                    // เปลี่ยน Password
                    var is_success = _acc.ChangePassword(_SessionVals.UserName, txtNewPass.Text.Trim());
                    if (is_success)
                    {
                        Response.Redirect(ResolveUrl(PageGlobal.SYS_INDEX));
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
}
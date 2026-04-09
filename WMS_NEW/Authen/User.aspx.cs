using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Authen
{
    public partial class User : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                popup1.AfterNewDataEvent += Popup1_AfterNewDataEvent;
                popup1.AfterSetEditDataEvent += Popup1_AfterSetEditDataEvent;
                popup1.PreSaveEntityEvent += Popup1_PreSaveEntityEvent;

                #region Initial Peoperty Column Grid

                GridColumnExt13.DropDownQueryProperty = delegate () { return SecurityM.Access.PropertyCollection.UserGroup.Instance.GetQueryProperty(_SessionVals.AppID); };
                GridColumnExt4.DropDownQueryProperty = delegate () { return global::SecurityM.Access.PropertyCollection.Locale.Instance.GetQueryProperty(); };
                GridColumnExt5.DropDownQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.ActiveType().AsQueryable(); };

                #endregion

                #region Initial Input Data

                ddlUserGroup.MethodQueryProperty = delegate () { return SecurityM.Access.PropertyCollection.UserGroup.Instance.GetQueryProperty(_SessionVals.AppID); };
                ddlLocal.MethodQueryProperty = delegate () { return global::SecurityM.Access.PropertyCollection.Locale.Instance.GetQueryProperty(); };

                #endregion

                if (!Page.IsPostBack)
                {
                    hidSessionIsAdmin.SetValue(_SessionVals.IsAdmin);
                    hidSessionUserGroupID.SetValue(_SessionVals.GroupID);
                    hidSessionAppID.SetValue(_SessionVals.AppID);
                }

                popup1.InitObjectsEvent += () => { popup1.ObjectDataAccess = new SecurityM.Access.Master.User(); };
                popup1.InitControlStatic();

                GridExt1.PopupEntitySource = popup1;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void Popup1_AfterNewDataEvent()
        {
            btResetPass.Visible = false;
        }

        private void Popup1_AfterSetEditDataEvent()
        {
            try
            {
                var acc = (SecurityM.Access.Master.User)popup1.ObjectDataAccess;

                var usr_group = acc._Model.t_com_user_application.Where(x => x.user_id == acc.Entity.user_id && x.app_id == _SessionVals.AppID).OrderByDescending(or => or.create_date).FirstOrDefault();
                if (usr_group != null)
                {
                    ddlUserGroup.SetValue(usr_group.user_group_id);
                }
                else
                {
                    ddlUserGroup.Clear();
                }

                btResetPass.Visible = true;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void Popup1_PreSaveEntityEvent()
        {
            try
            {
                var acc = (SecurityM.Access.Master.User)popup1.ObjectDataAccess;

                var user_id = txtUsername.GetValue();
                var user_group_id = (string)ddlUserGroup.GetValue();

                var usr_app_other = acc._Model.t_com_user_application.Where(x => x.user_id == user_id && x.app_id == _SessionVals.AppID && x.user_group_id != user_group_id);
                acc._Model.t_com_user_application.RemoveRange(usr_app_other);

                var has_usr_app = acc._Model.t_com_user_application.Any(x => x.user_id == user_id && x.user_group_id == user_group_id && x.app_id == _SessionVals.AppID);
                if (!has_usr_app)
                {
                    var usr_app = new SecurityM.Source.t_com_user_application();
                    usr_app.tran_id = Guid.NewGuid().ToString();
                    usr_app.app_id = _SessionVals.AppID;
                    usr_app.user_group_id = (string)ddlUserGroup.GetValue();
                    usr_app.user_id = txtUsername.GetValue();
                    usr_app.create_by = _SessionVals.UserName;
                    usr_app.create_date = DateTime.Now;

                    acc._Model.Entry(usr_app).State = System.Data.Entity.EntityState.Added;
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btResetPass_Click(object sender, EventArgs e)
        {
            try
            {
                using (var _acc = new SecurityM.Access.Master.User())
                {
                    var usrname = txtUsername.GetValue();

                    if (_acc.ResetPassword(usrname, usrname))
                    {
                        popup1.HideDialog();
                        this.MessageSuccess("Reset Password Success.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}
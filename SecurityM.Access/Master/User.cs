using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using SecurityM.Source;
using Prototype.Providers;

namespace SecurityM.Access.Master
{
    public class User : AEntityFormCommand<t_com_user>
    {
        public SecurityM_Entities _Model { get; set; }

        public User()
        {
            this._Model = new SecurityM_Entities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_com_user; };
        }


        #region ++INSTANCE STATIC++
        public static User Instance
        {
            get
            {
                using (User _Instance = new User())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public override void SetOptionalSaveNew(t_com_user ent)
        {
            ent.tran_id = Guid.NewGuid().ToString();
            ent.user_id = ent.user_id.Trim();
            ent.name = ent.user_id;
            ent.password = Encryption.Encrypt(ent.user_id);
        }

        public override bool ValidateSaveNew(t_com_user ent, ref string msg_validate)
        {
            if (this._Model.t_com_user.Any(qry => qry.user_id == ent.user_id))
            {
                msg_validate = "! Username and has in system.";
                return false;
            }
            else
                return true;
        }

        public bool ChangePassword(string _username, string _password)
        {
            return this._Model.Update(this, delegate ()
            {
                var ent = _Model.t_com_user.FirstOrDefault(x => x.user_id == _username);
                if (ent != null)
                {
                    ent.password = Encryption.Encrypt(_password);
                    ent.last_change_password = DateTime.Now;
                    // Save password log
                    t_com_user_change_password_log entIns = new t_com_user_change_password_log();
                    entIns.change_password_log_id = Guid.NewGuid().ToString();
                    entIns.user_id = _username;
                    entIns.password = Encryption.Encrypt(_password);
                    entIns.create_by = _username;
                    entIns.create_date = DateTime.Now;
                    this._Model.t_com_user_change_password_log.Add(entIns);
                }
                return _Model.SaveChanges();
            }, "Change Password Success.");
        }

        public bool ResetPassword(string _username, string _password) {
            return this._Model.Update(this, delegate () {
                var ent = _Model.t_com_user.FirstOrDefault(x => x.user_id == _username);
                if (ent != null) {
                    ent.password = Encryption.Encrypt(_password);
                    ent.last_change_password = null;    
                }
                return _Model.SaveChanges();
            }, "Change Password Success.");
        }

        public List<string> getPasswordLog(string _username) {
            List<string> passwrodLog = new List<string>();
            var ent = _Model.t_com_user_change_password_log.Where(x => x.user_id == _username);
            if (ent != null) {
                foreach (var item in ent) {
                    passwrodLog.Add(Encryption.Decrypt(item.password));
                }               
            }
            return passwrodLog;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var isAdmin = Convert.ToBoolean(this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_isAdmin").Value);
            var group_id = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_groupID").Value;
            var app_id = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_appID").Value;

            var result = from rows in this._Model.t_com_user

                         join usr_app in this._Model.t_com_user_application
                         on new { rows.user_id, app_id = app_id } equals new { usr_app.user_id, usr_app.app_id } into join_usr_app
                         from usr_app in join_usr_app.DefaultIfEmpty()

                         join usr_grb in this._Model.t_com_user_group
                         on new { usr_app.user_group_id, usr_app.app_id } equals new { usr_grb.user_group_id, usr_grb.app_id } into join_usr_grb
                         from usr_grb in join_usr_grb.DefaultIfEmpty()

                             //&& (isAdmin ? true : usr_app.user_group_id == group_id)
                         select new
                         {
                             KeyID = rows.tran_id,
                             rows.user_id,
                             rows.name,
                             rows.first_name,
                             rows.last_name,
                             usr_app.user_group_id,
                             user_group_name = usr_grb.name,
                             rows.department,
                             rows.email_address,
                             rows.locale_id,
                             local_name = rows.t_com_locale.name,
                             rows.is_active,
                             rows.create_date,
                             rows.create_by
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }
}

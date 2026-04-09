using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using SecurityM.Source;
using Prototype.Providers;

namespace SecurityM.Access.Config
{
    public class UserLogon : AGridObjectSourceQuery
    {
        public SecurityM_Entities _Model { get; set; }

        public UserLogon()
        {
            this._Model = new SecurityM_Entities();

            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static UserLogon Instance
        {
            get
            {
                using (UserLogon _Instance = new UserLogon())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public bool ClearUserLogon(string[] _users, string _app_id)
        {
            return this._Model.Update(this, delegate ()
            {
                foreach (var prms in _users)
                {
                    var arrs = prms.Split('|');
                    var id = arrs[0];
                    var device = arrs[1];

                    var ent = this._Model.t_com_user_device.FirstOrDefault(wh => wh.is_active == "YES" && wh.app_id == _app_id && wh.user_id == id && wh.device == device);
                    if (ent != null)
                    {
                        ent.is_active = "NO";
                    }
                }

                return this._Model.SaveChanges();
            }, "Clear users logon success.");
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _app_id = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_app_id").Value;

            var result = from rows in this._Model.t_com_user_device
                         join user in this._Model.t_com_user on rows.user_id equals user.user_id into join_usr
                         from user in join_usr.DefaultIfEmpty()
                         where rows.app_id == _app_id
                         select new
                         {
                             allow_clear = rows.is_active == "YES",
                             KeyID = rows.user_id + "|" + rows.device,
                             rows.user_id,
                             rows.device,
                             is_logon = rows.is_active,
                             full_name = user.first_name + " " + user.last_name,
                             last_login_date = rows.logon_datetime
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

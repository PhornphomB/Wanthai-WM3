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
    public class UserLogonView : AGridObjectSourceQuery
    {
        public SecurityM_Entities _Model { get; set; }

        public UserLogonView()
        {
            this._Model = new SecurityM_Entities();

            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static UserLogonView Instance
        {
            get
            {
                using (UserLogonView _Instance = new UserLogonView())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var app_id = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_appID").Value;

            var result = from rows in this._Model.t_com_user_logon
                         join user in this._Model.t_com_user on rows.user_id equals user.user_id into join_user
                         from user in join_user.DefaultIfEmpty()
                         where rows.app_id == app_id
                         select new
                         {
                             KeyID = rows.tran_id,
                             rows.user_id,
                             full_name = user.first_name + " " + user.last_name,
                             rows.device,
                             rows.tran_type,
                             rows.description,
                             rows.logon_start_datetime,
                             rows.logon_end_datetime
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

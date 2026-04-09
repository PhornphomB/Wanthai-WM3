using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Objects.SqlClient;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound
{
    public class OutboundComment : AEntityFormCommand<t_wms_outbound_comment>
    {
        public WMSEntities _Model { get; set; }

        public OutboundComment()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_outbound_comment; };
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_outbound_comment
                         join comm_type in this._Model.t_com_combobox_item on rows.comment_type equals comm_type.value_member
                         where comm_type.group_name == "outbound_order_comment_type"
                         select new
                         {
                             KeyId = rows.outbound_order_comment_id,
                             rows.outbound_order_master_id,
                             rows.sequence,
                             comm_type.display_member,
                             rows.comment_type,
                             rows.comment,
                             rows.comment_postion,
                             rows.create_by,
                             rows.create_date
                         };


            if (this.FilterCustom == null)
                return result.Where(x => false);

            var _outbound_order_master_id = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_master_id");
            if (_outbound_order_master_id != null)
            {
                Guid id = (Guid)_outbound_order_master_id.Value;
                result = result.Where(x => x.outbound_order_master_id == id);
            }

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }
}

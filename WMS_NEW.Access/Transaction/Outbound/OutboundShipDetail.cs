using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WMS_NEW.Source;
using Prototype.Providers;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Data.Objects.SqlClient;

namespace WMS_NEW.Access.Transaction.Outbound
{
    public class OutboundShipDetail : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public OutboundShipDetail()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;
            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

            base.GridObjContext = _Model;
        }


        #region Access Command Data

        public bool UpdateConfirmShip(Guid? _wh_master_id, Guid? _outbound_order_master_id, Guid? _carrier_id
            , string _truck_type, string _dri_license, string _container, DateTime? _post_date, DateTime? _ship_date)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_outbound_ship_confirm(_SessionVals.AppID, _wh_master_id, _SessionVals.DeviceID, _SessionVals.UserName, _outbound_order_master_id, _carrier_id,
                                            _truck_type, "", _dri_license, _container, "", _post_date, _ship_date, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());


            return !inverse;
        }

        public bool UpdateConfirmShipPartial(Guid _outbound_order_master_id)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_outbound_partial_ship_confirm(_SessionVals.AppID, _outbound_order_master_id, _SessionVals.DeviceID, _SessionVals.UserName, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());

            return !inverse;
        }

        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_outbound_detail
                         join uom in this._Model.t_wms_item_uom on rows.item_uom_id equals uom.item_uom_id
                         let status = rows.t_wms_outbound_master.order_status

                         select new
                         {
                             KeyId = rows.outbound_order_detail_id,
                             rows.line_number,
                             rows.item_number,
                             rows.item_description,
                             rows.quantity_order,
                             quantity_stage = rows.t_wms_outbound_pick_detail.Sum(sum => (status == "LOAD" ? sum.quantity_load : sum.quantity_stage)) ?? 0,
                             rows.quantity_ship,
                             uom.uom,
                             rows.outbound_order_master_id
                         };


            if (FilterCustom == null)
            {
                result = result.Where(x => false);
            }
            else
            {
                var _outbound_order_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_master_id").Value;

                result = result.Where(x => x.outbound_order_master_id == _outbound_order_master_id);
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

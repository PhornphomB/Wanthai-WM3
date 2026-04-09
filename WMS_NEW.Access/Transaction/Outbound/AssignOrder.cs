using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound {
    public class AssignOrder : AEntityFormCommand<t_wms_outbound_master> {
        #region ++INSTANCE STATIC++
        public static AssignOrder Instance {
            get {
                using (AssignOrder _Instance = new AssignOrder()) {
                    return _Instance;
                }
            }
        }
        #endregion
        protected WMSEntities _Model { get; set; }

        public AssignOrder() {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_outbound_master; };
        }        


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView() {           
            var result = from rows in this._Model.v_tms_assign_order
                         select new {
                             KeyId = rows.outbound_order_master_id
                             , rows.outbound_order_master_id
                             , rows.wh_master_id
                             , rows.wh_id
                             , rows.owner_id
                             , rows.owner_code
                             , rows.order_type
                             , rows.customer_order_number
                             , rows.outbound_order_number
                             , rows.order_status
                             , rows.customer_code
                             , rows.sum_quantity_order
                             , rows.sum_quantity_ship
                             , rows.sum_quantity_pick
                             , rows.sum_quantity_load
                             , rows.sum_quantity_stage
                             , rows.create_date
                         };
            return result;
        }

        public override IQueryable<dynamic> InitialQueryExport() {
            return this.InitialQueryView();
        }

        public v_tms_assign_order GetOrderHeader(Guid outbound_order_master_id) {
            var result = from rows in _Model.v_tms_assign_order
                         where rows.outbound_order_master_id == outbound_order_master_id
                         select rows;
            return result.FirstOrDefault();
        }

        #endregion


       
    }
}

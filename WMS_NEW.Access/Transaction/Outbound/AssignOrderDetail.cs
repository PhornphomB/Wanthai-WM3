using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound {

    // ยกเลิกใช้งาน Class นี้และไปใช้ AssignOrderDetailSourceStored แทน
    public class AssignOrderDetail : AEntityFormCommand<v_tms_assign_order_detail> {
        #region ++INSTANCE STATIC++
        public static AssignOrderDetail Instance {
            get {
                using (AssignOrderDetail _Instance = new AssignOrderDetail()) {
                    return _Instance;
                }
            }
        }
        #endregion
        protected WMSEntities _Model { get; set; }

        public AssignOrderDetail() {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_tms_manifest_order_mapping; };
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView() {               
            Guid _wh_master_id = new Guid();
            Guid _outbound_order_master_id = new Guid();

            if (this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_master_id").Value != null) {
                _wh_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_master_id").Value;                
            }
            if (this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_master_id").Value != null) {
                _outbound_order_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_master_id").Value;                
            }

            var result = from rows in this._Model.v_tms_assign_order_detail
                         where rows.wh_master_id == _wh_master_id
                         && (rows.outbound_order_master_id == _outbound_order_master_id || rows.outbound_order_master_id == null)
                         select new {
                             KeyId = rows.truck_manifest_id + "|" + rows.location_id + "|" + rows.door + "|in_vchOutboundPickMasterID"
                             , rows.location_id
                             , rows.is_checked
                             , rows.is_allow_check
                             , rows.door
                             , rows.truck_type
                             , rows.license
                             , rows.head_tail
                             , rows.assgin_date
                             , rows.wh_master_id
                             , rows.outbound_order_master_id
                             , rows.truck_manifest_id
                             //
                             , chk_is_checked = rows.is_checked == 1 ? true : false
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

        //_listKey คือ truck_manifest_id
        public bool SaveMapping(List<ConfigGlobal.DTO._Global.KeySelect> _listKey, string _create_by, Guid outbound_order_master_id, string outbound_order_number) {
            try {
                var listSelect = _listKey.Where(w => w.Active == true).Select(s => s.KeyId).ToList();                
                var listUnselect = _listKey.Where(w => w.Active == false).Select(s => Guid.Parse(s.KeyId.ToString())).ToList();

                //Insert กรณีไม่มีการ mapping มาก่อน
                if (listSelect.Count > 0) {
                    t_tms_manifest_order_mapping entIns;
                    Guid truck_manifest_id;
                    foreach (var item in listSelect) {                        
                        truck_manifest_id = Guid.Parse(item.ToString()); 
                        var result = from rows in this._Model.t_tms_manifest_order_mapping
                                     where rows.outbound_order_master_id == outbound_order_master_id
                                     && rows.truck_manifest_id == truck_manifest_id
                                     select rows;
                        if (result.Count() == 0) {
                            entIns = new t_tms_manifest_order_mapping();
                            entIns.manifest_order_mapping_id = Guid.NewGuid();
                            entIns.outbound_order_master_id = outbound_order_master_id;
                            entIns.outbound_order_number = outbound_order_number;
                            entIns.truck_manifest_id = truck_manifest_id;
                            entIns.create_by = _create_by;
                            entIns.create_date = DateTime.Now;

                            this._Model.t_tms_manifest_order_mapping.Add(entIns);
                        }
                    }
                }                

                //Delete
                if (listUnselect.Count > 0) {
                    var entDel = from rows in this._Model.t_tms_manifest_order_mapping
                                 where listUnselect.Contains(rows.truck_manifest_id)
                                 && rows.outbound_order_master_id == outbound_order_master_id
                                 select rows;
                    this._Model.t_tms_manifest_order_mapping.RemoveRange(entDel);
                }

                return this.GridObjContext.Save(this, delegate () {
                    return _Model.SaveChanges();
                });
            } catch (Exception ex) {
                throw ex;
            }
        }

        #endregion
    }
}

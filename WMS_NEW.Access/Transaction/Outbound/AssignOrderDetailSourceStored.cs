using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Xml.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound {
    public class AssignOrderDetailSourceStored : AGridObjectSourceStore {
        protected WMSEntities _Model { get; set; }

        public AssignOrderDetailSourceStored() {
            _Model = new WMSEntities();
            _Model.Database.CommandTimeout = 600;
            _Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

            base.GridObjContext = _Model;
        }       

        public override void InitialStoreView() {
            Guid outbound_order_master_id = Guid.Empty; 

            var ent = this.FilterCustom.Where(w => w.DataFieldValue == "_outbound_order_master_id").FirstOrDefault();
            if (ent != null && ent.Value != null && Guid.Parse(ent.Value.ToString()) != Guid.Empty) {
                outbound_order_master_id = Guid.Parse(ent.Value.ToString());
            }

            Guid wh_master_id = Guid.Empty;
            var entWh = this.FilterCustom.Where(w => w.DataFieldValue == "_wh_master_id").FirstOrDefault();
            if (entWh != null && entWh.Value != null) {
                wh_master_id = Guid.Parse(entWh.Value.ToString());
            }

            this.StoreNameQuery = "usp_outbound_assgin_order_detail";
            this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_outbound_order_master_id", outbound_order_master_id));
            this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_wh_master_id", wh_master_id));

        }

        public override void InitialStoreExport() {
            Guid outbound_order_master_id = Guid.Empty;

            var ent = this.FilterCustom.Where(w => w.DataFieldValue == "_outbound_order_master_id").FirstOrDefault();
            if (ent != null && ent.Value != null && Guid.Parse(ent.Value.ToString()) != Guid.Empty) {
                outbound_order_master_id = Guid.Parse(ent.Value.ToString());
            }

            Guid wh_master_id = Guid.Empty;
            var entWh = this.FilterCustom.Where(w => w.DataFieldValue == "_wh_master_id").FirstOrDefault();
            if (entWh != null && entWh.Value != null) {
                wh_master_id = Guid.Parse(entWh.Value.ToString());
            }

            this.StoreNameQuery = "usp_outbound_assgin_order_detail";
            this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_outbound_order_master_id", outbound_order_master_id));
            this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_wh_master_id", wh_master_id));
        }

        public string GetOrderStatus(Guid outbound_order_master_id) {
            string order_status = string.Empty;
            var result = from rows in _Model.t_wms_outbound_master
                         where rows.outbound_order_master_id == outbound_order_master_id
                         select new {
                             rows.order_status                            
                         };
            if (result != null) {
                order_status = result.FirstOrDefault().order_status;
            }
            return order_status;
        }

        public int GetCountAssignOrder(Guid outbound_order_master_id) {
            int CountAssignOrder = 0;
            var result = from rows in _Model.t_tms_manifest_order_mapping
                         where rows.outbound_order_master_id == outbound_order_master_id
                         select new {
                             rows.outbound_order_master_id
                         };
            if (result != null) {
                CountAssignOrder = result.Count();
            }
            return CountAssignOrder;
        }

        public int GetCountAlreadyCheckPickDetail(List<ConfigGlobal.DTO._Global.KeySelect> _listKey, Guid outbound_order_master_id, ref string door_already_checker) {
            //_listKey ที่ใช้คือ truck_manifest_id อยู่ index ที่ 0
            for (int i = 0; i < _listKey.Count; i++) {
                _listKey[i].KeyId = _listKey[i].KeyId.ToString().Split('|')[0];
            }
            //            
            var listUnselect = _listKey.Where(w => w.Active == false).Select(s => Guid.Parse(s.KeyId.ToString())).ToList();
            int CountChecker = 0;
            door_already_checker = string.Empty;
            var result = (from r_master in _Model.t_wms_outbound_pick_master
                         join r_detail in _Model.t_wms_outbound_pick_detail on r_master.outbound_pick_master_id equals r_detail.outbound_pick_master_id
                         join r_manifest in _Model.t_tms_truck_manifest on r_detail.truck_manifest_id equals r_manifest.truck_manifest_id
                         where r_master.outbound_order_master_id == outbound_order_master_id
                         && listUnselect.Contains((Guid)r_detail.truck_manifest_id)
                         select new {
                             r_manifest.door
                         }).ToList().Distinct();
            if (result != null) {
                foreach (var item in result) {
                    door_already_checker = door_already_checker + item.door + ", ";
                }
                CountChecker = result.Count();
            }
            return CountChecker;
        }

        public bool SaveMapping(List<ConfigGlobal.DTO._Global.KeySelect> _listKey, string _create_by, Guid outbound_order_master_id, string outbound_order_number) {
            try {
                //_listKey ที่ใช้คือ truck_manifest_id อยู่ index ที่ 0
                for (int i = 0; i < _listKey.Count; i++) {
                    _listKey[i].KeyId = _listKey[i].KeyId.ToString().Split('|')[0];
                }
                //
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

    }
}

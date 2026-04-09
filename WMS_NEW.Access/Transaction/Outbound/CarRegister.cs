using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound {
    public class CarRegister : AEntityFormCommand<t_tms_truck_manifest> {
        #region ++INSTANCE STATIC++
        public static CarRegister Instance {
            get {
                using (CarRegister _Instance = new CarRegister()) {
                    return _Instance;
                }
            }
        }
        #endregion
        protected WMSEntities _Model { get; set; }

        public CarRegister() {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_tms_truck_manifest; };
        }

        public override bool ValidateSaveNew(t_tms_truck_manifest ent, ref string msg_validate) {
            if (_Model.t_tms_truck_manifest.Any(x => x.license == ent.license && x.load_status != "CLOSED")) {
                msg_validate = "! This license already register";
                return false;
            }
            if (ent.door != "ลานรถ : ลานรถรอเข้า Door") {
                if (_Model.t_tms_truck_manifest.Any(x => x.location_id == ent.location_id && x.load_status != "CLOSED")) {
                    msg_validate = "! Door already in use";
                    return false;
                }
            }
            if (Convert.ToDateTime(ent.register_date) > DateTime.Now.AddMinutes(1))
            {
                msg_validate = "Can not select register date time future";
                return false;
            }
            return true;
        }

        public override bool ValidateSaveUpdate(t_tms_truck_manifest ent, ref string msg_validate) {           
            if (ent.door != "ลานรถ : ลานรถรอเข้า Door") {
                if (_Model.t_tms_truck_manifest.Any(x => x.location_id == ent.location_id && x.load_status != "CLOSED" && x.truck_manifest_id != ent.truck_manifest_id)) {
                    msg_validate = "! Door already in use";
                    return false;
                }
            }
            return true; 
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView() {

            //var result = from rows in this._Model.t_tms_truck_manifest
            //             join wh in this._Model.t_wms_wh on rows.wh_master_id equals wh.wh_master_id
            //             join ow in this._Model.t_wms_owner on rows.owner_id equals ow.owner_id
            //             select new {
            //                 KeyId = rows.truck_manifest_id
            //                 , rows.truck_manifest_id
            //                 , rows.wh_master_id
            //                 , rows.owner_id
            //                 , rows.truck_type
            //                 , rows.license
            //                 , rows.head_tail
            //                 , rows.register_date
            //                 , rows.register_by
            //                 , rows.location_id
            //                 , rows.door
            //                 , rows.dock_door_date
            //                 , rows.dock_door_by
            //                 , rows.load_status
            //                 , rows.close_date
            //                 , rows.close_by
            //                 , rows.create_by
            //                 , rows.create_date
            //                 , rows.update_by
            //                 , rows.update_date
            //                 //
            //                 , wh.wh_id
            //                 , ow.owner_code
            //             };

            var result = from rows in this._Model.v_tms_truck_manifest                         
                         select new {
                             KeyId = rows.truck_manifest_id
                             , rows.truck_manifest_id
                             , rows.wh_master_id
                             , rows.owner_id
                             , rows.truck_type
                             , rows.license
                             , rows.head_tail
                             , rows.register_date
                             , rows.register_by
                             , rows.location_id
                             , rows.door
                             , rows.dock_door_date
                             , rows.dock_door_by
                             , rows.load_status
                             , rows.close_date
                             , rows.close_by
                             , rows.create_by
                             , rows.create_date
                             , rows.update_by
                             , rows.update_date
                             //
                             , rows.wh_id
                             , rows.owner_code
                             //
                             , rows.is_able_delete
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport() {
            return this.InitialQueryView();
        }

        public bool updateCloseLoadStatus(Guid _truck_manifest_id) {
            return this._Model.Update(this, delegate () {
                var result = 0;
                var ent = this._Model.t_tms_truck_manifest.FirstOrDefault(x => x.truck_manifest_id == _truck_manifest_id);
                if (ent != null) {
                    ent.load_status = "CLOSED";                   
                    ent.update_by = _SessionVals.UserName;
                    ent.update_date = DateTime.Now;
                    ent.close_date = DateTime.Now;
                    ent.close_by = _SessionVals.UserName;
                    result = this._Model.SaveChanges();
                }

                return result;
            }, "Update CLOSED Success.");
        }

        // ต้องเช็คก่อนว่า ทุก Order Ship หมดหรือยัง ถ้า ship หมดแล้วจะไม่สามารถ Open ได้อีก เช็คจากปุ่มจะไม่แสดง
        // ต้องเช็คว่า Door เก่าที่เคยเลือกไว้ว่างหรือไม่ เข้าเคส ValidateSaveNew
        public bool updateCheckingLoadStatus(Guid _truck_manifest_id, ref string _message) {            
            var resultTruckManifest = (from rows in this._Model.t_tms_truck_manifest
                                       where rows.truck_manifest_id == _truck_manifest_id
                                       select rows).FirstOrDefault();
            if (resultTruckManifest != null) {         
                //
                if (!this.ValidateSaveNew(resultTruckManifest, ref _message)) {                    
                    return false;
                }
                //
                var result = 0;
                var ent = this._Model.t_tms_truck_manifest.FirstOrDefault(x => x.truck_manifest_id == _truck_manifest_id);
                if (ent != null) {
                    ent.load_status = "CHECKING";
                    ent.update_by = _SessionVals.UserName;
                    ent.update_date = DateTime.Now;
                    ent.close_date = null;
                    ent.close_by = null;
                    result = this._Model.SaveChanges();
                }
                if (result >= 1) {
                    return true;
                } else {
                    return false;
                }
            } else {
                _message = "Not found truck_manifest_id";
                return false;
            }            
        }

        #endregion


        #region Query Property

        public IQueryable<Property> GetCodeQueryDoor(Guid wh_master_id, Guid location_id) {
            //var doorInUse = from rows in _Model.t_tms_truck_manifest
            //                where rows.load_status != "CLOSED"
            //                && rows.location_id != location_id
            //                && rows.door != "ลานรถ : ลานรถรอเข้า Door"
            //                select rows;
            //var door = from rows in _Model.t_wms_location
            //           where rows.is_active == "YES"
            //           && rows.wh_master_id == wh_master_id
            //           && rows.loc_type == "DOOR"
            //           orderby rows.location ascending
            //           select new Property {
            //               guid_member = rows.location_id,
            //               display_member = rows.location + " : " + rows.description
            //           };
            ////เลือก Door ที่มีสถานะพร้อมใช้งานเท่านั้น
            //var result = door.Where(d => !doorInUse.Select(diu => diu.location_id).Contains(d.guid_member));


            var result = from rows in _Model.t_wms_location
                         where rows.is_active == "YES"
                         && rows.wh_master_id == wh_master_id
                         && rows.loc_type == "DOOR"
                         //orderby rows.location ascending
                         select new Property {
                             guid_member = rows.location_id,
                             display_member = rows.location + " : " + rows.description
                         };


            return result.Distinct().OrderBy(n => n.display_member);
        }

        public IQueryable<Property> GetCodeQueryDoorSTG(Guid wh_master_id) {  
            var result = from rows in _Model.t_wms_location
                         where rows.is_active == "YES"
                         && rows.wh_master_id == wh_master_id
                         && rows.loc_type == "DOOR"
                         && rows.location == "ลานรถ"
                         select new Property {
                             guid_member = rows.location_id,
                             display_member = rows.location + " : " + rows.description
                         };
            return result.Take(1);
        }

        #endregion
    }
}

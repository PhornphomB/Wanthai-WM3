using Prototype.Providers;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound
{
    public class OutboundPickDetail : AGridObjectSourceQuery, IDisposable
    {

        public WMSEntities _Model { get; set; }

        public OutboundPickDetail()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;
            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

            base.GridObjContext = _Model;
        }


        #region Access Command Data

        public bool UpdateConfirmPick(Guid _outbound_order_master_id, Guid _stage_location_id)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            var dt = (from rows in this._Model.v_wms_outbound_pick_detail_linenumber
                      join pickdet in this._Model.t_wms_outbound_pick_detail on rows.outbound_pick_detail_id equals pickdet.outbound_pick_detail_id
                      join detail in this._Model.t_wms_outbound_detail on pickdet.outbound_order_detail_id equals detail.outbound_order_detail_id
                      join uom in this._Model.t_wms_item_uom on detail.item_uom_id equals uom.item_uom_id
                      where detail.outbound_order_master_id == _outbound_order_master_id && rows.quantity_comfirm_pick_pc > 0 && rows.quantity_remain > 0
                      select new
                      {
                          outbound_order_detail_id = pickdet.outbound_order_detail_id,
                          outbound_pick_detail_id = rows.outbound_pick_detail_id,
                          pickdet.location_id,
                          location = pickdet.t_wms_location.location,
                          detail.line_number,
                          detail.wh_item_master_id,
                          detail.item_master_id,
                          detail.item_number,
                          quantity_plan = rows.quantity_plan,
                          quantity_confirm_pick = rows.quantity_comfirm_pick_pc,
                          uom_prompt = uom.uom_prompt,
                          parent_lpn = rows.parent_lpn,
                          lpn = rows.lpn,
                          lot_number = rows.lot_number,
                          expiry_date = rows.expiry_date,
                          serial_number = (pickdet.serial_number ?? string.Empty) == string.Empty ? pickdet.confirm_serial_number_pc : pickdet.serial_number,
                          rows.attribute1,
                          rows.attribute2,
                          rows.attribute3,
                          rows.attribute4,
                          rows.attribute5,
                          pickdet.receive_date,
                          detail.default_item_status,

                          pickdet.confirm_serial_number_pc

                      }).ToDataTable("ConfirmPick");

            var ds = new DataSet("ConfirmPick");
            ds.Tables.Add(dt);

            this._Model.usp_outbound_pick_confirm(_SessionVals.AppID, _SessionVals.DeviceID, _SessionVals.UserName, _outbound_order_master_id, ds.GetXml(), _stage_location_id, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());


            return !inverse;
        }

        public bool UpdateUnPick(string _wh_id, string _outbound_order_number, Guid[] _listKeyId)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            var result = (from rows in this._Model.t_wms_outbound_pick_detail
                          join detail in this._Model.t_wms_outbound_detail on rows.outbound_order_detail_id equals detail.outbound_order_detail_id
                          join uom in this._Model.t_wms_item_uom on detail.item_uom_id equals uom.item_uom_id

                          from location in this._Model.t_wms_location
                                          .Where(loc => loc.location_id == (rows.staging_location_id != null ? rows.staging_location_id : rows.pick_location_id))

                          where rows.quantity_pick > 0 && _listKeyId.Contains(rows.outbound_pick_detail_id)

                          let item = rows.t_wms_wh_item.t_wms_item

                          select new
                          {
                              rows.outbound_pick_detail_id,
                              location.location,
                              rows.parent_lpn,
                              rows.lpn,
                              item.item_number,
                              rows.lot_number,
                              rows.expiry_date,
                              rows.serial_number,
                              quantity = rows.quantity_stage,
                              uom.uom,
                              rows.attribute1,
                              rows.attribute2,
                              rows.attribute3,
                              rows.attribute4,
                              rows.attribute5,
                              rows.staging_lpn,
                              rows.staging_parent_lpn,
                              owner_code = rows.t_wms_outbound_detail.t_wms_outbound_master.t_wms_owner.owner_code
                          }).ToArray();

            foreach (var item in result)
            {
                this._Model.usp_outbound_unpick(_SessionVals.AppID,
                    _wh_id,
                    _SessionVals.DeviceID,
                    _SessionVals.UserName,
                    _outbound_order_number,
                    item.owner_code,//owner_code
                    item.location,
                    item.staging_parent_lpn,
                    item.staging_lpn,
                    item.item_number,
                    item.lot_number,
                    item.expiry_date,
                    item.serial_number,
                    item.attribute1,
                    item.attribute2,
                    item.attribute3,
                    item.attribute4,
                    item.attribute5,
                    item.quantity,
                    item.uom,
                    errCode, errMsg);

                var inverse = this._Model.Engaged(this, delegate ()
                {
                    if (errCode.Value.ToString() == "0") return false;
                    else return true;
                }, errMsg.Value.ToString());

                if (inverse)
                {
                    return false;
                }
            }

            return true;
        }

        public double UpdatePickQty(Guid _order_pick_detail_id, double _qty_pick)
        {
            var ent = this._Model.t_wms_outbound_pick_detail.First(qry => qry.outbound_pick_detail_id == _order_pick_detail_id);

            return this._Model.GetDataBy(this, delegate ()
            {
                if (_qty_pick >= 0 && (ent.quantity_plan - ent.quantity_pick) >= _qty_pick)
                {
                    ent.quantity_comfirm_pick_pc = _qty_pick;
                    this._Model.SaveChanges();

                    return 0;
                }
                else
                {
                    _qty_pick = (ent.quantity_plan - (ent.quantity_pick ?? 0));

                    ent.quantity_comfirm_pick_pc = _qty_pick;
                    this._Model.SaveChanges();

                    return _qty_pick;
                }
            });
        }

        public void UpdateResetQtyConfirm(Guid _outbound_order_master_id)
        {
            //this._Model.Usp_outbound_update_quantity_comfirm_pick_PC(_outbound_order_master_id);
        }

        public void UserPickOrder(Nullable<System.Guid> in_vchForkLocationID, Nullable<System.Guid> in_vchWhMasterID, Nullable<System.Guid> in_vchOutboundMasterID, Nullable<System.Guid> in_vchOwnerID, string in_vchBoxNumber, Nullable<System.Guid> in_vchLocationID, string in_vchParentLPN, string in_vchLPN, Nullable<System.Guid> in_vchWhItemMasterID, string in_vchItemStatus, Nullable<double> in_fltQuantity, Nullable<System.Guid> in_vchItemUOMID, string in_vchLotNumber, string in_vchExpiryDate, string in_vchSerialNumber, string in_vchAttribute1, string in_vchAttribute2, string in_vchAttribute3, string in_vchAttribute4, string in_vchAttribute5)
        {
            try
            {
                string in_vchApplicationID = _SessionVals.AppID;
                string in_vchLocalID = _SessionVals.LocaleID;
                string in_vchDeviceID = _SessionVals.DeviceID;
                string in_vchUserID = _SessionVals.UserName;

                var out_ErrorCode = new ObjectParameter("out_ErrorCode", typeof(string));
                var out_ErrorMessage = new ObjectParameter("out_ErrorMessage", typeof(string));

                this._Model.usp_outbound_generate_user_pick_by_order(in_vchApplicationID, in_vchLocalID, in_vchDeviceID, in_vchUserID, in_vchForkLocationID, in_vchWhMasterID, in_vchOutboundMasterID, in_vchOwnerID, in_vchBoxNumber, in_vchLocationID, in_vchParentLPN, in_vchLPN, in_vchWhItemMasterID, in_vchItemStatus, in_fltQuantity, in_vchItemUOMID, in_vchLotNumber, in_vchExpiryDate, in_vchSerialNumber, in_vchAttribute1, in_vchAttribute2, in_vchAttribute3, in_vchAttribute4, in_vchAttribute5, out_ErrorCode, out_ErrorMessage);


                if (out_ErrorCode.Value.ToString() == "0")
                {
                    this.MessageSuccess(this, out_ErrorMessage.Value.ToString());
                }
                else
                {
                    this.MessageWarning(this, out_ErrorMessage.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            DateTime min_date = new DateTime(1753, 1, 7);

            var result = from rows in this._Model.v_wms_outbound_pick_detail_linenumber

                         let _minDate = DbFunctions.CreateDateTime(0001, 1, 1, 0, 0, 0)
                         let _year = SqlFunctions.DatePart("year", (DbFunctions.Left(rows.expiry_date, 4).Trim()))
                         let _month = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(4, 2).Trim())
                         let _day = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(6, 2).Trim())
                         let expiry_date = !string.IsNullOrEmpty(rows.expiry_date)
                         ? SqlFunctions.DateAdd("day", _day - 1,
                               SqlFunctions.DateAdd("month", _month - 1,
                                   SqlFunctions.DateAdd("year", _year - 1, _minDate)))
                         : min_date

                         select new
                         {
                             KeyId = rows.outbound_pick_detail_id,
                             rows.is_override,
                             rows.location,
                             rows.item_number,
                             rows.item_description,
                             rows.item_category,
                             rows.category_description,
                             rows.uom,
                             rows.grade,
                             rows.price,
                             rows.quantity_plan,
                             rows.quantity_pick,
                             quantity_remain = (rows.quantity_plan - rows.quantity_pick),
                             rows.quantity_pack,
                             rows.quantity_stage,
                             rows.quantity_load,
                             rows.quantity_comfirm_pick_pc,
                             rows.parent_lpn,
                             rows.lpn,
                             rows.lot_number,
                             expiry_date = expiry_date == min_date ? null : expiry_date,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5,
                             rows.is_staging_location,// = rows.staging_location_id == null ? false : true,
                             rows.pick_line_number,
                             rows.pick_line_number_int,
                             rows.outbound_order_master_id,

                             serial_number = ((rows.serial_number ?? string.Empty) == string.Empty) ? rows.confirm_serial_number_pc : rows.serial_number,
                             sn_control = rows.sn_control == "FULL" ? true : false,
                             is_serial = !string.IsNullOrEmpty(rows.serial_number) ? true : false


                         };


            if (FilterCustom == null)
            {
                result = result.Where(x => false);
            }
            else
            {
                var _outbound_order_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_master_id").Value;
                var _allow_pick = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_allow_pick");
                var _allow_unpick = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_allow_unpick");

                result = result.Where(x => x.outbound_order_master_id == _outbound_order_master_id);

                if (_allow_pick != null)
                {
                    result = result.Where(qry => qry.is_override == "NO" && (qry.quantity_plan - qry.quantity_pick) > 0);
                }
                if (_allow_unpick != null)
                {
                    result = result.Where(qry => qry.quantity_pick > 0);
                }
            }

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

        public IQueryable<dynamic> ListData(Guid _outbound_order_master_id)
        {
            DateTime min_date = new DateTime(1753, 1, 7);

            var result = from rows in this._Model.v_wms_outbound_pick_detail_linenumber

                         let _minDate = DbFunctions.CreateDateTime(0001, 1, 1, 0, 0, 0)
                         let _year = SqlFunctions.DatePart("year", (DbFunctions.Left(rows.expiry_date, 4).Trim()))
                         let _month = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(4, 2).Trim())
                         let _day = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(6, 2).Trim())
                         let expiry_date = !string.IsNullOrEmpty(rows.expiry_date)
                         ? SqlFunctions.DateAdd("day", _day - 1,
                               SqlFunctions.DateAdd("month", _month - 1,
                                   SqlFunctions.DateAdd("year", _year - 1, _minDate)))
                         : min_date
                         where rows.outbound_order_master_id == _outbound_order_master_id
                         && rows.is_override == "NO"
                         && (rows.quantity_plan - rows.quantity_pick) > 0
                         select new
                         {
                             KeyId = rows.outbound_pick_detail_id,
                             rows.is_override,
                             rows.location,
                             rows.item_number,
                             rows.item_description,
                             rows.item_category,
                             rows.category_description,
                             rows.uom,
                             rows.grade,
                             rows.price,
                             rows.quantity_plan,
                             rows.quantity_pick,
                             quantity_remain = (rows.quantity_plan - rows.quantity_pick),
                             rows.quantity_pack,
                             rows.quantity_stage,
                             rows.quantity_load,
                             rows.quantity_comfirm_pick_pc,
                             rows.parent_lpn,
                             rows.lpn,
                             rows.lot_number,
                             expiry_date = expiry_date == min_date ? null : expiry_date,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5,
                             rows.is_staging_location,// = rows.staging_location_id == null ? false : true,
                             rows.pick_line_number,
                             rows.pick_line_number_int,
                             rows.outbound_order_master_id
                         };


            return result;
        }

        public IQueryable<Property> GetQuery(string _control_name, OutboundMasterDto _dto, Nullable<System.Guid> _location_id, Nullable<System.Guid> _item_master_id, string _item_status, string _lot_number, string _exp_date, string _serial, string _lpn)
        {
            try
            {
                return this._Model.GetDataBy(this, delegate ()
                {
                    string in_vchApplicationID = _SessionVals.AppID;
                    string in_vchControlName = _control_name;
                    string in_vchLocalID = _SessionVals.LocaleID;
                    string in_vchDeviceID = _SessionVals.DeviceID;
                    string in_vchUserID = _SessionVals.UserName;
                    Nullable<System.Guid> in_vchWhMasterID = _dto.wh_master_id;
                    string in_vchWhID = _dto.wh_id;
                    Nullable<System.Guid> in_vchOutboundMasterID = _dto.outbound_order_master_id;
                    string in_vchOutboundOrderNumber = _dto.outbound_order_number;
                    Nullable<System.Guid> in_vchOwnerID = _dto.owner_id;
                    string in_vchOwnerCode = _dto.owner_code;
                    Nullable<System.Guid> in_vchLocationID = _location_id;
                    var entLoc = this._Model.t_wms_location.Where(w => w.location_id == _location_id).FirstOrDefault();
                    string in_vchLocation = entLoc == null ? string.Empty : entLoc.location;
                    var entWhItem = this._Model.t_wms_wh_item.Where(w => w.wh_master_id == _dto.wh_master_id && w.item_master_id == _item_master_id).FirstOrDefault();
                    Nullable<System.Guid> wh_item_master_id = null;
                    if (entWhItem != null)
                    {
                        wh_item_master_id = entWhItem.wh_item_master_id;
                    }
                    Nullable<System.Guid> in_vchWhItemMasterID = wh_item_master_id;
                    Nullable<System.Guid> in_vchItemMasterID = _item_master_id;
                    var entItem = this._Model.t_wms_item.Where(w => w.item_master_id == _item_master_id).FirstOrDefault();
                    string in_vchItemNumber = entItem == null ? string.Empty : entItem.item_number;
                    string in_vchItemStatus = _item_status;
                    string in_vchLotNumber = _lot_number;
                    string in_vchExpiryDate = _exp_date;
                    string in_vchSerialNumber = _serial;
                    //---------------------------------------
                    string in_vchAttribute1 = string.Empty;
                    string in_vchAttribute2 = string.Empty;
                    string in_vchAttribute3 = string.Empty;
                    string in_vchAttribute4 = string.Empty;
                    string in_vchAttribute5 = string.Empty;
                    string in_vchPickRule = string.Empty;
                    string in_vchBoxNumber = string.Empty;
                    string in_vchParentLPN = string.Empty;
                    string in_vchLPN = _lpn;


                    DataTable dt = new DataTable();
                    using (var model = new Source.WMSEntities())
                    {
                        var cmd = model.Database.Connection.CreateCommand();
                        cmd.CommandText = "usp_api_binding_user_pick_order";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchApplicationID", Value = (object)in_vchApplicationID ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchControlName", Value = (object)in_vchControlName ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchLocalID", Value = (object)in_vchLocalID ?? DBNull.Value, SqlDbType = SqlDbType.VarChar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchDeviceID", Value = (object)in_vchDeviceID ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchUserID", Value = (object)in_vchUserID ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchWhMasterID", Value = (object)in_vchWhMasterID ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchWhID", Value = (object)in_vchWhID ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchOutboundMasterID", Value = (object)in_vchOutboundMasterID ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchOutboundOrderNumber", Value = (object)in_vchOutboundOrderNumber ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchOwnerID", Value = (object)in_vchOwnerID ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchOwnerCode", Value = (object)in_vchOwnerCode ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchBoxNumber", Value = (object)in_vchBoxNumber ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchLocationID", Value = (object)in_vchLocationID ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchLocation", Value = (object)in_vchLocation ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchParentLPN", Value = (object)in_vchParentLPN ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchLPN", Value = (object)in_vchLPN ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchWhItemMasterID", Value = (object)in_vchWhItemMasterID ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchItemMasterID", Value = (object)in_vchItemMasterID ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchItemNumber", Value = (object)in_vchItemNumber ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchItemStatus", Value = (object)in_vchItemStatus ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchLotNumber", Value = (object)in_vchLotNumber ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchExpiryDate", Value = (object)in_vchExpiryDate ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchSerialNumber", Value = (object)in_vchSerialNumber ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchAttribute1", Value = (object)in_vchAttribute1 ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchAttribute2", Value = (object)in_vchAttribute2 ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchAttribute3", Value = (object)in_vchAttribute3 ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchAttribute4", Value = (object)in_vchAttribute4 ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchAttribute5", Value = (object)in_vchAttribute5 ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchPickRule", Value = (object)in_vchPickRule ?? DBNull.Value });
                        cmd.Connection.Open();


                        dt.Load(cmd.ExecuteReader());
                    }

                    foreach (DataRow item in dt.Rows)
                    {
                        string typeName = string.Empty;
                        var propertyInfo = item["value_member"].GetType();
                        typeName = propertyInfo.Name.ToUpper();
                        if (typeName == "GUID")
                        {
                            var result = from rows in dt.AsEnumerable()
                                         select new Property
                                         {
                                             display_member = rows.Field<string>("display_member"),
                                             guid_member = rows.Field<Guid>("value_member")
                                         };

                            return result.AsQueryable();
                        }
                        else
                        {
                            var result = from rows in dt.AsEnumerable()
                                         select new Property
                                         {
                                             display_member = rows.Field<string>("display_member"),
                                             value_member = rows.Field<string>("value_member")
                                         };

                            return result.AsQueryable();
                        }
                    }

                    return null;
                });


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }

    public class PickSerial : AGridObjectSourceStore
    {
        public WMSEntities _Model { get; set; }

        public PickSerial()
        {
            _Model = new WMSEntities();
            _Model.Database.CommandTimeout = 600;
            _Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

            base.GridObjContext = _Model;


        }

        #region Inherit AGridObjectSourceQuery

        public override void InitialStoreView()
        {
            var entPick = this.FilterCustom.Where(w => w.DataFieldValue == "_outbound_pick_detail_id").FirstOrDefault();
            Guid outbound_pick_detail_id = entPick != null ? (Guid)entPick.Value : Guid.Empty;

            var ent = this._Model.t_wms_outbound_pick_detail.Where(w => w.outbound_pick_detail_id == outbound_pick_detail_id).FirstOrDefault();
            if (ent != null)
            {
                string outbound_pick_master_id = ent.outbound_pick_master_id.ToString();
                string wh_master_id = ent.t_wms_wh_item.wh_master_id.ToString();
                string wh_item_master_id = ent.wh_item_master_id.ToString();
                string location_id = ent.location_id.ToString();
                string owner_id = ent.t_wms_outbound_pick_master.t_wms_outbound_master.owner_id.ToString();

                string parent_lpn = (ent.parent_lpn ?? "");
                string lpn = (ent.lpn ?? "");
                string lot_number = (ent.lot_number ?? "");
                string expiry_date = (ent.expiry_date ?? "");
                string attribute1 = (ent.attribute1 ?? "");
                string attribute2 = (ent.attribute2 ?? "");
                string attribute3 = (ent.attribute3 ?? "");
                string attribute4 = (ent.attribute4 ?? "");
                string attribute5 = (ent.attribute5 ?? "");


                this.StoreNameQuery = "usp_pick_binding_serial_number";
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchWhMasterID", wh_master_id));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchOutboundPickMasterID", outbound_pick_master_id));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchOwnerID", owner_id));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchLocationID", location_id));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchParentLPN", parent_lpn));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchLPN", lpn));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchWhItemMasterID", wh_item_master_id));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchLotNumber", lot_number));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchExpiryDate", expiry_date));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_ReceiveDate", ent.receive_date));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchAttribute1", attribute1));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchAttribute2", attribute2));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchAttribute3", attribute3));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchAttribute4", attribute4));
                this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchAttribute5", attribute5));
            }

        }

        public override void InitialStoreExport()
        {
            var entPick = this.FilterCustom.Where(w => w.DataFieldValue == "_outbound_pick_detail_id").FirstOrDefault();
            Guid outbound_pick_detail_id = entPick != null ? (Guid)entPick.Value : Guid.Empty;

            var ent = this._Model.t_wms_outbound_pick_detail.Where(w => w.outbound_pick_detail_id == outbound_pick_detail_id).FirstOrDefault();
            if (ent != null)
            {
                string outbound_pick_master_id = ent.outbound_pick_master_id.ToString();
                string wh_master_id = ent.t_wms_wh_item.wh_master_id.ToString();
                string wh_item_master_id = ent.wh_item_master_id.ToString();
                string location_id = ent.location_id.ToString();
                string owner_id = ent.t_wms_outbound_pick_master.t_wms_outbound_master.owner_id.ToString();

                string parent_lpn = (ent.parent_lpn ?? "");
                string lpn = (ent.lpn ?? "");
                string lot_number = (ent.lot_number ?? "");
                string expiry_date = (ent.expiry_date ?? "");
                string attribute1 = (ent.attribute1 ?? "");
                string attribute2 = (ent.attribute2 ?? "");
                string attribute3 = (ent.attribute3 ?? "");
                string attribute4 = (ent.attribute4 ?? "");
                string attribute5 = (ent.attribute5 ?? "");

                this.StoreNameExport = "usp_pick_binding_serial_number";
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchWhMasterID", wh_master_id));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchOutboundPickMasterID", outbound_pick_master_id));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchOwnerID", owner_id));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchLocationID", location_id));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchParentLPN", parent_lpn));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchLPN", lpn));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchWhItemMasterID", wh_item_master_id));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchLotNumber", lot_number));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchExpiryDate", expiry_date));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_ReceiveDate", ent.receive_date));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchAttribute1", attribute1));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchAttribute2", attribute2));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchAttribute3", attribute3));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchAttribute4", attribute4));
                this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchAttribute5", attribute5));
            }
        }


        #endregion

    }
}

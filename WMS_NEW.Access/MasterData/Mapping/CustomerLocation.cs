using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Xml.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData.Mapping
{
    public class CustomerLocation : AGridObjectSourceStore
    {
        protected WMSEntities _Model { get; set; }

        public CustomerLocation()
        {
            _Model = new WMSEntities();
            _Model.Database.CommandTimeout = 600;
            _Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

            base.GridObjContext = _Model;
        }

        #region Inherit AGridObjectSourceQuery

        public override void InitialStoreView()
        {
            Guid customer_id = Guid.Empty; ;


            var ent = this.FilterCustom.Where(w => w.DataFieldValue == "_customer_id").FirstOrDefault();
            if (ent != null && ent.Value != null && Guid.Parse(ent.Value.ToString()) != Guid.Empty)
            {
                customer_id = Guid.Parse(ent.Value.ToString());
            }

            Guid wh_master_id = Guid.Empty;
            var entWh = this.FilterCustom.Where(w => w.DataFieldValue == "_wh_master_id").FirstOrDefault();
            if (entWh != null && entWh.Value != null)
            {
                wh_master_id = Guid.Parse(entWh.Value.ToString());
            }

            this.StoreNameQuery = "usp_mapping_customer_location";
            this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@customer_id", customer_id));
            this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@wh_master_id", wh_master_id));

        }

        public override void InitialStoreExport()
        {
            Guid customer_id = Guid.Empty;
            var ent = this.FilterCustom.Where(w => w.DataFieldValue == "_customer_id").FirstOrDefault();
            if (ent != null)
            {
                customer_id = Guid.Parse(ent.Value.ToString());
            }

            Guid wh_master_id = Guid.Empty;
            var entWh = this.FilterCustom.Where(w => w.DataFieldValue == "_wh_master_id").FirstOrDefault();
            if (entWh != null && entWh.Value != null)
            {
                wh_master_id = Guid.Parse(entWh.Value.ToString());
            }


            this.StoreNameExport = "usp_mapping_customer_location";
            this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@customer_id", customer_id));
            this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@wh_master_id", wh_master_id));
        }


        #endregion

        #region Function

        public bool SaveMapping(List<ConfigGlobal.DTO._Global.KeySelect> _listKey, Guid _customer_id, string _create_by)
        {
            try
            {
                if (_listKey.Count <= 0)
                {
                    return false;
                }
                //กรองเพื่อให้ where น้อยลง
                var entKey = _listKey[0];
                //Guid location_id = Guid.Parse(entKey.KeyId.ToString().Split('_').First());
                string customer_id = _customer_id.ToString().ToUpper();

                var _queryHasInDB = (from rows in this._Model.t_wms_customer_location
                                     where rows.customer_id == _customer_id
                                     select rows.t_wms_location.location_id.ToString().ToUpper() + "_" + rows.t_wms_customer.customer_id.ToString().ToUpper()).ToList();

                //-----------------------------
                //------- SELECT -------------
                var listSelect = _listKey.Where(w => w.Active == true).Select(s => s.KeyId.ToString().ToUpper() + "_" + customer_id).ToList();
                var listExcept = listSelect.Except(_queryHasInDB).ToList();

                string xSelect = null;

                if (listExcept.Count() > 0)
                {
                    var xmlSelect = new XElement("Select",
                                    from i in listExcept
                                    select new XElement("key_id",
                                        new XElement("location_id", i.Split('_').First()),
                                        new XElement("customer_id", i.Split('_').Last())
                                        ));

                    xSelect = xmlSelect.ToString();
                }

                //----------------------------------------
                //--------- UN SELECT -------------------

                var listUnselect = _listKey.Where(w => w.Active == false).Select(s => s.KeyId.ToString().ToUpper() + "_" + customer_id).ToList();

                string xUnselect = null;
                if (listUnselect.Count > 0)
                {
                    var resultDel = (from rows in listUnselect
                                     where _queryHasInDB.Contains(rows)
                                     select rows).ToList();

                    var xmlUnselect = new XElement("UnSelect",
                                      from i in resultDel
                                      select new XElement("key_id",
                                      new XElement("customer_key_id", i))
                                      );

                    xUnselect = xmlUnselect.ToString();
                }

                var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));


                this._Model.usp_mapping_customer_location_save(_SessionVals.AppID, _SessionVals.DeviceID, _SessionVals.UserName, xSelect, xUnselect, errCode, errMsg);

                if (errCode.Value.ToString() == "0")
                {
                    this.MessageSuccess(this, errMsg.Value.ToString());
                    return true;
                }
                else
                {
                    this.MessageWarning(this, errMsg.Value.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

    }

}

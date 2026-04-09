using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.Entity;
using System.Data.Entity.SqlServer;

using Prototype.Providers;
using WMS_NEW.Source;
using System.Data.Entity.Core.Objects;

namespace WMS_NEW.Access.Transaction.Count
{

    public class CountPlanExt : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public CountPlanExt()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;
            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static CountPlanExt Instance
        {
            get
            {
                using (CountPlanExt _Instance = new CountPlanExt())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Access Command Data

        public bool SaveByCycle(string _wh_id, string _count_id, string _count_type, string _desc, Guid _owner_id, Guid _customer_id)
        {

            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_create_count_plan_external(_SessionVals.AppID, _wh_id, _count_id, _count_type, _desc, _customer_id, DateTime.Now, _SessionVals.UserName, _owner_id, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());

            return !inverse;
        }

        public bool SaveByPhysical(string _wh_id, string _count_id, string _count_type, string _desc, Guid _owner_id, Guid _customer_id)
        {

            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_create_count_plan_by_wh_external(_SessionVals.AppID, _wh_id, _count_id, _count_type, _desc, _customer_id, DateTime.Now, _SessionVals.UserName, _owner_id, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());

            return !inverse;
        }

        public void ClosePlan(string _wh_id, string _count_id, string _close_remark, out string _errMsg, out string _errCode)
        {

            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_close_count_plan_external(
                _SessionVals.AppID
                , _wh_id
                , _count_id
                , DateTime.Now
                , _SessionVals.UserName
                , _close_remark
                , _SessionVals.DeviceID
                , _SessionVals.UserName
                , errCode
                , errMsg);

            _errCode = errCode.Value.ToString();
            _errMsg = errMsg.Value.ToString();
        }

        public bool Update(t_wms_count_plan_master_external ent)
        {
            return this._Model.Update(this, delegate ()
            {
                return this._Model.SaveChanges();
            });
        }

        public t_wms_count_plan_master_external GetByKeyID(Guid _id)
        {
            return this._Model.GetDataEntityBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_count_plan_master_external
                              where rows.count_master_id == _id
                              select rows).FirstOrDefault();

                return result;
            });
        }

        public CountMasterDto GetDTOByKeyID(Guid _count_master_id)
        {
            return this._Model.GetDataEntityBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_count_plan_master_external
                              where rows.count_master_id == _count_master_id
                              select new CountMasterDto
                              {
                                  count_master_id = rows.count_master_id,
                                  wh_id = rows.t_wms_wh.wh_id,
                                  count_id = rows.count_id,
                                  count_status = rows.count_status,
                                  count_type = rows.count_plan_type,
                                  description = rows.description,
                                  close_by = rows.close_by,
                                  close_date = rows.close_date,
                                  close_remark = rows.close_remark,
                                  create_by = rows.create_by,
                                  create_date = rows.create_date

                              }).FirstOrDefault();

                return result;
            });
        }

        public CountTotalSummaryDto GetTotalSummary(Guid _count_master_id)
        {
            var result = from rows in this._Model.v_wms_count_reconcile_merge_external
                         where rows.count_master_id == _count_master_id
                         group rows by rows.count_master_id into g
                         select new CountTotalSummaryDto
                         {
                             sum_stock = g.Sum(su => su.stock_qty),
                             sum_count = g.Sum(su => su.count_qty),
                             sum_diff = g.Sum(su => su.diff_qty),
                         };

            return result.FirstOrDefault();
        }

        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _userID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID").Value;

            var result = from rows in this._Model.t_wms_count_plan_master_external
                         where rows.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id == _userID)
                         select new
                         {
                             KeyId = rows.count_master_id,
                             rows.t_wms_wh.wh_id,
                             rows.count_id,
                             rows.count_status,
                             rows.count_plan_type,
                             rows.description,
                             rows.create_by,
                             rows.create_date,
                             rows.close_by,
                             rows.close_date,
                             rows.close_remark,
                             rows.count_generate_method
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

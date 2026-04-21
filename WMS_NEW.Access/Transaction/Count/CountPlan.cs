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
    public class CountMasterDto
    {
        public Guid count_master_id { get; set; }
        public string count_id { get; set; }
        public string wh_id { get; set; }
        public string count_status { get; set; }
        public string count_type { get; set; }
        public string description { get; set; }
        public string close_remark { get; set; }
        public string close_by { get; set; }
        public DateTime? close_date { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
    }
    public class CountTotalSummaryDto
    {
        public double sum_stock { get; set; }
        public double sum_count { get; set; }
        public double sum_diff { get; set; }
        public int sum_plan_pallet { get; set; }
        public int sum_pallet_count { get; set; }
        public int sum_diff_plus { get; set; }
        public int sum_diff_minus { get; set; }
        public int count_diff_zero { get; set; }

        public double count_miss { get; set; }
        public double count_used { get; set; }
        public double count_unknown { get; set; }


    }

    public class CountPlan : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public CountPlan()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;
            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static CountPlan Instance
        {
            get
            {
                using (CountPlan _Instance = new CountPlan())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Access Command Data

        public bool SaveByCycle(Guid _wh_master_id, string _count_id, string _count_type, string _desc, Guid _owner_id)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_create_count_plan(_SessionVals.AppID, _wh_master_id, _owner_id, _count_id, _count_type, _desc, _SessionVals.UserName, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());

            return !inverse;
        }

        public bool SaveByPhysical(Guid _wh_master_id, string _count_id, string _count_type, string _desc, Guid _owner_id)
        {

            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_create_count_plan_by_wh(_SessionVals.AppID, _wh_master_id, _owner_id, _count_id, _count_type, _desc, _SessionVals.DeviceID, _SessionVals.UserName, errCode, errMsg);

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

            this._Model.usp_close_count_plan(
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

        public bool Update(t_wms_count_plan_master ent)
        {
            return this._Model.Update(this, delegate ()
            {
                return this._Model.SaveChanges();
            });
        }

        public t_wms_count_plan_master GetByKeyID(Guid _id)
        {
            return this._Model.GetDataEntityBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_count_plan_master
                              where rows.count_master_id == _id
                              select rows).FirstOrDefault();

                return result;
            });
        }

        public CountMasterDto GetDTOByKeyID(Guid _count_master_id)
        {
            return this._Model.GetDataEntityBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_count_plan_master
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
            var result = from rows in this._Model.v_wms_count_reconcile_merge_summary
                         where rows.count_master_id == _count_master_id
                         select new CountTotalSummaryDto
                         {
                             sum_stock = rows.total_stock_qty ?? 0,
                             sum_count = rows.total_count_qty ?? 0,
                             sum_diff = rows.total_diff_qty ?? 0,
                         };

            return result.FirstOrDefault();
        }

        public CountTotalSummaryDto GetTotalSummaryPallet(Guid _count_master_id)
        {
            var result = from rows in this._Model.v_wms_count_reconcile_pallet_summary
                         where rows.count_master_id == _count_master_id
                         select new CountTotalSummaryDto
                         {
                             sum_plan_pallet = rows.sum_plan_pallet ?? 0,
                             sum_pallet_count = rows.sum_pallet_count ?? 0,
                             sum_diff_plus = rows.sum_diff_plus ?? 0,
                             sum_diff_minus = rows.sum_diff_minus ?? 0,
                             count_diff_zero = rows.count_diff_zero ?? 0,
                         };

            return result.FirstOrDefault();
        }

        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _userID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID").Value;

            var result = from rows in this._Model.t_wms_count_plan_master
                         where rows.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id == _userID)
                         && rows.t_wms_owner.t_wms_owner_user.Any(a => a.user_id == _userID && a.is_active == "YES")
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


        public bool ExistsCountId(string _count_id)
        {
            return this._Model.Existed(this, delegate ()
            {
                var has_id = this._Model.t_wms_count_plan_master.Any(x => x.count_id == _count_id);

                return has_id;
            }, "! Count Id already in table");
        }

    }
}

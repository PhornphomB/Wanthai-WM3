using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WMS_NEW.Access.Helper;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Viewer
{
    public class InboundDetail : AGridObjectSourceStore
    {
        public WMSEntities _Model { get; set; }

        public InboundDetail()
        {
            _Model = new WMSEntities();
            _Model.Database.CommandTimeout = 600;
            _Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static InboundDetail Instance
        {
            get
            {
                using (InboundDetail _Instance = new InboundDetail())
                {
                    return _Instance;
                }
            }
        }
        public override void InitialStoreView()
        {
            string _userID = _SessionVals.UserName;

            this.StoreNameQuery = "usp_export_inbound_detail_receipt";
            this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_vchUserID", _userID));

        }

        public override void InitialStoreExport()
        {
            string _userID = _SessionVals.UserName;

            this.StoreNameExport = "usp_export_inbound_detail_receipt";
            this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_vchUserID", _userID));
        }
        private string GridCondition(FilterList detail)
        {
            var whereCondition = string.Empty;
            SQLDynamicSearch iSQLDynamicSearch = new SQLDynamicSearch();
            foreach (var fileters in detail)
            {
                var filter = new FilterModel
                {
                    DataPropertyName = fileters.DataPropertyName.Replace("[", "").Replace("]", ""),
                    FilterAt = fileters.FilterAt,
                    Value = fileters.Value?.ToString(),
                    ValueTo = fileters.ValueTo?.ToString()
                };
                whereCondition += iSQLDynamicSearch.GenerateFilter(filter);
            }
            return whereCondition;
        }
        #endregion

        public decimal GetSummary(FilterList detail)
        {
            string _userID = _SessionVals.UserName;
            var whereCondition = string.Empty;
            if (detail != null && detail.Count > 0)
            {
                whereCondition = this.GridCondition(detail);
            }
            using (var context = new WMSEntities()) // ใช้คอนเท็กซ์ของเอนทิตี
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                context.Database.CommandTimeout = 180;

                var result = context.Database.SqlQuery<decimal>(
                    "EXEC usp_export_inbound_detail_receipt_summary @in_vchUserID,@in_WhereCondition ",
                    new SqlParameter("@in_vchUserID", _userID),
                    new SqlParameter("@in_WhereCondition", whereCondition)
                ).FirstOrDefault();

                return result;
            }
        }


    }
}

using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class ProductionLine : AEntityFormCommand<t_wms_production_line>
    {
        #region ++INSTANCE STATIC++
        public static ProductionLine Instance
        {
            get
            {
                using (ProductionLine _Instance = new ProductionLine())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        protected WMSEntities _Model { get; set; }

        public ProductionLine()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_production_line; };
        }

        public override bool ValidateSaveNew(t_wms_production_line ent, ref string msg_validate)
        {
            if (_Model.t_wms_production_line.Any(x => x.wh_master_id == ent.wh_master_id && x.production_line == ent.production_line))
            {
                msg_validate = "! Warehouse and Production line has in system.";
                return false;
            }
            else if (_Model.t_wms_production_line.Any(x => x.production_line == ent.production_line))
            {
                msg_validate = "! Production line has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            Guid wh_master_id = Guid.Empty;
            if (this.FilterCustom != null)
            {
                var entWhMasterId = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_master_id");
                if (entWhMasterId != null && entWhMasterId.Value != null)
                {
                    wh_master_id = Guid.Parse(entWhMasterId.Value.ToString());
                }
            }
            var result = from rows in this._Model.t_wms_production_line
                         where rows.is_active == "YES"
                         select new
                         {
                             KeyId = rows.production_line_id,
                             rows.wh_master_id,
                             wh_id = _Model.t_wms_wh.FirstOrDefault(x => x.wh_master_id == rows.wh_master_id).wh_id,
                             wh_name = _Model.t_wms_wh.FirstOrDefault(x => x.wh_master_id == rows.wh_master_id).wh_name,
                             rows.production_line,
                             rows.erp_production_line,
                             rows.description,
                             rows.is_active,
                             rows.create_by,
                             rows.create_date,
                         };

            

            if (wh_master_id != Guid.Empty)
            {
                result = result.Where(x => x.wh_master_id == wh_master_id);
            }

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        #region Query Property

        public IQueryable<Property> GetQuery()
        {
            var result = from rows in _Model.t_wms_production_line
                         where rows.is_active == "YES"
                         orderby rows.production_line ascending
                         select new Property
                         {
                             value_member = rows.production_line,
                             display_member = rows.production_line/* + " : " + rows.description*/
                         };

            return result;
        }

        public IQueryable<Property> GetQuery(Guid _wh_master_id)
        {
            var result = from rows in _Model.t_wms_production_line
                         where rows.is_active == "YES"
                         && rows.wh_master_id == _wh_master_id
                         orderby rows.production_line ascending
                         select new Property
                         {
                             value_member = rows.production_line,
                             display_member = rows.production_line/* + " : " + rows.description*/
                         };
            return result;
        }
        #endregion
    }
}

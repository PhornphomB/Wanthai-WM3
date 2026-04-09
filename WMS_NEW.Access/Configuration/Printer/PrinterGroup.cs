using Prototype.Providers;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Configuration.Printer
{
    public class PrinterGroup : AEntityFormCommand<t_com_config_printer_group>
    {
        #region ++INSTANCE STATIC++
        public static PrinterGroup Instance
        {
            get
            {
                using (PrinterGroup _Instance = new PrinterGroup())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        protected WMSEntities _Model { get; set; }

        public PrinterGroup()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_com_config_printer_group; };
        }

        public override bool ValidateSaveNew(t_com_config_printer_group ent, ref string msg_validate)
        {
            if (_Model.t_com_config_printer_group.Any(x => x.group_name == ent.group_name))
            {
                msg_validate = "! Group Print has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_com_config_printer_group
                         select new
                         {
                             rows.group_id,
                             rows.t_wms_wh.wh_id,
                             rows.wh_master_id,
                             rows.group_name,
                             rows.group_desc,
                             rows.group_type,
                             //rows.wh_master_id,
                             rows.location_id,
                             rows.is_active,
                             rows.create_date,
                             rows.create_by,

                         };


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
            var result = from rows in _Model.t_com_config_printer_group
                         where rows.is_active == "YES"
                         orderby rows.group_name ascending
                         select new Property
                         {
                             guid_member = rows.group_id,
                             display_member = rows.group_name
                         };

            return result;
        }

        #endregion

    }

}

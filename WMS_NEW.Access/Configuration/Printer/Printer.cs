using Prototype.Providers;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Configuration.Printer
{
    public class Printer : AEntityFormCommand<t_com_config_printer>
    {
        #region ++INSTANCE STATIC++
        public static Printer Instance
        {
            get
            {
                using (Printer _Instance = new Printer())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        protected WMSEntities _Model { get; set; }

        public Printer()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_com_config_printer; };
        }

        public override bool ValidateSaveNew(t_com_config_printer ent, ref string msg_validate)
        {
            if (_Model.t_com_config_printer.Any(x => x.printer_name == ent.printer_name))
            {
                msg_validate = "! Print Name has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_com_config_printer
                         select new
                         {
                             rows.printer_id,
                             rows.printer_name,
                             rows.printer_desc,
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
            var result = from rows in _Model.t_com_config_printer
                         where rows.is_active == "YES"
                         orderby rows.printer_name ascending
                         select new Property
                         {
                             guid_member = rows.printer_id,
                             display_member = rows.printer_name
                         };

            return result;
        }

        #endregion

    }

}

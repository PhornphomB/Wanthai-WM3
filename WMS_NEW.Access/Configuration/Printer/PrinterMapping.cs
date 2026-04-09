using Prototype.Providers;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Configuration.Printer
{
    public class PrinterMapping : AEntityFormCommand<t_com_config_printer_group_mapping>
    {
        #region ++INSTANCE STATIC++
        public static PrinterMapping Instance
        {
            get
            {
                using (PrinterMapping _Instance = new PrinterMapping())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        protected WMSEntities _Model { get; set; }

        public PrinterMapping()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_com_config_printer_group_mapping; };
        }

        public override bool ValidateSaveNew(t_com_config_printer_group_mapping ent, ref string msg_validate)
        {
            if (_Model.t_com_config_printer_group_mapping.Any(x => x.group_id == ent.group_id && x.printer_id == ent.printer_id))
            {
                msg_validate = "! Group Printer and printer has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_com_config_printer_group_mapping
                         select new
                         {
                             rows.group_printer_id,
                             rows.group_id,
                             rows.printer_id,
                             rows.sequence,
                             rows.bartender_exe_filepath,
                             rows.bartender_btw_filepath,
                             rows.bartender_argument_command,
                             rows.bartender_data_filepath,
                             rows.bartender_trigger_filepath,
                             rows.is_active,
                             rows.create_date,
                             rows.create_by,

                             rows.t_com_config_printer.printer_name,
                             rows.t_com_config_printer_group.group_name

                         };


            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        #region Query Property
        #endregion

    }

}

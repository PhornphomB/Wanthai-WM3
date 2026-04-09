using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using WMS_NEW.Source;
using Prototype.Providers;

namespace WMS_NEW.Access
{
    public class PrimatyFieldItem
    {
        public Guid item_master_id { get; set; }
        public string item_number { get; set; }
    }

    public class DataPrimaryField : IDisposable
    {

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion


        #region ++INSTANCE STATIC++
        public static DataPrimaryField Instance
        {
            get
            {
                using (DataPrimaryField _Instance = new DataPrimaryField())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public PrimatyFieldItem GetMasterItem(Guid _wh_item_master_id)
        {
            using (var _model = new WMSEntities())
            {
                var result = (from rows in _model.t_wms_item
                              where _model.t_wms_wh_item.Any(x => x.item_master_id == rows.item_master_id && x.wh_item_master_id == _wh_item_master_id)
                              select new PrimatyFieldItem
                              {
                                  item_master_id = rows.item_master_id,
                                  item_number = rows.item_number
                              }).First();

                return result;
            }
        }

       

    }
}

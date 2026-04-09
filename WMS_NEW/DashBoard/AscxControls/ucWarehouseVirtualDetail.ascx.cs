using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.DashBoard.AscxControls
{
    public partial class ucWarehouseVirtualDetail : UControlCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void InitForm(Guid _location_id)
        {
            try
            {
                using (var acc = new Access.DashBoard.WarehouseVirtual())
                {
                    txtLocation.Clear();
                    txtCapacityQty.Clear();
                    txtCurrentQty.Clear();
                    txtUsageQty.Clear();
                    var ent = acc._Model.v_wms_dashboard_warehouse_virtual.Where(w => w.location_id == _location_id).FirstOrDefault();
                    if (ent != null)
                    {
                        txtLocation.SetValue(ent.location);
                        txtCurrentQty.SetValue(ent.current_qty);
                        txtCapacityQty.SetValue(ent.capacity_qty);
                        txtUsageQty.SetValue(ent.usage_qty);
                    }

                    txtLocation.Update();
                    txtCapacityQty.Update();
                    txtCurrentQty.Update();
                    txtUsageQty.Update();
                }
                hidLocationId.SetValue(_location_id);
                GridExt1.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}
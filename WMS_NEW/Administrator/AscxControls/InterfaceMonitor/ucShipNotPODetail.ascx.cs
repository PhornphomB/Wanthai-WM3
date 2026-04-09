using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Administrator.AscxControls.InterfaceMonitor
{
    public partial class ucShipNotPODetail : UControlCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindParameterPage(string key_id)
        {
            SetControl(key_id);
            SerachData(key_id);
        }

        void SetControl(string key_id)
        {
            using (var acc = new Access.Administrator.InterfaceMonitor.ShipNotPO())
            {
                PlugEventResult(acc);

                var ent = acc.GetByKeyID(key_id);
                if (ent != null)
                {
                    hid_wmsorn.SetValue(ent.WMSORN);
                }
            }
        }

        void SerachData(string key_id)
        {
            gridDetail.Search();
        }

    }
}
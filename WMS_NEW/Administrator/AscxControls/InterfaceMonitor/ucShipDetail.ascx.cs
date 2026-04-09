using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Administrator.AscxControls.InterfaceMonitor
{
    public partial class ucShipDetail : UControlCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindParameterPage(string host_record_id)
        {
            SetControl(host_record_id);
            SerachData(host_record_id);
        }

        void SetControl(string host_record_id)
        {
            using (var acc = new Access.Administrator.InterfaceMonitor.Ship())
            {
                PlugEventResult(acc);

                var ent = acc.GetByKeyID(host_record_id);
                if (ent != null)
                {
                    //txtRecordType.SetValue(ent.record_type);
                    txtProcessingCode.SetValue(ent.processing_code);
                    txtProcessingStatus.SetValue(ent.processing_status);
                    txtWarehouse.SetValue(ent.wh_id);
                    txtOwner.SetValue(ent.owner_id);
                    //txtOrderType.SetValue(ent.type);
                    txtOrderNumber.SetValue(ent.order_number);
                    //txtExpectDate.SetValue(ent.expected_delivery_date);
                    txtOrderStatus.SetValue(ent.order_status);
                    //txtSupplier.SetValue(ent.supplier_code);
                    //txtCustomer.SetValue(ent.customer_code);
                    txtCreateBy.SetValue(ent.create_by);
                    txtCreateDate.SetValue(ent.create_date);
                }
            }
        }

        void SerachData(string host_record_id)
        {
            hidKeyID.SetValue(host_record_id);
            gridDetail.Search();

        }

    }
}
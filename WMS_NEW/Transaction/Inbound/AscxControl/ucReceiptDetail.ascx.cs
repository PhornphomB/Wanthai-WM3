using _UControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Inbound.AscxControl
{
    public partial class ucReceiptDetail : UControlCustom, IFormRelation
    {
        public Action<dynamic> UpdateParent { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binding Event
                #endregion

                #region Initial Peoperty Column Grid
                #endregion

                #region Initial Input Data
                #endregion


                if (!Page.IsPostBack)
                {

                    #region Initial PopupEntity
                    //กำหนด PanelID หรือ ControlID ที่เป็นตัวคลุม Group InputData สำหรับการทำงานในหน้านี้
                    #endregion

                    #region Initial Panel Tap
                    #endregion

                    #region BindDataSource DropDrown
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void BindParameterPage(string _inbound_order_master_id)
        {
            try
            {
                hdf_inbound_order_master_id.SetValue(_inbound_order_master_id);
                GridExt1.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                GridExt1.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void InitForm(dynamic _obj)
        {
            try
            {
                var ent = (Source.t_wms_inbound_master)_obj;
                hdf_inbound_order_master_id.SetValue(ent.inbound_order_master_id);
                GridExt1.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void Search(object _inbound_order_master_id)
        {
            Guid inbound_order_master_id = (Guid)_inbound_order_master_id;
            hdf_inbound_order_master_id.SetValue(inbound_order_master_id);
            GridExt1.Search();
        }


    }
}
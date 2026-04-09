using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using _UControls;

namespace WMS_NEW.Transaction.Outbound.AscxControls
{
    public partial class OutboundComment : UControlCustom, IFormRelation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    #region Initial Input Data

                    ddlCommentType.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("outbound_order_comment_type"); };
                    ddlPosition.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("outbound_order_comment_position"); };

                    #endregion
                }

                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = new Access.Transaction.Outbound.OutboundComment(); };
                popupEntity1.InitControlStatic();

                GridExt1.PopupEntitySource = popupEntity1;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public Action<dynamic> UpdateParent { get; set; }

        public void InitForm(dynamic _obj)
        {
            try
            {
                var ent = (Source.t_wms_outbound_master)_obj;

                hidMasterId.SetValue(ent.outbound_order_master_id);
                grid_outbound_order_master_id.SetValue(ent.outbound_order_master_id);

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
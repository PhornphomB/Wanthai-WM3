using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using _UControls;

namespace WMS_NEW.MasterData.AscxControls
{
    public partial class ItemCrossRef : UControlCustom, IFormRelation
    {
        private Guid item_master_id
        {
            get
            {
                if (ViewState["item_master_id"] == null)
                    ViewState["item_master_id"] = Guid.Empty;

                return (Guid)ViewState["item_master_id"];
            }
            set
            {
                ViewState["item_master_id"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ddlUOM.MethodQueryProperty = delegate () { return new Access.MasterData.ItemUom().GetQuery_Item(item_master_id); };

                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = new Access.MasterData.ItemCrossRef(); };
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
                var ent = (Source.t_wms_item)_obj;

                item_master_id = ent.item_master_id;
                ddlUOM.BindDataSource();

                txtItemNumber.SetValue(ent.item_number);
                hidItemMasterId.SetValue(ent.item_master_id);

                gridItemMasterId.SetValue(ent.item_master_id);
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
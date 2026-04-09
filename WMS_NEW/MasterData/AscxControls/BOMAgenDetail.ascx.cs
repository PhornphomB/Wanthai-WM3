using _UControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.MasterData.AscxControls
{
    public partial class BOMAgenDetail : UControlCustom, IFormRelation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binding Event  
                popup.AfterNewDataEvent += Popup_AfterNewDataEvent;
                popup.AfterSetEditDataEvent += PopupCategory_AfterSetEditDataEvent;
                popup.ValidateEntityEvent += popupCategory_ValidateEntityEvent;
                popup.PreSaveEntityEvent += popupCategory_PreSaveEntityEvent;
                popup.RaiseEntitySaved += popupCategory_RaiseEntitySaved;

                ddItem.PostValueChanged += DdItem_PostValueChanged;

                #endregion

                #region Initial Peoperty Column Grid

                #endregion

                #region Initial Input Data

                ddItem.MethodQueryProperty = delegate () { return WMS_NEW.Access.MasterData.Item.Instance.GetQueryPropertyByItemBom(WarehouseMsID, OwnerID, IsBom); };
               
                #endregion


                if (!Page.IsPostBack)
                {
                    WarehouseMsID = Guid.Empty; 
                    OwnerID = Guid.Empty;
                    IsBom = string.Empty;


                    #region Initial PopupEntity

                    //กำหนด PanelID หรือ ControlID ที่เป็นตัวคลุม Group InputData สำหรับการทำงานในหน้านี้
            
                
                    GridExt1.PopupEntitySource = popup;
                    #endregion

                    #region Initial Panel Tap

                    #endregion

                    #region BindDataSource DropDrown
                  
                    #endregion
                }

                popup.InitObjectsEvent += () => { popup.ObjectDataAccess = new Access.MasterData.BomDetail(); };
                popup.InitControlStatic();
              
                GridExt1.PopupEntitySource = popup;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void Popup_AfterNewDataEvent()
        {
            try
            {
                //var line_no = Access.MasterData.BomDetail.Instance.GetLineNumber(_bom_id);
                //txtLineNO.SetValue(line_no);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void DdItem_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value != null)
                {
                    var desc = WMS_NEW.Access.MasterData.Item.Instance.GetDescriptionByWHItem(_value);
                    txtDesc.SetValue(desc);
                }
                else
                    txtDesc.SetValue(string.Empty);

                txtDesc.Update();
                ddUom.MethodQueryProperty = delegate () { return WMS_NEW.Access.MasterData.ItemUom.Instance.GetQuery_WhItem(_value); };
                ddUom.BindDataSource();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        public Guid WarehouseMsID
        {
            get
            {
                return (Guid)ViewState["WarehouseMsID"];
            }
            set
            {
                ViewState["WarehouseMsID"] = value;
            }
        }
        public Guid OwnerID
        {
            get
            {
                return (Guid)ViewState["OwnerID"];
            }
            set
            {
                ViewState["OwnerID"] = value;
            }
        }
        public string IsBom
        {
            get
            {
                return (string)ViewState["IsBom"];
            }
            set
            {
                ViewState["IsBom"] = value;
            }
        }

        public Guid WarehouseItemMsID
        {
            get
            {
                return (Guid)ViewState["WarehouseItemMsID"];
            }
            set
            {
                ViewState["WarehouseItemMsID"] = value;
            }
        }

        public Action<dynamic> UpdateParent { get; set; }


        #region Properties for Parent Page

        public void BindParameterPage(int _bom_id, string _bom_code, Guid _owner_id, Guid _wh_master_id, string _is_bom)
        {
            try
            {
                var line_no = Access.MasterData.BomDetail.Instance.GetLineNumber(_bom_id);
                txtLineNO.SetValue(line_no);

                txtBomCode.SetValue(_bom_code);
                hidMasterId.SetValue(_bom_id);

                WarehouseMsID = _wh_master_id;
                OwnerID = _owner_id;
                IsBom = _is_bom;

                ddItem.MethodQueryProperty = delegate () { return WMS_NEW.Access.MasterData.Item.Instance.GetQueryPropertyByItemBom(WarehouseMsID, OwnerID, IsBom); };
                ddItem.BindDataSource();

                hidSearchMasterId.SetValue(_bom_id);
                GridExt1.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #endregion

        #region Popup Entity Event
     
        private void PopupCategory_AfterSetEditDataEvent()
        {
            try
            {
                var _objectEntity = (WMS_NEW.Source.t_wms_item_bom_detail)popup.ObjectDataAccess.Entity;

                DdItem_PostValueChanged(_objectEntity.wh_item_master_id);
                ddUom.SetValue(_objectEntity.item_uom_id);
                txtLineNO.SetValue(_objectEntity.line_no);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        bool popupCategory_ValidateEntityEvent()
        {
            //กำหนดเงื่อนไข Validate ที่เป็น Business Logic ให้ InputData บ่างส่วน ถ้าไม่มีให้ return true
            return true;
        }

        void popupCategory_PreSaveEntityEvent()
        {
            //กำหนดค่าให้ Property บางส่วนที่ไม่ได้อยู่ในส่วนของ InputData ก่อนที่จะบันทึก

            var _objectEntity = (WMS_NEW.Source.t_wms_item_bom_detail)popup.ObjectDataAccess.Entity;

            _objectEntity.uom = ddUom.GetText();
            _objectEntity.item_number = ddItem.GetText();
        }

        void popupCategory_RaiseEntitySaved(bool _saveStatus)
        {
            //รีเทิร์นค่า ที่บอกว่าบันทึกสำเร็จหรือไม่สำเร็จ เพื่อที่เราจะทำ Process อะไรต่อไปในหน้านี้ได้
            if (_saveStatus)
            {
                GridExt1.DataBind();

                popup.New();
            }
        }

        #endregion

        protected void btNew_Click(object sender, EventArgs e)
        {
            popup.New();
        }

        public void InitForm(dynamic _obj)
        {
            try
            {
                var ent = (WMS_NEW.Source.t_wms_item_bom)_obj;         
                hidMasterId.SetValue(ent.bom_id);           
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.MasterData
{
    public partial class Bom : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {          
                #region Binging DropDown Property Column Grid
                GridColumnExt3.DropDownQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.ActiveType().AsQueryable(); };
                #endregion

                #region Binging DropDown Property Input Data
                ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(); };
                ddAgency.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery_User(); };
                ddBom.MethodQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQueryPropertyByItemBom(ddlWarehouse.GetValue(), ddAgency.GetValue()); };

                ddAgency.PostValueChanged += DdAgency_PostValueChanged;
                ddBom.PostValueChanged += DdBom_PostValueChanged;
                chkBomType.PostValueChanged += ChkBomType_PostValueChanged;

                popupEntity1.AfterNewDataEvent += PopupEntity1_AfterNewDataEvent;
                popupEntity1.AfterSetEditDataEvent += PopupEntity1_AfterSetEditDataEvent;
                popupEntity1.PreSaveEntityEvent += PopupEntity1_PreSaveEntityEvent;            
                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = new Access.MasterData.BomByItem(); };
                popupEntity1.InitControlStatic();
              
                GridExt1.PopupEntitySource = popupEntity1;

                if (!Page.IsPostBack)
                {
                    ddlWarehouse.BindDataSource();
                    ddAgency.BindDataSource();                 
                }


                #endregion
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        //private void GridExt1_GridRowEdit(object _rowKeyValue)
        //{
        //    popupEntity1.Edit(_rowKeyValue);
        //}

        private void PopupEntity1_PreSaveEntityEvent()
        {
            //กำหนดค่าให้ Property บางส่วนที่ไม่ได้อยู่ในส่วนของ InputData ก่อนที่จะบันทึก
            var _objectEntity = (Source.t_wms_item_bom)popupEntity1.ObjectDataAccess.Entity;

            _objectEntity.wh_id = ddlWarehouse.GetText().Split(':')[0];
            _objectEntity.wh_master_id = ddlWarehouse.GetValue();
            _objectEntity.owner_code = ddAgency.GetText().Split(':')[0];
            _objectEntity.owner_id = ddAgency.GetValue();
            _objectEntity.item_number = ddBom.GetText();
            _objectEntity.is_bom_item = chkBomType.GetValue();
            _objectEntity.is_active =  chkActive.GetValue();
            _objectEntity.wh_item_master_id = ddBom.GetValue();

            if (_objectEntity.is_bom_item == "YES")
            {
                _objectEntity.uom = ddUom.GetText();
                _objectEntity.item_uom_id = ddUom.GetValue();
            }
            else
            {             
                _objectEntity.uom = null;
            }

        }

        private void PopupEntity1_AfterSetEditDataEvent()
        {
            try
            {
                var _objectEntity = (Source.t_wms_item_bom)popupEntity1.ObjectDataAccess.Entity;
            
                ChkBomType_PostValueChanged(chkBomType.GetValue());
              
                DdAgency_PostValueChanged(_objectEntity.owner_id);
                ddBom.SetValue(_objectEntity.wh_item_master_id);

                DdBom_PostValueChanged(_objectEntity.wh_item_master_id);
                ddUom.SetValue(_objectEntity.item_uom_id);

                BOMAgenDetail.BindParameterPage(_objectEntity.bom_id, _objectEntity.is_bom_item == "YES" ? _objectEntity.item_number : _objectEntity.bom_raw_code
                    , _objectEntity.owner_id, _objectEntity.wh_master_id, "NO");
                BOMAgenDetail.Visible = true;
            
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopupEntity1_AfterNewDataEvent()
        {
            try
            {
                chkActive.Checked = true;
                chkBomType.Checked = true;
                ChkBomType_PostValueChanged(chkBomType.GetValue());

                BOMAgenDetail.Visible = false;            
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void ChkBomType_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == "YES")
                {
                    ddBom.IsPrimary = ddUom.IsPrimary = true;
                    txtBomRawCode.IsPrimary = txtBomRawUOM.IsPrimary = false;

                    ddBom.VisibleExt = ddUom.Visible = true;
                    txtBomRawCode.Visible = txtBomRawUOM.Visible = false;

                    txtDesc.Enabled = false;
                }
                else
                {
                    ddBom.IsPrimary = ddUom.IsPrimary = false;
                    txtBomRawCode.IsPrimary = txtBomRawUOM.IsPrimary = true;

                    ddBom.VisibleExt = ddUom.Visible = false;
                    txtBomRawCode.Visible = txtBomRawUOM.Visible = true;

                    txtDesc.Enabled = true;
                }

                popupEntity1.UpdateContent();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void DdAgency_PostValueChanged(dynamic _value)
        {
            try
            {
                ddBom.MethodQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQueryPropertyByItemBom(ddlWarehouse.GetValue(), _value); };
                ddBom.BindDataSource();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void DdBom_PostValueChanged(dynamic _value)
        {
            try
            {
                if (chkBomType.GetValue() == "NO")
                    return;

                if (_value != null)
                {
                    var desc = WMS_NEW.Access.MasterData.Item.Instance.GetDescriptionByWHItem(_value);
                    txtDesc.SetValue(desc);
                }
                else
                    txtDesc.SetValue(string.Empty);

                txtDesc.Update();
                ddUom.MethodQueryProperty = delegate () { return WMS_NEW.Access.MasterData.ItemUom.Instance.GetQuery_WhItem(_value); };// AccessMaster.PropertyCollection.Item.ItemUOM.Instance.GetQueryProperty_ByWarehouseItem(_value); };
                ddUom.BindDataSource();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btNew_Click(object sender, EventArgs e)
        {
            popupEntity1.New();
        }


    }
}
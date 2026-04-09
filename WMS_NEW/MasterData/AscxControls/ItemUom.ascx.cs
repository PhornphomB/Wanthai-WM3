using _UControls;
using System;
using System.Web.UI;

namespace WMS_NEW.MasterData.AscxControls
{
    public partial class ItemUom : UControlCustom, IFormRelation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                popupEntity1.AfterNewDataEvent += popupUOM_AfterNewDataEvent;
                popupEntity1.AfterSetEditDataEvent += popupUOM_AfterSetEditDataEvent;
                popupEntity1.ValidateEntityEvent += PanelPopup1_ValidateEntityEvent;
                popupEntity1.RaiseEntitySaved += PanelPopup1_RaiseEntitySaved;
                popupEntity1.PreSaveEntityEvent += PopupEntity1_PreSaveEntityEvent;


                #region Binging DropDown Property Input Data

                if (!Page.IsPostBack)
                {
                    ddPickRule.MethodQueryProperty = delegate () { return Access.MasterData.Rule.Instance.GetQueryUpperTextId("PICK_RULE"); };
                    ddPutRule.MethodQueryProperty = delegate () { return Access.MasterData.Rule.Instance.GetQueryUpperTextId("PUTAWAY_RULE"); };
                }

                #endregion


                chkPrimaryUOM.PostValueChanged = chkPrimaryUOM_PostValueChanged;
                //chkIsPalletUOM.PostValueChanged = chkIsPalletUOM_PostValueChanged;

                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = new Access.MasterData.ItemUom(); };
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


        #region Popup Entity Event

        void popupUOM_AfterNewDataEvent()
        {
            SetControlDefault();
            ControlPrimaryUOM(chkPrimaryUOM.GetValue());
            //SetPalletControlDefault();
            //ControlIsPalletUOM(chkIsPalletUOM.GetValue());
        }

        void popupUOM_AfterSetEditDataEvent()
        {
            if (chkPrimaryUOM.GetValue() == "YES") //ถ้า UOM ที่แก้ไขเป็น Primary อยู่แล้ว 
                chkPrimaryUOM.Enabled = true;
            else
                chkPrimaryUOM.Readonly = CheckHasPrimaryUOM();

            ControlPrimaryUOM(chkPrimaryUOM.GetValue());

            //if (chkIsPalletUOM.GetValue() == "YES") //ถ้า UOM ที่แก้ไขเป็น Primary อยู่แล้ว 
            //    chkIsPalletUOM.Enabled = true;
            //else
            //    chkIsPalletUOM.Readonly = CheckHasPalletUOM();

            //ControlIsPalletUOM(chkIsPalletUOM.GetValue());
        }


        bool PanelPopup1_ValidateEntityEvent()
        {

            if (popupEntity1.FormState == FormState.New)
            {
                using (var acc = new Access.MasterData.ItemUom())
                {
                    var chk = acc.CheckUOMPromt((Guid)hidItemMasterId.GetValue(), txtuom_prompt.GetValue());

                    if (chk)
                    {
                        Page.MessageWarning("uom_prompt is duplicate!");
                        return false;
                    }
                }
            }
            else if (popupEntity1.FormState == FormState.Edit)
            {
                using (var acc = new Access.MasterData.ItemUom())
                {
                    var chk_edit = acc.CheckUOMPromt_Edit((Guid)popupEntity1.KeyFieldValue, (Guid)hidItemMasterId.GetValue(), txtuom_prompt.GetValue());
                    if (chk_edit)
                    {
                        Page.MessageWarning("uom_prompt is duplicate!");
                        return false;
                    }
                }
            }
            return true;
        }

        private void PopupEntity1_PreSaveEntityEvent()
        {
            var _objectEntity = (popupEntity1.ObjectDataAccess as Access.MasterData.ItemUom).Entity;
        }


        void PanelPopup1_RaiseEntitySaved(bool _saveStatus)
        {
            if (_saveStatus)
            {
                //chkPrimaryUOM.Readonly = CheckHasPrimaryUOM();
                //chkPrimaryUOM.Update();

                GridExt1.DataBind();

                //if (popupEntity1.FormState == FormState.New)
                //{
                //    popupEntity1.New();
                //}
            }
        }

        #endregion

        void SetControlDefault()
        {
            if (CheckHasPrimaryUOM())
            {
                //chkPrimaryUOM.Readonly = true;
                chkPrimaryUOM.Enabled = false;
                chkPrimaryUOM.Checked = false;
            }
            else
            {
                //chkPrimaryUOM.Readonly = false;
                chkPrimaryUOM.Enabled = true;
                chkPrimaryUOM.Checked = true;
            }

            chkPickingUOM.Checked = true;
            chkShippingUOM.Checked = true;
        }

        void chkPrimaryUOM_PostValueChanged(dynamic _value)
        {
            try
            {
                if ((popupEntity1.FormState == FormState.New) || (_value == "YES")) return;


                using (var _acc = new Access.MasterData.ItemUom())
                {
                    base.PlugEventResult(_acc);
                    if (_acc.CheckPrimaryHasUsed((Guid)hidItemMasterId.GetValue(), (Guid)popupEntity1.KeyFieldValue))
                    {
                        chkPrimaryUOM.Checked = true;
                    }
                }

                ControlPrimaryUOM(_value);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        bool CheckHasPrimaryUOM()
        {
            using (var _acc = new Access.MasterData.ItemUom())
            {
                base.PlugEventResult(_acc);

                return _acc.CheckHasPrimary((Guid)hidItemMasterId.GetValue());
            }
        }

        void ControlPrimaryUOM(string _value)
        {
            if (_value == "YES")
            {
                txtConversion.SetValue(1);
                txtConversion.Enabled = false;
            }
            else
            {
                txtConversion.Enabled = true;
            }
            txtConversion.Update();
        }

        //void SetPalletControlDefault()
        //{
        //    if (CheckHasPalletUOM())
        //    {
        //        chkIsPalletUOM.Enabled = false;
        //        chkIsPalletUOM.Checked = false;
        //    }
        //    else
        //    {
        //        chkIsPalletUOM.Enabled = true;
        //        chkIsPalletUOM.Checked = true;
        //    }
        //}
        //void chkIsPalletUOM_PostValueChanged(dynamic _value)
        //{
        //    try
        //    {
        //        if ((popupEntity1.FormState == FormState.New) || (_value == "YES")) return;


        //        using (var _acc = new Access.MasterData.ItemUom())
        //        {
        //            base.PlugEventResult(_acc);
        //            if (_acc.CheckPalletHasUsed((Guid)hidItemMasterId.GetValue(), (Guid)popupEntity1.KeyFieldValue))
        //            {
        //                chkIsPalletUOM.Checked = true;
        //            }
        //        }

        //        ControlIsPalletUOM(_value);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging = new Prototype.Providers.Logging(this, ex);
        //        RaiseLogging();
        //    }
        //}

        //bool CheckHasPalletUOM()
        //{
        //    using (var _acc = new Access.MasterData.ItemUom())
        //    {
        //        base.PlugEventResult(_acc);

        //        return _acc.CheckHasPallet((Guid)hidItemMasterId.GetValue());
        //    }
        //}

        //void ControlIsPalletUOM(string _value)
        //{
        //    if (_value == "YES")
        //    {
        //        txtConversion.SetValue(1);
        //        txtConversion.Enabled = false;
        //    }
        //    else
        //    {
        //        txtConversion.Enabled = true;
        //    }
        //    txtConversion.Update();
        //}
    }
}
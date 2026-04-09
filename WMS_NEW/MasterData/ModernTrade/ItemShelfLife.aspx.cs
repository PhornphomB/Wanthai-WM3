using System;
using System.Collections.Generic;
using System.Linq;
using Prototype.Providers;
using _UControls;

namespace WMS_NEW.MasterData.ModernTrade
{
    public partial class ItemShelfLife : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binding Event
                ddAgency.PostValueChanged += DdAgency_PostValueChanged;
                //ddlCategory.PostValueChanged += DdlCategory_PostValueChanged;

                chkCategory.PostValueChanged += chkCategory_PostValueChanged;
                chkItem.PostValueChanged += chkItem_PostValueChanged;


                #endregion

                #region Binging DropDown Property Column Grid
                iColWarehouse.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
                iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                iColCustomer.DropDownQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQuery(); };
                iColCategory.DropDownQueryProperty = delegate () { return Access.MasterData.ItemCategory.Instance.GetQuery(); };
                iColItem.DropDownQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery_Guid(); };
                #endregion

                #region Binging DropDown Property Input Data
                ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
                ddAgency.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                ddlCustomer.MethodQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQueryModernTrade(ddAgency.GetValue()); };
                ddlCategory.MethodQueryProperty = delegate () { return Access.MasterData.ItemCategory.Instance.GetQuery(); };
                ddlItem.MethodQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery(ddAgency.GetValue()); };
                #endregion


                #region Init PopupEntity 

                var access = new Access.MasterData.ModernTrade.ItemShelfLife();
                var entity = access.Entity;

                #region Init Controls Entity

                //var ddlOwner = new InputDropDown() { DataFieldValue = nameof(entity.owner_id), ComboType = ComboType.Guid, DisplayDefault = "--Select--", IsKey = true, LabelText = "Agency" };
                //var ddlCustomer = new InputDropDown() { DataFieldValue = nameof(entity.customer_id), ComboType = ComboType.Guid, DisplayDefault = "--Select--", LabelText = "Customer Code" };

                //if (!Page.IsPostBack)
                //{
                //    ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                //    ddlCustomer.MethodQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQueryModernTrade(); };
                //}

                //var custom_ctrls = new List<IEntityCustom>();
                //custom_ctrls.Add(new EntityCustom(ddlOwner) { RefGlobalVar = (REF) => { ddlOwner = REF; } });
                //custom_ctrls.Add(new EntityCustom(ddlCustomer) { RefGlobalVar = (REF) => { ddlCustomer = REF; } });



                //custom_ctrls.Add(new EntityCustom { DataFieldValue = nameof(entity.customer_id), IsKey = true });

                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.addr_line_1) });
                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.addr_line_2) });
                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.addr_line_3) });
                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.city) });
                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.province) });
                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.postal_code) });
                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.country_code) });
                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.country_name) });
                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.phone) });
                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.fax) });
                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.email) });
                //custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.contact) });

                #region Tab Modern Trade

                //chkAllowLotsOrder.PostValueChanged = chkAllowLotsOrder_PostValueChanged;
                //chkAllowLotsShipping.PostValueChanged = chkAllowLotsShipping_PostValueChanged;
                //chkControlShelfLife.PostValueChanged = chkControlShelfLife_PostValueChanged;
                //chkCustomerShelfLife.PostValueChanged = chkCustomerShelfLife_PostValueChanged;
                //chkItemShelfLife.PostValueChanged = chkItemShelfLife_PostValueChanged;


                //var refIControls = new List<EntityRef>();
                //refIControls.Add(new EntityRef(txtLotAllowOrder, (REF) => { txtLotAllowOrder = REF; }));
                //refIControls.Add(new EntityRef(chkAllowLotsShipping, (REF) => { chkAllowLotsShipping = REF; }));
                //refIControls.Add(new EntityRef(txtLotAllowShipping, (REF) => { txtLotAllowShipping = REF; }));

                //refIControls.Add(new EntityRef(chkControlShelfLife, (REF) => { chkControlShelfLife = REF; }));
                //refIControls.Add(new EntityRef(chkCustomerShelfLife, (REF) => { chkCustomerShelfLife = REF; }));
                //refIControls.Add(new EntityRef(chkItemShelfLife, (REF) => { chkItemShelfLife = REF; }));
                //refIControls.Add(new EntityRef(txtCustomerShelfLife, (REF) => { txtCustomerShelfLife = REF; }));

                //var ent_contents = contentEntity.GetEntitiesByContent(refIControls);
                //custom_ctrls.AddRange(ent_contents);

                #endregion

                #endregion

                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                ///popupEntity1.AutoCreateControlEntity(entity, custom_ctrls, null);
                popupEntity1.InitControlStatic();
                popupEntity1.AfterNewDataEvent += PopupEntity1_AfterNewDataEvent;
                popupEntity1.AfterSetEditDataEvent += PopupEntity1_AfterSetEditDataEvent;
                popupEntity1.PreSaveEntityEvent += PopupEntity1_PreSaveEntityEvent;

                GridExt1.PopupEntitySource = popupEntity1;

                var ucUser = new InputHidden();
                ucUser.DataFieldValue = "user_id";
                ucUser.SetValue(_SessionVals.UserName);

                GridExt1.AddFilterCustomInputInclude(ucUser);

                #endregion
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }



        #region Popup Entity
        private void PopupEntity1_AfterNewDataEvent()
        {
            try
            {
                ddlCategory.Enabled = true;
                ddlCategory.IsPrimary = true;
                ddlItem.Enabled = false;
                ddlItem.IsPrimary = false;

                chkCategory.Enabled = true;
                chkItem.Enabled = true;
                chkCategory.Checked = true;
                chkItem.Checked = false;

                ddlCategory.Update();
                chkCategory.Update();
                chkItem.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopupEntity1_AfterSetEditDataEvent()
        {
            try
            {
                var _objectEntity = (Source.t_wms_shelf_life)popupEntity1.ObjectDataAccess.Entity;

                //chkCategory.Enabled = true;
                //chkItem.Enabled = true;
                //if (_objectEntity.category_id != null)
                //{
                //    chkCategory.Checked = true;
                //    chkItem.Checked = false;

                //    chkCategory.Update();
                //    chkItem.Update();
                //}
                //else
                //{
                ddlCategory.Enabled = false;
                ddlCategory.IsPrimary = false;

                ddlItem.Enabled = true;
                ddlItem.IsPrimary = true;
                ddlCategory.Update();
                ddlItem.Update();

                chkCategory.Checked = false;
                chkItem.Checked = true;



                chkCategory.Update();
                chkItem.Update();
                //}
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopupEntity1_PreSaveEntityEvent()
        {
            try
            {
                //กำหนดค่าให้ Property บางส่วนที่ไม่ได้อยู่ในส่วนของ InputData ก่อนที่จะบันทึก
                var _objectEntity = (Source.t_wms_shelf_life)popupEntity1.ObjectDataAccess.Entity;


                if (chkCategory.Checked == true)
                {
                    _objectEntity.category_id = ddlCategory.GetValue();
                    _objectEntity.item_master_id = Guid.Empty;
                }
                else
                {
                    using (var model = new Source.WMSEntities())
                    {
                        Guid item_master_id = (Guid)ddlItem.GetValue();
                        var ent = model.t_wms_item.Where(w => w.item_master_id == item_master_id).FirstOrDefault();
                        if (ent != null)
                        {
                            _objectEntity.category_id = ent.category_id;
                            _objectEntity.item_master_id = ddlItem.GetValue();
                        }

                        //Guid customer_id = (Guid)ddlCustomer.GetValue();
                        //var entCustomer = model.t_wms_customer.Where(w => w.customer_id == customer_id).FirstOrDefault();
                        //if (entCustomer != null)
                        //    _objectEntity.customer_name = entCustomer.customer_name;
                    }

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        #endregion

        private void DdAgency_PostValueChanged(dynamic _value)
        {
            try
            {
                ddlCustomer.BindDataSource();
                ddlCategory.BindDataSource();
                ddlItem.BindDataSource();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        void chkCategory_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == "YES")
                {
                    ddlCategory.Enabled = true;
                    ddlCategory.IsPrimary = true;
                    ddlItem.Enabled = false;
                    ddlItem.IsPrimary = false;
                    ddlItem.SetValue(null);

                    chkItem.Checked = false;
                }
                else
                {
                    ddlCategory.Enabled = false;
                    ddlCategory.IsPrimary = false;
                    ddlItem.Enabled = true;
                    ddlItem.IsPrimary = true;
                    ddlCategory.SetValue(null);

                    chkItem.Checked = true;
                }

                ddlCategory.Update();
                ddlItem.Update();
                chkItem.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void chkItem_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == "YES")
                {
                    ddlCategory.Enabled = false;
                    ddlCategory.IsPrimary = false;
                    ddlItem.Enabled = true;
                    ddlItem.IsPrimary = true;
                    ddlCategory.SetValue(null);

                    chkCategory.Checked = false;
                }
                else
                {
                    ddlCategory.Enabled = true;
                    ddlCategory.IsPrimary = true;
                    ddlItem.Enabled = false;
                    ddlItem.IsPrimary = false;
                    ddlItem.SetValue(null);

                    chkCategory.Checked = true;
                }

                ddlCategory.Update();
                ddlItem.Update();
                chkCategory.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}
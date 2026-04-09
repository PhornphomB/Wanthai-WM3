using System;
using System.Collections.Generic;
using System.Linq;
using Prototype.Providers;
using _UControls;

namespace WMS_NEW.MasterData
{
    public partial class Customer : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binging DropDown Property Column Grid

                iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };

                #endregion


                #region Init PopupEntity 

                var access = new Access.MasterData.Customer();
                var entity = access.Entity;

                #region Init Controls Entity

                var ddlOwner = new InputDropDown() { DataFieldValue = nameof(entity.owner_id), ComboType = ComboType.Guid, DisplayDefault = "--Select--", IsKey = true };

                if (!Page.IsPostBack)
                    ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };

                var custom_ctrls = new List<IEntityCustom>();
                custom_ctrls.Add(new EntityCustom(ddlOwner) { RefGlobalVar = (REF) => { ddlOwner = REF; } });
                custom_ctrls.Add(new EntityCustom { DataFieldValue = nameof(entity.customer_code), IsKey = true });
                custom_ctrls.Add(new EntityCustom { DataFieldValue = nameof(entity.ref_customer_code), IsPrimary = true });


                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.addr_line_1) });
                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.addr_line_2) });
                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.addr_line_3) });
                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.city) });
                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.province) });
                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.postal_code) });
                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.country_code) });
                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.country_name) });
                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.phone) });
                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.fax) });
                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.email) });
                custom_ctrls.Add(new EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.contact) });


                #region Tab Modern Trade

                chkAllowLotsOrder.PostValueChanged = chkAllowLotsOrder_PostValueChanged;
                chkAllowLotsShipping.PostValueChanged = chkAllowLotsShipping_PostValueChanged;
                chkControlShelfLife.PostValueChanged = chkControlShelfLife_PostValueChanged;
                chkCustomerShelfLife.PostValueChanged = chkCustomerShelfLife_PostValueChanged;
                chkItemShelfLife.PostValueChanged = chkItemShelfLife_PostValueChanged;
                chkControlReturnedLotProcessing.PostValueChanged = chkControlReturnedLotProcessing_PostValueChanged;

                var refIControls = new List<EntityRef>();
                refIControls.Add(new EntityRef(txtLotAllowOrder, (REF) => { txtLotAllowOrder = REF; }));
                refIControls.Add(new EntityRef(txtDayBetweenLot, (REF) => { txtDayBetweenLot = REF; }));
                refIControls.Add(new EntityRef(chkAllowLotsShipping, (REF) => { chkAllowLotsShipping = REF; }));
                refIControls.Add(new EntityRef(txtLotAllowShipping, (REF) => { txtLotAllowShipping = REF; }));

                refIControls.Add(new EntityRef(chkControlShelfLife, (REF) => { chkControlShelfLife = REF; }));
                refIControls.Add(new EntityRef(chkCustomerShelfLife, (REF) => { chkCustomerShelfLife = REF; }));
                refIControls.Add(new EntityRef(chkItemShelfLife, (REF) => { chkItemShelfLife = REF; }));
                refIControls.Add(new EntityRef(txtCustomerShelfLife, (REF) => { txtCustomerShelfLife = REF; }));
                refIControls.Add(new EntityRef(chkControlNoBackDate, (REF) => { chkControlNoBackDate = REF; }));


                var ent_contents = contentEntity.GetEntitiesByContent(refIControls);
                custom_ctrls.AddRange(ent_contents);

                #endregion


                var tabs_attr = new EntityTab[]
                {
                    new EntityTab { TabIndex = 1, TabName = "Address",ResourceGroup="customer",ResourceName="tab_address"},
                    new EntityTab { TabIndex = 2, TabName = "Modern Trade" ,ResourceGroup="customer",ResourceName="tab_modern_trade"},
                    new EntityTab { TabIndex = 3, TabName = "User Define",ResourceGroup="customer",ResourceName="tab_user_define", IsAutoUserDefine =true }
                };

                #endregion

                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                popupEntity1.AutoCreateControlEntity(entity, custom_ctrls, tabs_attr);
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


        void chkItemShelfLife_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == "YES")
                {
                    chkCustomerShelfLife.Checked = false;
                }
                else
                {
                    chkCustomerShelfLife.Checked = true;
                }

                chkCustomerShelfLife.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void chkCustomerShelfLife_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == "YES")
                {
                    chkItemShelfLife.Checked = false;
                }
                else
                {
                    chkItemShelfLife.Checked = true;
                }

                chkItemShelfLife.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void chkControlShelfLife_PostValueChanged(dynamic _value)
        {
            try
            {
                chkCustomerShelfLife.Enabled = _value == "YES" ? true : false;
                chkItemShelfLife.Enabled = _value == "YES" ? true : false;

                if (_value == "NO")
                {
                    txtCustomerShelfLife.Enabled = false;

                    chkCustomerShelfLife.Checked = false;
                    chkItemShelfLife.Checked = false;
                }
                else
                {
                    chkCustomerShelfLife.Checked = true;
                    txtCustomerShelfLife.Enabled = true;
                }

                chkCustomerShelfLife.Update();
                chkItemShelfLife.Update();

                txtCustomerShelfLife.Update();

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void chkAllowLotsShipping_PostValueChanged(dynamic _value)
        {
            try
            {
                txtLotAllowShipping.Enabled = _value == "YES" ? true : false;
                txtLotAllowShipping.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }

        }

        void chkAllowLotsOrder_PostValueChanged(dynamic _value)
        {
            try
            {
                txtLotAllowOrder.Enabled = _value == "YES" ? true : false;
                txtLotAllowOrder.Update();
                txtDayBetweenLot.Enabled = _value == "YES" ? true : false;
                txtDayBetweenLot.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();


            }

        }
        void chkControlReturnedLotProcessing_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == "YES")
                {
                    chkControlNoBackDate.Checked = true;
                }
                chkControlNoBackDate.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Count
{
    public partial class CountPlanExt : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binding Event

                ddCountType.PostValueChanged += ddCountType_PostValueChanged;
                ddWarehouseId.PostValueChanged += ddWarehouseId_PostValueChanged;
                ddlOwner.PostValueChanged += ddlOwner_PostValueChanged;
                ddlCustomer.PostValueChanged += ddlCustomer_PostValueChanged;

                gridCyclePlan.GridRowDelete += gridCyclePlan_GridRowDelete;

                chkGenOrderNo.PostValueChanged += chkGenOrderNo_PostValueChanged;


                #endregion

                #region Binging DropDown Property Input Data

                ddWarehouseId.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
                ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                ddCountType.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("COUNT_TYPE"); };
                ddlCustomer.MethodQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQuery(ddlOwner.GetValue()); };

                #endregion

                #region Binging DropDown Cycle Count Search

                ddLocFrom.MethodQueryProperty = delegate () { return GetQueryLocation(); };
                ddLocTo.MethodQueryProperty = delegate () { return GetQueryLocation(); };

                ddItemCat.MethodQueryProperty = delegate () { return Access.MasterData.ItemCategory.Instance.GetQuery(); };
                ddItemNumber.MethodQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery(); };

                #endregion

                if (!Page.IsPostBack)
                {
                    DeleteCycleCountTemp();

                    hidUserID.SetValue(_SessionVals.UserName);

                    ddWarehouseId.BindDataSource();
                    ddlOwner.BindDataSource();

                    ddCountType.AutoPostBack = true;
                    ddCountType.BindDataSource();

                    SetDefaultControl();


                    ddItemCat.BindDataSource();
                    ddItemNumber.BindDataSource();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        IQueryable<Prototype.Providers.Property> GetQueryLocation()
        {
            var loctypes = Access.MasterData.Rule.Instance.GetListByRuleCode("RULE_LOCATION_TYPE_FOR_COUNT_EXTERNAL");

            return Access.MasterData.Location.Instance.GetLocationNameByLocType_Customer((Guid)ddWarehouseId.GetValue(), loctypes, (Guid)ddlCustomer.GetValue());
        }

        void DeleteCycleCountTemp()
        {
            using (var _acc = new Access.Transaction.Count.CycleCountTempExt())
            {
                base.PlugEventResult(_acc);

                if (_acc.CountTempByUser(_SessionVals.UserName) > 0)
                {
                    _acc.DeleteTempByUser(_SessionVals.UserName);
                }
            }
        }

        bool IsCycleCountTemp()
        {
            using (var _acc = new Access.Transaction.Count.CycleCountTempExt())
            {
                base.PlugEventResult(_acc);

                return _acc.CountTempByUser(_SessionVals.UserName) > 0;
            }
        }


        #region Search Item to Cycle Count

        protected void btAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if ((!string.IsNullOrEmpty(ddLocFrom.GetValue()) && string.IsNullOrEmpty(ddLocTo.GetValue())) || (string.IsNullOrEmpty(ddLocFrom.GetValue()) && !string.IsNullOrEmpty(ddLocTo.GetValue())))
                {
                    Page.MessageWarning("! Please Select Location From and Location To");
                    return;
                }

                var filters = new Prototype.Providers.FilterList();

                if (!string.IsNullOrEmpty(ddLocFrom.GetValue()) && !string.IsNullOrEmpty(ddLocTo.GetValue()))
                {
                    var fil = new Prototype.Providers.Filter();
                    fil.DataPropertyName = ddLocFrom.DataFieldValue;
                    fil.IsFilter = true;
                    fil.FilterAt = Prototype.Providers.FilterAt.Between;
                    fil.Symbol = "({0}>=@0 AND {1}<=@1)";
                    fil.Value = ddLocFrom.GetValue();
                    fil.ValueTo = ddLocTo.GetValue();

                    filters.Add(fil);
                }
                if (ddItemCat.GetValue() != null && ddItemCat.GetValue() != Guid.Empty)
                {
                    filters.Add(ddItemCat.GetFilter());
                }
                if (!string.IsNullOrEmpty(ddItemNumber.GetValue()))
                {
                    filters.Add(ddItemNumber.GetFilter());
                }

                if (filters.Count == 0)
                {
                    Page.MessageWarning("! Please select at least 1 filter");
                    return;
                }

                using (var _acc = new Access.Transaction.Count.CycleCountTempExt())
                {
                    base.PlugEventResult(_acc);

                    if (_acc.AddTempByInventory(filters, (Guid)ddWarehouseId.GetValue(), (Guid)ddlOwner.GetValue(), (Guid)ddlCustomer.GetValue()))
                    {
                        SetPermission();

                        gridCyclePlan.DataBind();
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


        void SetDefaultControl()
        {
            chkGenOrderNo.Checked = false;
            chkGenOrderNo_PostValueChanged(false);
        }

        void SetPermission()
        {
            ddWarehouseId.Enabled = ddlOwner.Enabled = ddlCustomer.Enabled = !IsCycleCountTemp();
            updateCreateCount.Update();
        }

        #region Control Event Button

        protected void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCountID.GetValue()))
                {
                    return;
                }

                var saved = false;

                using (var _acc = new Access.Transaction.Count.CountPlanExt())
                {
                    base.PlugEventResult(_acc);

                    if (ddCountType.GetValue() == "CYCLE")
                    {
                        if (Access.Transaction.Count.CycleCountTempExt.Instance.CountTempByUser(_SessionVals.UserName) == 0)
                        {
                            this.MessageWarning("! Not found items for count plan");
                            return;
                        }

                        saved = _acc.SaveByCycle(ddWarehouseId.GetText().Split(':')[0].Trim(), txtCountID.GetValue(), ddCountType.GetValue(), txtDescription.GetValue(), ddlOwner.GetValue(), (Guid)ddlCustomer.GetValue());
                    }
                    else if (ddCountType.GetValue() == "PHYSICAL")
                    {
                        saved = _acc.SaveByPhysical(ddWarehouseId.GetText().Split(':')[0].Trim(), txtCountID.GetValue(), ddCountType.GetValue(), txtDescription.GetValue(), ddlOwner.GetValue(), (Guid)ddlCustomer.GetValue());
                    }
                }

                if (saved)
                {
                    SetPermission();

                    gridCyclePlan.DataBind();

                    SetDefaultControl();

                    ddCountType.Clear();
                    ddCountType_PostValueChanged(ddCountType.GetValue());

                    txtDescription.Clear();
                    txtDescription.Update();

                    updateContent1.Update();

                    this.MessageSuccess("Create Count Plan Success.");
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        #endregion

        #region Control Event Dropdown

        void ddCountType_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == "PHYSICAL")
                {
                    updateCreateCount.Attributes["class"] = "col-sm-12";
                    updateCycleCount.Visible = false;
                }
                else
                {
                    updateCreateCount.Attributes["class"] = "col-sm-4";
                    updateCycleCount.Visible = true;
                }

                updateContent1.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void ddWarehouseId_PostValueChanged(dynamic _value)
        {
            try
            {
                ddLocFrom.BindDataSource();
                ddLocTo.BindDataSource();

                updateCycleCount.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void ddlOwner_PostValueChanged(dynamic _value)
        {
            try
            {
                ddlCustomer.BindDataSource();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void ddlCustomer_PostValueChanged(dynamic _value)
        {
            try
            {
                ddLocFrom.BindDataSource();
                ddLocTo.BindDataSource();

                updateCycleCount.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        #endregion

        #region Control Event Checkbox

        void chkGenOrderNo_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == true)
                {
                    var _no = Access.FunctionGenerate.Instance.GetGenerateRunning("COUNT_EXTERNAL");

                    txtCountID.SetValue(_no);
                    txtCountID.Enabled = false;
                }
                else
                {
                    txtCountID.Clear();
                    txtCountID.Enabled = true;
                }

                txtCountID.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #endregion

        #region Control Event Grid
        void gridCyclePlan_GridRowDelete(object _rowKeyValue)
        {
            try
            {
                var deleted = false;
                var id = Guid.Parse(_rowKeyValue.ToString());

                using (var _acc = new Access.Transaction.Count.CycleCountTempExt())
                {
                    this.PlugEventResult(_acc);

                    var keys = _acc.GetDeleteDtoById(id);
                    deleted = _acc.DeleteByNormal((Guid)keys.location_id, (Guid)keys.wh_item_master_id, keys.parent_lpn, keys.lpn);
                }

                if (deleted)
                {
                    SetPermission();

                    gridCyclePlan.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #endregion

        protected void btnRemoveItemAll_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteCycleCountTemp();

                SetPermission();

                gridCyclePlan.DataBind();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}
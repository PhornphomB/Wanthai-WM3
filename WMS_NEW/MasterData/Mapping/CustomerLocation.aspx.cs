using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.MasterData.Mapping
{
    public partial class CustomerLocation : PageCustom
    {
        public Guid wh_master_id
        {
            get
            {
                if (ViewState["wh_master_id"] == null)
                {
                    ViewState["wh_master_id"] = Guid.Empty;
                }
                return (Guid)ViewState["wh_master_id"];
            }
            set
            {
                ViewState["wh_master_id"] = value;
            }
        }

        public Guid customer_id
        {
            get
            {
                if (ViewState["customer_id"] == null)
                {
                    ViewState["customer_id"] = Guid.Empty;
                }
                return (Guid)ViewState["customer_id"];
            }
            set
            {
                ViewState["customer_id"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.GetCurrent(this.Page).AsyncPostBackTimeout = 600;

                #region Binding Event

                comboMapType.PostValueChanged += comboMapType_PostValueChanged;
                //GridExt1.GridRowCanSelectValidate += GridExt1_GridRowCanSelectValidate;

                // iColWh.DropDownPostValueChanged = iColWh_DropDownPostValueChanged;
                ddlCustomer.PostValueChanged = ddlCustomer_PostValueChanged;
                ddlWarehouse.PostValueChanged = ddlWarehouse_PostValueChanged;
                #endregion

                // iColWh.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery(); };
                iColLocation.DropDownQueryProperty = delegate () { return new Access.MasterData.Location().GetQuery_Warehouse(this.wh_master_id); };
                iColActive.DropDownQueryProperty = delegate () { return new ConfigGlobal.DTO._Global.MappingStatus().AsQueryable(); };

                ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(); };
                ddlCustomer.MethodQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQuery(); };


                if (!Page.IsPostBack)
                {
                    comboMapType.MethodQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.MappingType().AsQueryable(); };
                    comboMapType.BindDataSource();


                    btMapping.Visible = false;
                    comboMapType.VisibleExt = false;

                    GridExt1.GridAllowSelectBox = false;
                    GridExt1.UpdateCommand();

                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void ddlWarehouse_PostValueChanged(dynamic obj)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }

                this.wh_master_id = (Guid)obj;
                GridExt1.ClearFilter(iColLocation);

                if (this.wh_master_id != Guid.Empty && this.customer_id != Guid.Empty)
                {
                    GridExt1.Search();
                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void ddlCustomer_PostValueChanged(dynamic _value)
        {
            try
            {

                Guid customer_id = Guid.Empty;
                if (_value != null)
                {
                    customer_id = (Guid)_value;
                }
                this.customer_id = customer_id;

                if (_value == null)
                {
                    btMapping.Visible = false;
                    comboMapType.VisibleExt = false;

                    GridExt1.GridAllowSelectBox = false;
                    GridExt1.UpdateCommand();

                    return;
                }

                if (this.wh_master_id == Guid.Empty)
                {
                    Page.MessageWarning("Please select Warehouse !");
                    return;
                }


                btMapping.Visible = true;
                comboMapType.VisibleExt = true;
                comboMapType.Clear();

                if (this.wh_master_id != Guid.Empty && this.customer_id != Guid.Empty)
                {
                    GridExt1.Search();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void iColWh_DropDownPostValueChanged(dynamic obj)
        {
            try
            {
                //if (obj == null)
                //{
                //    return;
                //}

                //this.wh_master_id = (Guid)obj;
                //GridExt1.ClearFilter(iColLocation);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void comboMapType_PostValueChanged(dynamic _value)
        {
            try
            {
                GridExt1.DeleteAllKey();

                if (_value == "0")
                {
                    GridExt1.GridAllowSelectBox = true;
                    GridExt1.GridHighlightAllRow = false;
                }
                else if ((_value == "1") || (_value == "2"))
                {
                    GridExt1.GridAllowSelectBox = false;
                    GridExt1.GridHighlightAllRow = _value == "1" ? true : false;
                }

                if (Page.IsPostBack)
                {
                    GridExt1.UpdateContent();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btMapping_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridExt1 == null || GridExt1.Rows.Count <= 0)
                {
                    Page.MessageWarning("Please select customer !");
                    return;
                }

                using (var _acc = new Access.MasterData.Mapping.CustomerLocation())
                {
                    this.PlugEventResult(_acc);

                    if (comboMapType.GetValue() == "0")
                    {
                        //Manual
                        if (GridExt1.CountListKey() <= 0)
                        {
                            Page.MessageWarning("Customer has been already used !");

                            return;
                        }
                        if (_acc.SaveMapping(GridExt1.GetListKey(), this.customer_id, _SessionVals.UserName))
                        {
                            GridExt1.DeleteAllKey();
                            GridExt1.DataBind();
                        }
                    }
                    else if ((comboMapType.GetValue() == "1") || (comboMapType.GetValue() == "2"))
                    {
                        var checkAll = comboMapType.GetValue() == "1" ? true : false;
                        var listAll = from rows in GridExt1.GetAllKeyQuery<string>()
                                      select new ConfigGlobal.DTO._Global.KeySelect
                                      {
                                          KeyId = rows,
                                          Active = checkAll
                                      };


                        if (_acc.SaveMapping(listAll.ToList(), this.customer_id, _SessionVals.UserName))
                        {
                            GridExt1.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }

}
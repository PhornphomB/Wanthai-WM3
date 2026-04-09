using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.MasterData.Mapping
{
    public partial class ZoneLocation : PageCustom
    {
        private Guid wh_master_id
        {
            get
            {
                if (ViewState["wh_master_id"] == null)
                    ViewState["wh_master_id"] = Guid.Empty;

                return (Guid)ViewState["wh_master_id"];
            }
            set
            {
                ViewState["wh_master_id"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binding Event

                comboMapType.PostValueChanged += comboMapType_PostValueChanged;

                iColWarehouse.DropDownPostValueChanged = iColWarehouse_DropDownPostValueChanged;
                iColZone.DropDownPostValueChanged = iColZone_PostValueChanged;
                #endregion

                iColWarehouse.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(); };
                iColZone.DropDownQueryProperty = delegate () { return Access.MasterData.Zone.Instance.GetQuery_Warehouse(this.wh_master_id); };
                iColActive.DropDownQueryProperty = delegate () { return new ConfigGlobal.DTO._Global.MappingStatus().AsQueryable(); };

                if (!Page.IsPostBack)
                {
                    comboMapType.MethodQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.MappingType().AsQueryable(); };
                    comboMapType.BindDataSource();

                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void iColWarehouse_DropDownPostValueChanged(dynamic obj)
        {
            GridExt1.ClearFilter(iColZone);
            GridExt1.DataUnBind();

            if (obj == null)
            {
                this.wh_master_id = Guid.Empty;
                return;
            }

            this.wh_master_id = (Guid)obj;

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
                    GridExt1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        void iColZone_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == null)
                {
                    btMapping.Visible = false;
                    comboMapType.VisibleExt = false;

                    //GridExt1.GridAllowRowEdit = false;
                    GridExt1.GridAllowSelectBox = false;
                    GridExt1.UpdateCommand();
                }
                else
                {
                    btMapping.Visible = true;
                    comboMapType.VisibleExt = true;

                    //GridExt1.GridAllowRowEdit = true;
                    GridExt1.GridAllowSelectBox = true;
                    GridExt1.UpdateCommand();
                }

                if (Page.IsPostBack)
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

        protected void btMapping_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridExt1 == null || GridExt1.Rows.Count <= 0)
                {
                    Page.MessageWarning("Please select zone id. !");
                    return;
                }

                using (var _acc = new Access.MasterData.Mapping.ZoneLocation())
                {
                    this.PlugEventResult(_acc);

                    if (comboMapType.GetValue() == "0")
                    {
                        //Manual

                        if (GridExt1.GetListKey().Count <= 0)
                        {
                            Page.MessageWarning("Zone has been already used !");

                            return;
                        }


                        //if (_acc.SaveMapping(GridExt1.GetListKey(), _SessionVals.UserName, this.wh_master_id))
                        if (_acc.SaveMapping(GridExt1.GetListKey(), _SessionVals.UserName))
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


                        //if (_acc.SaveMapping(listAll.ToList(), _SessionVals.UserName, this.wh_master_id))
                        if (_acc.SaveMapping(listAll.ToList(), _SessionVals.UserName))
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
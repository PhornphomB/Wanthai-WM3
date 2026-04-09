using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Source;

namespace WMS_NEW.MasterData.Mapping
{
    public partial class LocationItem : PageCustom
    {
        public Guid wh_master_id
        {
            get
            {
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
                ScriptManager.GetCurrent(this.Page).AsyncPostBackTimeout = 600;

                #region Binding Event

                comboMapType.PostValueChanged += comboMapType_PostValueChanged;
                //GridExt1.GridRowCanSelectValidate += GridExt1_GridRowCanSelectValidate;

                iColWh.DropDownPostValueChanged = iColWh_DropDownPostValueChanged;
                iColLocation.DropDownPostValueChanged = iColLocation_PostValueChanged;
                #endregion

                iColWh.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(); };

                iColLocation.DropDownQueryProperty = delegate () { return new Access.MasterData.Location().GetQuery_Warehouse(this.wh_master_id); };
                iColActive.DropDownQueryProperty = delegate () { return new ConfigGlobal.DTO._Global.MappingStatus().AsQueryable(); };

                if (!Page.IsPostBack)
                {
                    comboMapType.MethodQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.MappingType().AsQueryable(); };
                    comboMapType.BindDataSource();


                    btMapping.Visible = false;
                    comboMapType.VisibleExt = false;

                    GridExt1.GridAllowSelectBox = false;
                    GridExt1.UpdateCommand();
                    GetWhMasterId();

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
                GridExt1.ClearFilter(iColLocation);
                GridExt1.DataUnBind();

                if (obj == null)
                {
                    this.wh_master_id = Guid.Empty;
                    return;
                }

                this.wh_master_id = (Guid)obj;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void GetWhMasterId()
        {
            try
            {
                using(var _Model = new WMSEntities())
                {
                    var mapping = _Model.v_wms_mapping_user_warehouse.Where(x => x.user_id == _SessionVals.UserName && x.is_active == true);
                    if(mapping.Count() == 1)
                    {
                        this.wh_master_id = mapping.FirstOrDefault().wh_master_id;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        //private bool GridExt1_GridRowCanSelectValidate(GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        var is_item = (bool)DataBinder.Eval(e.Row.DataItem, "is_item");
        //        return !is_item;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

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

                if (Page.IsPostBack && hidLocationID.GetValue() != null)
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
        void iColLocation_PostValueChanged(dynamic _value)
        {
            try
            {
                Guid location_id = Guid.Empty;
                if (_value != null)
                {
                    location_id = (Guid)_value;
                }
                hidLocationID.SetValue(location_id);


                if (_value == null)
                {
                    btMapping.Visible = false;
                    comboMapType.VisibleExt = false;

                    GridExt1.GridAllowSelectBox = false;
                    GridExt1.UpdateCommand();
                }
                else
                {
                    btMapping.Visible = true;
                    comboMapType.VisibleExt = true;
                    comboMapType.Clear();

                    //GridExt1.GridAllowSelectBox = true;
                    //GridExt1.UpdateCommand();
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
                    Page.MessageWarning("Please select location !");
                    return;
                }

                using (var _acc = new Access.MasterData.Mapping.LocationItem())
                {
                    this.PlugEventResult(_acc);

                    if (comboMapType.GetValue() == "0")
                    {
                        //Manual
                        if (GridExt1.CountListKey() <= 0)
                        {
                            Page.MessageWarning("Location has been already used !");

                            return;
                        }
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
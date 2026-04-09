using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.MasterData.Mapping
{
    public partial class WarehouseItem : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binding Event

                comboMapType.PostValueChanged += comboMapType_PostValueChanged;

                iColWarehouse.DropDownPostValueChanged = iColWarehouse_PostValueChanged;
                #endregion

                iColWarehouse.DropDownQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(); };
                iColActive.DropDownQueryProperty = delegate () { return new ConfigGlobal.DTO._Global.MappingStatus().AsQueryable(); };
                iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };

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
        void iColWarehouse_PostValueChanged(dynamic _value)
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

                GridExt1.Search();


                //if (Page.IsPostBack)
                //{
                //    GridExt1.Search();
                //}
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
                    Page.MessageWarning("Please select warehouse id. !");
                    return;
                }

                using (var _acc = new Access.MasterData.Mapping.WarehouseItem())
                {
                    this.PlugEventResult(_acc);

                    if (comboMapType.GetValue() == "0" )
                    {
                        //Manual

                        if (GridExt1.GetListKey().Count <= 0)
                        {
                            Page.MessageWarning("Warehouse id has been already used !");

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
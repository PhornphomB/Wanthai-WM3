using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Outbound {
    public partial class CarRegister : PageCustom {

        Access.Transaction.Outbound.CarRegister _carRegister = null;

        protected void Page_Load(object sender, EventArgs e) {
            try {
                _carRegister = new Access.Transaction.Outbound.CarRegister();
                #region Binging DropDown Property Column Grid

                iColWarehouse.DropDownQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };
                iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                iColTruckType.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("truck_type"); };
                iColHeadTail.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("container"); };
                iColLoadStatus.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("checker_load_status"); };


                #endregion Binging DropDown Property Column Grid     

                popupCarRegister.PreSaveEntityEvent += popupCarRegister_PreSaveEntityEvent;
                ddlWarehouse.PostValueChanged += ddlWarehouse_PostValueChanged;
                //dtpRegisterDate.PostValueChanged += dtpRegisterDate_PostValueChanged;
                dtpDockDoorDate.PostValueChanged += dtpDockDoorDate_PostValueChanged;
                ddlDoor.PostValueChanged += ddlDoor_PostValueChanged;
                GridExt1.GridRowEdit += GridExt1_GridRowEdit;
                GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;
                GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;
                popupCarRegister.AfterNewDataEvent += popupCarRegister_AfterNewDataEvent;


                #region Binging DropDown Property Input Data

                if (!Page.IsPostBack) {
                    ddlWarehouse.MethodQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };
                    ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                    ddTruckType.MethodQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("truck_type"); };
                    ddHeadTail.MethodQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("container"); };                                       
                }

                #endregion Binging DropDown Property Input Data

                popupCarRegister.InitObjectsEvent += () => { popupCarRegister.ObjectDataAccess = _carRegister; };
                popupCarRegister.InitControlStatic();

                GridExt1.PopupEntitySource = popupCarRegister;                
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExt1_GridRowAfterDataBound(GridViewRowEventArgs e) {
            try {
                var load_status = (string)DataBinder.Eval(e.Row.DataItem, "load_status");
                var btOpen= (Button)e.Row.FindControl("OPEN");                
                if (load_status == "CLOSED") {
                    btOpen.CssClass = "btn btn-sm btn-success btn-ingrid";                    
                    btOpen.Enabled = true;
                    btOpen.Text = "CHECKING";
                    btOpen.CommandName = "CHECKING";
                } else if (load_status == "CHECKING") {
                    btOpen.CssClass = "btn btn-sm btn-warning btn-ingrid";
                    btOpen.Enabled = true;
                    btOpen.Text = "CLOSED";
                    btOpen.CommandName = "CLOSED";
                } else {
                    btOpen.Visible = false;
                }
                //
                var is_able_delete = (string)DataBinder.Eval(e.Row.DataItem, "is_able_delete");
                var cmdDelete = (LinkButton)e.Row.FindControl("cmdDelete");
                if (is_able_delete == "YES") {
                    cmdDelete.Visible = true;
                } else {
                    cmdDelete.Visible = false;
                }               
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExt1_GridRowCommandClick(GridViewCommandEventArgs e) {
            try {
                var message_text = string.Empty;
                var is_success = false;
                var id = Guid.Parse(e.CommandArgument.ToString());
                //
                if (e.CommandName == "CLOSED") {
                    is_success = this._carRegister.updateCloseLoadStatus(id);
                    if (is_success) {
                        GridExt1.DataBind();
                        Page.MessageSuccess("Update CLOSED Success.");
                    }
                }
                //
                if (e.CommandName == "CHECKING") {
                    is_success = this._carRegister.updateCheckingLoadStatus(id, ref message_text);
                    if (is_success) {
                        GridExt1.DataBind();
                        Page.MessageSuccess("Update CHECKING Success.");
                    } else {
                        Page.MessageWarning(message_text);
                    }
                }
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void popupCarRegister_AfterNewDataEvent() {
            try {
                ddlWarehouse.Enabled = true;
                ddlOwner.Enabled = true;
                txtLicense.Enabled = true;
                dtpRegisterDate.Enabled = true;
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }            
        }

        private void GridExt1_GridRowEdit(object _rowKeyValue) {
            try {
                ddlWarehouse.Enabled = false;
                ddlOwner.Enabled = false;
                txtLicense.Enabled = false;
                dtpRegisterDate.Enabled = false;
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }            
        }

        private void ddlDoor_PostValueChanged(dynamic _val) {
            try {                
                if (_val == null || ddlDoor.GetText() == "ลานรถ : ลานรถรอเข้า Door") {
                    dtpDockDoorDate.IsPrimary = false;                    
                } else {
                    dtpDockDoorDate.IsPrimary = true;
                    dtpDockDoorDate.SetValue(DateTime.Now);
                }
                popupCarRegister.UpdateContent();
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void dtpRegisterDate_PostValueChanged(dynamic _val) {
            try {
                if (_val == null) {
                    return;
                } else {
                    // เพิ่มไปอีก 1 นาทีเพื่อลดการคาดเกี่ยวเวลา
                    if (Convert.ToDateTime(_val) >  DateTime.Now.AddMinutes(1)) {
                        dtpRegisterDate.SetValue(DateTime.Now);
                        this.MessageWarning("Can not select register date time future");
                    }
                }
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void dtpDockDoorDate_PostValueChanged(dynamic _val) {
            try {
                if (_val == null) {
                    return;
                } else {             
                    // เพิ่มไปอีก 1 นาทีเพื่อลดการคาดเกี่ยวเวลา
                    if (Convert.ToDateTime(_val) > DateTime.Now.AddMinutes(1)) {
                        dtpDockDoorDate.SetValue(DateTime.Now);
                        this.MessageWarning("Can not select previous dock door date time future");
                    }
                }
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void ddlWarehouse_PostValueChanged(dynamic _val) {
            try {
                if (_val == null) {
                    return;
                }
                //                
                Guid location_id = new Guid();
                if (popupCarRegister.FormState == _UControls.FormState.New) {                    
                    //
                    dtpRegisterDate.SetValue(DateTime.Now);
                }
                if (popupCarRegister.FormState == _UControls.FormState.Edit) {
                    var _objEntity = (popupCarRegister.ObjectDataAccess as Access.Transaction.Outbound.CarRegister).Entity;
                    if (_objEntity.location_id != null)  {
                        location_id = (Guid)_objEntity.location_id;
                    }                   
                    //
                    dtpRegisterDate.SetValue(DateTime.Now);
                }
                //
                ddlDoor.ClearItems();
                ddlDoor.Clear();
                //
                ddlDoor.MethodQueryProperty = delegate () { return Access.Transaction.Outbound.CarRegister.Instance.GetCodeQueryDoor(_val, location_id); };
                ddlDoor.BindDataSource();
                // กำหนดให้เป็นลานรถเสมอ
                if (popupCarRegister.FormState == _UControls.FormState.New) {
                    IQueryable<Property> location_stg = this._carRegister.GetCodeQueryDoorSTG(_val);
                    ddlDoor.SetValue(location_stg.FirstOrDefault().guid_member);
                }                
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void popupCarRegister_PreSaveEntityEvent() {
            try {
                var _objEntity = (popupCarRegister.ObjectDataAccess as Access.Transaction.Outbound.CarRegister).Entity;
                if (popupCarRegister.FormState == _UControls.FormState.New) {                    
                    _objEntity.load_status = "REGISTERED";
                    _objEntity.door = (ddlDoor.GetText() == "--Select--"? string.Empty: ddlDoor.GetText());
                    _objEntity.register_by = _SessionVals.UserName;
                    _objEntity.create_by = _SessionVals.UserName;
                    _objEntity.create_date = DateTime.Now;
                } else if (popupCarRegister.FormState == _UControls.FormState.Edit) {
                    _objEntity.door = (ddlDoor.GetText() == "--Select--" ? string.Empty : ddlDoor.GetText());
                    _objEntity.update_by = _SessionVals.UserName;
                    _objEntity.update_date = DateTime.Now;
                }
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}
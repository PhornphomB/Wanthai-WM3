using System;
using System.Collections.Generic;
using System.Web.UI;
using WMS_NEW.Report.AscxControls;

namespace WMS_NEW.Transaction.Outbound.AscxControls
{
    public partial class PickListDetail : UControlCustom, _UControls.IFormRelation
    {
        #region ++ DELEGATE ++
        delegate void dg_Search();
        event dg_Search eSearch;
        #endregion

        public bool is_pick_system
        {
            get
            {
                if (ViewState["is_pick_system"] == null)
                    return false;
                else
                    return (bool)ViewState["is_pick_system"];
            }
            set
            {
                ViewState["is_pick_system"] = value;
            }
        }
        public Delegate dg_CallBackSearch;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                eSearch += new dg_Search(Pick_CallBack);
                PickDetailConfirmPickUser.dg_CallBackSearch = eSearch;

                popupPickDetail.CloseClick += popupPickDetail_CloseClick;
                PickDetailConfirmPick1.CloseClick += TransactionProcess_CloseClick;
                PickDetailUnPick1.CloseClick += TransactionProcess_CloseClick;
                PickDetailConfirmShip1.CloseClick += TransactionProcess_CloseClick;

                ReportViewer.BindingParameter += ReportViewer_BindingParameter;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void Pick_CallBack()
        {
            try
            {
                gridDetail.Search();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private Access.Transaction.Outbound.OutboundMasterDto OutboundMasterDTO
        {
            get
            {
                if (ViewState["OutboundMasterDTO"] == null)
                    return new Access.Transaction.Outbound.OutboundMasterDto();
                else
                    return (Access.Transaction.Outbound.OutboundMasterDto)ViewState["OutboundMasterDTO"];
            }
            set
            {
                ViewState["OutboundMasterDTO"] = value;
            }
        }

        public Action<dynamic> UpdateParent { get; set; }

        public void InitForm(dynamic _dto)
        {
            try
            {
                var is_pick_system = (bool)_dto.is_pick_system;
                this.is_pick_system = is_pick_system;
                var _ent = _dto.dto;

                OutboundMasterDTO = (Access.Transaction.Outbound.OutboundMasterDto)_dto.dto;

                string isPick = Access.MasterData.Rule.Instance.GetRuleValue("OUTBOUND_PICK_PC");
                string isPickUser = Access.MasterData.Rule.Instance.GetRuleValue("OUTBOUND_USER_PICK_PC");
                if (is_pick_system)
                {
                    btConfirmPick.Visible = isPick == "YES";
                }
                else
                {
                    btConfirmPick.Visible = isPickUser == "YES";
                }

                btConfirmPick.Enabled = Access.MasterData.Rule.Instance.Any_Rule("RULE_OUTBOUND_ORDER_STATUS_FOR_PICK", _ent.order_status);
                btUnPick.Enabled = Access.MasterData.Rule.Instance.Any_Rule("RULE_OUTBOUND_ORDER_STATUS_FOR_UNPICK", _ent.order_status);
                btConfirmShip.Enabled = Access.MasterData.Rule.Instance.Any_Rule("RULE_OUTBOUND_ORDER_STATUS_FOR_SHIP", _ent.order_status);

                //btPartialShip.Enabled = btConfirmShip.Enabled;

                var allow_close = (OutboundMasterDTO.order_status == "SHIP") || (OutboundMasterDTO.order_status == "PICK") || (OutboundMasterDTO.order_status == "PICKED");
                btCloseOrder.Enabled = allow_close;

                txtWarehouseId.SetValue(_ent.wh_id);
                txtOrderNo.SetValue(_ent.outbound_order_number);
                txtCustomer.SetValue(_ent.customer_name);
                txtOwner.SetValue(_ent.owner_code);
                txtStatus.SetValue(_ent.order_status);

                txtQtyPlan.SetValue(_ent.sum_qty_plan);
                txtQtyPick.SetValue(_ent.sum_qty_pick);
                txtQtyStaging.SetValue(_ent.sum_qty_stage);
                txtQtyLoad.SetValue(_ent.sum_qty_load);
                txtQtyShip.SetValue(_ent.sum_qty_ship);

                hidOrderMasterId.SetValue(_ent.outbound_order_master_id);
                gridDetail.Search();

                hidPickRefOrderMasterId.SetValue(_ent.outbound_order_master_id);
                gridPickDetail.Search();

                hidSrlPickRefOrderMasterId.SetValue(_ent.outbound_order_master_id);
                gridPickSerialDetail.Search();

                PanelTab1.ChangeActivePanel(1);
                popupPickDetail.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        void popupPickDetail_CloseClick(object sender, EventArgs e)
        {
            try
            {
                gridDetail.DataUnBind();
                gridPickDetail.DataUnBind();
                gridPickSerialDetail.DataUnBind();
                if (dg_CallBackSearch != null)
                    dg_CallBackSearch.DynamicInvoke();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void TransactionProcess_CloseClick(object sender, EventArgs e)
        {
            try
            {
                this.RefreshViewData();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void RefreshViewData()
        {
            try
            {
                var ent = Access.Transaction.Outbound.Outbound.Instance.GetDtoByKeyId(OutboundMasterDTO.outbound_order_master_id);
                if (ent != null)
                {
                    var dto_new = new { dto = ent, is_pick_system = this.is_pick_system };
                    this.InitForm(dto_new);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btConfirmPick_Click(object sender, EventArgs e)
        {
            if (this.is_pick_system)
            {
                PickDetailConfirmPick1.InitForm(OutboundMasterDTO);
            }
            else
            {
                PickDetailConfirmPickUser.InitForm(OutboundMasterDTO);

            }
        }

        protected void btUnPick_Click(object sender, EventArgs e)
        {
            PickDetailUnPick1.InitForm(OutboundMasterDTO);
        }


        protected void btConfirmShip_Click(object sender, EventArgs e)
        {
            PickDetailConfirmShip1.InitForm(OutboundMasterDTO);
        }

        protected void btCloseOrder_Click(object sender, EventArgs e)
        {
            txtOutboundOrderNoClose.SetValue(txtOrderNo.GetValue());
            txtCloseDate.SetValue(DateTime.Now);
            txtCloseRemark.Clear();
            popClose.ShowDialog();
        }

        protected void btCloseConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                bool is_success;

                using (var acc = new Access.Transaction.Outbound.OutboundRelease())
                {
                    this.PlugEventResult(acc);

                    is_success = acc.CloseOrder(OutboundMasterDTO.wh_id, OutboundMasterDTO.outbound_order_master_id, txtCloseRemark.GetValue(), txtCloseDate.GetValue().Value);
                }

                if (is_success)
                {
                    this.RefreshViewData();
                    popClose.HideDialog();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void btRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshViewData();
        }

        //protected void btPartialShip_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        bool is_success;
        //        using (var acc = new Access.Transaction.Outbound.OutboundShipDetail())
        //        {
        //            this.PlugEventResult(acc);

        //            is_success = acc.UpdateConfirmShipPartial(OutboundMasterDTO.outbound_order_master_id);
        //        }

        //        if (is_success)
        //        {
        //            this.RefreshViewData();
        //            Page.MessageSuccess("Partial Ship Success.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging = new Prototype.Providers.Logging(this, ex);
        //        RaiseLogging();
        //    }
        //}

        #region Function Report Viewer

        protected void btReport_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer.InitialForm("Outbound");
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        List<ReportParameter> ReportViewer_BindingParameter(string _report_id)
        {
            var _outbound_order_master_id = OutboundMasterDTO.outbound_order_master_id.ToString();


            var prms = new List<ReportParameter>();

            if (_report_id == "0FBEB6E6-4836-4129-8106-C097665A13D8")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_master_id", Value = _outbound_order_master_id });
            }
            else if (_report_id == "AFE196FF-CEBC-4D02-9B41-E2345B91E866")
            {
                prms.Add(new ReportParameter { Name = "@outboundID", Value = _outbound_order_master_id });
            }
            else if (_report_id == "B357A61D-0739-45D7-B19C-002A5DFC0312")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_master_id", Value = _outbound_order_master_id });
            }
            else if (_report_id == "B98A4179-4B85-4DDE-9696-E1BE387D6CA2")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_number", Value = OutboundMasterDTO.outbound_order_number });
            }
            else if (_report_id == "B6E859EF-EABE-4BAD-9919-C78E4BD01458")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_number", Value = OutboundMasterDTO.outbound_order_number });
            }
            else if (_report_id == "081693A6-6FFF-46CB-B048-975F7A5D2D15")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_master_id", Value = _outbound_order_master_id });
            }
            else if (_report_id == "C642DEAB-4818-496D-A5BD-85ED403740E4")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_master_id", Value = _outbound_order_master_id });
            }

            else if (_report_id == "536D453B-133C-4B61-A4F5-285D9AEC824D")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_number", Value = OutboundMasterDTO.outbound_order_number });
                prms.Add(new ReportParameter { Name = "@outbound_order_master_id", Value = _outbound_order_master_id });
            }
            else if (_report_id == "00D30626-5BC0-4185-826E-31E65B160474")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_number", Value = OutboundMasterDTO.outbound_order_number });
            }
            else if (_report_id == "BC87FB2D-73C5-426F-A256-E06E0D0028DB")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_master_id", Value = _outbound_order_master_id });
            }
            else if (_report_id == "A7462C88-E941-426A-A5F7-A3B545D44A47")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_master_id", Value = _outbound_order_master_id });
            }


            else if (_report_id == "9B26F97D-9389-4256-871F-194EB85A90E4")
            {
                prms.Add(new ReportParameter { Name = "@invch_outbound_order_number", Value = OutboundMasterDTO.outbound_order_number });
                prms.Add(new ReportParameter { Name = "@invch_wh_id", Value = OutboundMasterDTO.wh_id });
                prms.Add(new ReportParameter { Name = "@invch_owner_code", Value = OutboundMasterDTO.owner_code });
            }
            else if (_report_id == "22BAA2EA-6434-48A1-97FE-7F3D3A7BF39B")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_number", Value = OutboundMasterDTO.outbound_order_number });
            }
            else if (_report_id == "A702A7E4-A2AB-4480-853F-73F8173A506C")
            {
                prms.Add(new ReportParameter { Name = "@order_number", Value = OutboundMasterDTO.outbound_order_number });
            }
            else if (_report_id == "A4357E71-0A75-4ADE-B43D-4EDF7BFDC012")
            {
                prms.Add(new ReportParameter { Name = "@order_number", Value = OutboundMasterDTO.outbound_order_number });
            }
            else if (_report_id == "D3DE9772-9EC8-4C2B-A1BC-B5A18B7DCD80")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_number", Value = OutboundMasterDTO.outbound_order_number });
                prms.Add(new ReportParameter { Name = "@wh_id", Value = OutboundMasterDTO.wh_id });
                prms.Add(new ReportParameter { Name = "@owner_id", Value = OutboundMasterDTO.owner_id.ToString() });
            }
            else if (_report_id == "7A82AEF1-4B91-4ED6-A61C-CCEC6D5D2132")
            {
                prms.Add(new ReportParameter { Name = "@outbound_order_master_id", Value = _outbound_order_master_id });
            }

            return prms;
        }
        #endregion
    }
}
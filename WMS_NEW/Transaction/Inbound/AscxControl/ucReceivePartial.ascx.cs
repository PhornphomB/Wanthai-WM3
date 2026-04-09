using _UControls;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Web.UI;
using WMS_NEW.Source;

namespace WMS_NEW.Transaction.Inbound.AscxControl
{
    public partial class ucReceivePartial : UControlCustom, IFormRelation
    {
        string msgCloseComplete = "Close Receipt complete.";

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

        public string order_status
        {
            get
            {
                return (string)ViewState["order_status"];
            }
            set
            {
                ViewState["order_status"] = value;
            }
        }
        public string order_type
        {
            get
            {
                return (string)ViewState["order_type"];
            }
            set
            {
                ViewState["order_type"] = value;
            }
        }

        public Guid inbound_order_master_id
        {
            get
            {
                return (Guid)ViewState["inbound_order_master_id"];
            }
            set
            {
                ViewState["inbound_order_master_id"] = value;
            }
        }
        public bool udf_9
        {
            get
            {
                return (bool)ViewState["udf_9"];
            }
            set
            {
                ViewState["udf_9"] = value;
            }
        }
        public bool df_month
        {
            get
            {
                return (bool)ViewState["df_month"];
            }
            set
            {
                ViewState["df_month"] = value;
            }
        }
        public bool isSaveAsPopup
        {
            get
            {
                return (bool)ViewState["isSaveAsPopup"];
            }
            set
            {
                ViewState["isSaveAsPopup"] = value;
            }
        }
        public DateTime mfg_date
        {
            get
            {
                return (DateTime)ViewState["mfg_date"];
            }
            set
            {
                ViewState["mfg_date"] = value;
            }
        }
        public bool checkMaxDate
        {
            get
            {
                if (ViewState["checkMaxDate"] == null)
                {
                    return false;
                }
                return (bool)ViewState["checkMaxDate"];
            }
            set
            {
                ViewState["checkMaxDate"] = value;
            }
        }
        public Action<dynamic> UpdateParent { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binding Event

                ddlReceiptNo.PostValueChanged += ddlReceiptNo_PostValueChanged;
                popupCloseReceipt.CloseClick += PopupCloseReceipt_CloseClick;
                #endregion


                if (!Page.IsPostBack)
                {
                    #region Initial Panel Tap
                    #endregion

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }



        #region Control Event
        void ddlReceiptNo_PostValueChanged(dynamic _value)
        {
            try
            {
                hdfreceipt_header_id.SetValue(_value);
                hdfreceipt_header_id_summary.SetValue(_value);

                var _ent = (WMS_NEW.Source.t_wms_receipt_header)WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptHeader.Instance.GetByKeyID(_value);
                if (_ent != null)
                {
                    txtStatus.SetValue(_ent.receipt_status);
                    txtCreateBy.SetValue(_ent.create_by);
                    txtCreateDate.SetValue(_ent.create_date);
                    txtCloseBy.SetValue(_ent.close_by);
                    txtCloseDate.SetValue(_ent.close_date);
                    txtUDF1.SetValue(_ent.user_def1);
                    txtUDF2.SetValue(_ent.user_def2);
                    txtUDF3.SetValue(_ent.user_def3);
                    txtUDF4.SetValue(_ent.user_def4);
                    txtUDF5.SetValue(_ent.user_def5);
                    txtUDF6.SetValue(_ent.user_def6);
                    txtUDF7.SetValue(_ent.user_def7);
                    txtUDF8.SetValue(_ent.user_def8);
                    if (df_month && _ent.user_def9 == null)
                    {
                        var mfg_date = WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptDetail.Instance.GetMfgDate(_ent.receipt_header_id);
                        this.mfg_date = Convert.ToDateTime(mfg_date);


                    }
                    else
                    {
                        txtUDF9.SetValue(_ent.user_def9);
                        this.mfg_date = Convert.ToDateTime(_ent.user_def9); 
                    }

                    if (df_month)
                    {
                        if (this.mfg_date.Month == DateTime.Now.Month && this.mfg_date.Year == DateTime.Now.Year)
                        {
                            txtUDF9.Enabled = false;
                            txtUDF9.SetValue(this.mfg_date);
                        }
                        else
                        {
                            if (DateTime.Now > this.mfg_date && DateTime.Now.Day == 1)
                            {
                                txtUDF9.Enabled = true;
                                txtUDF9.SetValue(this.mfg_date);
                            }
                            else if (DateTime.Now > this.mfg_date)
                            {
                                txtUDF9.Enabled = false;
                                txtUDF9.SetValue(DateTime.Now);
                            }
                        }
                    }
                    txtUDF10.SetValue(_ent.user_def10);

                    if (this.order_status.ToUpper() == "CLOSE")
                    {
                        btnCloseReceipt.Enabled = false;
                    }
                    else
                    {
                        btnCloseReceipt.Enabled = _ent.receipt_status == "CLOSE" ? false : true;
                    }
                }
                else
                {
                    txtStatus.Clear();
                    txtCreateBy.SetValue(string.Empty);
                    txtCreateDate.SetValue(null);
                    txtCloseBy.SetValue(string.Empty);
                    txtCloseDate.SetValue(null);
                    txtUDF1.SetValue(null);
                    txtUDF2.SetValue(null);
                    txtUDF3.SetValue(null);
                    txtUDF4.SetValue(null);
                    txtUDF5.SetValue(null);
                    txtUDF6.SetValue(null);
                    txtUDF7.SetValue(null);
                    txtUDF8.SetValue(null);
                    txtUDF9.SetValue(null);
                    txtUDF10.SetValue(null);


                    btnCloseReceipt.Enabled = false;
                }


                PanelTab1.UpdateContent();
                updateCloseReceipt.Update();

                GridExtSummary.Search();
                GridExtDetail.Search();
                this.isSaveAsPopup = false;
                this.checkMaxDate = false;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        protected void btnSaveReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validate_Udf_9())
                {
                    return;
                }

                using (var acc = new WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptHeader())
                {
                    this.PlugEventResult(acc);

                    acc.GetByKeyID(ddlReceiptNo.GetValue());
                    acc.Entity.user_def1 = txtUDF1.GetValue();
                    acc.Entity.user_def2 = txtUDF2.GetValue();
                    acc.Entity.user_def3 = txtUDF3.GetValue();
                    acc.Entity.user_def4 = txtUDF4.GetValue();
                    acc.Entity.user_def5 = txtUDF5.GetValue();
                    acc.Entity.user_def6 = txtUDF6.GetValue();
                    acc.Entity.user_def7 = txtUDF7.GetValue();
                    acc.Entity.user_def8 = txtUDF8.GetValue();
                    acc.Entity.user_def9 = txtUDF9.GetValue();
                    acc.Entity.user_def10 = txtUDF10.GetValue();

                    acc.Update();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void btnCloseReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validate_Udf_9_Close_Receipt())
                {
                    return;
                }
                if (!this.isSaveAsPopup)
                {
                    WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptHeader.Instance.UpdateUserDef9(hdfreceipt_header_id.GetValue(), txtUDF9.GetValue());
                }
                Close_Receipt();
                this.checkMaxDate = false;
                ddlReceiptNo_PostValueChanged(hdfreceipt_header_id.GetValue());

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

       
        protected void btRefreshDetail_Click(object sender, EventArgs e)
        {
            try
            {
                var receipt_header_id = ddlReceiptNo.GetValue();
                ddlReceiptNo.MethodQueryProperty = delegate () { return WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptHeader.Instance.GetQuery(this.inbound_order_master_id); };
                ddlReceiptNo.BindDataSource();
                ddlReceiptNo.SetValue(receipt_header_id);
                ddlReceiptNo_PostValueChanged(receipt_header_id);

                GridExtDetail.DataBind();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btRefreshDetailSummary_Click(object sender, EventArgs e)
        {
            try
            {
                var receipt_header_id = ddlReceiptNo.GetValue();
                ddlReceiptNo.MethodQueryProperty = delegate () { return WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptHeader.Instance.GetQuery(this.inbound_order_master_id); };
                ddlReceiptNo.BindDataSource();
                ddlReceiptNo.SetValue(receipt_header_id);
                ddlReceiptNo_PostValueChanged(receipt_header_id);

                GridExtSummary.DataBind();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        #endregion

        #region Function

        public void InitForm(dynamic _obj)
        {
            try
            {
                var ent = (Source.t_wms_inbound_master)_obj;
                this.inbound_order_master_id = ent.inbound_order_master_id;
                this.wh_master_id = ent.wh_master_id;
                this.order_status = ent.order_status;
                this.order_type = ent.order_type;

                ddlReceiptNo.MethodQueryProperty = delegate () { return WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptHeader.Instance.GetQuery(ent.inbound_order_master_id); };
                ddlReceiptNo.BindDataSource();

                btnCloseReceipt.Enabled = ent.order_status.ToUpper() == "CLOSE" ? false : true;


            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void Close_Receipt()
        {
            try
            {
                if (ddlReceiptNo.GetValue() == null)
                {
                    Page.MessageWarning("Please select Receipt Number !");
                    return;
                }

                using (var _acc = new WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptDetail())
                {
                    this.PlugEventResult(_acc);
                    string msg = string.Empty;

                    Guid receipt_header_id = ddlReceiptNo.GetValue();

                    var isClose = _acc.Close_Receipt(this.wh_master_id, this.inbound_order_master_id, receipt_header_id);

                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void Refresh_Data()
        {
            try
            {
                var receipt_header_id = ddlReceiptNo.GetValue();
                ddlReceiptNo.MethodQueryProperty = delegate () { return WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptHeader.Instance.GetQuery(this.inbound_order_master_id); };
                ddlReceiptNo.BindDataSource();
                ddlReceiptNo.SetValue(receipt_header_id);
                ddlReceiptNo_PostValueChanged(receipt_header_id);

                GridExtDetail.DataBind();
                GridExtSummary.DataBind();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Is_Primary_Udf_9(bool isPrimary, bool dfMonth)
        {
            txtUDF9.IsPrimary = isPrimary;
            this.udf_9 = isPrimary;
            this.df_month = dfMonth;
            txtUDF9.Update();
        }

        public bool Validate_Udf_9()
        {
            if (this.udf_9)
            {
                DateTime currentDate = DateTime.Now;
                DateTime userDef9Date = Convert.ToDateTime(txtUDF9.GetValue());

                if (userDef9Date.Year != currentDate.Year || userDef9Date.Month != currentDate.Month)
                {
                    Page.MessageWarning(Access.Configuration.ResourceDetail.GetResource("inbound_master", "validate_user_def9_same_month_and_year"));
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        public bool Validate_Udf_9_Close_Receipt()
        {
            this.isSaveAsPopup = false;

            if (this.df_month)
            {
                DateTime userDef9Date = Convert.ToDateTime(txtUDF9.GetValue());
                string userDefString = userDef9Date.ToString("yyyy-MM-dd");

                if (!checkMaxDate)
                {
                    lblValidateCloseOrder.Text = _SessionVals.LocaleID == "1033" ? "Do you want to save the close date as " + userDefString + "?" : "ต้องการบันทึกข้อมูลวันที่ปิดรายการเป็นวันที่ " + userDefString + "หรือไม่";
                    popupCloseReceipt.ShowDialog();
                    return false;
                }
                else
                {

                    DateTime lastDay = new DateTime(this.mfg_date.Year, this.mfg_date.Month, DateTime.DaysInMonth(this.mfg_date.Year, this.mfg_date.Month));
                    string firstDateString = this.mfg_date.ToString("yyyy-MM-dd");
                    string lastDateString = lastDay.ToString("yyyy-MM-dd");

                    if (userDef9Date > lastDay || userDef9Date < this.mfg_date)
                    {
                        string message_en = "You can enter data from the date of manufacture: " + firstDateString + " until the end of the month: " + lastDateString + "";
                        string message_th = "สามารถกรอกข้อมูลได้ตั้งแต่วันที่ผลิต: " + firstDateString + " จนถึงวันสิ้นสุดของเดือนที่ผลิต: " + lastDateString + "";
                        Page.MessageWarning(_SessionVals.LocaleID == "1033" ? message_en : message_th);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return Validate_Udf_9();
            }
        }
        protected void btComfirmClose_Click(object sender, EventArgs e)
        {
            WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptHeader.Instance.UpdateUserDef9(hdfreceipt_header_id.GetValue(), txtUDF9.GetValue());
            this.isSaveAsPopup = true;
            checkMaxDate = false;
            Close_Receipt();
            popupCloseReceipt.HideDialog();
        }
        private void PopupCloseReceipt_CloseClick(object sender, EventArgs e)
        {
            txtUDF9.Enabled = true;
            checkMaxDate = true;
            PanelTab1.UpdateContent();
        }

        #endregion
    }
}
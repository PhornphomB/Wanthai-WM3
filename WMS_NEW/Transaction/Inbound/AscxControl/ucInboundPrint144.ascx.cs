using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WMS_NEW.Transaction.Inbound.AscxControl
{
    public partial class ucInboundPrint144 : UControlCustom
    {

        #region View State

        protected string wh_id
        {
            get
            {
                return (string)ViewState["wh_id"];
            }
            set
            {
                ViewState["wh_id"] = value;
            }
        }

        protected string owner_code
        {
            get
            {
                return (string)ViewState["owner_code"];
            }
            set
            {
                ViewState["owner_code"] = value;
            }
        }

        protected Guid inbound_order_master_id
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

        protected Guid inbound_order_detail_id
        {
            get
            {
                if (ViewState["inbound_order_detail_id"] == null)
                    ViewState["inbound_order_detail_id"] = Guid.Empty;


                return (Guid)ViewState["inbound_order_detail_id"];
            }
            set
            {
                ViewState["inbound_order_detail_id"] = value;
            }
        }

        protected int? day_to_expire
        {
            get
            {
                if (ViewState["day_to_expire"] == null)
                    ViewState["day_to_expire"] = 0;

                return (int?)ViewState["day_to_expire"];
            }
            set
            {
                ViewState["day_to_expire"] = value;
            }
        }

        protected string uom_primary
        {
            get
            {
                return (string)ViewState["uom_primary"];
            }
            set
            {
                ViewState["uom_primary"] = value;
            }
        }

        protected string inbound_order_number
        {
            get
            {
                return (string)ViewState["inbound_order_number"];
            }
            set
            {
                ViewState["inbound_order_number"] = value;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ddlItemUom.PostValueChanged += ddlItemUom_PostValueChanged;
                txtMfgDate.PostValueChanged += txtMfgDate_PostValueChanged;

                chkLPN.PostValueChanged = chkLPN_PostValueChanged;

                if (!Page.IsPostBack)
                {
                    ddlPrint.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQuery_Print(); };
                    ddlPrint.BindDataSource();

                    ddlGroupPrint.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQuery_PrintGroup(); };
                    ddlGroupPrint.BindDataSource();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }

        }

        private void chkLPN_PostValueChanged(dynamic _value)
        {
            if (_value)
            {
                string lpn = string.Empty;
                Get_LPN("GET", ref lpn);

                txtLpn.Enabled = false;
                txtLpn.SetValue(lpn);
            }
            else
            {
                txtLpn.Enabled = true;
                txtLpn.Clear();

            }
            txtLpn.Update();
        }

        private void ddlItemUom_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == null)
                    _value = 0d;

                txtPackSize.SetValue(_value);
                txtPackSize.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void txtMfgDate_PostValueChanged(dynamic _value)
        {
            try
            {
                var mfg_date = txtMfgDate.GetValue();

                if (mfg_date != null && this.day_to_expire.HasValue && this.day_to_expire.Value > 0)
                {
                    var exp_date = mfg_date.Value.AddDays(this.day_to_expire.Value);
                    txtExpDate.SetValue(exp_date);
                }
                else
                {
                    txtExpDate.Clear();
                }

                txtExpDate.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void InitialForm(Guid _inbound_order_detail_id)
        {
            this.inbound_order_detail_id = _inbound_order_detail_id;

            Clear();

            using (var _model = new Source.WMSEntities())
            {
                var ent = _model.t_wms_inbound_detail.Single(x => x.inbound_order_detail_id == _inbound_order_detail_id);
                var dto_uom = _model.t_wms_item_uom.Select(se => new { se.primary_uom, se.uom }).FirstOrDefault(x => x.primary_uom == "YES");

                this.wh_id = ent.t_wms_inbound_master.wh_id;
                this.owner_code = ent.t_wms_inbound_master.owner_code;
                this.uom_primary = dto_uom.uom;
                this.day_to_expire = ent.t_wms_wh_item.t_wms_item.days_to_expire;
                this.inbound_order_number = ent.t_wms_inbound_master.inbound_order_number;

                txtItemNumber.SetValue(ent.item_number);
                txtItemDesc.SetValue(ent.t_wms_wh_item.t_wms_item.description);

                ddlItemUom.MethodQueryProperty = delegate () { return Access.MasterData.ItemUom.Instance.GetQueryPackSize_WhItem(ent.wh_item_master_id); };
                ddlItemUom.BindDataSource();

                txtQTY.SetValue(ent.quantity_order);
                txtQTY.Update();

                //Default print lot by p'nut 2020-10-19
                txtLotPrint.SetValue(ent.lot_number);
                txtLotPrint.Update();
            }

            txtWarehouse.SetValue(this.wh_id);
            txtWarehouse.Update();

            chkFullAmt.Checked = false;

            txtNumberofCopy.SetValue(1);
            txtNumberofCopy.Update();
        }

        void Clear()
        {
            foreach (var control in Panel1.Controls)
            {
                if (control is _UControls._IInputControl)
                {
                    var ictrl = ((_UControls._IInputControl)control);

                    ictrl.Clear();
                    ictrl.Update();
                }
            }
        }

        public void ShowDialog()
        {
            popupPrint.ShowDialog();
        }
        public void HideDialog()
        {
            popupPrint.HideDialog();
        }


        protected void btnConfrimPrint_Click(object sender, EventArgs e)
        {
            try
            {
                Print_Barcode();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void Print_Barcode()
        {
            try
            {
                //Get Parth
                string path = string.Empty;
                string path_trigger = string.Empty;

                using (var acc = new Access.Transaction.Inbound.InboundDetail())
                {
                    Guid printer_id = ddlPrint.GetValue();
                    Guid group_id = ddlGroupPrint.GetValue();

                    var ent = acc._Model.t_com_config_printer_group_mapping.Where(w => w.group_id == group_id && w.printer_id == printer_id).FirstOrDefault();
                    if (ent != null)
                    {
                        path = ent.bartender_data_filepath;
                        path_trigger = ent.bartender_trigger_filepath;
                    }
                    else
                    {
                        this.Page.MessageWarning("Please set printer and group printer !");
                        return;
                    }

                    //Exist Folder
                    string[] arr = path.Split('\\');
                    string folder = path.Substring(0, path.Length - (arr.Last().Length + 1));
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false, System.Text.UnicodeEncoding.Unicode))
                    {
                        string txtData = string.Empty;
                        string txtSplit = "|";

                        StringBuilder sb = new StringBuilder();

                        sb.Append("warehouse|");
                        sb.Append("owner|");
                        sb.Append("item_number|");
                        sb.Append("item_description|");
                        sb.Append("uom|");
                        sb.Append("pack_size|");
                        sb.Append("lot|");
                        sb.Append("mfg_date|");
                        sb.Append("expiry_date|");
                        sb.Append("remark|");
                        sb.Append("lpn_prefix|");
                        sb.Append("lpn_suffix|");
                        sb.Append("lpn|");
                        sb.Append("order_number");

                        file.WriteLine(sb.ToString());


                        var qty = (double)txtQTY.GetValue();
                        var pack = (double)txtPackSize.GetValue();


                        int copy = txtNumberofCopy.GetValue().Value;
                        int qty_loop = 1;
                        double qty_loose = (qty % pack);

                        if (chkFullAmt.GetValue() == true)
                            qty_loop = Convert.ToInt32(Math.Floor(qty / pack));

                        if (qty_loose > 0)
                            qty_loop++;


                        for (int copy_inx = 1; copy_inx <= copy; copy_inx++)
                        {
                            for (int loop_inx = 1; loop_inx <= qty_loop; loop_inx++)
                            {
                                sb.Clear();

                                string LPN_split = string.Empty;
                                string LPN_Print = string.Empty;

                                Get_LPN("SET", ref LPN_Print);

                                if (chkLPN.Checked)
                                {
                                    int lenPrefix = txtPrefix.GetValue() == null ? 0 : txtPrefix.GetValue().Length;
                                    int lenSuffix = txtSuffix.GetValue() == null ? 0 : txtSuffix.GetValue().Length;
                                    int lenLPN = LPN_Print == null ? 0 : LPN_Print.Length;

                                    if (LPN_Print == null)
                                    {
                                        LPN_split = string.Empty;
                                    }
                                    else
                                    {
                                        LPN_split = LPN_Print.Substring(lenPrefix);
                                        LPN_split = LPN_split.Substring(0, LPN_split.Length - lenSuffix);
                                    }
                                }
                                else
                                {
                                    LPN_split = txtLpn.GetValue() == null ? string.Empty : txtLpn.GetValue();
                                }

                                sb.Append(txtWarehouse.GetValue() + txtSplit);
                                sb.Append(this.owner_code + txtSplit);
                                sb.Append(txtItemNumber.GetValue() + txtSplit);
                                sb.Append(txtItemDesc.GetValue() + txtSplit);
                                sb.Append(this.uom_primary + txtSplit);
                                sb.Append(((loop_inx == qty_loop && qty_loose > 0) ? qty_loose : pack) + txtSplit);
                                sb.Append(txtLotPrint.GetValue() + txtSplit);
                                sb.Append((txtMfgDate.GetValue().HasValue ? txtMfgDate.GetValue().Value.ToString("yyyyMMdd", FieldsStatic.CultureInfo) : "") + txtSplit);
                                sb.Append((txtExpDate.GetValue().HasValue ? txtExpDate.GetValue().Value.ToString("yyyyMMdd", FieldsStatic.CultureInfo) : "") + txtSplit);
                                sb.Append(txtRemark.GetValue() + txtSplit);

                                sb.Append(txtPrefix.GetValue() + txtSplit);
                                sb.Append(txtSuffix.GetValue() + txtSplit);
                                sb.Append(LPN_split + txtSplit);
                                sb.Append(this.inbound_order_number);

                                file.WriteLine(sb.ToString());
                            }
                        }

                        var fileTrigger = new StreamWriter(path_trigger, false, System.Text.UnicodeEncoding.Unicode);
                        fileTrigger.WriteLine("Write data complete." + DateTime.Now.ToString());
                        fileTrigger.Close();

                        this.Page.MessageSuccess("Print success.");

                        HideDialog();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void Get_LPN(string GenerateType, ref string _lpn)
        {
            using (var _acc = new Access.Transaction.Inbound.InboundDetail())
            {
                string appID = _SessionVals.AppID;
                string deviceName = _SessionVals.DeviceID;
                string userID = _SessionVals.UserName;


                string prefix = txtPrefix.GetValue() ?? string.Empty;
                string suffix = txtSuffix.GetValue() ?? string.Empty;
                string LPN = string.Empty;
                string errCode = string.Empty;
                string errMsg = string.Empty;
                _acc.GetLPN(appID, this.wh_id, deviceName, userID, prefix, suffix, 1, GenerateType
                    , ref LPN, ref errCode, ref errMsg);

                if (errCode == "0")
                {
                    _lpn = LPN;
                }
            }
        }
    }
}
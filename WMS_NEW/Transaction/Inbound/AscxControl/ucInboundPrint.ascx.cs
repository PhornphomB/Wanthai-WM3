using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WMS_NEW.Transaction.Inbound.AscxControl
{
    public partial class ucInboundPrint : UControlCustom
    {
        #region View State
        public string wh_id
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
        public Guid inbound_order_detail_id
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
                txtNumberofSerial.Enabled = true;
            }
            else
            {
                txtLpn.Enabled = true;
                txtLpn.Clear();
                txtNumberofSerial.SetValue(1);
                txtNumberofSerial.Enabled = false;

            }
            txtLpn.Update();
            txtNumberofSerial.Update();
        }

        public void InitialForm(Guid _inbound_order_master_id, Guid? _inbound_order_detail_id)
        {
            this.inbound_order_master_id = _inbound_order_master_id;
            this.inbound_order_detail_id = _inbound_order_detail_id != null ? _inbound_order_detail_id.Value : Guid.Empty;


            using (var acc = new Access.Transaction.Inbound.InboundMaster())
            {
                var ent = acc.GetByKeyID(_inbound_order_master_id);
                if (ent != null)
                {
                    this.wh_id = ent.wh_id;
                }
            }

            Clear();

            txtWarehouse.SetValue(this.wh_id);
            txtWarehouse.Update();

            txtNumberofCopy.SetValue(1);
            txtNumberofCopy.Update();
        }

        void Clear()
        {
            foreach (var control in Panel1.Controls)
            {
                if (control is _UControls._IInputControl)
                {
                    ((_UControls._IInputControl)control).Clear();
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
                Print_Barcode(this.inbound_order_master_id, this.inbound_order_detail_id);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void Print_Barcode(Guid _inbound_order_master_id, Guid? _inbound_order_detail_id)
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

                        var result = acc._Model.v_wms_print_lpn.Where(w => w.inbound_order_master_id == _inbound_order_master_id);
                        if (_inbound_order_detail_id != Guid.Empty)
                        {
                            result = result.Where(w => w.inbound_order_detail_id == _inbound_order_detail_id);
                        }

                        StringBuilder sb = new StringBuilder();
                        //Header
                        var names = typeof(Source.v_wms_print_lpn).GetProperties().Select(property => property.Name).ToList();
                        foreach (var item in names)
                        {
                            sb.Append(item);
                            sb.Append(txtSplit);
                        }
                        sb.Append("WareHousePrint|");
                        sb.Append("PrefixPrint|");
                        sb.Append("SuffixPrint|");
                        sb.Append("LPNPrint|");
                        sb.Append("LotPrint|");
                        sb.Append("QTYPrint|");
                        sb.Append("NumberPrint|");
                        sb.Append("SerialPrint");
                        file.WriteLine(sb.ToString());

                        //Detail
                        foreach (var item in result)
                        {
                            sb.Clear();
                            foreach (var det in item.GetType().GetProperties().ToList())
                            {
                                string val = det.GetValue(item) != null ? det.GetValue(item).ToString() : string.Empty;
                                sb.Append(val);
                                sb.Append(txtSplit);
                            }
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
                            sb.Append(txtPrefix.GetValue() + txtSplit);
                            sb.Append(txtSuffix.GetValue() + txtSplit);
                            sb.Append(LPN_split + txtSplit);// txtLPNPrint.GetValue() + txtSplit;
                            sb.Append(txtLotPrint.GetValue() + txtSplit);
                            sb.Append(txtQTY.GetValue() + txtSplit);
                            sb.Append(txtNumberofCopy.GetValue() + txtSplit);
                            sb.Append(txtNumberofSerial.GetValue());
                            file.WriteLine(sb.ToString());
                        }

                        System.IO.StreamWriter fileTrigger = new System.IO.StreamWriter(path_trigger, false, System.Text.UnicodeEncoding.Unicode);
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
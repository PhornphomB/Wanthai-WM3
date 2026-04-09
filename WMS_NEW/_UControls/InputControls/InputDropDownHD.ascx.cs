using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls
{
    public class JsonDropDown
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public partial class InputDropDownHD : EntityCustomLayout, _IInputDropDown
    {

        #region CallBack Function

        protected void CallBackHandler1_RaiseCustomCallback(string[] eventArgs, string HandlerId, ref string CallBackResult, ref string ExcutableClientScript, System.Exception RaiseException)
        {
            if (RaiseException != null) return;

            var keyWordHas = eventArgs[0];
            var keySelectedHas = eventArgs[1];

            var keyWord = "";
            var keySelected = "";

            if (keyWordHas == "1" && keySelectedHas == "1")
            {
                keyWord = eventArgs[2];
                keySelected = eventArgs[3];
            }
            else if (keyWordHas == "1")
            {
                keyWord = eventArgs.Count() == 4 ? eventArgs[3] : eventArgs[2];
            }
            else if (keySelectedHas == "1")
            {
                keySelected = eventArgs.Count() == 4 ? eventArgs[3] : eventArgs[2];
            }

            string jsonString = this.GetJsonData(keyWord, keySelected, this.LoadLazyLimit);

            var excutableScript = new StringBuilder();

            CallBackResult = jsonString;
            ExcutableClientScript = excutableScript.ToString();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            baseControl.BaseFilterChangedValue += baseControl_BaseFilterChangedValue;

            CallBackHandler1.MethodAsynCall = comboValue.ClientID;
            CallBackHandler1.RaiseCustomCallback += CallBackHandler1_RaiseCustomCallback;

            if (!Page.IsPostBack)
            {
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.Contains);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.LessThan);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.LessThanEqual);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.MoreThan);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.MoreThanEqual);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.Between);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.Empty);
                 
                if (this.IsPrimary && Enabled)
                {
                    reqValidate.ControlToValidate = txtValue.ID;
                    reqValidate.ValidationGroup = this.ValidateGroup;
                }
                else
                {
                    reqValidate.Visible = false;
                }

                //Set Label
                baseControl.LabelText.IsPrimary = this.IsPrimary;

                if (!string.IsNullOrEmpty(LabelText))
                    baseControl.LabelText.InnerText = LabelText;

                if (!string.IsNullOrEmpty(DataFieldShowValue))
                {
                    baseControl.LabelText.DefaultText = DataFieldShowValue;
                }
                else if (!string.IsNullOrEmpty(DataFieldValue))
                {
                    baseControl.LabelText.DefaultText = DataFieldValue;
                }
                else
                {
                    baseControl.LabelText.DefaultText = this.ResourceName;
                }

                if (this.AutoPostBack)
                {
                    linkTrigger.Attributes.Add("onclick", "doPostBackAsync('" + linkTrigger.ClientID + "', 'CHANGE_VAL');");
                }

                this.TextInLine = this.TextInLine;

                if (baseControl.VisibleBaseContent != VisibleExt)
                    baseControl.VisibleBaseContent = VisibleExt;


            }
        }

        protected void Page_PreRender(object sender, EventArgs e) //Page Stat working after Load Control events
        {
            if (!Page.IsPostBack)
            {

            }
            else //Manual Event Postback of Trigger Control
            {
                if (Page.IsPostBack)
                {
                    var eventControl = Request.Params.Get("__EVENTTARGET");
                    var eventArgument = Request.Params.Get("__EVENTARGUMENT");

                    if (eventControl == linkTrigger.ClientID && eventArgument != string.Empty)
                    {
                        if (eventArgument.ToUpper() == "CHANGE_VAL")
                        {
                            comboValue_SelectedIndexChanged("CHANGE_VAL", EventArgs.Empty);
                        }
                    }
                }
            }

            BindingScript();
        }

        void BindingScript()
        {
            var servCssClass = string.Empty;
            if (this.IsPrimary)
                servCssClass = "validate";

            var servEnable = this.Enabled;
            if (this.Enabled)
            {
                if (this.Readonly)
                    servEnable = false;
            }

            var asyncFunction = "function(keywords,elmId,panelDataId) { " + CallBackHandler1.MethodAsynCall + "_asyncall(keywords,elmId,panelDataId); }";

            var clientScript = @" $('document').ready(function () {";

            clientScript += "";

            clientScript += " bindDropDownSearch('#" + comboValue.ClientID + "', '#" + txtValue.ClientID + "','#" + hidDisplay.ClientID + "','#" + panelData.ClientID + "','#"
                + linkTrigger.ClientID + "'," + this.AutoPostBack.ToString().ToLower() + ", " + this.ControlWidth.ToString() + ","
                + servEnable.ToString().ToLower() + ",'" + servCssClass + "'," + this.AllowMultiValue.ToString().ToLower() + "," + asyncFunction + "," + this.IsPrimary.ToString().ToLower() + ");";

            clientScript += "});";

            Page.ScriptPageRegister(clientScript);
        }


        public void SetEnableFilter(Prototype.Providers.FilterAt filter)
        {
            baseControl.SetEnableFilter(filter);
        }
        public void SetDisableFilter(Prototype.Providers.FilterAt filter)
        {
            baseControl.SetDisableFilter(filter);
        }


        public void Update()
        {
            baseControl.UpdateContent();
        }


        #region Properties Filter

        void baseControl_BaseFilterChangedValue(Prototype.Providers.FilterAt _filterAt)
        {
            if (_filterAt == Prototype.Providers.FilterAt.None)
            {
                if (Filterable == true)
                {
                    Enabled = false;
                }
            }
            else
            {
                if (Enabled == false) Enabled = true;
            }
        }

        public Prototype.Providers.FilterAt DefaultFilter
        {
            get
            {
                return baseControl.DefaultFilter;
            }
            set
            {
                baseControl.DefaultFilter = value;
            }
        }

        public bool Filterable
        {
            get
            {
                if (ViewState["Filterable"] == null)
                    ViewState["Filterable"] = false;

                return (bool)ViewState["Filterable"];
            }
            set
            {
                baseControl.Filterable = value;

                ViewState["Filterable"] = value;
            }
        }

        public bool FixFilter
        {
            get
            {
                return baseControl.FixFilter;
            }
            set
            {
                baseControl.FixFilter = value;
            }
        }

        public bool IsFilter
        {
            get { return baseControl.IsFilter; }
        }

        public void ClearFilter()
        {
            baseControl.ClearFilter();
        }

        #endregion


        #region Properties Control

        public string ValidateMessage
        {
            get
            {
                if (ViewState["ValidateMessage"] == null)
                    ViewState["ValidateMessage"] = string.Empty;

                return (string)ViewState["ValidateMessage"];
            }
            set
            {
                ViewState["ValidateMessage"] = value;
            }
        }
        public string ValidateGroup
        {
            get
            {
                if (ViewState["ValidateGroup"] == null)
                    ViewState["ValidateGroup"] = string.Empty;

                return (string)ViewState["ValidateGroup"];
            }
            set
            {
                ViewState["ValidateGroup"] = value;

                try
                {
                    reqValidate.ValidationGroup = value;
                }
                catch { }
            }
        }
        public string DataFieldValue
        {
            get
            {
                if (ViewState["DataFieldValue"] == null)
                    ViewState["DataFieldValue"] = string.Empty;

                return (string)ViewState["DataFieldValue"];
            }
            set
            {
                ViewState["DataFieldValue"] = value;
            }
        }
        public string DataFieldTempValue
        {
            get
            {
                if (ViewState["DataFieldTempValue"] == null)
                    ViewState["DataFieldTempValue"] = string.Empty;

                return (string)ViewState["DataFieldTempValue"];
            }
            set
            {
                ViewState["DataFieldTempValue"] = value;
            }
        }
        public string DataFieldShowValue
        {
            get
            {
                if (ViewState["DataFieldShowValue"] == null)
                    ViewState["DataFieldShowValue"] = string.Empty;

                return (string)ViewState["DataFieldShowValue"];
            }
            set
            {
                ViewState["DataFieldShowValue"] = value;
            }
        }
        public bool IsPrimary
        {
            get
            {
                if (ViewState["IsPrimary"] == null)
                    ViewState["IsPrimary"] = false;

                return (bool)ViewState["IsPrimary"];
            }
            set
            {
                ViewState["IsPrimary"] = value;

                if (Page != null && Page.IsPostBack && reqValidate != null && txtValue != null)
                {
                    try
                    {
                        baseControl.LabelText.IsPrimary = value;

                        if (value && Enabled)
                        {
                            if (txtValue != null)
                            {
                                reqValidate.ControlToValidate = txtValue.ID;
                                reqValidate.ValidationGroup = this.ValidateGroup;
                                reqValidate.Visible = true;
                            }
                        }
                        else
                        {
                            reqValidate.Visible = false;
                        }
                    }
                    catch { }
                }
            }
        }
        public bool IsKey
        {
            get
            {
                if (ViewState["IsKey"] == null)
                    ViewState["IsKey"] = false;

                return (bool)ViewState["IsKey"];
            }
            set
            {
                ViewState["IsKey"] = value;
            }
        }
        public bool IsStaticValue
        {
            get
            {
                if (ViewState["IsStaticValue"] == null)
                    ViewState["IsStaticValue"] = false;

                return (bool)ViewState["IsStaticValue"];
            }
            set
            {
                ViewState["IsStaticValue"] = value;
            }
        }

        public string LabelText
        {
            get
            {
                return ViewState["LabelText"] == null ? string.Empty : ViewState["LabelText"].ToString();
            }
            set
            {
                ViewState["LabelText"] = value;

                if (baseControl != null)
                    baseControl.LabelText.InnerText = value;
            }
        }


        #endregion


        #region Properties Custom By Control

        public bool AutoPostBack
        {
            get
            {
                if (ViewState["AutoPostBack"] == null)
                    ViewState["AutoPostBack"] = false;

                return (bool)ViewState["AutoPostBack"];
            }
            set
            {
                ViewState["AutoPostBack"] = value;
            }
        }

        public bool AllowSearchMultiValue { get; set; }

        public bool AllowMultiValue
        {
            get
            {
                if (ViewState["AllowMultiValue"] == null)
                    ViewState["AllowMultiValue"] = false;

                return (bool)ViewState["AllowMultiValue"];
            }
            set
            {
                ViewState["AllowMultiValue"] = value;
            }
        }

        public ComboType ComboType
        {
            get
            {
                if (ViewState["ComboType"] == null)
                    ViewState["ComboType"] = ComboType.Guid;

                return (ComboType)ViewState["ComboType"];
            }
            set
            {
                ViewState["ComboType"] = value;
            }
        }

        public bool IsDefaultValue
        {
            get
            {
                if (ViewState["IsDefaultValue"] == null)
                    ViewState["IsDefaultValue"] = false;

                return (bool)ViewState["IsDefaultValue"];
            }
            set
            {
                ViewState["IsDefaultValue"] = value;
            }
        }

        private dynamic GetValueByString(string _value)
        {
            if (string.IsNullOrEmpty(_value.Trim()))
            {
                if (this.IsDefaultValue)
                    return string.Empty;
                else
                    return null;
            }

            switch (ComboType)
            {
                case ComboType.String:
                    return _value;

                case ComboType.Integer:
                    if (!_value.IsEmpty())
                        return Convert.ToInt32(_value);
                    break;

                case ComboType.Double:
                    if (!_value.IsEmpty())
                        return Convert.ToDouble(_value);
                    break;

                case ComboType.Decimal:
                    if (!_value.IsEmpty())
                        return Convert.ToDecimal(_value);
                    break;

                case ComboType.Boolean:
                    if (!_value.IsEmpty())
                        return Convert.ToBoolean(_value);
                    break;

                case ComboType.Guid:
                    if (!_value.IsEmpty())
                        return Guid.Parse(_value.ToString());
                    else
                        return Guid.Empty;

                default:
                    return _value;

            }

            return null;
        }

        public string GetText()
        {
            if (this.AllowMultiValue || hidDisplay.Value == null)
                return string.Empty;
            else
                return hidDisplay.Value;
        }
        public dynamic GetValue()
        {
            if (this.AllowMultiValue)
                return null;

            return GetValueByString(comboValue.Value);
        }

        public List<string> GetMultiText()
        {
            if (!this.AllowMultiValue)
                return new List<string>();

            var serializer = new JavaScriptSerializer();

            if (string.IsNullOrEmpty(hidDisplay.Value))
            {
                string jsonString = serializer.Serialize(new List<JsonDropDown>());
                hidDisplay.Value = jsonString;
            }

            var obj = (List<JsonDropDown>)serializer.Deserialize(hidDisplay.Value, typeof(List<JsonDropDown>));
            var values = obj.Select(se => se.text);

            return values.ToList();
        }
        public List<T> GetMultiValue<T>()
        {
            var values = new List<T>();

            try
            {
                if (this.AllowMultiValue && !string.IsNullOrEmpty(comboValue.Value))
                {
                    values = comboValue.Value.Split(',').Select(id => (T)Convert.ChangeType(id, typeof(T))).ToList();
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return values;
        }

        public void SetMultiValue(params object[] values)
        {
            try
            {
                panelData.Value = "[]";

                if (this.AllowMultiValue && values != null && values.Count() > 0)
                {
                    var ids = values.Select(val => val.ToString()).ToList();
                    var str_ids = string.Join(",", ids);

                    var listData = GetJsonListById(ids);
                    string jsonString = new JavaScriptSerializer().Serialize(listData);

                    comboValue.Value = str_ids;
                    txtValue.Text = str_ids;
                    hidDisplay.Value = jsonString;
                }
                else
                {
                    comboValue.Value = string.Empty;
                    txtValue.Text = string.Empty;
                    hidDisplay.Value = string.Empty;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            baseControl.UpdateContent();
        }

        public void SetValue(dynamic _val)
        {
            try
            {
                string value = string.Empty;
                if (_val != null)
                {
                    value = _val.ToString();
                }

                panelData.Value = "[]";

                if (string.IsNullOrEmpty(value) || this.AllowMultiValue == true)
                {
                    comboValue.Value = string.Empty;
                    txtValue.Text = string.Empty;
                    hidDisplay.Value = string.Empty;

                    if (!string.IsNullOrEmpty(this.DisplayDefault) && !this.AllowMultiValue)
                    {
                        hidDisplay.Value = this.DisplayDefault;
                    }
                }
                else
                {
                    comboValue.Value = value;
                    txtValue.Text = value;
                    hidDisplay.Value = string.Empty;

                    var entity = this.GetJsonById(value);
                    if (entity != null)
                        hidDisplay.Value = entity.text;
                }
            }
            catch (Exception ex)
            {

            }

            baseControl.UpdateContent();
        }

        public virtual void ClearItems()
        {
            this.MethodQueryProperty = delegate () { return new List<Prototype.Providers.Property>().AsQueryable(); };
            this.BindDataSource();
        }

        #endregion


        #region Customize Binding Function

        public int CountItem
        {
            get
            {
                if (ViewState["CountItem"] == null)
                    ViewState["CountItem"] = 0;

                return (int)ViewState["CountItem"];
            }
        }

        public Action<dynamic> PostValueChanged { get; set; }

        public Func<IQueryable<Property>> MethodQueryProperty { get; set; }

        public string DisplayDefault
        {
            get
            {
                if (ViewState["DisplayDefault"] == null)
                    ViewState["DisplayDefault"] = string.Empty;

                return (string)ViewState["DisplayDefault"];
            }
            set
            {
                ViewState["DisplayDefault"] = value;
            }
        }

        public bool UseDefaultDisplay
        {
            get
            {
                if (ViewState["UseDefaultDisplay"] == null)
                    ViewState["UseDefaultDisplay"] = false;

                return (bool)ViewState["UseDefaultDisplay"];
            }
            set
            {
                ViewState["UseDefaultDisplay"] = value;
            }
        }



        public int LoadLazyLimit
        {
            get
            {
                if (ViewState["LoadLazyLimit"] == null)
                    ViewState["LoadLazyLimit"] = 20;

                return ((int)ViewState["LoadLazyLimit"]);
            }
            set
            {
                ViewState["LoadLazyLimit"] = value;
            }
        }

        protected virtual string GetJsonData(string _keyword, string _keySelected, int _limit)
        {
            if (MethodQueryProperty == null) return string.Empty;

            var query = MethodQueryProperty();

            if (string.IsNullOrEmpty(_keyword))
            {
                query = from rows in query
                        where (!string.IsNullOrEmpty(rows.display_member))
                        orderby rows.display_member
                        select rows;
            }
            else
            {
                query = from rows in query
                        where (rows.display_member.Contains(_keyword))
                        orderby rows.display_member
                        select rows;
            }



            var listValues = query.Take(_limit).AsEnumerable().Select(se => new JsonDropDown { id = se.Code, text = se.Name }).ToList();

            //if (!string.IsNullOrEmpty(this.DisplayDefault) && this.AllowMultiValue == false)
            //{

            //    listValues.Insert(0, new JsonDropDown() { id = "", text = this.DisplayDefault });
            //}

            if (listValues.Count == 1 && this.IsPrimary == true)
            {

            }
            else
            {
                listValues.Insert(0, new JsonDropDown() { id = "", text = this.DisplayDefault });
            }



            var serializer = new JavaScriptSerializer();
            string jsonString = serializer.Serialize(listValues);

            return jsonString;
        }

        protected virtual JsonDropDown GetJsonById(string _id)
        {
            if (MethodQueryProperty == null) return null;

            var query = MethodQueryProperty();

            if (ComboType == ComboType.Guid)
            {
                Guid id_gu = new Guid(_id);
                var result = from rows in query
                             where rows.guid_member == id_gu
                             select rows;
                var entity = result.AsEnumerable().Select(se => new JsonDropDown { id = se.guid_member.ToString(), text = se.display_member }).FirstOrDefault();
                return entity;
            }
            else
            {
                var entity = (from rows in query
                              where rows.value_member == _id
                              select new JsonDropDown
                              {
                                  id = rows.value_member,
                                  text = rows.display_member
                              }).FirstOrDefault();

                return entity;

            }
        }
        protected virtual List<JsonDropDown> GetJsonListById(List<string> _ids)
        {
            if (MethodQueryProperty == null) return null;

            var query = MethodQueryProperty();
            if (ComboType == ComboType.Guid)
            {
                List<Guid> listGu = _ids.Select(Guid.Parse).ToList();
                var result = from rows in query
                             where listGu.Contains(rows.guid_member)
                             select rows;
                var entity = result.AsEnumerable().Select(se => new JsonDropDown { id = se.guid_member.ToString(), text = se.display_member }).ToList();
                return entity;
            }
            else
            {
                var result = (from rows in query
                              where _ids.Contains(rows.value_member)
                              select new JsonDropDown
                              {
                                  id = rows.value_member,
                                  text = rows.display_member
                              }).ToList();

                return result;

            }
        }

        public void BindDataSource()
        {
            this.BindDataSource(this.DisplayDefault);
        }
        public void BindDataSource(string _displayDefault)
        {
            try
            {
                //if (this.DisplayDefault != _displayDefault)
                //    this.DisplayDefault = _displayDefault;

                //if (MethodQueryProperty != null && MethodQueryProperty().Count() == 1)
                //{
                //    var data = MethodQueryProperty().First();
                //    this.SetValue(data.guid_member != Guid.Empty ? data.guid_member.ToString() : data.value_member);
                //}
                //else
                //{
                //    if (this.AllowMultiValue)
                //        this.SetMultiValue(null);
                //    else
                //        this.SetValue(null);
                //}

                //if (MethodQueryProperty != null && ((this.IsPrimary && MethodQueryProperty().Count() == 1 && this.UseDefaultDisplay == false) || this.UseDefaultDisplay == false || string.IsNullOrEmpty(this.DisplayDefault)))
                if (MethodQueryProperty != null && this.UseDefaultDisplay == false && (((this.IsPrimary || this.IsKey) && MethodQueryProperty().Count() == 1 ) || string.IsNullOrEmpty(this.DisplayDefault)))
                {
                    var data = MethodQueryProperty().First();
                    if (data != null)
                    {
                        var ent = data.guid_member != Guid.Empty ? data.guid_member.ToString() : data.value_member;
                        this.DefaultValue = ent;
                        this.SetValue(ent);
                    }
                    else {
                        this.DefaultValue = null;
                    }
                }
                else
                {
                    if (this.AllowMultiValue)
                        this.SetMultiValue(null);
                    else
                        this.SetValue(null);
                }


                ViewState["CountItem"] = 1;

                baseControl.UpdateContent();
                comboValue_SelectedIndexChanged(null, EventArgs.Empty);
            }
            catch
            {

            }
        }

        protected void comboValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.AutoPostBack)
            {
                if (PostValueChanged != null)
                {
                    if (this.AllowMultiValue)
                    {
                        switch (ComboType)
                        {
                            case ComboType.Integer:
                                PostValueChanged(this.GetMultiValue<int>());
                                break;

                            case ComboType.Double:
                                PostValueChanged(this.GetMultiValue<double>());
                                break;

                            case ComboType.Decimal:
                                PostValueChanged(this.GetMultiValue<decimal>());
                                break;

                            case ComboType.Boolean:
                                PostValueChanged(this.GetMultiValue<bool>());
                                break;

                            case ComboType.Guid:
                                PostValueChanged(this.GetMultiValue<Guid>());
                                break;

                            default:
                                PostValueChanged(this.GetMultiValue<string>());
                                break;
                        }
                    }
                    else
                    {
                        PostValueChanged(this.GetValue());
                    }
                }
            }
        }

        #endregion


        #region Properties Implement Difference

        public Prototype.Providers.IFilter GetFilter()
        {
            baseControl.Filter.DataPropertyName = DataFieldValue;
            baseControl.Filter.Value = GetValue();
            return baseControl.Filter;
        }

        public InputType InputType
        {
            get { return _UControls.InputType.DropDownLazy; }
        }
        public int MaxLength
        {
            get
            {
                return 0;
            }
            set { }
        }
        public int ControlWidth
        {
            get
            {
                if (ViewState["ControlWidth"] == null)
                    ViewState["ControlWidth"] = 0;

                return (int)ViewState["ControlWidth"];
            }
            set
            {
                ViewState["ControlWidth"] = value;
            }
        }
        public short ControlIndex
        {
            get
            {
                if (ViewState["TabIndex"] == null)
                    ViewState["TabIndex"] = 0;

                return Convert.ToSByte(ViewState["TabIndex"].ToString());
            }
            set
            {
                ViewState["TabIndex"] = value;
            }
        }
        public bool Readonly
        {
            get
            {
                if (ViewState["Readonly"] == null)
                    ViewState["Readonly"] = false;

                return (bool)ViewState["Readonly"];
            }
            set
            {
                ViewState["Readonly"] = value;
            }
        }
        public bool Enabled
        {
            get
            {
                if (ViewState["Enabled"] == null)
                    ViewState["Enabled"] = true;

                return (bool)ViewState["Enabled"];
            }
            set
            {
                ViewState["Enabled"] = value;

                try
                {
                    if (!value)
                    {
                        if (this.IsPrimary)
                            IsPrimaryState = this.IsPrimary;

                        //this.IsPrimary = false;
                    }
                    else if (IsPrimaryState && !this.IsPrimary)
                    {
                        this.IsPrimary = this.IsPrimaryState;
                    }
                }
                catch { }
            }
        }

        private bool IsPrimaryState
        {
            get
            {
                if (ViewState["IsPrimaryState"] == null)
                    ViewState["IsPrimaryState"] = false;

                return (bool)ViewState["IsPrimaryState"];
            }
            set
            {
                ViewState["IsPrimaryState"] = value;
            }
        }


        public void SetObjectValue(object _value)
        {
            SetValue(_value);
        }

        public object GetObjectValue()
        {
            return GetValue();
        }

        public void Clear()
        {
            if (this.IsStaticValue) return;

            if (!string.IsNullOrEmpty(comboValue.Value))
            {
                if (this.AllowMultiValue)
                {
                    this.SetMultiValue(null);
                }
                else
                {
                    if (DefaultValue != null)
                        SetObjectValue(DefaultValue);
                    else
                    {
                        this.SetValue(null);
                    }
                }

                comboValue_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                this.SetValue(null);
            }
        }

        public dynamic DefaultValue
        {
            get
            {
                return (dynamic)ViewState["DefaultValue"];
            }
            set
            {
                ViewState["DefaultValue"] = value;
            }
        }

        //public bool VisibleExt
        //{
        //    get
        //    {
        //        if (ViewState["VisibleExt"] == null)
        //            ViewState["VisibleExt"] = true;

        //        return (bool)ViewState["VisibleExt"];
        //    }
        //    set
        //    {
        //        ViewState["VisibleExt"] = value;

        //        try
        //        {
        //            if (IsPostBack)
        //            {
        //                // panelValue.Visible = value;
        //                baseControl.Visible = value;
        //            }
        //        }
        //        catch { }
        //    }
        //}

        #endregion


        public int ControlSequence
        {
            get
            {
                if (ViewState["ControlSequence"] == null)
                    ViewState["ControlSequence"] = -1;

                return (int)ViewState["ControlSequence"];
            }
            set
            {
                ViewState["ControlSequence"] = value;
            }
        }
        public string ControlGroup
        {
            get
            {
                if (ViewState["ControlGroup"] == null)
                    ViewState["ControlGroup"] = string.Empty;

                return (string)ViewState["ControlGroup"];
            }
            set
            {
                ViewState["ControlGroup"] = value;
            }
        }

        public bool ClearByIControl
        {
            get
            {
                if (ViewState["ClearByIControl"] == null)
                    ViewState["ClearByIControl"] = false;

                return (bool)ViewState["ClearByIControl"];
            }
            set
            {
                ViewState["ClearByIControl"] = value;
            }
        }

        public string ResourceGroup
        {
            get
            {
                if (ViewState["ResourceGroup"] == null)
                    ViewState["ResourceGroup"] = string.Empty;

                return (string)ViewState["ResourceGroup"];
            }
            set
            {
                ViewState["ResourceGroup"] = value;

                if (baseControl != null)
                    baseControl.LabelText.ResourceGroup = value;
            }
        }

        public string ResourceName
        {
            get
            {
                if (ViewState["ResourceName"] == null)
                    ViewState["ResourceName"] = string.Empty;

                return (string)ViewState["ResourceName"];
            }
            set
            {
                ViewState["ResourceName"] = value;

                if (baseControl != null)
                    baseControl.LabelText.ResourceName = value;
            }
        }

        public string ResourceValue
        {
            get
            {
                if (ViewState["ResourceValue"] == null)
                    ViewState["ResourceValue"] = string.Empty;

                return (string)ViewState["ResourceValue"];
            }
            set
            {
                ViewState["ResourceValue"] = value;

                if (baseControl != null)
                    baseControl.LabelText.ResourceValue = value;
            }
        }

        public dynamic DefaultSelectedValue { get; set; }

        public void ValueChange()
        {
            comboValue_SelectedIndexChanged(null, EventArgs.Empty);
        }


        public bool TextInLine
        {
            get
            {
                if (ViewState["TextInLine"] == null)
                    ViewState["TextInLine"] = false;

                return (bool)ViewState["TextInLine"];
            }
            set
            {
                if (baseControl != null)
                    baseControl.TextInLine = value;

                ViewState["TextInLine"] = value;
            }
        }

        public string ControlId
        {
            set
            {
                ID = value;
            }
            get
            {
                return ID;
            }
        }

        public void AddAtributeBaseContent(string _key, string _value)
        {
            if (baseControl != null)
                baseControl.AddAtributeBaseContent(_key, _value);
        }

        public string BaseContentCss
        {
            get
            {
                if (baseControl != null)
                    return baseControl.BaseContentCss;
                else
                    return string.Empty;
            }
            set
            {
                if (baseControl != null)
                    baseControl.BaseContentCss = value;
            }
        }

        public bool VisibleExt
        {
            get
            {
                if (ViewState["VisibleExt"] == null)
                    ViewState["VisibleExt"] = true;

                return (bool)ViewState["VisibleExt"];
                //return base.Visible;
            }

            set
            {
                ViewState["VisibleExt"] = value;
                //base.Visible = value;

                if (baseControl != null)
                    baseControl.VisibleBaseContent = value;
            }
        }
    }

}
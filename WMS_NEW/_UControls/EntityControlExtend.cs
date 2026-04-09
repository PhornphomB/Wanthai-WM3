using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Prototype.Providers;
using Prototype.Providers.Controls;
using _UControls.EntityExtend;
using _UControls.PanelCustom;

namespace _UControls
{
    public class EntityControlExtend : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public EntityControlExtend()
        {
            IControls = new List<_IInputControl>();
            LayoutAutoGenColFix = 0;
        }


        public int LayoutAutoGenColFix { get; set; }
        public List<_IInputControl> IControls { get; set; }

        public List<EntityTab> TabsRelation { get; set; }
        public PanelTab Tab { get; set; }


        private static string[] except_common_types = new string[] { "t_wms_", "ICollection" };
        private static string[] except_common_fields = new string[] { "create_by", "create_date", "update_by", "update_date", "rowversion" };

        private const string prop_is_act_name = "is_active";
        private const string tab_custom_prefix = "panel_custom_tab_";

        private const int LayoutAutoGen_Grid = 12;
        private const int LayoutAutoGen_ColDef = 4;

        private short prop_arr_index = -1;
        private short control_index = 1;


        public void AutoCreateControlEntity(TemplateControl _pageTmpControl, ref PlaceHolder _placeHolder, object _objEntity, IEnumerable<IEntityCustom> _override_ctrls = null, IEnumerable<EntityTab> _tabs = null, bool _autoGenTabUdf = false, params string[] _exclude_fields)
        {

            var props_info = new List<EntityInfo>();

            if (_objEntity != null)
                props_info = _objEntity.GetType().GetProperties().Where(x => !except_common_fields.Contains(x.Name.ToLower())
                                                                          && !except_common_types.Any(name => x.PropertyType.Name.Contains(name)))
                                                                 .Select(se => new EntityInfo { DataFieldValue = se.Name, ValueType = se.PropertyType }).ToList();

            if (_exclude_fields != null)
                props_info = props_info.Where(x => !_exclude_fields.Contains(x.DataFieldValue)).ToList();

            if (LayoutAutoGenColFix == 0 && props_info.Count <= 5)
                LayoutAutoGenColFix = 1;
            else if (LayoutAutoGenColFix == 0 && props_info.Count <= 12)
                LayoutAutoGenColFix = 2;


            EntityInfo prop_active_new = null;
            var props_tab_custom = new List<EntityInfo>();
            var props_tab_udf = new List<EntityInfo>();

            Tab = null;
            var tabs_custom = new short[0];
            var tabs_name_used = new List<string>();

            Func<int> FuncNewTab = () =>
            {
                if (Tab == null)
                    Tab = (PanelTab)_pageTmpControl.LoadControl("~/_UControls/PanelTab.ascx");

                return 0;
            };


            var prop_active_old = props_info.FirstOrDefault(x => x.DataFieldValue.ToLower() == prop_is_act_name);
            if (prop_active_old != null)
            {
                prop_active_new = new EntityInfo { DataFieldValue = prop_active_old.DataFieldValue, ValueType = prop_active_old.ValueType };
                props_info.Remove(prop_active_old);
            }


            #region Custom Controls

            if (_override_ctrls != null)
            {
                if (_autoGenTabUdf)
                {
                    props_tab_udf.AddRange(props_info.Where(x => x.DataFieldValue.ToLower().Contains("user_def")));
                    props_info.RemoveAll(x => x.DataFieldValue.ToLower().Contains("user_def"));
                }

                foreach (var ctrl in _override_ctrls)
                {
                    if (ctrl.ControlIndex == 0)
                        ctrl.ControlIndexAuto = (short)(_override_ctrls.Where(x => x.TabIndex == ctrl.TabIndex && x.ControlIndexAuto != 999).Count() + 1);
                }

                var fields_notin_props = _override_ctrls.Where(x => !props_info.Any(any => any.DataFieldValue == x.DataFieldValue)).OrderBy(or => or.TabIndex).ThenBy(or => or.ControlIndex);
                foreach (var field in fields_notin_props)
                {
                    props_info.Add(new EntityInfo() { DataFieldValue = field.DataFieldValue, ValueType = field.ValueType });
                }

                var props_reorder = props_info.Where(x => _override_ctrls.Any(any => any.TabIndex == 0 && any.DataFieldValue == x.DataFieldValue)).ToList();
                props_tab_custom = props_info.Where(x => _override_ctrls.Any(any => any.TabIndex > 0 && any.DataFieldValue == x.DataFieldValue)).ToList();

                props_info.RemoveAll(x => _override_ctrls.Any(any => any.DataFieldValue == x.DataFieldValue));

                var reorder_ctrls = from rows in _override_ctrls
                                    where rows.TabIndex == 0
                                    orderby rows.ControlIndexAuto, rows.ControlIndex
                                    select rows;

                foreach (var rows in reorder_ctrls)
                {
                    var prop_new = props_reorder.First(x => x.DataFieldValue == rows.DataFieldValue);
                    var index = (rows.ControlIndexAuto != 999 ? rows.ControlIndexAuto : rows.ControlIndex) - 1;
                    var index_insert = index;

                    var props_exc_guid = props_info.Where(x => _override_ctrls.Any(any => any.IControl != null && any.DataFieldValue == x.DataFieldValue)
                              || (x.ValueType != typeof(Guid) && x.ValueType != typeof(Nullable<Guid>)));

                    if (props_exc_guid.Count() > index)
                    {
                        var prop_curr = props_exc_guid.ElementAt(index);
                        index_insert = props_info.IndexOf(prop_curr);
                    }

                    if (index_insert <= props_info.Count)
                        props_info.Insert(index_insert, prop_new);
                    else
                        props_info.Add(prop_new);
                }

                if (_tabs != null)
                {
                    tabs_custom = _override_ctrls.Where(x => x.TabIndex > 0).GroupBy(grb => grb.TabIndex).Select(se => se.Key).ToArray();
                    foreach (var inx in tabs_custom)
                    {
                        var attr = _tabs.FirstOrDefault(x => x.TabIndex == inx);
                        if (attr != null)
                            tabs_name_used.Add(attr.TabName);
                    }
                }

            }

            #endregion


            if (props_info.Count == 0)
                return;

            if (prop_active_new != null)
                props_info.Add(prop_active_new);

            AutoCreateForm(_pageTmpControl, props_info, _override_ctrls, props_info.Count, "panel_autogen_form", _placeHolder); // Create Panel Base 


            #region Create Panel Tab 

            #region Autogen

            //var count_form_control = (LayoutAutoGen_RowMax * LayoutAutoGen_ColMax);

            //if ((prop_arr_index + 1) < props_info.Count) // Autogen PanelTab
            //{
            //    var count_tab = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(props_info.Count - (prop_arr_index + 1)) / Convert.ToDouble(count_form_control)));

            //    tab = (PanelTab)LoadControl("~/_UControls/PanelTab.ascx");

            //    int tab_index = 1;
            //    while (tab_index <= count_tab)
            //    {
            //        var panel_id = "panel_autogen_tab_" + tab_index;
            //        var tab_name = "Data " + tab_index;

            //        if (_tabs != null)
            //        {
            //            var attr = _tabs.Where(x => !tabs_name_used.Contains(x.TabName)).OrderBy(or => or.TabIndex).FirstOrDefault();
            //            if (attr != null)
            //            {
            //                tab_name = attr.TabName;
            //                tabs_name_used.Add(attr.TabName);
            //            }
            //        }

            //        tab.AddPanel(new PanelControl(tab_name, panel_id));

            //        AutoCreateForm(props_info, _override_ctrls, (props_info.Count - (prop_arr_index + 1)), panel_id, tab.PleaceHolderTemplate); // Create Panel By Tab 

            //        tab_index++;
            //    }
            //}

            #endregion

            #region Custom

            var reorder_tabs = from rows in _override_ctrls
                               where rows.TabIndex > 0
                               group rows by new { rows.TabIndex } into grb
                               orderby grb.Key.TabIndex
                               select new
                               {
                                   grb.Key.TabIndex,
                                   Controls = grb.Select(se => new { se.ControlIndex, se.ControlIndexAuto, se.DataFieldValue })
                               };

            foreach (var tab_grb in reorder_tabs)
            {
                props_info.Clear();

                foreach (var rows in tab_grb.Controls.OrderBy(or => or.ControlIndexAuto).ThenBy(or => or.ControlIndex))
                {
                    var prop_new = props_tab_custom.First(x => x.DataFieldValue == rows.DataFieldValue);
                    props_info.Add(prop_new);
                }


                if (props_info.Count > 0)
                {
                    prop_arr_index = -1;

                    FuncNewTab();

                    var panel_id = tab_custom_prefix + tab_grb.TabIndex;
                    var tab_name = "Tab " + tab_grb.TabIndex;
                    var resource_group = string.Empty;
                    var resource_name = string.Empty;

                    if (_tabs != null)
                    {
                        var attr = _tabs.FirstOrDefault(x => x.TabIndex == tab_grb.TabIndex && x.IsAutoUserDefine == false);
                        if (attr != null)
                        {
                            tab_name = attr.TabName;
                            resource_group = attr.ResourceGroup;
                            resource_name = attr.ResourceName;
                        }
                    }

                    if (tab_grb.TabIndex <= Tab.PanelControls.Count)
                        Tab.InsertPanel(new PanelTabChild(tab_name, panel_id) { ResourceGroup = resource_group, ResourceName = resource_name }, (tab_grb.TabIndex - 1));
                    else
                        Tab.AddPanel(new PanelTabChild(tab_name, panel_id) { ResourceGroup = resource_group, ResourceName = resource_name });

                    AutoCreateForm(_pageTmpControl, props_info, _override_ctrls, (props_info.Count - (prop_arr_index + 1)), panel_id, Tab.PleaceHolderTemplate); // Create Panel By Tab 
                }
            }

            #endregion

            #region User Define

            if (props_tab_udf.Count > 0)
            {
                props_info.Clear();
                props_info.AddRange(props_tab_udf);

                prop_arr_index = -1;

                FuncNewTab();

                var panel_id = "panel_custom_tab_udf";
                var tab_name = "User Define";
                var resource_group = "general";
                var resource_name = "tab_user_define";

                if (_tabs != null)
                {
                    var attr = _tabs.FirstOrDefault(x => x.IsAutoUserDefine == true);
                    if (attr != null)
                    {
                        tab_name = attr.TabName;

                        if (!string.IsNullOrEmpty(attr.ResourceGroup) && !string.IsNullOrEmpty(attr.ResourceName))
                        {
                            resource_group = attr.ResourceGroup;
                            resource_name = attr.ResourceName;
                        }
                    }
                }

                Tab.AddPanel(new PanelTabChild(tab_name, panel_id) { ResourceGroup = resource_group, ResourceName = resource_name });

                AutoCreateForm(_pageTmpControl, props_info, _override_ctrls, (props_info.Count - (prop_arr_index + 1)), panel_id, Tab.PleaceHolderTemplate); // Create Panel By Tab 
            }

            #endregion

            #region Forms Relation

            if (_tabs != null)
            {
                var tabsRelation = _tabs.Where(x => x.IFormRelation != null);
                if (tabsRelation.Any())
                {
                    FuncNewTab();

                    TabsRelation = new List<EntityTab>();

                    foreach (var tab in tabsRelation.OrderBy(or => or.TabIndex))
                    {
                        var type = tab.IFormRelation.GetType();
                        var path = type.FullName.Replace("WMS_NEW.", "~/").Replace(".", "/").Replace(type.Name, type.Name + ".ascx");
                        var panel_id = tab_custom_prefix + tab.TabIndex;

                        var ctrl = _pageTmpControl.LoadControl(path);
                        tab.IFormRelation = (IFormRelation)ctrl;

                        Control panel = Tab.FindControl(panel_id);
                        if (panel == null)
                        {
                            if (tab.TabIndex <= Tab.PanelControls.Count)
                                Tab.InsertPanel(new PanelTabChild(tab.TabName, panel_id), (tab.TabIndex - 1));
                            else
                                Tab.AddPanel(new PanelTabChild(tab.TabName, panel_id));

                            panel = new Panel() { ID = panel_id };
                            Tab.PleaceHolderTemplate.Controls.Add(panel);
                        }

                        panel.Controls.Add(ctrl);
                        TabsRelation.Add(tab);
                    }
                }
            }

            #endregion


            if (Tab != null)
                _placeHolder.Controls.Add(Tab);

            #endregion


            #region Init Auto Binding DropDown

            var iDropDowns = IControls.Where(x => x.InputType == InputType.DropDownNormal || x.InputType == InputType.DropDownLazy).Cast<_IInputDropDown>();

            if (_pageTmpControl.Page != null && !_pageTmpControl.Page.IsPostBack && iDropDowns.Count() > 0)
            {
                var single = from ctrl in iDropDowns
                             where string.IsNullOrEmpty(ctrl.ControlGroup) && ctrl.MethodQueryProperty != null
                             select ctrl;

                foreach (var ctrl in single)
                {
                    ctrl.BindDataSource();
                }

                var parent_group = from ctrl in iDropDowns
                                   where !string.IsNullOrEmpty(ctrl.ControlGroup) && ctrl.ControlSequence > -1 && ctrl.MethodQueryProperty != null
                                   group ctrl by ctrl.ControlGroup into grb
                                   let parent_field = grb.OrderBy(mn => mn.ControlSequence).FirstOrDefault()
                                   select parent_field;

                foreach (var ctrl in parent_group)
                {
                    ctrl.BindDataSource();
                }
            }

            #endregion
        }

        private void AutoCreateForm(TemplateControl _pageTmpControl, List<EntityInfo> props, IEnumerable<IEntityCustom> _override_ctrls, int _props_count, string _panel_id, Control _template)
        {
            Panel panelRow = null;
            Panel panelCol;

            Panel panelContainer = new Panel() { ID = _panel_id, CssClass = "row" };
            panelContainer.Attributes.Add("style", "margin-top:6px;");

            _template.Controls.Add(panelContainer);

            Func<int> NewRow = () =>
            {
                panelRow = new Panel() { CssClass = "row col-lg-12" };
                panelContainer.Controls.Add(panelRow);

                return 0;
            };

            NewRow();

            var col_span_def = (LayoutAutoGenColFix > 0 ? (LayoutAutoGen_Grid / LayoutAutoGenColFix) : (LayoutAutoGen_Grid / LayoutAutoGen_ColDef));
            var col_span = col_span_def;

            bool has_new_row = false;

            while (prop_arr_index < (props.Count() - 1))
            {
                panelCol = new Panel();
                panelCol.Attributes.Add("style", "position: relative;");
                panelRow.Controls.Add(panelCol);

                has_new_row = false;
                col_span = col_span_def;

                prop_arr_index++;

                var prp = props[prop_arr_index];

                _IInputControl iCtrl = null;
                dynamic dyInput;

                if (_override_ctrls != null)
                {
                    var objCtrl = _override_ctrls.FirstOrDefault(x => x.DataFieldValue == prp.DataFieldValue);
                    if (objCtrl != null)
                    {
                        if (objCtrl.ColumnSpan > 0)
                        {
                            col_span = objCtrl.ColumnSpan;
                        }

                        has_new_row = objCtrl.EndOfLRow;

                        if (objCtrl.IControl == null)
                        {
                            objCtrl.IControl = CreateControlType(prp);

                            objCtrl.IControl.IsPrimary = objCtrl.IsPrimary;
                            objCtrl.IControl.IsKey = objCtrl.IsKey;
                            objCtrl.IControl.LabelText = objCtrl.LabelText;
                            objCtrl.IControl.ValidateMessage = objCtrl.ValidateMessage;
                        }

                        if (objCtrl.IControl != null)
                            AutoCreateControl(_pageTmpControl, ref panelCol, ref objCtrl);

                        if (objCtrl.RefGlobalVar != null)
                            objCtrl.RefGlobalVar(objCtrl.IControl);

                        iCtrl = objCtrl.IControl;
                    }
                }

                if (iCtrl == null)
                {
                    dyInput = CreateControlType(prp);
                    if (dyInput != null)
                    {
                        AutoCreateControl(_pageTmpControl, ref panelCol, ref dyInput);
                        iCtrl = (_IInputControl)dyInput;
                    }
                }

                if (iCtrl != null)
                {
                    if (string.IsNullOrEmpty(iCtrl.LabelText))
                    {
                        iCtrl.LabelText = iCtrl.DataFieldValue;
                        iCtrl.LabelText = iCtrl.LabelText.GetTextAutoCreate();
                    }

                    if (iCtrl.ControlIndex == 0)
                        iCtrl.ControlIndex = control_index;

                    IControls.Add(iCtrl);
                    control_index++;

                    if (iCtrl.InputType == InputType.Hidden)
                    {
                        panelCol.CssClass = string.Empty;
                        panelCol.Attributes.Add("style", "display:none;");
                    }
                    else
                    {
                        panelCol.CssClass = "col-sm-6 col-md-4 col-lg-" + col_span;
                    }

                    if (has_new_row)
                    {
                        NewRow();
                    }
                }

            }
        }

        private dynamic CreateControlType(EntityInfo prp)
        {
            dynamic dyInput = null;
            var value_type = prp.ValueType;

            if (prp.DataFieldValue.ToLower() == prop_is_act_name)
            {
                var iInput = new InputCheckBox() { DataFieldValue = prp.DataFieldValue, CheckBoxType = CheckBoxType.String };

                dyInput = iInput;
            }
            else if ((value_type == typeof(Guid)) || (value_type == typeof(Nullable<Guid>)))
            {
                var iInput = new InputHidden() { DataFieldValue = prp.DataFieldValue };

                dyInput = iInput;
            }
            else if ((value_type == typeof(int)) || (value_type == typeof(Nullable<int>)))
            {
                var iInput = new InputTextInteger() { DataFieldValue = prp.DataFieldValue };

                dyInput = iInput;
            }
            else if ((value_type == typeof(DateTime)) || (value_type == typeof(Nullable<DateTime>)))
            {
                var iInput = new InputTextDate() { DataFieldValue = prp.DataFieldValue, TextMode = DateTimeType.Date };

                dyInput = iInput;
            }
            else if ((value_type == typeof(double)) || (value_type == typeof(Nullable<double>)))
            {
                var iInput = new InputTextNumber() { DataFieldValue = prp.DataFieldValue, NumberType = NumberType.Double };

                dyInput = iInput;
            }
            else if ((value_type == typeof(decimal)) || (value_type == typeof(Nullable<decimal>)))
            {
                var iInput = new InputTextNumber() { DataFieldValue = prp.DataFieldValue, NumberType = NumberType.Decimal };

                dyInput = iInput;
            }
            else if (value_type == typeof(string))
            {
                var iInput = new InputTextBox() { DataFieldValue = prp.DataFieldValue };

                dyInput = iInput;
            }

            return dyInput;
        }
        private void AutoCreateControl(TemplateControl _pageTmpControl, ref Panel panelForm, ref IEntityCustom ctrlOverride)
        {
            dynamic ctrl = ctrlOverride.IControl;
            AutoCreateControl(_pageTmpControl, ref panelForm, ref ctrl);

            ctrlOverride.IControl = ctrl;
        }
        private void AutoCreateControl(TemplateControl _pageTmpControl, ref Panel panelForm, ref dynamic inputTarget)
        {
            const string pathControl = "~/_UControls/InputControls/";
            var clone_except_fields = new List<string>() { "DefaultFilter", "Filterable", "FixFilter", "IsFilter", "BaseContent" };

            var iCtrl = (_IInputControl)inputTarget;

            switch (iCtrl.InputType)
            {
                case InputType.DropDownNormal:
                    inputTarget = (InputDropDown)_pageTmpControl.LoadControl(pathControl + "InputDropDown.ascx");
                    break;
                case InputType.DropDownLazy:
                    inputTarget = (InputDropDownHD)_pageTmpControl.LoadControl(pathControl + "InputDropDownHD.ascx");
                    break;
                case InputType.Text:
                    inputTarget = (InputTextBox)_pageTmpControl.LoadControl(pathControl + "InputTextBox.ascx");
                    break;
                case InputType.TextDate:
                    inputTarget = (InputTextDate)_pageTmpControl.LoadControl(pathControl + "InputTextDate.ascx");
                    break;
                case InputType.TextInteger:
                    inputTarget = (InputTextInteger)_pageTmpControl.LoadControl(pathControl + "InputTextInteger.ascx");
                    break;
                case InputType.TextNumber:
                    inputTarget = (InputTextNumber)_pageTmpControl.LoadControl(pathControl + "InputTextNumber.ascx");
                    break;
                case InputType.CheckBox:
                    inputTarget = (InputCheckBox)_pageTmpControl.LoadControl(pathControl + "InputCheckBox.ascx");
                    break;
                case InputType.Hidden:
                    inputTarget = (InputHidden)_pageTmpControl.LoadControl(pathControl + "InputHidden.ascx");
                    break;
            }

            if (inputTarget != null)
            {
                if (inputTarget is _IInputDropDown)
                {
                    var source = (_IInputDropDown)iCtrl;
                    var target = (_IInputDropDown)inputTarget;

                    target.CloneObjectByInterface(source, false, clone_except_fields);
                }
                else if (inputTarget is _IInputTextDate)
                {
                    var source = (_IInputTextDate)iCtrl;
                    var target = (_IInputTextDate)inputTarget;

                    target.CloneObjectByInterface(source, false, clone_except_fields);
                }
                else if (inputTarget is _IInputCheckBox)
                {
                    var source = (_IInputCheckBox)iCtrl;
                    var target = (_IInputCheckBox)inputTarget;

                    target.CloneObjectByInterface(source, false, clone_except_fields);

                    if (target.DataFieldValue == prop_is_act_name)
                        target.DefaultValue = "YES";
                }
                else
                {
                    var source = iCtrl;
                    var target = (_IInputControl)inputTarget;

                    target.CloneObjectByInterface(source, false, clone_except_fields);
                }

                inputTarget.ID = "ctrlAutoGen_" + iCtrl.DataFieldValue;
                panelForm.Controls.Add(inputTarget);
            }
        }
    }

    public static class EntityControlFunc
    {
        public static List<IEntityCustom> GetEntitiesByContent(this EntityCustomContent _content, IEnumerable<EntityRef> _refIControls = null)
        {
            var list = new List<IEntityCustom>();

            EntityCustom entity = null;

            foreach (var iField in _content.IControls)
            {
                entity = new EntityCustom();
                var iImport = (_IFieldImportant)entity;
                var iLayout = (IEntityCustomLayout)entity;

                iImport.CloneObjectByInterface(iField, false);
                iLayout.CloneObjectByInterface((IEntityCustomLayout)iField, false);

                if (iField is _IInputControl)
                {
                    entity.IControl = (_IInputControl)iField;
                }

                if (_refIControls != null)
                {
                    var refICtrl = _refIControls.FirstOrDefault(x => x.DataFieldValue == iField.DataFieldValue);
                    if (refICtrl != null)
                    {
                        entity.RefGlobalVar = (REF) => { if (refICtrl.RefGlobalVar != null) refICtrl.RefGlobalVar(REF); };
                    }
                }

                list.Add(entity);
            }

            _content.ClearIControls();

            return list;
        }
    }


    #region Interface IEntity

    public interface IPopupEntity
    {
        event SaveEntityHandle RaiseEntitySaved;

        FormState FormState { get; set; }
        object KeyFieldValue { get; set; }
        bool IsSaveSuccess { get; set; }
        void New();
        void Edit(object _keyId);
        void Save();

        void ShowDialog();
        void HideDialog();
        void UpdateContent();
        void UpdateCommand();
    }

    public interface IFormRelation
    {
        Action<dynamic> UpdateParent { get; set; }
        void InitForm(dynamic _obj);
        bool Visible { get; set; }
    }

    public interface IEntityCustomLayout
    {
        short TabIndex { get; set; }
        short ColumnSpan { get; set; }
        bool EndOfLRow { get; set; }
    }

    public interface IEntityCustom : IEntityCustomLayout, _IFieldImportant
    {
        Action<dynamic> RefGlobalVar { get; set; }
        _IInputControl IControl { get; set; }
        Type ValueType { get; set; }
        short ControlIndexAuto { get; set; }
    }

    #endregion


    #region Class Entity

    public class EntityCustomLayout : UserControl, IEntityCustomLayout
    {
        public short ColumnSpan { get; set; }
        public bool EndOfLRow { get; set; }
        public short TabIndex { get; set; }
    }

    public class EntityCustom : EntityCustomLayout, IEntityCustom
    {
        public EntityCustom()
        {
            ValueType = typeof(string);
            ControlIndexAuto = 999;
        }

        public EntityCustom(_IInputControl _control)
        {
            var iImport = (_IFieldImportant)this;
            var iLayout = (IEntityCustomLayout)this;

            iImport.CloneObjectByInterface(_control, false);
            iLayout.CloneObjectByInterface((IEntityCustomLayout)_control, false);

            IControl = _control;
            ValueType = typeof(string);
            ControlIndexAuto = 999;

            //DataFieldValue = _control.DataFieldValue;
            //LabelText = _control.LabelText;
            //ControlIndex = _control.ControlIndex;
        }

        public Action<dynamic> RefGlobalVar { get; set; }
        public _IInputControl IControl { get; set; }
        public Type ValueType { get; set; }
        public short ControlIndexAuto { get; set; }


        #region _IFieldImportant

        public bool IsPrimary { get; set; }
        public bool IsKey { get; set; }
        public string LabelText { get; set; }
        public string ValidateMessage { get; set; }
        public short ControlIndex { get; set; }

        public string DataFieldValue { get; set; }


        #region IResourceInput

        public string ResourceGroup { get; set; }

        public string ResourceName { get; set; }

        public string ResourceValue { get; set; }

        #endregion

        #endregion

    }

    public class EntityTab : ConfigGlobal.Interface.IResource
    {
        public short TabIndex { get; set; }
        public string TabName { get; set; }

        public IFormRelation IFormRelation { get; set; }


        public bool IsAutoUserDefine { get; set; }

        #region IResource

        public string ResourceGroup { get; set; }
        public string ResourceName { get; set; }
        public string ResourceValue { get; set; }

        #endregion
    }

    public class EntityInfo
    {
        public string DataFieldValue { get; set; }
        public Type ValueType { get; set; }
    }

    public class EntityRef
    {
        public EntityRef(_IInputControl _control, Action<dynamic> _refIControl)
        {
            DataFieldValue = _control.DataFieldValue;
            RefGlobalVar = _refIControl;
        }

        public string DataFieldValue { get; set; }
        public Action<dynamic> RefGlobalVar { get; set; }
    }

    #endregion
}
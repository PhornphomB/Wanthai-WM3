using _UControls.PanelCustom;
using ConfigGlobal.Interface;
using Prototype.Providers;
using Prototype.Providers.Controls;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web.UI;

namespace _UControls
{
    public delegate bool ValidateEntityHandle();
    public delegate void EventEntityObjectHandle();
    public delegate void SaveEntityHandle(bool _saveStatus);

    public enum FormState
    {
        New,
        Edit
    }


    public partial class PanelPopupEntity : UControlCustom, IPopupEntity
    {
        #region Template

        private ITemplate _template1 = null;

        [TemplateContainer(typeof(TemplateControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate ControlTemplate
        {
            get
            {
                return _template1;
            }
            set
            {
                _template1 = value;
            }
        }

        private ITemplate _template2 = null;

        [TemplateContainer(typeof(TemplateControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate CommandTemplate
        {
            get
            {
                return _template2;
            }
            set
            {
                _template2 = value;
            }
        }

        #endregion


        List<Resource> _resourceGeneral = new List<Resource>();

        void Page_Init()
        {
            if (_template1 != null)
            {
                _template1.InstantiateIn(PlaceHolderControl);
            }
            if (_template2 != null)
            {
                _template2.InstantiateIn(PlaceCustomCommand);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PanelPopup1.CloseClick += PanelPopup1_CloseClick;

                if (!Page.IsPostBack)
                {
                    SetValidateGroupInput();

                    if (_resourceGeneral.Count == 0)
                    {
                        _resourceGeneral = (Session["Resource_General"] as List<ConfigGlobal.Interface.Resource>);
                    }

                    #region Initial Resource Language

                    //Save
                    var res_Save = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "Save");
                    if (res_Save != null) btSave.Text = res_Save.ResourceValue;

                    //Clear
                    var res_Clear = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "Clear");
                    if (res_Clear != null) btClear.Text = res_Clear.ResourceValue;

                    #endregion

                    btSave.Enabled = EnableSave;
                    btClear.Enabled = EnableClear;

                    spanSave.Visible = VisibleSave;
                    spanClear.Visible = VisibleClear;
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void PanelPopup1_CloseClick(object sender, EventArgs e)
        {
            if (CloseClick != null)
                CloseClick(sender, e);
        }

        #region Object DataAccess

        public dynamic ObjectDataAccess { get; set; }
        //public object ObjectEntitySave { get; set; }
        //private object ObjectEntityEdit { get; set; }

        #endregion

        public object KeyFieldValue
        {
            get
            {
                return (object)ViewState["KeyID"];
            }
            set
            {
                ViewState["KeyID"] = value;
            }
        }

        //public string KeyFieldName { get; set; }

        public List<string> ContainerInputID
        {
            get
            {
                if (ViewState["ContainerInputID"] == null)
                    ViewState["ContainerInputID"] = new string[0];

                return new List<string>((string[])ViewState["ContainerInputID"]);
            }
            set
            {
                ViewState["ContainerInputID"] = value.ToArray();
            }
        }

        public bool AutoClear
        {
            get
            {
                if (ViewState["AutoClear"] == null)
                    ViewState["AutoClear"] = true;

                return (bool)ViewState["AutoClear"];
            }
            set
            {
                ViewState["AutoClear"] = value;
            }
        }

        void SetValidateGroupInput()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "_");
            btSave.ValidationGroup = guid;

            foreach (var control in GetInputControls().Where(wh => string.IsNullOrEmpty(wh.ValidateGroup)))
            {
                control.ValidateGroup = guid;
            }

            this.ValidateGroupId = guid;
        }

        public string ValidateGroupId
        {
            get
            {
                return ViewState["ValidateGroupId"].ToString();
            }
            set
            {
                ViewState["ValidateGroupId"] = value;
            }
        }

        List<_UControls._IInputControl> iControlList = null;
        IEnumerable<_IInputControl> GetInputControls()
        {
            if (iControlList != null)
                return EntityExt.IControls.Union(iControlList);

            iControlList = new List<_UControls._IInputControl>();

            if (ContainerInputID.Count > 0)
            {
                try
                {
                    foreach (var id in ContainerInputID)
                    {
                        var panel = PlaceHolderControl.FindControlDeep(id);

                        if (panel != null)
                            panel.Controls.FindControlsDeepByType<_UControls._IInputControl>(ref iControlList);
                    }
                }
                catch (Exception ex)
                {
                    Logging = new Prototype.Providers.Logging(this, ex);
                    RaiseLogging();
                }
            }

            return EntityExt.IControls.Union(iControlList);
        }

        #region Optional Propertise

        public event System.EventHandler CloseClick;

        string ClassAccessName
        {
            get
            {
                if (ViewState["ClassAccessName"] == null)
                    ViewState["ClassAccessName"] = string.Empty;

                return ViewState["ClassAccessName"].ToString();
            }
            set
            {
                ViewState["ClassAccessName"] = value;
            }
        }

        public string HeaderText
        {
            get
            {
                return PanelPopup1.HeaderText;
            }
            set
            {
                PanelPopup1.HeaderText = value;
            }
        }
        //public string HeaderCssStyle
        //{
        //    set { PanelPopup1.HeaderCssStyle = value; }
        //}
        //public string CssClass
        //{
        //    set
        //    {
        //        PanelPopup1.CssClass = value;
        //    }
        //}

        public void UpdateContent()
        {
            PanelPopup1.UpdateContent();
        }
        public void UpdateCommand()
        {
            PanelPopup1.UpdateCommand();
        }
        public void ShowDialog()
        {
            //if (!PanelPopup1.IsShowDialog)
            PanelPopup1.ShowDialog();
            //else
            //    PanelPopup1.UpdateContent();
        }
        public void HideDialog()
        {
            PanelPopup1.HideDialog();
        }

        public bool ShowDialogNow
        {
            get
            {
                return PanelPopup1.ShowDialogNow;
            }
            set
            {
                PanelPopup1.ShowDialogNow = value;
            }
        }
        public bool IsShowDialog
        {
            get
            {
                return PanelPopup1.IsShowDialog;
            }
            set
            {
                PanelPopup1.IsShowDialog = value;
            }
        }

        public bool CloseDialogAfterSave
        {
            get
            {
                if (ViewState["CloseDialogAfterSave"] == null)
                    ViewState["CloseDialogAfterSave"] = false;

                return (bool)ViewState["CloseDialogAfterSave"];
            }
            set
            {
                ViewState["CloseDialogAfterSave"] = value;
            }
        }

        public bool EnableSave
        {
            get
            {
                if (ViewState["EnableSave"] == null)
                    ViewState["EnableSave"] = true;

                return (bool)ViewState["EnableSave"];
            }
            set
            {
                ViewState["EnableSave"] = value;

                try
                {
                    if (IsPostBack)
                        btSave.Enabled = value;
                }
                catch { }
            }
        }
        public bool EnableClear
        {
            get
            {
                if (ViewState["EnableClear"] == null)
                    ViewState["EnableClear"] = true;

                return (bool)ViewState["EnableClear"];
            }
            set
            {
                ViewState["EnableClear"] = value;

                try
                {
                    if (IsPostBack)
                        btClear.Enabled = value;
                }
                catch { }
            }
        }

        public bool VisibleSave
        {
            get
            {
                if (ViewState["VisibleSave"] == null)
                    ViewState["VisibleSave"] = true;

                return (bool)ViewState["VisibleSave"];
            }
            set
            {
                ViewState["VisibleSave"] = value;

                try
                {
                    if (IsPostBack)
                        spanSave.Visible = value;
                }
                catch { }
            }
        }
        public bool VisibleClear
        {
            get
            {
                if (ViewState["VisibleClear"] == null)
                    ViewState["VisibleClear"] = true;

                return (bool)ViewState["VisibleClear"];
            }
            set
            {
                ViewState["VisibleClear"] = value;

                try
                {
                    if (IsPostBack)
                        spanClear.Visible = value;
                }
                catch { }
            }
        }

        #endregion

        #region Command Button

        protected void btSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            this.New();
        }

        #endregion

        #region Form Method

        public event EventEntityObjectHandle InitObjectsEvent;
        public FormState FormState
        {
            get
            {
                if (ViewState["FormState"] == null)
                    ViewState["FormState"] = FormState.New;

                return (FormState)ViewState["FormState"];
            }
            set
            {
                ViewState["FormState"] = value;
            }
        }

        private void InitObjects()
        {
            try
            {
                if (InitObjectsEvent != null)
                {
                    InitObjectsEvent();
                    PlugEventResult(ObjectDataAccess);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void InitForm()
        {
            #region Resource Content

            if (_resourceGeneral.Count == 0 && Session["Resource_General"] != null)
            {
                _resourceGeneral = (Session["Resource_General"] as List<ConfigGlobal.Interface.Resource>);
            }


            ClassAccessName = (ObjectDataAccess.GetType().Name as string).GetTextAutoCreate();

            switch (FormState)
            {
                case global::_UControls.FormState.New:

                    //New
                    if (string.IsNullOrEmpty(PanelPopup1.HeaderText) || PanelPopup1.HeaderText.Contains(ClassAccessName))
                    {
                        var res_New = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "NewData");
                        if (res_New != null) PanelPopup1.HeaderText = res_New.ResourceValue;
                        else PanelPopup1.HeaderText = "New " + ClassAccessName;
                    }

                    VisibleClear = true;
                    break;

                case global::_UControls.FormState.Edit:

                    //Edit
                    if (string.IsNullOrEmpty(PanelPopup1.HeaderText) || PanelPopup1.HeaderText.Contains(ClassAccessName))
                    {
                        var res_Edit = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "EditData");
                        if (res_Edit != null) PanelPopup1.HeaderText = res_Edit.ResourceValue;
                        else PanelPopup1.HeaderText = "Edit " + ClassAccessName;
                    }

                    VisibleClear = false;
                    break;
            }

            #endregion

            var _iControls = GetInputControls().OrderBy(or => or.ControlGroup).ThenBy(or => or.ControlSequence);

            foreach (_UControls._IInputControl control in _iControls)
            {
                PermissionControl(control);
            }
        }
        private void PermissionControl(_UControls._IInputControl control)
        {
            if (control == null || control.IsStaticValue) return;

            switch (FormState)
            {
                case global::_UControls.FormState.New:
                    if (!string.IsNullOrEmpty(control.DataFieldValue) || control.ClearByIControl)
                    {
                        control.Clear();

                        if (control.IsKey)
                            control.Enabled = true;
                        //control.Readonly = false;
                    }
                    break;

                case global::_UControls.FormState.Edit:
                    if ((control.IsKey) || (string.IsNullOrEmpty(control.DataFieldValue)))
                        control.Enabled = false;
                    //control.Readonly = true;
                    break;
            }
        }

        private bool GetControlValue()
        {
            try
            {
                foreach (_UControls._IInputControl control in GetInputControls().Where(wh => !string.IsNullOrEmpty(wh.DataFieldValue)))
                {

                    if (control.DataFieldValue == "expiry_date")
                    {
                        continue;
                    }

                    switch (FormState)
                    {
                        case FormState.New:
                            Prototype.Providers.ContextExtension.SetPropertyValue(ObjectDataAccess.Entity, control.DataFieldValue, control.GetObjectValue());
                            break;
                        case FormState.Edit:
                            if (!control.IsKey)
                                Prototype.Providers.ContextExtension.SetPropertyValue(ObjectDataAccess.Entity, control.DataFieldValue, control.GetObjectValue());
                            break;
                    }

                }


                dynamic dynObj = ObjectDataAccess.Entity;

                try
                {
                    switch (FormState)
                    {
                        case FormState.New:
                            dynObj.create_by = _SessionVals.UserName;
                            dynObj.create_date = DateTime.Now;
                            break;
                        case FormState.Edit:
                            dynObj.update_by = _SessionVals.UserName;
                            dynObj.update_date = DateTime.Now;
                            break;
                    }
                }
                catch { }

                ObjectDataAccess.Entity = dynObj;

                return true;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();

                return false;
            }
        }
        private bool SetControlValue()
        {
            try
            {
                var _iControls = GetInputControls()
                       .Where(wh => !string.IsNullOrEmpty(wh.DataFieldValue) || !string.IsNullOrEmpty(wh.DataFieldShowValue))
                       .OrderBy(or => or.ControlGroup).ThenBy(or => or.ControlSequence);

                // First cycle set edit value 
                foreach (_IInputControl control in _iControls)
                {
                    if (control.DataFieldValue == "expiry_date")
                    {
                        continue;
                    }

                    if (control.IsStaticValue) continue;

                    var fieldName = control.DataFieldValue;

                    if (!string.IsNullOrEmpty(control.DataFieldShowValue))
                        fieldName = control.DataFieldShowValue;

                    object obj = Prototype.Providers.ContextExtension.GetPropertyValue(ObjectDataAccess.Entity, fieldName);
                    control.SetObjectValue(obj);

                    if (control is _IInputPostBack)
                    {
                        var iPostBack = (_IInputPostBack)control;
                        if (iPostBack.AutoPostBack)
                        {
                            iPostBack.ValueChange();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();

                return false;
            }
        }

        #endregion

        #region Implement Interface ICommand

        public event ValidateEntityHandle ValidateEntityEvent;
        public event EventEntityObjectHandle AfterNewDataEvent;
        public event EventEntityObjectHandle AfterSetEditDataEvent;
        public event EventEntityObjectHandle PreSaveEntityEvent;
        public event SaveEntityHandle RaiseEntitySaved;

        public bool ValidateData()
        {
            foreach (var control in GetInputControls())
            {
                if (control.IsPrimary && !string.IsNullOrEmpty(control.DataFieldValue)) // Is Validate
                {
                    if (control.GetObjectValue() == null) // Validate value not empty.
                    {
                        if (!string.IsNullOrEmpty(control.ValidateMessage))
                            Page.MessageWarning(control.ValidateMessage);
                        else
                            Page.MessageWarning("! Please Input [" + control.DataFieldValue.Split('_').Aggregate((x, y) => x + " " + y.UpperFirst()).UpperFirst() + "]");

                        return false;
                    }
                }
            }

            if (ValidateEntityEvent != null)
            {
                return ValidateEntityEvent();
            }

            return true;
        }

        public void New()
        {
            try
            {
                FormState = global::_UControls.FormState.New;
                KeyFieldValue = null;

                InitObjects();
                InitForm();

                if (EntityExt.TabsRelation != null)
                {
                    foreach (var tab in EntityExt.TabsRelation)
                    {
                        tab.IFormRelation.Visible = false;

                        EntityExt.Tab.VisiblePanel(tab.TabIndex, false);
                    }
                }

                if (EntityExt.Tab != null)
                    EntityExt.Tab.ChangeActivePanel(1);

                if (AfterNewDataEvent != null)
                    AfterNewDataEvent();

                this.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void Edit(object _keyId)
        {
            try
            {
                FormState = FormState.Edit;
                KeyFieldValue = _keyId;

                InitObjects();
                ObjectDataAccess.GetEditKeyID(KeyFieldValue);
                SetControlValue();
                InitForm();


                bool IsOpenFirstTab = false;

                if (EntityExt.TabsRelation != null)
                {
                    foreach (var tab in EntityExt.TabsRelation)
                    {
                        tab.IFormRelation.InitForm(ObjectDataAccess.Entity);
                        tab.IFormRelation.Visible = true;

                        EntityExt.Tab.VisiblePanel(tab.TabIndex, true);

                        if (IsOpenFirstTab == false)
                        {
                            EntityExt.Tab.ChangeActivePanel(tab.TabIndex);
                            IsOpenFirstTab = true;
                        }
                    }
                }

                if (AfterSetEditDataEvent != null)
                    AfterSetEditDataEvent();

                this.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void Save()
        {
            try
            {
                if (!ValidateData())
                    return;

                InitObjects();

                IsSaveSuccess = false;
                switch (FormState)
                {
                    case FormState.New:

                        if (!GetControlValue()) return;

                        if (PreSaveEntityEvent != null)
                            PreSaveEntityEvent();

                        IsSaveSuccess = ObjectDataAccess.Save();
                        break;
                    case FormState.Edit:

                        ObjectDataAccess.GetByKeyID(KeyFieldValue);

                        if (!GetControlValue()) return;

                        if (PreSaveEntityEvent != null)
                            PreSaveEntityEvent();

                        IsSaveSuccess = ObjectDataAccess.Update();
                        break;
                }

                if (IsSaveSuccess == true)
                {
                    if (CloseDialogAfterSave)
                    {
                        this.HideDialog();
                    }
                    else
                    {
                        switch (FormState)
                        {
                            case FormState.New:

                                if (EntityExt.TabsRelation != null && EntityExt.TabsRelation.Count > 0)
                                {
                                    var keys = (EdmProperty[])ContextExtension.GetEntityKeys(ObjectDataAccess.GridObjContext, ObjectDataAccess.Entity.GetType());
                                    foreach (var key in keys)
                                    {
                                        this.KeyFieldValue = Prototype.Providers.ContextExtension.GetPropertyValue(ObjectDataAccess.Entity, key.Name);
                                        break;
                                    }

                                    this.Edit(this.KeyFieldValue);
                                }
                                else
                                {
                                    if (this.AutoClear)
                                    {
                                        this.New();
                                    }
                                }

                                break;
                            case FormState.Edit:

                                break;
                        }

                    }
                }

                if (RaiseEntitySaved != null)
                    RaiseEntitySaved(IsSaveSuccess);

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public bool IsSaveSuccess { get; set; }

        #endregion


        #region Entity Control

        private bool IsGetEntityMember
        {
            get
            {
                if (ViewState["IsGetEntityMember"] == null)
                    ViewState["IsGetEntityMember"] = false;

                return (bool)ViewState["IsGetEntityMember"];
            }
            set
            {
                ViewState["IsGetEntityMember"] = value;
            }
        }

        private EntityControlExtend EntityExt = new EntityControlExtend();

        public List<_IInputControl> IControls
        {
            get
            {
                return EntityExt.IControls;
            }
            set
            {
                EntityExt.IControls = value;
            }
        }

        public List<EntityTab> TabsRelation
        {
            get
            {
                return EntityExt.TabsRelation;
            }
            set
            {
                EntityExt.TabsRelation = value;
            }
        }

        public PanelTab Tab
        {
            get
            {
                return EntityExt.Tab;
            }
        }


        private const string col_class = "col-sm-6 col-md-4 col-lg-";
        private const int LayoutAutoGen_Grid = 12;
        private const short ColumnControlDef = 4;

        public short ColumnControlFix { get; set; }

        public void InitControlStatic()
        {
            SetAttributeIControls(PlaceHolderControl.Controls);

            var tab = PlaceHolderControl.Controls.OfType<PanelTab>().FirstOrDefault();
            if (tab != null)
            {
                EntityExt.Tab = tab;

                if (TabsRelation == null)
                    TabsRelation = new List<EntityTab>();

                short tab_index = 1;
                foreach (var panel in tab.PleaceHolderTemplate.Controls.OfType<PanelControlTab>())
                {
                    //if (!tab.PanelControls.Any(x => x.ControlID == panel.ControlID))
                    //    tab.AddPanel(panel);

                    SetAttributeIControls(panel.Controls);

                    foreach (var iform in panel.Controls.OfType<IFormRelation>())
                    {
                        TabsRelation.Add(new EntityTab()
                        {
                            TabName = panel.PanelName,
                            IFormRelation = iform,
                            TabIndex = tab_index
                        });
                    }

                    tab_index++;
                }
            }

            AutoSetAttrEntity();

            if ((ColumnControlFix == 0) || (ColumnControlFix >= ColumnControlDef))
                PanelPopup1.StyleSize = _StyleSize.Large;
            else if (ColumnControlFix == 3)
                PanelPopup1.StyleSize = _StyleSize.Default;
            else
                PanelPopup1.StyleSize = _StyleSize.Small;
        }

        private void SetAttributeIControls(ControlCollection _controls)
        {
            short col_width = (short)(LayoutAutoGen_Grid / (ColumnControlFix > 0 ? ColumnControlFix : ColumnControlDef));

            if (IControls == null)
                IControls = new List<_IInputControl>();

            var rows = _controls.OfType<PanelControlRow>();
            foreach (var row in rows)
            {
                var iControls = new List<_IInputControl>();
                row.Controls.FindControlsDeepByType(ref iControls);

                foreach (var iCtrl in iControls)
                {
                    if (!IControls.Any(x => x.ControlId == iCtrl.ControlId))
                        IControls.Add(iCtrl);

                    if (!Page.IsPostBack && iCtrl.DataFieldValue.ToLower() == "is_active")
                    {
                        iCtrl.DefaultValue = "YES";
                    }

                    if (!Page.IsPostBack && (iCtrl.InputType == InputType.DropDownNormal || iCtrl.InputType == InputType.DropDownLazy))
                    {
                        var iDropdown = (_IInputDropDown)iCtrl;
                        iDropdown.BindDataSource();
                    }

                    if (iCtrl.InputType != InputType.Hidden && string.IsNullOrEmpty(iCtrl.BaseContentCss))
                    {
                        var col_span = (iCtrl as IEntityCustomLayout).ColumnSpan;
                        short col_width_custom = 0;
                        if (col_span > 0)
                            col_width_custom = col_span;
                        else
                            col_width_custom = col_width;

                        iCtrl.BaseContentCss = col_class + col_width_custom;
                    }
                }

                if (string.IsNullOrEmpty(row.CssClass))
                    row.CssClass = "row " + "col-lg-" + LayoutAutoGen_Grid;
            }
        }

        public void AutoSetAttrEntity()
        {
            if (!IsGetEntityMember)
            {
                if (ObjectDataAccess == null)
                    InitObjects();

                try
                {
                    var props = (EdmProperty[])ContextExtension.GetEntityAttrs(ObjectDataAccess.GridObjContext, ObjectDataAccess.Entity.GetType());
                    foreach (var prp in props)
                    {
                        var iCtrl = EntityExt.IControls.FirstOrDefault(x => x.DataFieldValue == prp.Name);
                        if (iCtrl != null && iCtrl.InputType != InputType.CheckBox && iCtrl.InputType != InputType.Hidden)
                        {
                            if (prp.Nullable == false)
                                iCtrl.IsPrimary = !prp.Nullable;

                            if (prp.MaxLength != null)
                                iCtrl.MaxLength = prp.MaxLength.Value;
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                IsGetEntityMember = true;
            }
        }

        public void AutoCreateControlEntity<TEntity>(TEntity _entity, IEnumerable<IEntityCustom> _override_ctrls = null, IEnumerable<EntityTab> _tabs = null, params string[] _exclude_fields) where TEntity : class
        {
            EntityExt.AutoCreateControlEntity(this.TemplateControl, ref PlaceHolderControl, _entity, _override_ctrls, _tabs, true, _exclude_fields);

            AutoSetAttrEntity();

            if (EntityExt.LayoutAutoGenColFix == 1)
                PanelPopup1.StyleSize = _StyleSize.Small;
            else if (EntityExt.LayoutAutoGenColFix == 2)
                PanelPopup1.StyleSize = _StyleSize.Default;
            else
                PanelPopup1.StyleSize = _StyleSize.Large;

            EntityExt.Dispose();
        }

        #endregion
    }
}
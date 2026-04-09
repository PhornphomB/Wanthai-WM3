using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Authen
{
    public partial class Menu : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binding Event

                ddPlatform.PostValueChanged += ddPlatform_PostValueChanged;
                ddParentName.PostValueChanged += ddParentName_PostValueChanged;

                #endregion

                #region Initial Peoperty Column Grid

                GridColumnExt4.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("platform"); };

                #endregion

                #region Initial Input Data

                ddPlatform.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("platform"); };
                ddParentName.MethodQueryProperty = delegate () { return SecurityM.Access.PropertyCollection.Menu.Instance.GetQueryByPlatform(hidApplicationID.GetValue(), ddPlatform.GetValue()); };

                #endregion

                if (!Page.IsPostBack)
                {
                    hidApplicationID.DefaultValue = _SessionVals.AppID;

                    hid_is_admin.SetValue(_SessionVals.IsAdmin);
                    hid_app_id.SetValue(_SessionVals.AppID);
                }

                popup1.InitObjectsEvent += () => { popup1.ObjectDataAccess = new SecurityM.Access.Master.Menu(); };
                popup1.InitControlStatic();

                GridExt1.PopupEntitySource = popup1;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void ddPlatform_PostValueChanged(dynamic _value)
        {
            try
            {
                ddParentName.BindDataSource();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        void ddParentName_PostValueChanged(dynamic _value)
        {
            try
            {
                if (popup1.FormState == _UControls.FormState.Edit)
                    return;

                var menu_parent_id = (string)_value;
                int? group_inx_last;
                int menu_inx_last = 0;

                using (var _model = new SecurityM.Source.SecurityM_Entities())
                {
                    if (string.IsNullOrEmpty(menu_parent_id))
                    {
                        group_inx_last = _model.t_com_menu.Max(mx => mx.menu_group_sequence);

                        if (group_inx_last == null)
                            group_inx_last = 0;

                        group_inx_last++;
                    }
                    else
                    {
                        group_inx_last = _model.t_com_menu.Where(x => x.menu_id == menu_parent_id).Max(mx => mx.menu_group_sequence).Value;

                        var qry_parents = _model.t_com_menu.Where(x => x.parent_menu_id == menu_parent_id);

                        if (qry_parents.Any())
                            menu_inx_last = qry_parents.Max(mx => mx.menu_sequence);
                    }
                }

                menu_inx_last++;

                txtMenuGroupSeq.SetValue(group_inx_last);
                txtMenuGroupSeq.Update();

                txtMenuSeq.SetValue(menu_inx_last);
                txtMenuSeq.Update();

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}
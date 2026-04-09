using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prototype.Providers.Controls
{
    public static class ControlsList
    {
        #region Web Form

        public static void BindListBox(ref DropDownList ListBox, List<Property> SourceList)
        {
            try
            {
                BindListBox(ref ListBox, SourceList, "-- Choose Item --");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void BindListBox(ref DropDownList ListBox, List<Property> SourceList, string DisplayDefault)
        {
            try
            {
                BindListBox(ref ListBox, SourceList, DisplayDefault, string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void BindListBoxNoneDefault(ref DropDownList ListBox, List<Property> SourceList)
        {
            try
            {
                ListBox.DataTextField = Property.KeyName;
                ListBox.DataValueField = Property.KeyCode;
                ListBox.DataSource = SourceList;
                ListBox.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void BindListBox(ref DropDownList ListBox, List<Property> SourceList, string DisplayDefault, string ValueDefault)
        {
            try
            {
                if (!string.IsNullOrEmpty(DisplayDefault))
                {
                    Property prop = new Property();
                    prop.Code = ValueDefault;
                    prop.Name = DisplayDefault;
                    SourceList.Insert(0, prop);
                }
               

                ListBox.DataTextField = Property.KeyName;
                ListBox.DataValueField = Property.KeyCode;
                ListBox.DataSource = SourceList;
                ListBox.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
    }

}

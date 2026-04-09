using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Prototype.Providers.Controls
{
    public static class ControlWebExtension
    {
        #region Find Control

        public static T FindControlDeepByType<T>(this ControlCollection controls) where T : class
        {
            T found = default(T);

            if (controls != null && controls.Count > 0)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (found != null) break;
                    if (controls[i] is T)
                    {
                        found = controls[i] as T;
                        break;
                    }
                    found = FindControlDeepByType<T>(controls[i].Controls);
                }
            }

            return found;
        }

        public static void FindControlsDeepByType<T>(this ControlCollection Controls, ref List<T> listControls) where T : class
        {
            if (Controls == null || Controls.Count == 0)
                return;

            foreach (Control control in Controls)
            {
                if (control is T)
                    listControls.Add(control as T);

                FindControlsDeepByType<T>(control.Controls, ref listControls);
            }
        }

        public static Control FindControlDeep(this Control parentControl, string id)
        {
            if (parentControl.ID == id)
            {
                return parentControl;
            }

            foreach (Control c in parentControl.Controls)
            {
                Control t = FindControlDeep(c, id);
                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }
        public static T FindControlDeep<T>(this Control parentControl, string id) where T : Control
        {
            T ctrl = default(T);

            if ((parentControl is T) && (parentControl.ID == id))
                return (T)parentControl;

            foreach (Control c in parentControl.Controls)
            {
                ctrl = c.FindControlDeep<T>(id);

                if (ctrl != null)
                    break;
            }
            return ctrl;
        }

        #endregion

    }
}

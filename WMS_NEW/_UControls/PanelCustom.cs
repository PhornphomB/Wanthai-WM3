using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using ConfigGlobal.Interface;

namespace _UControls.PanelCustom
{
    public interface IPanelControlTab : IResource
    {
        string PanelName { get; set; }
        int? Index { get; set; }
        string ControlID { get; set; }
        bool IsActive { get; set; }
    }


    public class PanelControlTab : Panel, IPanelControlTab
    {

        private string _controlId = string.Empty;
        public string ControlID
        {
            get
            {
                if (string.IsNullOrEmpty(_controlId))
                    return this.ID;
                else
                    return _controlId;
            }
            set
            {
                _controlId = value;
            }
        }
        public string PanelName { get; set; }
        public int? Index { get; set; }
        public bool IsActive { get; set; }


        #region Interface Resource

        public string ResourceGroup { get; set; }
        public string ResourceName { get; set; }
        public string ResourceValue { get; set; }
        public string DataFieldValue { get ; set; }

        #endregion
    }

    public class PanelControlRow : Panel { }

}
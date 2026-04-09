using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls.EntityExtend
{
    public partial class EntityCustomContent : UserControl
    {
        private PlaceHolder _placeHolderIControl = new PlaceHolder();
        public IEnumerable<_IFieldImportant> IControls { get { return _placeHolderIControl.Controls.OfType<_IFieldImportant>(); } }


        #region Template

        private ITemplate _template1 = null;

        [TemplateContainer(typeof(TemplateControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate TemplateIControls
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

        #endregion

        void Page_Init()
        {
            if (_template1 != null)
            {
                _template1.InstantiateIn(_placeHolderIControl);
            }
        }

        public void ClearIControls()
        {
            if (_placeHolderIControl.Controls.Count > 0)
                _placeHolderIControl.Controls.Clear();
        }
    }
}
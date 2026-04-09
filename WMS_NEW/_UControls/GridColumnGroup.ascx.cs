using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls
{
    public partial class GridColumnGroup : System.Web.UI.UserControl, IGridColumnGroup
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string DataFieldPrefix { get; set; }
        public string HeaderCssStyle { get; set; }
    }
}
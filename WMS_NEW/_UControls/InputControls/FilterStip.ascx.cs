using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Prototype.Providers;

namespace _UControls
{
    public delegate void FilterChangedHandler(Prototype.Providers.FilterAt _filterAt);

    public partial class FilterStip : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindFilterEvent();

            if (!Page.IsPostBack)
            {
                if (btEq.Text == string.Empty)
                {
                    BindFilterData();
                }

                if (Filter.FilterAt == FilterAt.None) // ถ้าไม่มีการเซต Fix Filter
                {
                    ClearFilter();
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e) //Page Stat working after Load Control events
        {
            if (!Page.IsPostBack)
            {

            }
            else //Manual Event Postback of Trigger Control
            {
            }
        }

        public string CurrentIDFilter
        {
            get
            {
                if (ViewState["CurrentIDFilter"] == null)
                    ViewState["CurrentIDFilter"] = string.Empty;

                return (string)ViewState["CurrentIDFilter"];
            }
            set
            {
                ViewState["CurrentIDFilter"] = value;
            }
        }

        protected void btFilter_Click(object sender, EventArgs e)
        {
            var refSender = (LinkButton)sender;

            if (refSender.ID != btActive.ID)
            {
                var filter = Filter;
                filter.FilterAt = refSender.Text.Split(' ')[0].GetEnumAttrEntry<FilterAt>();
                Filter = filter;
            }
        }

        void BindFilterEvent()
        {
            btActive.Click += new System.EventHandler(btFilter_Click);

            foreach (var en in (FilterAt[])Enum.GetValues(typeof(FilterAt)))
            {
                var control = GetControlFilter(en);
                control.Click += new System.EventHandler(btFilter_Click);
            }
        }

        void BindFilterData()
        {
            foreach (var en in (FilterAt[])Enum.GetValues(typeof(FilterAt)))
            {
                var entry = en.GetAttrEntry();
                var control = GetControlFilter(en);

                control.Text = entry.Name + " " + entry.Description;
            }
        }

        LinkButton GetControlFilter(Prototype.Providers.FilterAt filter)
        {
            switch (filter)
            {
                case FilterAt.None:
                    return btNone;

                case FilterAt.Equal:
                    return btEq;

                case FilterAt.NotEqual:
                    return btNotEq;

                case FilterAt.MoreThan:
                    return btThMore;

                case FilterAt.LessThan:
                    return btLeMore;

                case FilterAt.MoreThanEqual:
                    return btThMoreEq;

                case FilterAt.LessThanEqual:
                    return btLeMoreEq;

                case FilterAt.Contains:
                    return btContain;

                case FilterAt.Between:
                    return btBetween;

                //case FilterAt.Empty:
                //    return btEmpty;


                default:
                    return btNone;
            }
        }

        public event FilterChangedHandler FilterChangedValue;

        public Prototype.Providers.Filter Filter
        {
            get
            {
                if (ViewState["Filter"] == null)
                    ViewState["Filter"] = new Prototype.Providers.Filter();

                var filter = (Prototype.Providers.Filter)ViewState["Filter"];
                if (filter.FilterAt == FilterAt.None)
                {
                    filter.IsFilter = false;
                }
                else
                {
                    filter.IsFilter = true;
                }
                return filter;
            }
            set
            {
                ViewState["Filter"] = value;

                var entry = value.FilterAt.GetAttrEntry();

                if (value.FilterAt == FilterAt.None)
                {
                    btActive.Text = "<i class=\"icon-filter\" style=\"height:16px;\"></i>";
                    btActive.ToolTip = "Filter";
                }
                else
                {
                    btActive.Text = entry.Name;
                    btActive.ToolTip = entry.Description;
                }

                var refCurrent = (LinkButton)divFilterContent.FindControl(CurrentIDFilter);
                if (refCurrent != null)
                {
                    refCurrent.Visible = true;
                }

                var refSender = GetControlFilter(value.FilterAt);
                CurrentIDFilter = refSender.ID;
                refSender.Visible = false;

                if (FilterChangedValue != null)
                    FilterChangedValue(value.FilterAt);
            }
        }

        public bool Enabled
        {
            get
            {
                return divFilterContent.Enabled;
            }
            set
            {
                divFilterContent.Enabled = value;
                linkFilterOpt.Visible = value;
                if (!value)
                {
                    btActive.CssClass = "dropdown-header text-center dropdown-filter filter-disabled";
                }
            }
        }

        public bool Visibled
        {
            get
            {
                return divFilterContent.Visible;
            }
            set
            {
                divFilterContent.Visible = value;
            }
        }

        public void ClearFilter()
        {
            var filter = Filter;
            filter.FilterAt = FilterAt.None;
            Filter = filter;
        }

        public void SetEnableFilter(Prototype.Providers.FilterAt filter)
        {
            SetFilterable(filter, true);
        }
        public void SetDisableFilter(Prototype.Providers.FilterAt filter)
        {
            SetFilterable(filter, false);
        }

        private void SetFilterable(Prototype.Providers.FilterAt filter, bool _bool)
        {
            if (btEq.Text == string.Empty)
            {
                BindFilterData();
            }

            GetControlFilter(filter).Visible = _bool;
        }
    }
}
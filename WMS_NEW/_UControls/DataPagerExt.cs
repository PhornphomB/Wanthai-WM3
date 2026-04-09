using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls.DataPager
{
    public class DataPagerExt : System.Web.UI.WebControls.DataPager
    {
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Ul;
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (HasControls())
            {
                foreach (Control child in Controls)
                {
                    var item = child as DataPagerFieldItem;
                    if (item == null || !item.HasControls())
                    {
                        child.RenderControl(writer);
                        continue;
                    }

                    foreach (Control button in item.Controls)
                    {
                        var space = (button as LiteralControl);
                        if (space != null && space.Text == "&nbsp;")
                            continue;

                        try
                        {
                            if (button.GetType() == typeof(LinkButton))
                            {
                                (button as LinkButton).CssClass = "page-link";
                                writer.RenderBeginTag("li class=\"page-item\"");
                            }
                            else if (button.GetType() == typeof(Label))
                            {
                                (button as Label).CssClass = "page-link";
                                writer.RenderBeginTag("li class=\"page-item active\"");
                            }
                            else
                            {
                                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                            }
                        }
                        catch (Exception)
                        {
                            writer.RenderBeginTag(HtmlTextWriterTag.Li);
                        }

                        button.RenderControl(writer);
                        writer.RenderEndTag();
                    }
                }
            }
        }
    }
}
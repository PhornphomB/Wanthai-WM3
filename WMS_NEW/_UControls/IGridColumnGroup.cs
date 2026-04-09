using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _UControls
{
    public interface IGridColumnGroup
    {
        string DataFieldPrefix { get; set; }
        string HeaderCssStyle { get; set; }
    }
}

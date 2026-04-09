using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Prototype.Providers;

namespace _UControls
{
    public enum _StyleColor
    {
        [AttributeEntry("primary")]
        Default,
        [AttributeEntry("secondary")]
        Secondary,
        [AttributeEntry("success")]
        Success,
        [AttributeEntry("warning")]
        Warning,
        [AttributeEntry("danger")]
        Danger,
        [AttributeEntry("info")]
        Info
    }
}
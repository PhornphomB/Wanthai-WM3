using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.Providers
{
    public class LogBase
    {
        public LogBase()
        {
            this.EventResulted += new Prototype.Providers.EventHandler(_Object_EventResulted);
        }

        #region Implement Logging

        protected global::Prototype.Providers.Logging Logging;
        public event global::Prototype.Providers.EventHandler EventResulted;

        protected void RaiseMessage()
        {
            this.Logging.Raise(EventResulted);
        }

        protected void RaiseException(Exception ex)
        {
            this.Logging = new Prototype.Providers.Logging(this, ex);
            this.Logging.Raise(EventResulted);
        }

        protected void _Object_EventResulted(object sender, Prototype.Providers.EventArgsCustom e)
        {

        }

        #endregion
    }
}

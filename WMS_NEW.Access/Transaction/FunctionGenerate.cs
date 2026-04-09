using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects.SqlClient;

using WMS_NEW.Source;
using Prototype.Providers;
using System.Data.Entity.Core.Objects;

namespace WMS_NEW.Access
{
    public class FunctionGenerate : IDisposable
    {

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion


        #region ++INSTANCE STATIC++
        public static FunctionGenerate Instance
        {
            get
            {
                using (FunctionGenerate _Instance = new FunctionGenerate())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public string GetGenerateRunning(string _transacType)
        {
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));
            var dateNow = DateTime.Now.Date;
            var order_number = string.Empty;

            using (var _model = new WMSEntities())
            {
                switch (_transacType)
                {
                    case "IN":
                        order_number = _model.usp_inbound_generate_running_order(dateNow, errMsg).FirstOrDefault();
                        break;
                    case "OUT":
                        order_number = _model.usp_outbound_generate_running_order(dateNow, errMsg).FirstOrDefault();
                        break;
                    case "COUNT":
                        order_number = _model.usp_count_generate_running_order(errMsg).FirstOrDefault();
                        break;
                    case "COUNT_EXTERNAL":
                        order_number = _model.usp_count_external_generate_running_order(errMsg).FirstOrDefault();
                        break;
                    case "OUT_BY_GROUP":
                        order_number = _model.usp_outbound_generate_running_load_id(errMsg).FirstOrDefault();
                        break;
                    default:
                        break;
                }

                _model.Engaged(this, delegate ()
                {
                    if (errMsg.Value.ToString() == "0") return false;
                    else return true;
                }, errMsg.Value.ToString());

            }

            return order_number;
        }
    }
}

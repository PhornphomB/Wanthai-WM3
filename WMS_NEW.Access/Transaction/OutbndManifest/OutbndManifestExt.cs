using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WMS_NEW.Source;
using Prototype.Providers;

namespace WMS_NEW.Access.Transaction.OutbndManifest
{
    public class OutbndManifestExt : IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        public WMSEntities _Model { get; set; }

        public OutbndManifestExt()
        {
            this._Model = new WMSEntities();
        }

        #region ++INSTANCE STATIC++
        public static OutbndManifestExt Instance
        {
            get
            {
                using (OutbndManifestExt _Instance = new OutbndManifestExt())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public void ClearTransportData(string _session_id)
        {
            var dels = _Model.t_wms_outbound_manifest_tsp_order_de.Where(x => x.session_id == _session_id);

            _Model.t_wms_outbound_manifest_tsp_order_de.RemoveRange(dels);
            _Model.SaveChanges();
        }

        public IQueryable<Property> GetQuery()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = from rows in _Model.t_wms_outbound_manifest_tsp
                             orderby rows.create_date descending
                             select new Property
                             {
                                 display_member = rows.manifest_code,
                                 value_member = rows.manifest_code
                             };

                return result;
            });
        }

    }
}

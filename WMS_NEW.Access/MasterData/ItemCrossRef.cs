using Prototype.Providers;
using System;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class ItemCrossRef : AEntityFormCommand<t_wms_item_crossref>
    {
        public WMSEntities _Model { get; set; }
        public ItemCrossRef()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_item_crossref; };
        }

        public override bool ValidateSaveNew(t_wms_item_crossref ent, ref string msg_validate)
        {
            if (_Model.t_wms_item_crossref.Any(qry => qry.alternate_item_number == ent.alternate_item_number))
            {
                msg_validate = "! Alternate Number has in system.";
                return false;
            }
            else
                return true;
        }

        public override bool ValidateSaveUpdate(t_wms_item_crossref ent, ref string msg_validate)
        {
            if (_Model.t_wms_item_crossref.Any(qry => qry.alternate_id != ent.alternate_id && qry.alternate_item_number == ent.alternate_item_number))
            {
                msg_validate = "! Alternate Number has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_item_crossref
                         join uom in this._Model.t_wms_item_uom on rows.item_uom_id equals uom.item_uom_id
                         select new
                         {
                             KeyId = rows.alternate_id,
                             rows.item_master_id,
                             rows.alternate_item_number,
                             uom.uom,
                             rows.is_active
                         };


            if (this.FilterCustom == null)
                return result.Where(x => false);

            var entItem = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "item_master_id");
            if (entItem != null)
            {
                Guid item_master_id = (Guid)entItem.Value;
                result = result.Where(w => w.item_master_id == item_master_id);
            }

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }
}

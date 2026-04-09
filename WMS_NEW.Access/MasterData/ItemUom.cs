using Prototype.Providers;
using System;
using System.Linq;
using WMS_NEW.Source;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace WMS_NEW.Access.MasterData
{
    public class ItemUom : AEntityFormCommand<t_wms_item_uom>
    {

        #region ++INSTANCE STATIC++

        public static ItemUom Instance
        {
            get
            {
                using (ItemUom _Instance = new ItemUom())
                {
                    return _Instance;
                }
            }
        }

        #endregion


        public WMSEntities _Model { get; set; }

        public ItemUom()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_item_uom; };

        }

        public override bool ValidateSaveNew(t_wms_item_uom ent, ref string msg_validate)
        {
            if (_Model.t_wms_item_uom.Any(x => x.item_master_id == ent.item_master_id && x.uom == ent.uom))
            {
                msg_validate = "! Uom has in Item.";
                return false;
            }
            else
                return true;
        }

        public override void SetOptionalSaveNew(t_wms_item_uom ent)
        {
            ent.picking_class = ent.picking_class.ToUpper();
            ent.putaway_class = ent.putaway_class.ToUpper();
        }

        public override void SetOptionalSaveUpdate(t_wms_item_uom ent)
        {
            ent.picking_class = ent.picking_class.ToUpper();
            ent.putaway_class = ent.putaway_class.ToUpper();
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_item_uom
                         select new
                         {
                             KeyId = rows.item_uom_id,
                             rows.item_master_id,
                             rows.uom,
                             rows.uom_prompt,
                             rows.conversion_factor,
                             rows.primary_uom,
                             rows.picking_uom,
                             rows.shipping_uom,
                             rows.is_active,
                             rows.is_pack_size_uom,
                             rows.is_pallet_uom,
                             rows.sequence,
                             rows.is_full_pallet_uom
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


        #region Custom Functions

        public bool CheckHasPrimary(Guid item_master_id)
        {
            var chk = this._Model.GetDataBy(this, delegate ()
            {
                if (this._Model.t_wms_item_uom.Any(qry => qry.item_master_id == item_master_id && qry.primary_uom == "YES"))
                    return true;
                else
                    return false;
            });

            return chk;

        }
        public bool CheckHasPallet(Guid item_master_id)
        {
            var chk = this._Model.GetDataBy(this, delegate ()
            {
                if (this._Model.t_wms_item_uom.Any(qry => qry.item_master_id == item_master_id && qry.is_pallet_uom == "YES"))
                    return true;
                else
                    return false;
            });

            return chk;

        }

        public bool CheckHasPrimary_ByWareHouseItem(Guid wh_item_master_id)
        {
            var chk = this._Model.Engaged(this, delegate ()
            {
                var uom = (from rows in _Model.t_wms_item_uom
                           where rows.t_wms_item.t_wms_wh_item.Any(a => a.wh_item_master_id == wh_item_master_id
                                && rows.primary_uom == "YES")
                           select rows).FirstOrDefault();

                return uom != null ? true : false;
            });

            return chk;
        }

        public bool CheckPrimaryHasUsed(Guid item_master_id, Guid item_uom_id)
        {
            var chk = this._Model.Engaged(this, delegate ()
            {
                if (this._Model.t_wms_inventory.Any(whi => whi.t_wms_wh_item.item_master_id == item_master_id)
                 && this._Model.t_wms_item_uom.Any(x => x.item_master_id == item_master_id && x.item_uom_id == item_uom_id && x.primary_uom == "YES"))
                    return true;
                else
                    return false;
            }, "! Not allow uncheck Primary UOM, Because this item used in inventory");

            return chk;

        }

        public bool CheckPalletHasUsed(Guid item_master_id, Guid item_uom_id)
        {
            var chk = this._Model.Engaged(this, delegate ()
            {
                if (this._Model.t_wms_inventory.Any(whi => whi.t_wms_wh_item.item_master_id == item_master_id)
                 && this._Model.t_wms_item_uom.Any(x => x.item_master_id == item_master_id && x.item_uom_id == item_uom_id && x.is_pallet_uom == "YES"))
                    return true;
                else
                    return false;
            }, "! Not allow uncheck Pallet UOM, Because this item used in inventory");

            return chk;

        }

        public object Get_MinimumUOM(Guid wh_item_master_id, Guid item_uom_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_wh_item
                              where rows.wh_item_master_id == wh_item_master_id

                              let uom_curr = rows.t_wms_item.t_wms_item_uom.FirstOrDefault(f => f.item_uom_id == item_uom_id)
                              let uom_pri = rows.t_wms_item.t_wms_item_uom.FirstOrDefault(f => f.primary_uom == "YES")

                              select new ItemUOMDto
                              {
                                  uom_id = uom_curr.item_uom_id,
                                  conversion = uom_curr != null ? uom_curr.conversion_factor : 0,
                                  uom = uom_pri.uom,
                              }).FirstOrDefault();

                return result;
            });
        }

        public object Get_MinimumUOM(Guid wh_item_master_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_wh_item
                              where rows.wh_item_master_id == wh_item_master_id
                              select new ItemUOMDto
                              {
                                  uom_id = rows.t_wms_item.t_wms_item_uom.FirstOrDefault(f => f.primary_uom == "YES").item_uom_id,
                                  uom = rows.t_wms_item.t_wms_item_uom.FirstOrDefault(f => f.primary_uom == "YES").uom_prompt,
                              }).FirstOrDefault();

                return result;
            });
        }


        public bool isPrimaryUOM(Guid wh_item_master_id)
        {
            var chk = this._Model.Engaged(this, delegate ()
            {
                if (this._Model.t_wms_wh_item.Any(wh => wh.wh_item_master_id == wh_item_master_id
                    && wh.t_wms_item.t_wms_item_uom.Count(w => w.primary_uom == "YES") > 0))
                    return true;
                else
                    return false;
            });

            return chk;
        }

        public bool CheckUOMPromt(Guid item_master_id, string uom_prompt)
        {
            var chk = this._Model.Engaged(this, delegate ()
            {
                var uom = (from rows in _Model.t_wms_item_uom
                           where rows.item_master_id == item_master_id
                                && rows.uom_prompt == uom_prompt
                           select rows).FirstOrDefault();

                return uom != null ? true : false;
            });

            return chk;
        }

        public bool CheckUOMPromt_Edit(Guid key_id, Guid item_master_id, string uom_prompt)
        {
            var realId = key_id;

            var old_uom_prompt = this._Model.t_wms_item_uom.Where(wh => wh.item_uom_id == realId).FirstOrDefault();

            var result = (from rows in _Model.t_wms_item_uom
                          where rows.item_master_id == item_master_id
                          && rows.uom_prompt != old_uom_prompt.uom_prompt
                          && rows.uom_prompt == uom_prompt
                          select rows).FirstOrDefault();

            return result != null ? true : false;

        }


        [Serializable()]
        public class ItemUOMDto
        {
            public Guid uom_id { get; set; }
            public double conversion { get; set; }
            public string uom { get; set; }
        }

        #endregion


        #region Query Property
        public IQueryable<Property> GetQuery_Item(Guid _item_master_id)
        {
            var result = from rows in _Model.t_wms_item_uom
                         where rows.is_active == "YES"
                         && rows.t_wms_item.item_master_id == _item_master_id
                         orderby rows.sequence ascending
                         select new Property
                         {
                             guid_member = rows.item_uom_id,
                             display_member = rows.uom
                         };

            return result;
        }
        public IQueryable<Property> GetQuery_WhItem(Guid _wh_item_master_id)
        {
            var result = from rows in _Model.t_wms_item_uom
                         where rows.is_active == "YES" && rows.is_pack_size_uom == "YES" 
                         && rows.t_wms_item.t_wms_wh_item.Any(a => a.wh_item_master_id == _wh_item_master_id)
                         orderby rows.sequence ascending
                         select new Property
                         {
                             guid_member = rows.item_uom_id,
                             display_member = rows.uom
                         };

            return result;
        }

        //public IQueryable<Property> GetQuery_PaxkSizeByWhItem(Guid _wh_item_master_id)
        //{
        //    var result = from rows in _Model.t_wms_item_uom
        //                 where rows.is_active == "YES" && rows.is_pack_size_uom == "YES"
        //                 && rows.t_wms_item.t_wms_wh_item.Any(a => a.wh_item_master_id == _wh_item_master_id)
        //                 orderby rows.sequence ascending
        //                 select new Property
        //                 {
        //                     guid_member = rows.item_uom_id,
        //                     display_member = rows.uom
        //                 };

        //    return result;
        //}
        public IQueryable<Property> GetQuery_WhPalletItem(Guid _wh_item_master_id)
        {
            var result = from rows in _Model.t_wms_item_uom
                         where rows.is_active == "YES" && rows.is_pallet_uom == "YES"
                         && rows.t_wms_item.t_wms_wh_item.Any(a => a.wh_item_master_id == _wh_item_master_id)
                         orderby rows.sequence ascending
                         select new Property
                         {
                             guid_member = rows.item_uom_id,
                             display_member = rows.uom
                         };

            return result;
        }
        public IQueryable<Property> GetQueryCode_WhItem(Guid _wh_item_master_id)
        {
            var result = from rows in _Model.t_wms_item_uom
                         where rows.is_active == "YES"
                         && rows.t_wms_item.t_wms_wh_item.Any(a => a.wh_item_master_id == _wh_item_master_id)
                         orderby rows.sequence ascending
                         select new Property
                         {
                             value_member = rows.uom,
                             display_member = rows.uom
                         };

            return result;
        }

        public IQueryable<Property> GetQueryPackSize_WhItem(Guid _wh_item_master_id)
        {
            var result = from rows in _Model.t_wms_item_uom
                         where rows.is_active == "YES"
                         && rows.t_wms_item.t_wms_wh_item.Any(a => a.wh_item_master_id == _wh_item_master_id)
                         orderby rows.sequence ascending
                         select new Property
                         {
                             value_member = SqlFunctions.StringConvert(rows.conversion_factor).Trim(),
                             display_member = rows.uom
                         };

            return result;
        }

        #endregion
    }
}

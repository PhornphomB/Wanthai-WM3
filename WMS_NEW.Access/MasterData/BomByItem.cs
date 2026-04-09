using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using WMS_NEW.Source;
using Prototype.Providers;
using WMS_NEW.Access;

namespace WMS_NEW.Access.MasterData
{

    public class BomByItem : AEntityFormCommand<t_wms_item_bom>
    {
        public WMSEntities _Model { get; set; }


        public BomByItem()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_item_bom; };
        }


        #region ++INSTANCE STATIC++
        public static BomByItem Instance
        {
            get
            {
                using (BomByItem _Instance = new BomByItem())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Inherit IEntityCommandForm

        //public bool Save(object _objectEntity)
        //{
        //    var ent = (t_wms_item_bom)_objectEntity;
        //    ent.create_date = DateTime.Now;

        //    if (ent.is_bom_item == "YES")
        //    {
        //        var chk = this._Model.Existed(this, delegate ()
        //        {
        //            if (this._Model.t_wms_item_bom.Any(qry => qry.wh_item_master_id == ent.wh_item_master_id && qry.is_bom_item == "YES"))
        //                return true;
        //            else
        //                return false;
        //        }, "! Warehouse and Bom Code has existed key");

        //        if (chk)
        //            return false;
        //    }
        //    else
        //    {
        //        var chk = this._Model.Existed(this, delegate ()
        //        {
        //            if (this._Model.t_wms_item_bom.Any(qry => qry.wh_master_id == ent.wh_master_id && qry.bom_raw_code == ent.bom_raw_code && qry.is_bom_item == "NO"))
        //                return true;
        //            else
        //                return false;
        //        }, "! Warehouse and Bom Code has existed key");

        //        if (chk)
        //            return false;
        //    }

        //    return this._Model.Save(this, delegate ()
        //    {
        //        if (ent.is_bom_item == "YES")
        //        {
        //            var item = DataPrimaryField.Instance.GetMS_Item(ent.wh_item_master_id);
        //            ent.item_master_id = item.item_master_id;
        //            ent.item_number = item.item_number;
        //        }

        //        this._Model.t_wms_item_bom.AddObject(ent);

        //        return this._Model.SaveChanges();
        //    });
        //}

        //public bool Update(object _objectEntity)
        //{
        //    var ent = (t_wms_item_bom)_objectEntity;
        //    ent.update_date = DateTime.Now;

        //    if (ent.is_active == "NO")
        //    {
        //        var chk = this._Model.Existed(this, delegate ()
        //        {
        //            if (this._Model.t_wms_item_bom_detail.Any(qry => qry.bom_id == ent.bom_id))
        //                return true;
        //            else
        //                return false;
        //        }, "! Cannot set Active = [NO], This bom has used");

        //        if (chk)
        //            return false;
        //    }

        //    return this._Model.Update(this, delegate ()
        //    {
        //        if (ent.is_bom_item == "YES")
        //        {
        //            var item = DataPrimaryField.Instance.GetMS_Item(ent.wh_item_master_id);
        //            ent.item_master_id = item.item_master_id;
        //            ent.item_number = item.item_number;
        //        }

        //        return this._Model.SaveChanges();
        //    });
        //}

        //public object GetByKeyID(object Id)
        //{
        //    var realId = Convert.ToInt32(Id);

        //    return this._Model.GetDataEntityBy<t_wms_item_bom>(this, delegate ()
        //    {
        //        return (from rows in this._Model.t_wms_item_bom
        //                where rows.bom_id == realId
        //                select rows).FirstOrDefault();
        //    });
        //}

        //public object GetEditKeyID(object Id)
        //{
        //    var realId = Convert.ToInt32(Id);

        //    return this._Model.GetDataEntityBy<t_wms_item_bom>(this, delegate ()
        //    {
        //        return (from rows in this._Model.t_wms_item_bom
        //                where rows.bom_id == realId
        //                select rows).FirstOrDefault();
        //    });
        //}



        public bool DeleteByKeyID(object Id)
        {
            var realId = Convert.ToInt32(Id);

            var chk = this._Model.Existed(this, delegate ()
            {
                if (this._Model.t_wms_item_bom_detail.Any(qry => qry.bom_id == realId))
                    return true;
                else
                    return false;
            }, "! This Bom has used");

            if (chk)
                return false;

            return this._Model.Update(this, delegate ()
            {
                var result = this._Model.t_wms_item_bom.FirstOrDefault(qry => qry.bom_id == realId);
                if (result != null)
                {
                    this._Model.t_wms_item_bom.Remove(result);
                }

                return this._Model.SaveChanges();
            });
        }

        public override bool ValidateSaveNew(t_wms_item_bom ent, ref string msg_validate)
        {
            //var ent = (t_wms_item_bom)_objectEntity;
            //ent.create_date = DateTime.Now;

            if (ent.is_bom_item == "YES")
            {
                var chk = this._Model.Existed(this, delegate ()
                {
                    if (this._Model.t_wms_item_bom.Any(qry => qry.wh_item_master_id == ent.wh_item_master_id && qry.is_bom_item == "YES"))
                        return true;
                    else
                        return false;
                }, "! Warehouse and Bom Code has existed key");

                if (chk)
                    return false;
            }
            else
            {
                var chk = this._Model.Existed(this, delegate ()
                {
                    if (this._Model.t_wms_item_bom.Any(qry => qry.wh_master_id == ent.wh_master_id && qry.bom_raw_code == ent.bom_raw_code && qry.is_bom_item == "NO"))
                        return true;
                    else
                        return false;
                }, "! Warehouse and Bom Code has existed key");

                if (chk)
                    return false;
            }

            //return this._Model.Save(this, delegate ()
            //{
            if (ent.is_bom_item == "YES")
            {
                var item = DataPrimaryField.Instance.GetMasterItem(ent.wh_item_master_id);
                ent.item_master_id = item.item_master_id;
                ent.item_number = item.item_number;
            }

            return true;

            //    this._Model.t_wms_item_bom.Add(ent);

            //    return this._Model.SaveChanges();
            //});
            //if (_Model.t_wms_item.Any(x => x.item_number == ent.item_number && x.owner_id == ent.owner_id))
            //{
            //    msg_validate = "! Item Number and Owner ID has in system.";
            //    return false;
            //}
            //else
            //    return true;
        }

        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in _Model.t_wms_item_bom
                         select new
                         {
                             KeyId = rows.bom_id,
                             rows.wh_master_id,
                             rows.wh_id,
                             rows.owner_id,
                             rows.owner_code,
                             rows.is_bom_item,
                             bom_code = rows.is_bom_item == "YES" ? rows.item_number : rows.bom_raw_code,
                             bom_desc = rows.bom_desc,
                             uom = rows.is_bom_item == "YES" ? rows.uom : rows.bom_raw_uom,
                             qty = _Model.t_wms_item_bom_detail.Where(x => x.bom_id == rows.bom_id).Sum(sm => sm.quantity) ?? 0,
                             rows.is_active,
                             rows.create_by,
                             rows.create_date
                         };


            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        public t_wms_item GetItemWithDesc(Guid _item_master_id)
        {

            return this._Model.GetDataEntityBy<t_wms_item>(this, delegate ()
            {
                var result = (from tb in _Model.t_wms_item
                              where tb.item_master_id == _item_master_id
                              select tb).FirstOrDefault();

                //if (result != null)
                //{
                //    result.t_wms_item_crossref.Load();
                //}

                return result;//.FirstOrDefault();
            });
        }

        public IQueryable<Property> GetQueryPropertyByItemBom(Guid _wh_master_id, Guid owner_id, string _is_bom = "YES")
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = from rows in this._Model.t_wms_wh_item
                             let item = rows.t_wms_item
                             where rows.is_active == "YES"
                             && item.is_bom == _is_bom
                             && rows.wh_master_id == _wh_master_id && item.owner_id == owner_id
                             orderby item.item_number
                             select new Property
                             {
                                 guid_member = rows.wh_item_master_id,
                                 display_member = item.item_number
                             };

                return result;
            });
        }
    }
}

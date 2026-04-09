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

    public class BomDetail : AEntityFormCommand<t_wms_item_bom_detail>
    {
        public WMSEntities _Model { get; set; }


        public BomDetail()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_item_bom_detail; };
        }


        #region ++INSTANCE STATIC++
        public static BomDetail Instance
        {
            get
            {
                using (BomDetail _Instance = new BomDetail())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public string GetLineNumber(int _bom_id)
        {

            return this._Model.GetDataBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_item_bom_detail
                              where rows.bom_id == _bom_id
                              select new
                              {
                                  rows.line_no
                              }).ToList();

                int count = 0;
                if (result.Count > 0)
                {
                    count = result.Max(ss => Convert.ToInt32(ss.line_no));
                }

                //count = result == null ? 0 : Convert.ToInt32(result);
                string _lineNumber = (count + 1).ToString().PadLeft(6, '0');
                return _lineNumber;

            });

        }


        #region Inherit IEntityCommandForm

        public override bool ValidateSaveNew(t_wms_item_bom_detail ent, ref string msg_validate)
        {        
            var chk = this._Model.Existed(this, delegate ()
            {
                if (this._Model.t_wms_item_bom_detail.Any(qry => qry.bom_id == ent.bom_id && qry.wh_item_master_id == ent.wh_item_master_id))
                    return true;
                else
                    return false;
            }, "! Item Number has existed in this bom.");

            if (chk)
                return false;

            ent.line_no = GetLineNumber(ent.bom_id);
            ent.create_date = DateTime.Now;

            var item = DataPrimaryField.Instance.GetMasterItem(ent.wh_item_master_id);
            ent.item_master_id = item.item_master_id;
            ent.item_number = item.item_number;

            return true;
        }

        public bool Save(object _objectEntity)
        {
            var ent = (t_wms_item_bom_detail)_objectEntity;

            var chk = this._Model.Existed(this, delegate ()
            {
                if (this._Model.t_wms_item_bom_detail.Any(qry => qry.bom_id == ent.bom_id && qry.wh_item_master_id == ent.wh_item_master_id))
                    return true;
                else
                    return false;
            }, "! Item Number has existed in this bom.");

            if (chk)
                return false;

            ent.line_no = GetLineNumber(ent.bom_id);
            ent.create_date = DateTime.Now;

            var item = DataPrimaryField.Instance.GetMasterItem(ent.wh_item_master_id);
            ent.item_master_id = item.item_master_id;
            ent.item_number = item.item_number;

            return this._Model.Save(this, delegate ()
            {
                this._Model.t_wms_item_bom_detail.Add(ent);
                return this._Model.SaveChanges();
            });
        }

        public bool Update(object _objectEntity)
        {
            var ent = (t_wms_item_bom_detail)_objectEntity;
            ent.update_date = DateTime.Now;

            var item = DataPrimaryField.Instance.GetMasterItem(ent.wh_item_master_id);
            ent.item_master_id = item.item_master_id;
            ent.item_number = item.item_number;

            return this._Model.Update(this, delegate ()
            {
                return this._Model.SaveChanges();
            });
        }

        //public object GetByKeyID(object Id)
        //{
        //    var realId = Convert.ToInt32(Id);

        //    return this._Model.GetDataEntityBy<t_wms_item_bom_detail>(this, delegate ()
        //    {
        //        return (from rows in this._Model.t_wms_item_bom_detail
        //                where rows.bom_detail_id == realId
        //                select rows).FirstOrDefault();
        //    });
        //}

        //public object GetEditKeyID(object Id)
        //{
        //    var realId = Convert.ToInt32(Id);

        //    return this._Model.GetDataEntityBy<t_wms_item_bom_detail>(this, delegate ()
        //    {
        //        return (from rows in this._Model.t_wms_item_bom_detail
        //                where rows.bom_detail_id == realId
        //                select rows).FirstOrDefault();
        //    });
        //}

        public bool DeleteByKeyID(object Id)
        {
            var realId = Convert.ToInt32(Id);

            return this._Model.Update(this, delegate ()
            {
                var result = this._Model.t_wms_item_bom_detail.FirstOrDefault(qry => qry.bom_detail_id == realId);
                if (result != null)
                {
                    this._Model.t_wms_item_bom_detail.Remove(result);
                }

                return this._Model.SaveChanges();
            });
        }

        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var bom_id = (int)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "bom_id").Value;

            var result = from rows in _Model.t_wms_item_bom_detail
                         where rows.bom_id == bom_id
                         select new
                         {
                             KeyId = rows.bom_detail_id,
                             rows.line_no,
                             rows.item_number,
                             rows.description,
                             rows.uom,
                             rows.quantity,
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
    }
}

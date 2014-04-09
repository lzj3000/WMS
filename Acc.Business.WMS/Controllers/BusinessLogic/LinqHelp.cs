using System;
using System.Collections.Generic;
using System.Linq;
using Way.EAP.DataAccess.Entity;
using Acc.Business.WMS.Model;
using Acc.Business.Model;

namespace Acc.Business.WMS.Controllers
{
    public class LinqHelp
    {
        /// <summary>
        /// 合并出库通知明细行重复方法
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<StockOutNoticeMaterials> MergerStockOutNoticeMaterials(List<StockOutNoticeMaterials> list)
        {
            //注:处理目标->将"编号(MATERIALCODE)"相同的产品记录，"库存量(NUM)"合并
            //合并处理
            list.ForEach(c =>
            {
                var group = list.Where(a => a.MATERIALCODE == c.MATERIALCODE);
                c.NUM = group.Sum(x => x.NUM);
            });
            //去重复
            return list.Distinct(new ComparerStockOutNotice()).ToList();

        }

        /// <summary>
        /// 合并出库通知明细行重复方法
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<StockOutOrderMaterials> MergerStockOutOrderMaterials(List<StockOutOrderMaterials> list)
        {
            //注:处理目标->将"编号(MATERIALCODE)"相同的产品记录，"库存量(NUM)"合并
            //合并处理
            list.ForEach(c =>
            {
                var group = list.Where(a => a.MATERIALCODE == c.MATERIALCODE);
                c.NUM = group.Sum(x => x.NUM);
            });
            //去重复
            return list.Distinct(new ComparerStockOutOrder()).ToList();

        }

        /// <summary>
        /// 合并出库通知明细行重复方法
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<BatchChangeMaterials> MergerBatchChangeMaterials(List<BatchChangeMaterials> list)
        {
            //注:处理目标->将"编号(MATERIALCODE)"相同的产品记录，"库存量(NUM)"合并
            //合并处理
            list.ForEach(c =>
            {
                var group = list.Where(a => a.NEWMCODE == c.NEWMCODE).Where(b => b.BATCHNO == c.BATCHNO).Where(d => d.NEWFMODEL == c.NEWFMODEL).Where(e => e.NEWBATCHNO==c.NEWBATCHNO);
                c.NEWNUM = group.Sum(x => x.NEWNUM);
            });
            //var fda = from l in list  
            //        group l by new { l.NEWCODE, l.BATCHNO,l.NEWFMODEL } into g  
            //        select new  
            //        {  
            //           Name = g.Key.NEWCODE,  
            //            NUM = g.Sum(a => a.NUM) 
            //       };  

            //去重复
            return list.Distinct(new ComparerBatchChange()).ToList();
        }

        /// <summary>
        /// 合并入库单明细行重复方法
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<StockInOrderMaterials> MergerStockInOrderMaterials(List<StockInOrderMaterials> list)
        {
            //注:处理目标->将"编号(MATERIALCODE)"相同的产品记录，"库存量(NUM)"合并
            //合并处理
            list.ForEach(c =>
            {
                var group = list.Where(a => a.MATERIALCODE == c.MATERIALCODE);
                c.NUM = group.Sum(x => x.NUM);
            });
            //去重复
            return list.Distinct(new ComparerStockInOrder_Materials()).ToList();

        }

        /// <summary>
        /// 通化东宝项目合并明细
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<StockInOrderMaterials> XHY_MergerStockInOrderMaterials(List<StockInOrderMaterials> list)
        {
            //注:处理目标->将"编号(MATERIALCODE)"相同的产品记录，"库存量(NUM)"合并
            //合并处理
            list.ForEach(c =>
            {
                var group = list.Where(a => a.MATERIALCODE == c.MATERIALCODE && a.BATCHNO == c.BATCHNO);
                c.NUM = group.Sum(x => x.NUM);
            });
            //去重复
            return list.Distinct(new ComparerStockInOrder_Materials()).ToList();

        }
        /// <summary>
        /// 合并入库通知明细行重复方法
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<StockInNoticeMaterials> MergerStockNoticeOrderMaterials(List<StockInNoticeMaterials> list)
        {
            //注:处理目标->将"编号(MATERIALCODE)"相同的产品记录，"库存量(NUM)"合并
            //合并处理
            list.ForEach(c =>
            {
                var group = list.Where(a => a.MATERIALCODE == c.MATERIALCODE);
                c.NUM = group.Sum(x => x.NUM);
            });
            //去重复
            return list.Distinct(new ComparerStockInNoticeMaterials()).ToList();

        }
        /// <summary>
        /// 合并产品BOM明细行重复方法
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<PackUnitList> MergerPackUnitListMaterials(List<PackUnitList> list)
        {
            //注:处理目标->将"编号(MATERIALCODE)"相同的产品记录，"库存量(NUM)"合并
            //合并处理
            list.ForEach(c =>
            {
                var group = list.Where(a => a.PACKUNITCODE == c.PACKUNITCODE);
                c.NUM = group.Sum(x => x.NUM);
            });
            //去重复
            return list.Distinct(new ComparerPackUnitList()).ToList();

        }

    }

        

    /// <summary>
    /// 工具类(一般开发中，可定义在自己的工具类库里)
    /// </summary>
    static class Utils
    {

        /// <summary>
        /// List扩展方法，将List元素用分隔符连接后，返回字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public static String Join<T>(this List<T> list, string splitStr = ",")
        {
            string result = string.Empty;
            foreach (var item in list)
            {
                result += item.ToString() + splitStr;
            }
            return result.Trim(splitStr.ToCharArray());
        }
    }

    /// <summary>
    /// 产品实体类
    /// </summary>
    class Product
    {
        /// <summary>
        /// 库存
        /// </summary>
        public int NUM { set; get; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public String MATERIALCODE { set; get; }

        /// <summary>
        /// 附属标签
        /// </summary>
        public String Tag { set; get; }
    }

    /// <summary>
    /// 去"重复"时候的比较器(只要MATERIALCODE相同，即认为是相同记录)
    /// </summary>
    class ComparerStockOutNotice : IEqualityComparer<StockOutNoticeMaterials>
    {
        public bool Equals(StockOutNoticeMaterials p1, StockOutNoticeMaterials p2)
        {
            if (p1 == null)
                return p2 == null;
            return p1.MATERIALCODE == p2.MATERIALCODE;
        }

        public int GetHashCode(StockOutNoticeMaterials p)
        {
            if (p == null)
                return 0;
            return p.MATERIALCODE.GetHashCode();
        }
    }

    /// <summary>
    /// 去"重复"时候的比较器(只要MATERIALCODE相同，即认为是相同记录)
    /// </summary>
    class ComparerStockOutOrder : IEqualityComparer<StockOutOrderMaterials>
    {
        public bool Equals(StockOutOrderMaterials p1, StockOutOrderMaterials p2)
        {
            if (p1 == null)
                return p2 == null;
            return p1.MATERIALCODE == p2.MATERIALCODE;
        }

        public int GetHashCode(StockOutOrderMaterials p)
        {
            if (p == null)
                return 0;
            return p.MATERIALCODE.GetHashCode();
        }
    }

    /// <summary>
    /// 去"重复"时候的比较器(只要NEWMCODE、BATCHNO、NEWFMODEL同时相同，即认为是相同记录)
    /// </summary>
    class ComparerBatchChange : IEqualityComparer<BatchChangeMaterials>
    {
        public bool Equals(BatchChangeMaterials p1, BatchChangeMaterials p2)
        {
            if (p1 == null)
                return p2 == null;
            return p1.NEWMCODE == p2.NEWMCODE && p1.BATCHNO == p2.BATCHNO && p1.NEWFMODEL == p2.NEWFMODEL && p1.NEWBATCHNO == p2.NEWBATCHNO;
           
        }

        public int GetHashCode(BatchChangeMaterials p)
        {
            if (p == null)
                return 0;
            return p.NEWMCODE.GetHashCode();
        }
    }

    /// <summary>
    /// 去"重复"时候的比较器(只要MATERIALCODE相同，即认为是相同记录)
    /// </summary>
    class ComparerStockInOrder_Materials : IEqualityComparer<StockInOrderMaterials>
    {
        public bool Equals(StockInOrderMaterials p1, StockInOrderMaterials p2)
        {
            if (p1 == null)
                return p2 == null;
            return p1.MATERIALCODE == p2.MATERIALCODE;
        }

        public int GetHashCode(StockInOrderMaterials p)
        {
            if (p == null)
                return 0;
            return p.MATERIALCODE.GetHashCode();
        }
    }
    /// <summary>
    /// 去"重复"时候的比较器(只要MATERIALCODE相同，即认为是相同记录)
    /// </summary>
    class ComparerStockInNoticeMaterials : IEqualityComparer<StockInNoticeMaterials>
    {
        public bool Equals(StockInNoticeMaterials p1, StockInNoticeMaterials p2)
        {
            if (p1 == null)
                return p2 == null;
            return p1.MATERIALCODE == p2.MATERIALCODE;
        }

        public int GetHashCode(StockInNoticeMaterials p)
        {
            if (p == null)
                return 0;
            return p.MATERIALCODE.GetHashCode();
        }
    }
    /// <summary>
    /// 去"重复"时候的比较器(只要PACKUNITCODE相同，即认为是相同记录)
    /// </summary>
    class ComparerPackUnitList : IEqualityComparer<PackUnitList>
    {
        public bool Equals(PackUnitList p1, PackUnitList p2)
        {
            if (p1 == null)
                return p2 == null;
            return p1.PACKUNITCODE == p2.PACKUNITCODE;
        }

        public int GetHashCode(PackUnitList p)
        {
            if (p == null)
                return 0;
            return p.PACKUNITCODE.GetHashCode();
        }
    }
}
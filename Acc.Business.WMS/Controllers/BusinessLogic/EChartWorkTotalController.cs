using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Contract;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;
using Acc.Business.Model;
using Acc.ECharts;
using System.Data;
using Acc.Contract.Data.ControllerData;

namespace Acc.Business.WMS.Controllers.BusinessLogic
{
    /// <summary>
    /// 工作总量
    /// 按当前用户左侧菜单机型统计汇总
    /// </summary>
    public class EChartWorkTotalController : EChartsController
    {
        public EChartWorkTotalController()
        {
            //主子表不是c.parentid=c.id  暂时手动忽视
            IngoreTables.Add("Acc_WMS_PackUnit");
            IngoreTables.Add("Acc_WMS_LoanOrder");
            IngoreTables.Add("Acc_Pur_Purinvoice");
            IngoreTables.Add("Acc_Pur_PurTpayment");
        }

        #region 参数

        /// <summary>
        ///主表、子表
        /// </summary>
        [WhereParameter(visible = false)]
        public string LengedName { set; get; }

        /// <summary>
        ///时间列 2013-1-2
        /// </summary>
        [WhereParameter(visible = false)]
        public string DataName { set; get; }

        [WhereParameter(visible = true, field = "Creationdate", type = "date", title = "时间")]
        public string Creationdate { set; get; }

        #endregion

        protected List<MenuTableRelation> mtrs = new List<MenuTableRelation>();
        private string SelectHasChild;
        private string SelectNoChild;
        private List<string> IngoreTables = new List<string>();

        #region 初始化菜单

        /// <summary>
        /// 获取当前用户业务菜单及菜单对应主子表
        /// </summary>
        private void InitMenus()
        {
            mtrs.Clear();

            foreach (Dic menu in this.user.menu)
            {
                AddChildMenu(menu, mtrs);
            }
        }

        /// <summary>
        /// 递归查询所有DicMenu
        /// </summary>
        /// <param name="parentDic"></param>
        /// <param name="parentList"></param>
        private void AddChildMenu(Dic parentDic, List<MenuTableRelation> parentList)
        {
            if (parentDic.childs.Length > 0)
            {
                foreach (Dic child in parentDic.childs)
                {
                    AddChildMenu(child, parentList);
                }
            }
            else
            {
                if (parentDic.menus.Length > 0)
                {
                    foreach (DicMenu child in parentDic.menus)
                    {
                        MenuTableRelation mtr = GetMTR(child);
                        if (mtr != null && !parentList.Exists(m => m.MenuName.Equals(mtr.MenuName)))
                        {
                            parentList.Add(mtr);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取MenuTableRelation
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        private MenuTableRelation GetMTR(DicMenu child)
        {
            MenuTableRelation result = mtrs.Where(mt => mt.MenuName.Equals(child.name)).FirstOrDefault();
            if (result != null)
                return null;

            //如果是业务菜单 mmm不为空
            if (child.mmm == null)
                return null;

            SystemModel sm = (SystemModel)child.mmm;

            //忽略数据库中没有填写ControllerName实体
            if (string.IsNullOrEmpty(sm.ControllerName))
                return null;

            //忽略基类Controller
            if (sm.ControllerName.StartsWith("Acc.Business.Controllers"))
                return null;

            //忽略基类Model
            ControllerBase controllerBase;
            try
            {
                controllerBase = ControllerCenter.GetCenter.GetController(sm.ControllerName, this.user.tempid);
            }
            catch (Exception ex)
            {
                //当前Controller在系统中不存在
                return null;
            }
            if (controllerBase.model == null)
                return null;


            //基类model和视图忽略
            //string tmpName=controllerBase.model.GetType().FullName;
            //if (tmpName.StartsWith("Acc.Business.Model") || tmpName.ToLower().Contains("view"))
            //    return null;

            //因为要用到创建人 所以要用到 BasicInfo类
            bool isBinfo = controllerBase.model is BasicInfo;
            if (!isBinfo)
                return null;

            result = new MenuTableRelation();

            //忽略非法Model
            IEntityBase eb = controllerBase.model as IEntityBase;
            if (eb == null)
                return null;

            //忽略没有绑定表的实体
            EntityClassAttribut eca = eb.GetEntityClassAttribut;
            if (eca == null)
                return null;

            //忽略不加载的表
            if (IngoreTables.Exists(m => m.Equals(eca.TableName, StringComparison.OrdinalIgnoreCase)))
                return null;

            //忽略已经存在的主表
            if (mtrs.Where(mt => mt.ParentTableName.Equals(eca.TableName)).Any())
                return null;

            result.MenuName = child.name;
            result.ParentTableName = eca.TableName;


            IHierarchicalEntityView[] hevs = eb.GetChildEntityList();
            foreach (IHierarchicalEntityView hev in hevs)
            {
                EntityClassAttribut ceca = ((IEntityBase)hev.ChildEntity).GetEntityClassAttribut;
                //根据主表进行匹配
                if (ceca.TableName.StartsWith(eca.TableName))
                {
                    result.ChildTableNames.Add(ceca.TableName);
                }
            }
            return result;
        }

        /// <summary>
        /// 菜单与数据库表对应关系
        /// </summary>
        protected class MenuTableRelation
        {
            public MenuTableRelation()
            {
                ChildTableNames = new List<string>();
            }

            /// <summary>
            /// 菜单名称
            /// </summary>
            public string MenuName { set; get; }
            /// <summary>
            /// 主表名称
            /// </summary>
            public string ParentTableName { set; get; }
            /// <summary>
            /// 子表集合
            /// </summary>
            public List<string> ChildTableNames { set; get; }

        }

        #endregion

        #region 重写基类

        protected override void InitOptions(title title, toolbox toobox, tooltip tootip)
        {
            title.text = "测试";
        }

        protected override string GetSqlString()
        {
            InitMenus();

            DateTime startTime = GetStartDay();
            //获取时间区间
            string timeWhere;
            if (string.IsNullOrEmpty(LengedName) || string.IsNullOrEmpty(DataName))  //周模式
            {
                timeWhere = GetTimeWheres(startTime, startTime.AddDays(7).AddSeconds(-1));
            }
            else
            {
                DateTime.TryParse(DataName, out startTime);
                timeWhere = GetTimeWheres(startTime, startTime.AddDays(1).AddSeconds(-1));
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < mtrs.Count; i++)
            {
                if (mtrs[i].ChildTableNames.Count > 0)
                {
                    sb.AppendLine(string.Format(SelectHasChild, mtrs[i].MenuName, mtrs[i].ParentTableName, mtrs[i].ChildTableNames[0], this.user.ID, timeWhere));
                }
                else
                {
                    sb.AppendLine(string.Format(SelectNoChild, mtrs[i].MenuName, mtrs[i].ParentTableName, this.user.ID, timeWhere));
                }
                if (i < mtrs.Count - 1)
                {
                    sb.AppendLine("UNION ALL");
                }
            }
            return sb.ToString();
        }

        protected override void SetDataConfig(DataTable dt, EChartsController.DataConfig dataConfig)
        {
            if (string.IsNullOrEmpty(LengedName) || string.IsNullOrEmpty(DataName))  //周模式
            {
                //维度
                Dimension dsn1 = new Dimension();
                dsn1.name = "Creationdate";
                dsn1.orderType = OrderType.DESC;
                dataConfig.dimensions.Add(dsn1);

                //指标
                Indicator ind1 = new Indicator();
                ind1.name = "主表";
                ind1.gatherType = GatherType.Count;
                Indicator ind2 = new Indicator();
                ind2.name = "子表";
                ind2.gatherType = GatherType.Count;
                dataConfig.indicators.Add(ind1);
                dataConfig.indicators.Add(ind2);
            }
            else   //日期模式
            {
                // 维度
                Dimension dsn1 = new Dimension();
                dsn1.name = "名称";  //图例列 名称必须是lenged
                dsn1.orderType = OrderType.DESC;
                dataConfig.dimensions.Add(dsn1);

                //指标
                Indicator ind1 = new Indicator();
                ind1.name = LengedName;
                ind1.gatherType = GatherType.Count;
                dataConfig.indicators.Add(ind1);
            }
        }

        protected override void SetEchartConfig(DataTable dt, EChartsController.EchartConfig ec)
        {
            if (string.IsNullOrEmpty(LengedName) || string.IsNullOrEmpty(DataName))  //周模式
            {
                ec.category = "Creationdate";
                ec.dataColumn.isSplit = false;
                ec.dataColumn.lengedType = "bar";
                ec.dataColumn.isSplit = true;
            }
            else   //日模式
            {
                ec.lenged = "名称";
                ec.dataColumn.name = LengedName;
                ec.dataColumn.lengedType = "pie";
                ec.dataColumn.isSplit = false;
            }
        }

        /// <summary>
        /// 一般此方法不能重写
        /// 但是查询结果集特殊，所以在此重写了此方法
        /// </summary>
        /// <param name="echartConfig"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected override DataTable ProcessingEchart(EChartsController.EchartConfig echartConfig, DataTable dt)
        {
            if (string.IsNullOrEmpty(LengedName) || string.IsNullOrEmpty(DataName))  //周模式
            {
                DataTable seriesDT = new DataTable();
                DataColumn column;
                column = new DataColumn();
                column.ColumnName = "lenged";   //图例列 列名必须固定
                seriesDT.Columns.Add(column);

                foreach (DataRow row in dt.Rows)
                {
                    column = new DataColumn();
                    column.ColumnName = row[echartConfig.category].ToString();
                    seriesDT.Columns.Add(column);
                }

                foreach (DataColumn col in dt.Columns)
                {
                    DataRow newRow = seriesDT.NewRow();
                    if (col.ColumnName == "主表" || col.ColumnName == "子表")
                    {
                        newRow["lenged"] = col.ColumnName;
                        foreach (DataRow row in dt.Rows)
                        {
                            newRow[row["Creationdate"].ToString()] = row[col.ColumnName];
                        }
                        seriesDT.Rows.Add(newRow);
                    }
                }
                return seriesDT;
            }
            else
            {
                DataTable seriesDT = new DataTable();
                DataColumn column;
                column = new DataColumn();
                column.ColumnName = "lenged";
                seriesDT.Columns.Add(column);
                column = new DataColumn();
                column.ColumnName = echartConfig.dataColumn.name;
                seriesDT.Columns.Add(column);

                foreach (DataRow row in dt.Rows)
                {
                    DataRow newRow = seriesDT.NewRow();
                    newRow["lenged"] = row["名称"];
                    newRow[echartConfig.dataColumn.name] = row[echartConfig.dataColumn.name];
                    seriesDT.Rows.Add(newRow);
                }
                return seriesDT;
            }
        }

        protected override void EndSetEchart(EChart echart)
        {
            ClearParams();
        }

        #endregion

        /// <summary>
        /// 获取开始时间
        /// 此处默认是周一  以后扩展为月 季度 年
        /// </summary>
        /// <returns></returns>
        private DateTime GetStartDay()
        {
            int index = 0;
            DayOfWeek dayIndex = DateTime.Now.DayOfWeek;
            if (dayIndex == DayOfWeek.Sunday)
            {
                index = 6;
            }
            else
            {
                index = (Int32)dayIndex - 1;
            }
            return DateTime.Now.AddDays(0 - index).Date;
        }

        /// <summary>
        /// 获取时间区间格式  where条件
        /// </summary>
        /// <returns></returns>
        private string GetTimeWheres(DateTime beginDT, DateTime endDT)
        {
            string strStart = beginDT.ToString("yyyy-MM-dd HH:mm:ss");
            string strEnd = endDT.ToString("yyyy-MM-dd HH:mm:ss");
            if (((EntityBase)this.model).Regulation.ToString().Contains("SQLServerRegulation"))  //sql server
            {
                SelectHasChild = "SELECT '{0}' 名称,1 主表,C.ID 子表,P.ISSUBMITED,p.ISREVIEWED,CONVERT(varchar(10),P.Creationdate,120) Creationdate FROM {1} P LEFT JOIN {2} C ON P.ID=C.ParentID  WHERE P.Createdby='{3}' {4}";
                SelectNoChild = "SELECT '{0}' 名称,1 主表,0 子表,P.ISSUBMITED,P.ISREVIEWED,CONVERT(varchar(10),P.Creationdate,120) Creationdate  FROM {1} P WHERE P.Createdby='{2}' {3}";
                return " AND P.Creationdate>='" + strStart + "' AND P.Creationdate<='" + strEnd + "' ";
            }
            else  //oracle
            {
                SelectHasChild = "SELECT '{0}' 名称,1 主表,C.ID 子表,P.ISSUBMITED,p.ISREVIEWED,trunc(P.Creationdate,'dd') Creationdate  FROM {1} P LEFT JOIN {2} C ON P.ID=C.ParentID  WHERE P.Createdby='{3}' {4}";
                SelectNoChild = "SELECT '{0}' 名称,1 主表,0 子表,P.ISSUBMITED,P.ISREVIEWED,trunc(P.Creationdate,'dd') Creationdate  FROM {1} P WHERE P.Createdby='{2}' {3}";
                return " AND P.Creationdate>=to_date('" + strStart + "','yyyy-mm-dd hh24:mi:ss') AND P.Creationdate<=to_date('" + strEnd + "','yyyy-mm-dd hh24:mi:ss') ";
            }
        }

        /// <summary>
        /// 清空参数
        /// </summary>
        private void ClearParams()
        {
            this.DataName = this.LengedName = string.Empty;
        }
    }
}
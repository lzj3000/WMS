using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Way.EAP.DataAccess.Data;
using System.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.Data;
using Acc.Contract.MVC;
using System.IO;

namespace Acc.Business.WMS.Controllers
{
    public class WareHouseController : BusinessController
    {
        /// <summary>
        /// 仓库管理控制器
        /// 创建人：柳强
        /// 创建时间：2012-12-24
        /// </summary>
        public WareHouseController() : base(new WareHouse()) { }
        public WareHouseController(IModel model) : base(model) { }
        public override bool IsReviewedState
        {
            get
            {
                return false;
            }
        }

        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }

        protected override string OnControllerName()
        {
            return "仓库管理";
        }

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/WareHouse/WareHouse.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "仓库管理";
        }

        protected override void OnInitViewChildItem(ModelData data, ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("WareHouse"))
            {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    //case "createdby":
                    //case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "isdisable":
                    case "isdelete":
                    case "state":
                    case "id":
                    case "address":
                    case "carphone":
                    case "stocktype":
                    case "sourcecode":
                    case "wbscode":
                    case "type":
                        item.visible = false;
                        break;
                    case "warhousename":
                        item.disabled = false;
                        break;
                    case "creationdate":
                    case "createdby":
                    case "issubmited":
                    case "isreviewed":
                        item.disabled = true;
                        break;
                    case "remark":
                        item.title = "地址";
                        item.visible = true;
                        break;
                    case "parentid":
                        item.title = "父级";
                        item.visible = true;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
            if (data.name.EndsWith("StockInfoMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    case "createdby":
                    case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "isdisable":
                    case "isdelete":
                    case "state":
                    case "id":
                    case "status":
                    case "salearea":
                    case "portcode":
                    case "housecode":
                    case "orderno":
                    case "issubmited":
                    case "isreviewed":
                    case "lastouttime":
                        item.visible = false;
                        break;
                    case "remark":
                        item.visible = true;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
            if (data.name.EndsWith("OWorker"))
            {
                data.title = "管理员";
                switch (item.field.ToLower())
                {
                    case "workname":
                        item.visible = true;
                        break;
                    default:
                        item.visible = false;
                        break;
                }
            }
        }

        /// <summary>
        /// 货区是否存在仓库根据
        /// </summary>
        /// <param name="whType"></param>
        /// <returns></returns>
        private void OnExistsWareHouse(Contract.MVC.ControllerBase.SaveEvent item)
        {
            IDataAction action = this.model.GetDataAction();
            WareHouse wh = item.Item as WareHouse;
            //DataTable dtname = null;
            Validate(wh);

        }

        private void Validate(WareHouse wh)
        {
            if (wh.WHTYPE == 0 && wh.PARENTID != 0)
            {
                throw new Exception("添加仓库不存在父级");
            }
            EntityList<WareHouse> whList = new EntityList<WareHouse>(this.model.GetDataAction());
            whList.GetData("id='" + wh.PARENTID + "'");
            if (whList.Count > 0)
            {
                if (wh.WHTYPE != whList[0].WHTYPE + 1)
                {
                    throw new Exception("异常：请逐级添加类型");
                }

                if (whList[0].PARENTID <= 0)
                {
                    wh.type = whList[0].ID;
                }
                else
                {
                    if (whList[0].PARENTID > 0)
                    {
                        whList.GetData("id='" + whList[0].PARENTID + "'");
                        if (whList.Count > 0)
                        {
                            if (whList[0].PARENTID <= 0)
                            {
                                wh.type = whList[0].ID;
                            }
                            else
                            {
                                whList.GetData("id='" + whList[0].PARENTID + "'");
                                if (whList.Count > 0)
                                {
                                    if (whList[0].PARENTID <= 0)
                                    {
                                        wh.type = whList[0].ID;
                                    }
                                    else
                                    {
                                        throw new Exception("异常：不能为货位添加子集");
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="item"></param>
        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
            //WareHouse wareHouse = item.Item as WareHouse;
            //item.Breakoff = true;
            // this.OnExistsWareHouse(wareHouse.WHTYPE.ToString(),wareHouse.PARENTID);
            this.OnExistsWareHouse(item);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        protected override void OnRemoveing(ControllerBase.SaveEvent item)
        {
            WareHouse wh = item.Item as WareHouse;
            EntityList<StockOutOrder> soo = new EntityList<StockOutOrder>(this.model.GetDataAction());
            soo.GetData("TOWHNO='" + wh.ID + "' and isdelete =0");
            if (soo.Count > 0)
            {
                throw new Exception("异常：删除项 " + wh.WAREHOUSENAME + "已关联出库单据不能删除！");
            }
            EntityList<StockInOrder> sio = new EntityList<StockInOrder>(this.model.GetDataAction());
            sio.GetData("TOWHNO='" + wh.ID + "' and isDelete=0");
            if (sio.Count > 0)
            {
                throw new Exception("异常：删除项 " + wh.WAREHOUSENAME + " 已关联入库单据不能删除！");
            }
            base.OnRemoveing(item);
        }

        protected override void OnReviewedData(Business.Model.BasicInfo info)
        {
            base.OnReviewedData(info);
        }


        #region 添加按钮方法
        [ActionCommand(name = "货位视图", title = "货位视图", index = 9, icon = "icon-search", isalert = true, onclick = "Layout", isselectrow = true, visible = true)]
        public void TestData()
        {
            Console.WriteLine("aaaa");
        }
        //[ActionCommand(name = "设置模板", title = "设置打印模板", index = 10, icon = "icon-ok", onclick = "SetPrintModel", isselectrow = false)]
        //public void SetPrintModel()
        //{
        //    //生成界面方法按钮用于权限控制，本方法无代码
        //}
        [ActionCommand(name = "生成货位", title = "生成库内货位", index = 4, icon = "icon-ok", isalert = false, onclick = "openAutoNext", isselectrow = true)]
        public void test1()
        {
            Console.WriteLine("aaaa");
        }

        #endregion

        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            return base.OnInitCommand(commands);
        }

        /// <summary>
        /// 加载所有仓库
        /// </summary>
        /// <returns></returns>
        [WhereParameter]
        public string whtype { get; set; }
        public string GetWareHouse()
        {
            IDataAction action = this.model.GetDataAction();
            string sql = string.Format("select ID,WAREHOUSENAME,WBSCODE,WHTYPE FROM " + this.ActionItem.ToString() + " where whtype='{0}'", whtype);
            DataTable dt = action.GetDataTable(sql);
            return Acc.Contract.JSON.Serializer(dt);
        }

        /// <summary>
        /// 加载所有仓库
        /// </summary>
        /// <returns></returns>
        [WhereParameter]
        public string warehouseID { get; set; }
        public string GetWareHouseByID()
        {
            IDataAction action = this.model.GetDataAction();
            string sql = string.Format("select ID,WAREHOUSENAME,WBSCODE,WHTYPE FROM " + this.ActionItem.ToString() + " where ID='{0}'", warehouseID);
            DataTable dt = action.GetDataTable(sql);
            return Acc.Contract.JSON.Serializer(dt);
        }

        /// <summary>
        /// 根据仓库编码查询货区
        /// </summary>
        [WhereParameter]
        public string wareHouseID { get; set; }
        public string GetWareHouseDepots()
        {
            IDataAction action = this.model.GetDataAction();
            string sql = string.Format("select ID,WAREHOUSENAME,WBSCODE,WHTYPE FROM " + this.ActionItem.ToString() + " where parentId='{0}' order by ID desc", wareHouseID);
            DataTable dt = action.GetDataTable(sql);
            return Acc.Contract.JSON.Serializer(dt);
        }

        /// <summary>
        /// 根据仓库编码查询货区
        /// </summary>
        public string GetWareHouseDepotsCount()
        {
            IDataAction action = this.model.GetDataAction();
            string sql = string.Format("select ID,WAREHOUSENAME,WBSCODE,WHTYPE FROM " + this.ActionItem.ToString() + " where whtype='2' order by ID desc");
            DataTable dt = action.GetDataTable(sql);
            return Acc.Contract.JSON.Serializer(dt);
        }

        /// <summary>
        /// 根据货位类型查询对应类型下面的具体货位（whtype=003）
        /// </summary>
        [WhereParameter]
        public string depotID { get; set; }//更名为warehousename（参数）
        public string GetAreaNum()
        {
            IDataAction action = this.model.GetDataAction();
            string sql = string.Format(
            "select w.id,w.warehousename,w.wbscode,w.whtype,w.parentid,count(wh.WAREHOUSENAME) childcount from acc_bus_warehouse w " +
            "inner join " + this.ActionItem.ToString() + " wh  on w.ID = wh.PARENTNO where w.parentid in(select ID from " + this.ActionItem.ToString() + " where WAREHOUSENAME = '{0}')" +
            "group by w.ID,w.WAREHOUSENAME,w.WBSCODE,w.WHTYPE,w.PARENTNO", depotID);
            DataTable dt = action.GetDataTable(sql);
            return Acc.Contract.JSON.Serializer(dt);
        }

        /// <summary>
        /// 根据仓库具体货位名称查询对应货位上产品的具体信息
        /// </summary>
        [WhereParameter]
        public string areaName { get; set; }
        public string GetAreaInfo()
        {
            IDataAction action = this.model.GetDataAction();
            string sql = string.Format("select sm.Code,sm.NUM,sm.LASTINTIME from Acc_Bus_StockInfo_Materials sm inner join " + this.ActionItem.ToString() + " wh on sm.DEPOTWBS = wh.WBSCODE where wh.WAREHOUSENAME = '{0}'", areaName);
            DataTable dt = action.GetDataTable(sql);
            return Acc.Contract.JSON.Serializer(dt);
        }

        ///// <summary>
        ///// 根据仓库具体货位名称查询对应货位上产品的具体信息
        ///// </summary>
        //[WhereParameter]
        //public string id { get; set; }
        //public string AutoNext()
        //{
        //    String[] str = id.Split(',');
        //    EntityList<WareHouse> whList = null;
        //    try
        //    {
        //        int id_key = int.Parse(str[0]);
        //        int whtype = int.Parse(str[1]);
        //        if (whtype == 3)
        //        {
        //            throw new Exception("最后一级不能添加下一级");
        //        }
        //        int startnum = int.Parse(str[2]);
        //        int endnum = int.Parse(str[3]);
        //        string nextname = str[4];
        //        int cnum = int.Parse(str[5]);
        //        string enname = str[6];
        //        IDataAction action = this.model.GetDataAction();
        //        whList = new EntityList<WareHouse>(this.model.GetDataAction());
        //        WareHouse wh = null;
        //        for (int i = startnum; i <= endnum; i++)
        //        {
        //            //循环开始数量到结束数量，根据id查询当前type并将insert语句的whtype的值自动设置
        //            ///添加warehousename值为nextname
        //            wh = new WareHouse();
        //            wh.PARENTID = id_key;
        //            wh.WAREHOUSENAME = nextname + i.ToString().PadLeft(cnum, '0')+enname;
        //            wh.type = id_key;
        //            wh.WHTYPE = whtype + 1;
        //            wh.ISDELSTATUS = false;
        //            wh.STATUS = "0";
        //            wh.Createdby = this.user.ID;
        //            wh.Creationdate = DateTime.Now;
        //            Validate(wh);
        //            whList.Add(wh);
        //        }
        //        whList.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return Acc.Contract.JSON.Serializer(whList);
        //}

        /// <summary>
        /// 根据仓库具体货位名称查询对应货位上产品的具体信息
        /// </summary>
        [WhereParameter]
        public string id { get; set; }
        public string AutoNext()
        {
            String[] str = id.Split(',');
            try
            {
                int id_key = int.Parse(str[0]);//行ID
                int setName = int.Parse(str[1]);//下拉框选择的值，1 代表货区 2代表货位
                int StartHNum = int.Parse(str[2]);//行开始数量
                int EndHNum = int.Parse(str[3]);//行结束数量
                int StartLNum = int.Parse(str[4]);//列开始数量
                int EndLNum = int.Parse(str[5]);//列结束数量
                int cengnum = int.Parse(str[6]);//层数量
                string sql = string.Empty;
                string tableName = new WareHouse().ToString();
                int parentid = id_key;
                string code = string.Empty;
                string warehouseName = string.Empty;
                int type = id_key;
                int whtype = setName;
                string createdby = this.user.ID;
                DateTime creationdate = DateTime.Now;
                string status = "0";
                for (int i = StartHNum; i <= EndHNum; i++)
                {
                    for (int j = StartLNum; j <= EndLNum; j++)
                    {
                        for (int z = 1; z <= cengnum; z++)
                        {
                            code = "H" + i.ToString().PadLeft(3, '0') + "-L" + j.ToString().PadLeft(3, '0') + "-" + z.ToString();
                            warehouseName = code;
                            sql = string.Format("insert into " + tableName + "(WAREHOUSENAME,WHTYPE,PARENTID,STATUS,type,code,ISDELSTATUS) values('{0}',{1},{2},{3},{4},'{5}',{6})", warehouseName, whtype, parentid, status, type, warehouseName, 0);
                            this.model.GetDataAction().Execute(sql);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return "";
        }

        private string filePath = "SysFiles\\WarehouseLayout";

        /// <summary>
        /// 仓库的平面图
        /// lsc 保存
        /// </summary>
        [WhereParameter]
        public string jsonWareHouse { get; set; }
        public string SaveWareHouse()
        {
            string basePath = ControllerCenter.GetCenter.printPath.Substring(0, ControllerCenter.GetCenter.printPath.Length - 5);
            string fullPath = basePath + filePath;
            string fullFileName = fullPath + "\\" + wareHouseID + ".txt";
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            if (File.Exists(fullFileName))
            {
                File.Delete(fullFileName);
            }
            using (StreamWriter sw = File.CreateText(fullFileName))
            {
                sw.Write(jsonWareHouse);
            }
            return "保存成功.";
        }

        /// <summary>
        /// 仓库的平面图
        /// lsc 查询
        /// </summary>
        public string GetWareHouseJson()
        {

            string basePath = ControllerCenter.GetCenter.printPath.Substring(0, ControllerCenter.GetCenter.printPath.Length - 5);
            string fullPath = basePath + filePath;
            string fullFileName = fullPath + "\\" + wareHouseID + ".txt";
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"config\":");
            if (File.Exists(fullFileName))
            {
                using (StreamReader sr = File.OpenText(fullFileName))
                {
                    string result = sr.ReadToEnd();
                    sb.Append(result);
                    sb.Append(",");
                }
            }
            else
            {
                sb.Append("null,");
            }
            sb.Append(GetValueJson());
            sb.Append("\"database\":");
            IDataAction action = this.model.GetDataAction();
            //首先查询当前仓库下面的货区和货位
            string pSql = string.Format("select ID,WAREHOUSENAME,CODE,WHTYPE,STATUS FROM Acc_WMS_WareHouse where parentId='{0}' and WHTYPE in(1,2) order by WHTYPE,CODE asc", wareHouseID);
            DataTable dt = action.GetDataTable(pSql);
            List<Section> sections = GetSections(dt);
            string dtbase = Acc.Contract.JSON.Serializer(sections);
            sb.Append(dtbase);
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 只是考虑货位，货区暂时排除
        /// lsc
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<Section> GetSections(DataTable dt)
        {
            List<Section> sections = new List<Section>();
            foreach (DataRow row in dt.Rows)
            {
                Section s = new Section();
                s.ID = row["ID"].ToString();
                s.Code = row["CODE"].ToString();
                s.Name = row["WAREHOUSENAME"].ToString();
                s.Status = row["STATUS"].ToString();
                s.WhType = row["WHTYPE"].ToString();
                if (row["WHTYPE"].ToString().Equals("1"))
                {
                    //多级暂时不考虑
                    //IDataAction action = this.model.GetDataAction();
                    //string sqlChildren = "select ID,WAREHOUSENAME,CODE,WHTYPE,STATUS FROM Acc_WMS_WareHouse where parentId='" + s.ID + "' and WHTYPE in(1,2) order by WHTYPE,CODE asc";
                    //DataTable dtChildren = action.GetDataTable(sqlChildren);
                    //s.Chidren = GetSections(dtChildren);
                }
                sections.Add(s);
            }
            return sections;
        }

        /// <summary>
        /// 货区(货位)
        /// lsc
        /// </summary>
        protected class Section
        {
            public Section()
            {
                Chidren = new List<Section>();
            }

            public string ID { set; get; } //id
            public string Code { set; get; }  //编码
            public string Name { set; get; } //名称
            public string Status { set; get; }  //状态
            public string WhType { set; get; }  //类型
            public List<Section> Chidren { set; get; }  //子节点
        }

        private string GetValueJson()
        {
            Dictionary<string, ValueTypePropertyAttribute> vtpa = ((IEntityBase)this.model).GetValueTypeProperty();
            if (vtpa.Keys.Contains("STATUS"))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"status\":[");
                foreach (PropertyValueType vtp in vtpa["STATUS"].GetValueTypeCollection)
                {
                    sb.Append("{\"value\":" + vtp.Value + ",\"text\":\"" + vtp.Text + "\"},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("],");
                return sb.ToString();
            }
            else
            {
                return "null,";
            }
        }
    }
}

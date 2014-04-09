using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.Outside;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Controllers;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Data;
using Acc.Contract.Data.ControllerData;
using Acc.Business.Model.Interface;

//Acc.Business.K3.Controllers.InterfaceLogsController
namespace Acc.Business.Controllers
{
    [AutoTimeClass]
    public class DataTransformController : BusinessController
    {

        public DataTransformController() : base(new MappingInfo()) { }

        #region 重写基类属性

        //显示在菜单
        protected override string OnControllerName()
        {
            return "数据转换";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/MappingInfo.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "胡文杰";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "数据转换";
        }

        #endregion

        /// <summary>
        /// 控制按钮
        /// </summary>
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            //获取所有按钮集合
            foreach (ActionCommand ac in commands)
            {
                if (ac.command == "remove" || ac.command == "add" || ac.command == "SubmitData")
                {
                    ac.visible = false;
                }
            }
            return base.OnInitCommand(commands);
        }

        [ActionCommand(name = "转换数据", title = "转换数据", index = 1, icon = "icon-ok", isselectrow = false)]
        public void TransformData()
        {
            MappingInfo mi = this.ActionItem as MappingInfo;
            if (mi != null)
            {
                if (mi.TemplateID < 1)
                {
                    throw new Exception("请选择接口模板！");
                    return;
                }
                //外系统连接
                InterfaceTemplate connect = mi.GetForeignObject<InterfaceTemplate>(this.model.GetDataAction());
                //IDataAction outDa = new DataBaseManage(new SQLServerDataBase(connect.ServerIP, connect.DBName, connect.DBUserName, "sa_AccTrue"));
                IDataAction outDa = new DataBaseManage(new SQLServerDataBase(connect.ServerIP, connect.DBName, connect.DBUserName, connect.DBPassword));
                //IDataAction da = new DataBaseManage(new SQLServerDataBase("192.168.20.137", "K3", "sa", "2012"));
                //重置转换记录
                //mi.TransformCount = 0;
                //mi.SuccessCount = 0;

                //转换数据
                switch (mi.XmlName)
                {
                    //case "Organization.xml"://部门
                    //    DepartmentMappingController dmc = new DepartmentMappingController();
                    //    dmc.ExectueMain(outDa, bill);
                    //    break;
                    //case "Organization1.xml"://客户
                    //    DepartmentMappingController1 dmc1 = new DepartmentMappingController1();
                    //    dmc1.ExectueMain(outDa, bill);
                    //    break;
                    //case "OfficeWorker.xml"://职员
                    //    OfficeWorkerMappingController omc = new OfficeWorkerMappingController();
                    //    omc.ExectueMain(outDa, bill);
                    //    break;
                    //case "Unit.xml"://计量单位
                    //    UnitMappingController umc = new UnitMappingController();
                    //    umc.ExectueMain(outDa, mi);
                    //    break;
                    //case "Stock.xml"://仓库
                    //    StockMappingController smc = new StockMappingController();
                    //    smc.ExectueMain(outDa, bill);
                    //    break;
                    //case "Commodity.xml"://商品
                    //    CommodityMappingController cmc = new CommodityMappingController();
                    //    cmc.ExectueMain(outDa, bill);
                    //    break;
                    //case "Customer.xml"://客户
                    //    CustomerMappingController cusmc = new CustomerMappingController();
                    //    cusmc.ExectueMain(outDa, bill);
                    //    break;
                    //case "Contact.xml"://联系人
                    //    ContactMappingController conmc = new ContactMappingController();
                    //    conmc.ExectueMain(outDa, bill);
                    //    break;
                    //case "StockOutNotice.xml":// 出库通知
                    //    if (bill.SourceSystem == "k3")
                    //    {
                    //        StockOutNoticeMappingController sonmc = new StockOutNoticeMappingController();
                    //        sonmc.ExectueMain(outDa, bill);
                    //    }
                    //    //else if (bill.SourceSystem == "crm")
                    //    //{
                    //    //    //RStockOutOrderMappingController rsoomc = new RStockOutOrderMappingController();
                    //    //    //rsoomc.ExectueMain(outDa, bill);
                    //    //}
                    //    break;
                    //case "StockOutOrder.xml":// 出库
                    //    if (bill.SourceSystem == "k3")
                    //    {
                    //        //ContractMappingController contractmc = new ContractMappingController();
                    //        //contractmc.ExectueMain(outDa, bill);
                    //    }
                    //    else if (bill.SourceSystem == "crm")
                    //    {
                    //        RStockOutOrderMappingController rsoomc = new RStockOutOrderMappingController();
                    //        rsoomc.ExectueMain(outDa, bill);
                    //    }
                    //    break;

                    //case "StockInNotice.xml":// 入库通知
                    //    if (bill.SourceSystem == "k3")
                    //    {
                    //        StockInNoticeMappingController sonmc = new StockInNoticeMappingController();
                    //        sonmc.ExectueMain(outDa, bill);
                    //    }
                    //    break;
                    //case "StockInOrder.xml":// 入库
                    //    if (bill.SourceSystem == "k3")
                    //    {
                    //        //ContractMappingController contractmc = new ContractMappingController();
                    //        //contractmc.ExectueMain(outDa, bill);
                    //    }
                    //    else if (bill.SourceSystem == "crm")
                    //    {
                    //        RStockInOrderMappingController rsoomc = new RStockInOrderMappingController();
                    //        rsoomc.ExectueMain(outDa, bill);
                    //    }
                    //    break;
                    //case "Contract.xml":
                    //    if (bill.SourceSystem == "k3")
                    //    {
                    //        //ContractMappingController contractmc = new ContractMappingController();
                    //        //contractmc.ExectueMain(outDa, bill);
                    //    }
                    //    else if (bill.SourceSystem == "crm")
                    //    {
                    //        //RContractMappingController rcontractmc = new RContractMappingController();
                    //        //rcontractmc.ExectueMain(outDa, bill);
                    //    }
                    //    break;
                    //case "DispatchNote.xml":
                    //    if (bill.SourceSystem == "k3")
                    //    {
                    //        DispatchNoteMappingController dnmc = new DispatchNoteMappingController();
                    //        dnmc.ExectueMain(outDa, bill);
                    //    }
                    //    //else if (bill.SourceSystem == "crm")
                    //    //{
                    //    //    //RStockOutOrderMappingController rsoomc = new RStockOutOrderMappingController();
                    //    //    //rsoomc.ExectueMain(outDa, bill);
                    //    //}
                    //    break;
                    default:
                        break;
                }

                ////转换数据结果记录
                //EntityList<DataTransform> dtList = new EntityList<DataTransform>(this.model.GetDataAction());
                //dtList.Add(bill);
                //dtList.Save();


            }
        }

        //[ActionCommand(name = "转换数据", title = "转换数据", index = 1, icon = "icon-ok", isselectrow = false)]
        //public void TransformData()
        //{
        //    DataTransform bill = this.ActionItem as DataTransform;
        //    if (bill != null)
        //    {
        //        if (bill.TemplateID < 1)
        //        {
        //            throw new Exception("请选择接口模板！");
        //            return;
        //        }
        //        //外系统连接
        //        InterfaceTemplate connect = bill.GetForeignObject<InterfaceTemplate>(this.model.GetDataAction());
        //        //IDataAction outDa = new DataBaseManage(new SQLServerDataBase(connect.ServerIP, connect.DBName, connect.DBUserName, "sa_AccTrue"));
        //        IDataAction outDa = new DataBaseManage(new SQLServerDataBase(connect.ServerIP, connect.DBName, connect.DBUserName, connect.DBPassword));
        //        //IDataAction da = new DataBaseManage(new SQLServerDataBase("192.168.20.137", "K3", "sa", "2012"));
        //        //重置转换记录
        //        bill.TransformCount = 0;
        //        bill.SuccessCount = 0;

        //        //转换数据
        //        switch (bill.XmlName)
        //        {
        //            case "Organization.xml"://部门
        //                DepartmentMappingController dmc = new DepartmentMappingController();
        //                dmc.ExectueMain(outDa, bill);
        //                break;
        //            case "Organization1.xml"://客户
        //                DepartmentMappingController1 dmc1 = new DepartmentMappingController1();
        //                dmc1.ExectueMain(outDa, bill);
        //                break;
        //            case "OfficeWorker.xml"://职员
        //                OfficeWorkerMappingController omc = new OfficeWorkerMappingController();
        //                omc.ExectueMain(outDa, bill);
        //                break;
        //            case "Unit.xml"://计量单位
        //                UnitMappingController umc = new UnitMappingController();
        //                umc.ExectueMain(outDa, bill);
        //                break;
        //            case "Stock.xml"://仓库
        //                StockMappingController smc = new StockMappingController();
        //                smc.ExectueMain(outDa, bill);
        //                break;
        //            case "Commodity.xml"://商品
        //                CommodityMappingController cmc = new CommodityMappingController();
        //                cmc.ExectueMain(outDa, bill);
        //                break;
        //            case "Customer.xml"://客户
        //                CustomerMappingController cusmc = new CustomerMappingController();
        //                cusmc.ExectueMain(outDa, bill);
        //                break;
        //            case "Contact.xml"://联系人
        //                ContactMappingController conmc = new ContactMappingController();
        //                conmc.ExectueMain(outDa, bill);
        //                break;
        //            case "StockOutNotice.xml":// 出库通知
        //                if (bill.SourceSystem == "k3")
        //                {
        //                    StockOutNoticeMappingController sonmc = new StockOutNoticeMappingController();
        //                    sonmc.ExectueMain(outDa, bill);
        //                }
        //                //else if (bill.SourceSystem == "crm")
        //                //{
        //                //    //RStockOutOrderMappingController rsoomc = new RStockOutOrderMappingController();
        //                //    //rsoomc.ExectueMain(outDa, bill);
        //                //}
        //                break;
        //            case "StockOutOrder.xml":// 出库
        //                if (bill.SourceSystem == "k3")
        //                {
        //                    //ContractMappingController contractmc = new ContractMappingController();
        //                    //contractmc.ExectueMain(outDa, bill);
        //                }
        //                else if (bill.SourceSystem == "crm")
        //                {
        //                    RStockOutOrderMappingController rsoomc = new RStockOutOrderMappingController();
        //                    rsoomc.ExectueMain(outDa, bill);
        //                }
        //                break;

        //            case "StockInNotice.xml":// 入库通知
        //                if (bill.SourceSystem == "k3")
        //                {
        //                    StockInNoticeMappingController sonmc = new StockInNoticeMappingController();
        //                    sonmc.ExectueMain(outDa, bill);
        //                }
        //                break;
        //            case "StockInOrder.xml":// 入库
        //                if (bill.SourceSystem == "k3")
        //                {
        //                    //ContractMappingController contractmc = new ContractMappingController();
        //                    //contractmc.ExectueMain(outDa, bill);
        //                }
        //                else if (bill.SourceSystem == "crm")
        //                {
        //                    RStockInOrderMappingController rsoomc = new RStockInOrderMappingController();
        //                    rsoomc.ExectueMain(outDa, bill);
        //                }
        //                break;
        //            //case "Contract.xml":
        //            //    if (bill.SourceSystem == "k3")
        //            //    {
        //            //        //ContractMappingController contractmc = new ContractMappingController();
        //            //        //contractmc.ExectueMain(outDa, bill);
        //            //    }
        //            //    else if (bill.SourceSystem == "crm")
        //            //    {
        //            //        //RContractMappingController rcontractmc = new RContractMappingController();
        //            //        //rcontractmc.ExectueMain(outDa, bill);
        //            //    }
        //            //    break;
        //            //case "DispatchNote.xml":
        //            //    if (bill.SourceSystem == "k3")
        //            //    {
        //            //        DispatchNoteMappingController dnmc = new DispatchNoteMappingController();
        //            //        dnmc.ExectueMain(outDa, bill);
        //            //    }
        //            //    //else if (bill.SourceSystem == "crm")
        //            //    //{
        //            //    //    //RStockOutOrderMappingController rsoomc = new RStockOutOrderMappingController();
        //            //    //    //rsoomc.ExectueMain(outDa, bill);
        //            //    //}
        //            //    break;
        //            default:
        //                break;
        //        }

        //        //转换数据结果记录
        //        EntityList<DataTransform> dtList = new EntityList<DataTransform>(this.model.GetDataAction());
        //        dtList.Add(bill);
        //        dtList.Save();


        //    }
        //}


        //[ActionCommand(name = "基础数据导入", title = "基础数据导入", index = 1, icon = "icon-ok", isselectrow = false)]
        //public void TransformDataAll()
        //{
        //    //string itName= this.InterfaceTemplateName;
        //    //EntityList<InterfaceTemplate> its = new EntityList<InterfaceTemplate>(this.model.GetDataAction());//源数据集
        //    //its.GetData("TemplateName='" + itName + "'");
        //    //if (its.Count == 0)
        //    //{
        //    //    throw new Exception("请填写接口模版参数！");
        //    //}
        //    //InterfaceTemplate it = its[0];
        //    //DateTime dt=TimeSpan.Parse("").;
        //    IEntityList billList = new EntityList<DataTransform>(this.model.GetDataAction());//源数据集
        //    billList.GetData();
        //    //billList.GetData("TemplateID =" + it.ID);
        //    foreach (DataTransform bill in billList)
        //    {
        //        if (bill.TemplateID == 10)
        //        { continue; }
        //        //外系统连接
        //        InterfaceTemplate connect = bill.GetForeignObject<InterfaceTemplate>(this.model.GetDataAction());
        //        IDataAction outDa = new DataBaseManage(new SQLServerDataBase(connect.ServerIP, connect.DBName, connect.DBUserName, "sa_AccTrue"));
        //        //IDataAction outDa = new DataBaseManage(new SQLServerDataBase(connect.ServerIP, connect.DBName, connect.DBUserName, connect.DBPassword));
        //        //IDataAction da = new DataBaseManage(new SQLServerDataBase("192.168.20.137", "K3", "sa", "2012"));
        //        //重置转换记录
        //        bill.TransformCount = 0;
        //        bill.SuccessCount = 0;
        //        //转换数据
        //        switch (bill.XmlName)
        //        {
        //            case "Organization.xml"://部门
        //                DepartmentMappingController dmc = new DepartmentMappingController();
        //                dmc.ExectueMain(outDa, bill);
        //                break;
        //            case "OfficeWorker.xml"://职员
        //                OfficeWorkerMappingController omc = new OfficeWorkerMappingController();
        //                omc.ExectueMain(outDa, bill);
        //                break;
        //            case "Unit.xml"://计量单位
        //                UnitMappingController umc = new UnitMappingController();
        //                umc.ExectueMain(outDa, bill);
        //                break;
        //            case "Stock.xml"://仓库
        //                StockMappingController smc = new StockMappingController();
        //                smc.ExectueMain(outDa, bill);
        //                break;
        //            case "Commodity.xml"://商品
        //                CommodityMappingController cmc = new CommodityMappingController();
        //                cmc.ExectueMain(outDa, bill);
        //                break;
        //            case "Customer.xml"://客户
        //                CustomerMappingController cusmc = new CustomerMappingController();
        //                cusmc.ExectueMain(outDa, bill);
        //                break;
        //            //case "Contact.xml"://联系人
        //            //    ContactMappingController conmc = new ContactMappingController();
        //            //    conmc.ExectueMain(outDa, bill);
        //            //    break;
        //            default:
        //                break;
        //        }

        //    }
        //    billList.Save();
        //    //throw new Exception("基础数据导入完成！");
        //}

        //[AutoTimeMethod]
        //public string TransformBasicData()
        //{
        //    string result = "";
        //    string itName = this.InterfaceTemplateName;
        //    EntityList<InterfaceTemplate> its = new EntityList<InterfaceTemplate>(this.model.GetDataAction());//源数据集
        //    its.GetData("TemplateName='" + itName + "'");
        //    if (its.Count == 0)
        //    {
        //        throw new Exception("请填写接口模版参数！");
        //    }
        //    InterfaceTemplate it = its[0];
        //    //DateTime dt=TimeSpan.Parse("").;
        //    IEntityList billList = new EntityList<DataTransform>(this.model.GetDataAction());//源数据集
        //    billList.GetData();
        //    //billList.GetData("TemplateID =" + it.ID);
        //    foreach (DataTransform bill in billList)
        //    {
        //        if (bill.TemplateID == 10)
        //        { continue; }
        //        //外系统连接
        //        InterfaceTemplate connect = bill.GetForeignObject<InterfaceTemplate>(this.model.GetDataAction());
        //        IDataAction outDa = new DataBaseManage(new SQLServerDataBase(connect.ServerIP, connect.DBName, connect.DBUserName, "sa_AccTrue"));
        //        //IDataAction outDa = new DataBaseManage(new SQLServerDataBase(connect.ServerIP, connect.DBName, connect.DBUserName, connect.DBPassword));
        //        //IDataAction da = new DataBaseManage(new SQLServerDataBase("192.168.20.137", "K3", "sa", "2012"));
        //        //重置转换记录
        //        bill.TransformCount = 0;
        //        bill.SuccessCount = 0;
        //        //转换数据
        //        switch (bill.XmlName)
        //        {
        //            case "Organization.xml"://部门
        //                DepartmentMappingController dmc = new DepartmentMappingController();
        //                dmc.ExectueMain(outDa, bill);
        //                result += bill.Remark + "执行成功" + bill.TransformCount + "条；";
        //                break;
        //            case "OfficeWorker.xml"://职员
        //                OfficeWorkerMappingController omc = new OfficeWorkerMappingController();
        //                omc.ExectueMain(outDa, bill);
        //                result += bill.Remark + "执行成功" + bill.TransformCount + "条；";
        //                break;
        //            case "Unit.xml"://计量单位
        //                UnitMappingController umc = new UnitMappingController();
        //                umc.ExectueMain(outDa, bill);
        //                result += bill.Remark + "执行成功" + bill.TransformCount + "条；";
        //                break;
        //            case "Stock.xml"://仓库
        //                StockMappingController smc = new StockMappingController();
        //                smc.ExectueMain(outDa, bill);
        //                result += bill.Remark + "执行成功" + bill.TransformCount + "条；";
        //                break;
        //            case "Commodity.xml"://商品
        //                CommodityMappingController cmc = new CommodityMappingController();
        //                cmc.ExectueMain(outDa, bill);
        //                result += bill.Remark + "执行成功" + bill.TransformCount + "条；";
        //                break;
        //            case "Customer.xml"://客户
        //                CustomerMappingController cusmc = new CustomerMappingController();
        //                cusmc.ExectueMain(outDa, bill);
        //                result += bill.Remark + "执行成功" + bill.TransformCount + "条；";
        //                break;
        //            //case "Contact.xml"://联系人
        //            //    ContactMappingController conmc = new ContactMappingController();
        //            //    conmc.ExectueMain(outDa, bill);
        //            //result += bill.Remark + "执行成功" + bill.TransformCount + "条；";
        //            //    break;
        //            default:
        //                break;
        //        }

        //    }
        //    billList.Save();
        //    return result;
        //    //throw new Exception("基础数据导入完成！");
        //}

        /// <summary>
        /// 接口模版
        /// </summary>
        [AutoTimeProperty]
        [WhereParameter]
        public string InterfaceTemplateName { get; set; }

    }
}

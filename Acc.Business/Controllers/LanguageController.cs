using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.Model;
using Acc.Contract.MVC;
using Acc.Contract.Data;
using Acc.Contract.Data.ControllerData;
using System.Xml;
using System.IO;
using System.Data;
using System.Web;
using Acc.Contract;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Controllers
{
    public class LanguageController:BusinessController
    {
        public static string name{get;set;}
        public static bool isbtn=true; 
        /// <summary>
        /// 描述：包装管理控制器
        /// 作者：路聪
        /// 创建日期:2012-12-25
        /// </summary>
        
        public LanguageController() : base() { }
        
        //是否启用审核
        public override bool IsReviewedState
        {
            get
            {
                return false;
            }
        }
        //是否启用提交
        public override bool IsSubmit
        {
            get
            {
                return false;
            }
        }
        //是否启用回收站
        public override bool IsClearAway
        {
            get
            {
                return true;
            }
        }

        public void langchenge()
        { 
        
        }
        [WhereParameter] 
        public string classids { get; set; }
        public string CGSQ()
        {

            string str = classids;
            string filePath = "C:\\infor.txt";//这里是你的已知文件
            
            if (File.Exists(filePath))
            {
                
                Console.WriteLine("{0} already exists.", filePath);                
            }
            StreamWriter sr = File.CreateText(filePath);
            sr.Write(str);
            sr.Close();
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            fs.SetLength(0);//首先把文件清空了。
            sw.Write(str);//写你的字符串。            
            sw.Close();
            
            OfficeWorker mb = new OfficeWorker();
            EntityList<SystemModel> list = new EntityList<SystemModel>(mb.GetDataAction());
            for (int i = 0; i < list.Count; i++)
            {
                list[i].ModelName = "aa";
            }
            if (isbtn)
            {
                if (classids == "btn"&&classids!=null)
                {
                    IDataAction action = mb.GetDataAction();
                    string ApthModelDataToEnglish = "/XML/ModelDataToEnglish.xml";
                    string ApthModelData = "/XML/ModelData.xml";
                    string ApthTitleName = "/XML/titleName.xml";
                    string ApthSystemModel = "/XML/apthSystemModel.xml";
                    string ApthSDtory = "/XML/ApthSDtory.xml";//主目录
                    string ApthSMdel = "/XML/ApthSMdel.xml";//子目录
                    string sql="select * from Acc_Bus_ModelData";
                    string sqltitlename = "select distinct TitleName,ModelTableName,parentID from Acc_Bus_ModelData ";
                    string sqlSystemModel = "select * from  Acc_Bus_ModelCommand";
                    string sqlSDtory = "select * from Acc_Bus_SystemDirectory order by parentid,OrderIndex";//主目录
                    string sqlSMdel;
                    if (user.IsAdministrator)//子目录
                        sqlSMdel = "select * from Acc_Bus_SystemModel where IsDisable=0 order by SystemDirectoryID,OrderIndex";
                    else
                        sqlSMdel = "  select distinct c.* from Acc_Bus_UserRole a,Acc_Bus_RoleModel b,Acc_Bus_SystemModel c where a.RoleID=b.RoleID and b.ModelID=c.ID and a.UserID=" + user.ID + " order by c.SystemDirectoryID,c.OrderIndex";
                    DataTable Tabletitlename = action.GetDataTable(sqltitlename);
                    DataTable TableSystemModel = action.GetDataTable(sqlSystemModel);
                    for (int i = 0; i < Tabletitlename.Rows.Count; i++)
                    {
                        string[] aa = (Tabletitlename.Rows[i]["ModelTableName"].ToString()).Split('_');
                        int cc=aa.Count();
                        if (cc<3)
                        {
                            Tabletitlename.Rows[i]["ModelTableName"] = Tabletitlename.Rows[i]["ModelTableName"].ToString();  
                        }
                        else
                        {
                            Tabletitlename.Rows[i]["ModelTableName"] = (Tabletitlename.Rows[i]["ModelTableName"].ToString()).Split('_')[2].ToString();    
                        }
                        
                    }
                    DataTable table = action.GetDataTable(sql);
                    DataTable table1 = action.GetDataTable(sql);
                    DataTable tableSDtory = action.GetDataTable(sqlSDtory);
                    DataTable tableSMdel = action.GetDataTable(sqlSMdel);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {                        
                        table.Rows[i]["title"]=table.Rows[i]["field"].ToString();
                    }
                    CDataToXmlFile(TableSystemModel, ApthSystemModel);
                    CDataToXmlFile(Tabletitlename, ApthTitleName);
                    CDataToXmlFile(table, ApthModelDataToEnglish);
                    CDataToXmlFile(table1, ApthModelData);
                    CDataToXmlFile(tableSDtory, ApthSDtory);
                    CDataToXmlFile(tableSMdel, ApthSMdel);
                    isbtn = false;
                }
            }
            name = classids;
            
            return "";
            //return classids;
        }
        private void setcommand(IDataAction action, AcctrueUser user)
        {
            string sql = "select distinct d.ControllerName,c.command "
                         + " from Acc_Bus_UserRole a,"
                         + " Acc_Bus_RoleModel b,"
                         + " Acc_Bus_RoleCommand c,"
                         + " Acc_Bus_SystemModel d"
                         + " where a.RoleID=b.RoleID and b.ID=c.RoleModelID and c.ParentID=d.ID and a.UserID=" + user.ID;
            DataTable t = action.GetDataTable(sql);
            InitData data = null;
            foreach (DataRow r in t.Rows)
            {
                string cn = r["ControllerName"].ToString();
                if (data == null || data.name != cn)
                {
                    data = new InitData();
                    data.name = cn;
                    List<ActionCommand> al = new List<ActionCommand>();
                    DataRow[] rs = t.Select("ControllerName='" + cn + "'");
                    foreach (DataRow d in rs)
                    {
                        ActionCommand ac = new ActionCommand();
                        ac.command = d["command"].ToString();
                        ac.visible = false;
                        al.Add(ac);
                    }
                    data.commands = al.ToArray();
                    user.AddControll(data);
                }
            }
        }
        private void SetAdminUser(AcctrueUser user, IDataAction action)
        {
            List<Dic> list = new List<Dic>();
            Dic dic = new Dic();
            dic.ID = 0;
            dic.index = 0;
            dic.Name = "系统管理员功能";
            DicMenu d1 = new DicMenu();
            d1.name = "系统目录管理";
            d1.url = "Views/manager/systemdirectory.htm";
            d1.index = 1;
            dic.AddMenu(d1);
            DicMenu d2 = new DicMenu();
            d2.name = "系统模块管理";
            d2.url = "Views/manager/systemmodel.htm";
            d2.index = 2;
            dic.AddMenu(d2);

            DicMenu d3 = new DicMenu();
            d3.name = "系统角色管理";
            d3.url = "Views/manager/role.htm";
            d3.index = 3;
            dic.AddMenu(d3);

            DicMenu d5 = new DicMenu();
            d5.name = "系统观察器管理";
            d5.url = "Views/manager/observermanage.htm";
            d5.index = 4;
            dic.AddMenu(d5);

            DicMenu d4 = new DicMenu();
            d4.name = "登陆用户管理";
            d4.url = "Views/manager/systemmanager.htm";
            d4.index = 5;
            dic.AddMenu(d4);

            user.AddChild(dic);

            Dic[] ds = getdic(action, user, true);
            foreach (Dic dd in ds)
                user.AddChild(dd);
        }
        public Dic[] getdic(IDataAction action, AcctrueUser user, bool isadmin)
        {
            string sql = "select * from Acc_Bus_SystemDirectory order by parentid,OrderIndex";
            DataTable tt = action.GetDataTable(sql);
            //目录转换
            string apth = "/XML/upload.xml";

            CDataToXmlFile(tt, apth);

            DataTable t = CXmlToDataTable(apth);
            List<Dic> list = new List<Dic>();
            Dic d = null;
            string sql1;
            if (isadmin)
                sql1 = "select * from Acc_Bus_SystemModel where IsDisable=0 order by SystemDirectoryID,OrderIndex";
            else
                sql1 = "  select distinct c.* from Acc_Bus_UserRole a,Acc_Bus_RoleModel b,Acc_Bus_SystemModel c where a.RoleID=b.RoleID and b.ModelID=c.ID and a.UserID=" + user.ID + " order by c.SystemDirectoryID,c.OrderIndex";
            DataTable tmm = action.GetDataTable(sql1);
            CDataToXmlFile(tmm, apth);

            DataTable tm = CXmlToDataTable(apth);
            DataRow[] rows = t.Select("parentid=0");
            DataRow[] rrm;
            foreach (DataRow r in rows)
            {
                d = new Dic();
                
                d.ID = Convert.ToInt32(r["ID"]);
                d.Name = Convert.ToString(r["DirectoryName"]);
                d.index = Convert.ToInt32(r["OrderIndex"]);
                rrm = tm.Select("SystemDirectoryID=" + d.ID);
                
                foreach (DataRow m in rrm)
                {
                    SystemModel model = new SystemModel();
                    foreach (DataColumn c in m.Table.Columns)
                        model[c.ColumnName] = m[c];
                    DicMenu dm = new DicMenu();
                    dm.name = model.ModelName;
                    dm.url = model.Url;
                    dm.desc = model.Remark;
                    dm.id = model.ID;
                    dm.mmm = model;
                    dm.index = model.OrderIndex;
                    d.AddMenu(dm);
                    
                }
                childdic(d, t, tm);
                list.Add(d);
                list.ToArray();
            }
            
            return list.ToArray();
        }
        private void childdic(Dic d, DataTable t, DataTable tm)
        {
            DataRow[] rows = t.Select("parentid=" + d.ID);
            Dic cd = null;
            DataRow[] rrm;
            foreach (DataRow r in rows)
            {
                cd = new Dic();
                cd.ID = Convert.ToInt32(r["ID"]);
                cd.Name = Convert.ToString(r["DirectoryName"]);
                cd.index = Convert.ToInt32(r["OrderIndex"]);
                d.Add(cd);
                rrm = tm.Select("SystemDirectoryID=" + cd.ID);
                foreach (DataRow m in rrm)
                {
                    SystemModel model = new SystemModel();
                    foreach (DataColumn c in m.Table.Columns)
                        model[c.ColumnName] = m[c];
                    DicMenu dm = new DicMenu();
                    dm.name = model.ModelName;
                    dm.url = model.Url;
                    dm.desc = model.Remark;
                    dm.id = model.ID;
                    dm.mmm = model;
                    dm.index = model.OrderIndex;
                    cd.AddMenu(dm);
                }
                childdic(cd, t, tm);
            }
        }
        /**/
        /// <summary>
        /// 将DataSet对象数据保存为XML文件
        /// </summary>
        /// <param name="dt">DataSet</param>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <returns>bool值</returns>
        public  bool CDataToXmlFile(DataTable dt, string xmlFilePath)
        {
            if ((dt != null) && (!string.IsNullOrEmpty(xmlFilePath)))
            {
                string path = HttpContext.Current.Server.MapPath(xmlFilePath);
                MemoryStream ms = null;
                XmlTextWriter XmlWt = null;
                try
                {
                    ms = new MemoryStream();
                    //根据ms实例化XmlWt
                    XmlWt = new XmlTextWriter(ms, Encoding.Unicode);
                    //获取ds中的数据
                    dt.WriteXml(XmlWt);
                    int count = (int)ms.Length;
                    byte[] temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);
                    //返回Unicode编码的文本
                    UnicodeEncoding ucode = new UnicodeEncoding();
                    //写文件
                    StreamWriter sw = new StreamWriter(path);
                    //sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?> ");
                    sw.WriteLine(ucode.GetString(temp).Trim());
                    sw.Close();
                    return true;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源
                    if (XmlWt != null)
                    {
                        XmlWt.Close();
                        ms.Close();
                        ms.Dispose();
                    }
                }
            }
            else
            {
                return false;
            }
        }
        /**/
        /// <summary>
        /// 读取Xml文件信息,并转换成DataSet对象
        /// </summary>
        /// <remarks>
        /// DataSet ds = new DataSet();
        /// ds = CXmlFileToDataSet("/XML/upload.xml");
        /// </remarks>
        /// <param name="xmlFilePath">Xml文件地址</param>
        /// <returns>DataSet对象</returns>
        public static DataSet CXmlFileToDataSet(string xmlFilePath)
        {

            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                string path = HttpContext.Current.Server.MapPath(xmlFilePath);

                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    XmlDocument xmldoc = new XmlDocument();
                    //根据地址加载Xml文件
                    xmldoc.Load(path);

                    DataSet ds = new DataSet();
                    //读取文件中的字符流
                    StrStream = new StringReader(xmldoc.InnerXml);
                    //获取StrStream中的数据
                    Xmlrdr = new XmlTextReader(StrStream);
                    //ds获取Xmlrdr中的数据
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    //释放资源
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                        StrStream.Dispose();
                    }
                }
            }
            else
            {
                return null;
            }
        }
        /**/
        /// <summary>
        /// 读取Xml文件信息,并转换成DataTable对象
        /// </summary>
        /// <param name="xmlFilePath">xml文江路径</param>
        /// <returns>DataTable对象</returns>
        public  DataTable CXmlToDataTable(string xmlFilePath)
        {
            return CXmlFileToDataSet(xmlFilePath).Tables[0];
        }
        //public string login(string name,string pass)
        //{
        //    return "";
        //}
    }
}

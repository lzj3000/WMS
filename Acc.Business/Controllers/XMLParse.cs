using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model.Interface;
using Acc.Contract.Data.ControllerData;
using System.IO;
using System.Data;

namespace Acc.Business.Controllers
{
    public  class XMLParse
    {
        static string ur = AppDomain.CurrentDomain.BaseDirectory;
        public static Dictionary<string, string> XmlRead(string xmlName)
        {
            //string url = "E:\\TSF-Project\\CRM_WMS\\AcctrueCRM\\Acc.K3\\Mapping\\" + xmlName;
            string url = ur + "Interface\\K3\\Mapping\\" + xmlName;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(url);
            XmlNodeList colPare = xmldoc.SelectNodes("//ColProperty");

            foreach (XmlElement element in colPare)
            {
                string col = element.GetElementsByTagName("name")[0].InnerText;
                string col1 = element.GetElementsByTagName("Kname")[0].InnerText;
                dic.Add(col, col1);
            }
            return dic;
        }
        
        public static Dictionary<string, string> XmlGetTableName(string xmlName)
        {
            //string url = "E:\\TSF-Project\\CRM_WMS\\AcctrueCRM\\Acc.K3\\Mapping\\" + xmlName;
            string url = ur + "Interface\\K3\\Mapping\\" + xmlName;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(url);
            XmlNodeList colPare = xmldoc.SelectNodes("//TableProperty");

            foreach (XmlElement element in colPare)
            {
                string col = element.GetElementsByTagName("name")[0].InnerText;
                string col1 = element.GetElementsByTagName("Kname")[0].InnerText;
                dic.Add(col, col1);
            }
            return dic;
        }
        /// <summary>
        /// 获取文件夹下所有文件，并返回文件里的相关信息
        /// </summary>
        /// <returns></returns>
        public static ReadTable GetTable()
        {
            string url = ur + "Interface\\XML\\U8\\";
            DirectoryInfo dir = new DirectoryInfo(url);
            FileInfo[] listfile = dir.GetFiles();//获取所有文件名称集合
            ReadTable tab = new ReadTable();
            DataTable t = new DataTable();
            int i = 0;
            foreach (FileInfo file in listfile)
            {
                Dictionary<string, string> dic = XmlNamePare(url+file.Name);//获取节点下的数据

                DataRow r = t.NewRow();
                if (i == 0)
                {
                    t.Columns.Add("XMLNAME");
                    t.Columns.Add("OUTNAME");
                }
                r["XMLNAME"] = file.Name;
                r["OUTNAME"] = "U8";
                foreach (KeyValuePair<string, string> pa in dic)
                {
                    if (i == 0)
                    {
                        t.Columns.Add(pa.Key);
                    }
                    r[pa.Key] = pa.Value;
                }
                t.Rows.Add(r);
                i++;
            }
            tab.rows = t;
            tab.total = t.Rows.Count;
            return tab;
        }
        /// <summary>
        /// 返回所有表描述
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Dictionary<string, string> XmlNamePare(string url)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(url);
            XmlNodeList namePare = xmldoc.SelectNodes("//namePare");
            foreach (XmlNode elemname in namePare)
            {
                foreach (XmlNode node in elemname.ChildNodes)
                {
                    dic.Add(node.Name, node.InnerText);
                }
            }
            return dic;
        }
        /// <summary>
        /// 返回所有关联表字段
        /// </summary>
        /// <param name="url">xml文件路径</param>
        /// <returns></returns>
        public static Dictionary<string, string> XmlReadU8(string name)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(ur+"Interface\\XML\\" + name);
            XmlNodeList colPare = xmldoc.SelectNodes("//colPare");

            foreach (XmlElement element in colPare)
            {
                string col = element.GetElementsByTagName("col")[0].InnerText;
                string col1 = element.GetElementsByTagName("col1")[0].InnerText;
                dic.Add(col, col1);
            }
            return dic;
        }
        /// <summary>
        /// U8的接口配置调用方法
        /// </summary>
        /// <param name="xmlName"></param>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public static EntityList<MappingInfo> XmlConfig(string xmlName, int templateID)
        {
            string url = AppDomain.CurrentDomain.BaseDirectory + "Interface\\U8\\" + xmlName;
            EntityList<MappingInfo> list = new EntityList<MappingInfo>();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(url);
            XmlNodeList colPare = xmldoc.SelectNodes("//DocumentElement");
            foreach (XmlElement element in colPare)
            {
                foreach (XmlNode enode in element.ChildNodes)
                {
                    if (enode.Name == "ColumnMap")
                    {
                        MappingInfo mi = ListMapping(enode, templateID);
                        list.Add(mi);
                        foreach (XmlNode n in enode.ChildNodes)
                        {
                            if (n.Name == "Body")
                            {
                                MappingInfo mibody = ListMapping(n, templateID);
                                mi.MapChild.Add(mibody);
                            }
                        }
                    }
                }
            }
            return list;
        }
        public static MappingInfo ListMapping(XmlNode n, int templateID)
        {
            MappingInfo mi = new MappingInfo();
            mi.MappingInfoName = n.ChildNodes[0].InnerText;
            mi.XmlName = n.ChildNodes[1].InnerText;
            mi.SourceSystem = n.ChildNodes[2].InnerText;
            mi.TargetSystem = n.ChildNodes[3].InnerText;
            mi.SourceTable = n.ChildNodes[4].InnerText;
            mi.TargetTable = n.ChildNodes[5].InnerText;
            mi.Remark = n.ChildNodes[7].InnerText;
            mi.Visible = n.ChildNodes[8].InnerText;
            mi.SourceMODEL = n.ChildNodes[9].InnerText;
            mi.TargetMODEL = n.ChildNodes[10].InnerText;
            mi.ISORDER = int.Parse(n.ChildNodes[12].InnerText);
            mi.TemplateID = templateID;
            return mi;
        }
        public static IEntityList XmlMappingInfoConfig(string xmlName,int templateID)
        {
            //string url = "E:\\TSF-Project\\CRM_WMS\\AcctrueCRM\\Acc.K3\\Mapping\\" + xmlName;
            string url = AppDomain.CurrentDomain.BaseDirectory + "Interface\\K3\\Mapping\\" + xmlName;
            IEntityList list = new EntityList<MappingInfo>();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(url);
            XmlNodeList mis = xmldoc.SelectNodes("//DataTransform");
            XmlNodeList mics = xmldoc.SelectNodes("//DataTransformChild");
            foreach (XmlElement element in mis)
            {
                MappingInfo mi = new MappingInfo();
                mi.MappingInfoName = element.GetElementsByTagName("MappingInfoName")[0].InnerText;
                mi.XmlName = element.GetElementsByTagName("XmlName")[0].InnerText;
                mi.SourceSystem = element.GetElementsByTagName("SourceSystem")[0].InnerText;
                mi.TargetSystem = element.GetElementsByTagName("TargetSystem")[0].InnerText;
                mi.SourceTable = element.GetElementsByTagName("SourceTable")[0].InnerText;
                mi.TargetTable = element.GetElementsByTagName("TargetTable")[0].InnerText;
                mi.Remark = element.GetElementsByTagName("Remark")[0].InnerText;
                mi.Visible = element.GetElementsByTagName("IsVisible")[0].InnerText.ToString();
                mi.TemplateID = templateID;
                foreach (XmlElement elementChild in mics)
                {
                    string ParentXmlName = elementChild.GetElementsByTagName("ParentXmlName")[0].InnerText;
                    if (mi.XmlName == ParentXmlName)
                    {
                        MappingInfo mic = new MappingInfo();
                        mic.MappingInfoName = elementChild.GetElementsByTagName("MappingInfoName")[0].InnerText;
                        mic.XmlName = elementChild.GetElementsByTagName("XmlName")[0].InnerText;
                        mic.SourceSystem = elementChild.GetElementsByTagName("SourceSystem")[0].InnerText;
                        mic.TargetSystem = elementChild.GetElementsByTagName("TargetSystem")[0].InnerText;
                        mic.SourceTable = elementChild.GetElementsByTagName("SourceTable")[0].InnerText;
                        mic.TargetTable = elementChild.GetElementsByTagName("TargetTable")[0].InnerText;
                        mic.Remark = elementChild.GetElementsByTagName("Remark")[0].InnerText;
                        mic.Visible = elementChild.GetElementsByTagName("IsVisible")[0].InnerText.ToString();
                        mic.TemplateID = templateID;
                        mi.MapChild.Add(mic);
                    }
                }
                list.Add(mi);
            }
            return list;

  //            <DataTransformChild>
  //  <MappingInfoName>采购入库通知明细</MappingInfoName>
  //  <XmlName>InNoticeData.xml</XmlName>
  //  <ParentXmlName>InNotice.xml</ParentXmlName>
  //  <SourceSystem>K3</SourceSystem>
  //  <TargetSystem>WMS</TargetSystem>
  //  <SourceTable>POInStockEntry</SourceTable>
  //  <TargetTable>Acc_WMS_InNoticeMaterials</TargetTable>
  //  <maxTimestamp>0</maxTimestamp>
  //  <Remark>采购入库通知明细（收料通知）</Remark>
  //  <IsVisible>1</IsVisible>
  //</DataTransformChild>
        }

        public static void XmlColumnMapConfig(string xmlName, MappingInfo mi)
        {
            //string url = "E:\\TSF-Project\\CRM_WMS\\AcctrueCRM\\Acc.K3\\Mapping\\" + xmlName;
            string url = AppDomain.CurrentDomain.BaseDirectory + "Interface\\K3\\Mapping\\" + xmlName;
            //IEntityList list = new EntityList<ColumnMap>();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(url);
            XmlNodeList colPare = xmldoc.SelectNodes("//ColProperty");
            foreach (XmlElement element in colPare)
            {
                ColumnMap cm = new ColumnMap();
                cm.SourceName = element.GetElementsByTagName("SourceName")[0].InnerText;
                cm.TargetName = element.GetElementsByTagName("TargetName")[0].InnerText;
                //cm.IsSelect = element.GetElementsByTagName("IsSelect")[0].InnerText;
                //cm.IsInsert = element.GetElementsByTagName("IsInsert")[0].InnerText;
                //cm.IsUpdate = element.GetElementsByTagName("IsUpdate")[0].InnerText;
                cm.Deluft = element.GetElementsByTagName("TargetTable")[0].InnerText;
                cm.MappingInfoID = mi.ID;
                //dtf.TransformCount = 0;
                //dtf.SuccessCount = 0;
                //mi.MapColumns.Add(cm);
                //list.Add(cm);
            }
            /// <summary>
            /// 列映射关系表
            /// 0 SourceName 源列名
            /// 1 TargetName 目标列名
            /// 2 IsSelect   是否查询（指对源列）
            /// 3 IsInsert   是否插入（指对目标列）
            /// 4 IsUpdate   是否更新（指对目标列）
            /// 5 Deluft     默认值（指对目标列）
        }

    }
}

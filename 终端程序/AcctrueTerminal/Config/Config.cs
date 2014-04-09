using System;
using System.Xml;
using System.Text;
using System.IO;
namespace AcctrueTerminal.Config
{
    public static class Config
    {       
        #region Config配置文件
        public static string GetConfig(string strKey, string strAssemblyPath)
        {
            string strRet = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();    
                if (string.IsNullOrEmpty(strAssemblyPath))
                {
                    strAssemblyPath = GetPath(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                }                        
               
                strAssemblyPath += "\\Config.xml";              
                xmlDoc.Load(strAssemblyPath);

                XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;
                foreach (XmlNode node in childNodes)
                {
                    if (node.Name.ToLower() == "appsettings")
                    {
                        XmlNodeList _childNode = node.ChildNodes;
                        foreach (XmlNode _node in _childNode)
                        {
                            if (_node.Name == "add")
                                if (_node.Attributes.GetNamedItem("key").Value == strKey)
                                    strRet = _node.Attributes.GetNamedItem("value").Value;
                        }
                    }
                }
            }
            catch
            {
                return "无配置文件";
            }
            return strRet;
        }

        public static bool setConfig(string strPdaKey,string strWebBusKey,string strWebLoginKey,string strUrl,string strPort)
        {
            string strRet = string.Empty;
            string strPdaValue = "http://"+strUrl+"/AccWms/";
            string strWebBusValue = "http://" + strUrl + ":" + strPort + "/web.aspx?";
            string strWebLoginValue = "http://" + strUrl + ":" + strPort + "/user.aspx?";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string strAssemblyPath = GetPath(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                strAssemblyPath += "\\Config.xml";
                xmlDoc.Load(strAssemblyPath);

                XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;
                foreach (XmlNode node in childNodes)
                {
                    if (node.Name.ToLower() == "appsettings")
                    {
                        XmlNodeList _childNode = node.ChildNodes;
                        foreach (XmlNode _node in _childNode)
                        {
                            if (_node.Name == "add")
                            {
                                //if (string.IsNullOrEmpty(_node.Attributes.GetNamedItem("key").Value))
                                //{
                                //    _node.Attributes.GetNamedItem("key").Value = strPdaKey;
                                //}
                                if (_node.Attributes.GetNamedItem("key").Value == strPdaKey)
                                {
                                    _node.Attributes.GetNamedItem("value").Value = strPdaValue;
                                }
                                if (_node.Attributes.GetNamedItem("key").Value == strWebBusKey)
                                {
                                    _node.Attributes.GetNamedItem("value").Value = strWebBusValue;
                                }
                                if (_node.Attributes.GetNamedItem("key").Value == strWebLoginKey)
                                {
                                    _node.Attributes.GetNamedItem("value").Value = strWebLoginValue;
                                }
                            }
                        }
                    }
                }
                xmlDoc.Save(strAssemblyPath);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 版本配置文件
        public static string GetVersionConfig(string strKey,string XmlName)
        {
            string strRet = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();              
                string strAssemblyPath = GetPath(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                strAssemblyPath += "\\" + XmlName;
                xmlDoc.Load(strAssemblyPath);

                XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;
                foreach (XmlNode node in childNodes)
                {
                    if (node.Name.ToLower() == "appsettings")
                    {
                        XmlNodeList _childNode = node.ChildNodes;
                        foreach (XmlNode _node in _childNode)
                        {
                            if (_node.Name == "add")
                                if (_node.Attributes.GetNamedItem("key").Value == strKey)
                                    strRet = _node.Attributes.GetNamedItem("value").Value;
                        }
                    }
                }
            }
            catch
            {
                return "无配置文件";
            }
            return strRet;
        }

        /// <summary>
        /// 服务器端读取版本号
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string GetServerConfig(string strKey,string Url)
        {
            string strRet = string.Empty;
            try
            {                
                XmlDocument xmlDoc = new XmlDocument();                
                xmlDoc.Load(Url);
                XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;
                foreach (XmlNode node in childNodes)
                {
                    if (node.Name.ToLower() == "appsettings")
                    {
                        XmlNodeList _childNode = node.ChildNodes;
                        foreach (XmlNode _node in _childNode)
                        {
                            if (_node.Name == "add")
                                if (_node.Attributes.GetNamedItem("key").Value == strKey)
                                    strRet = _node.Attributes.GetNamedItem("value").Value;
                        }
                    }
                }
            }
            catch
            {
                return "无配置文件";
            }
            return strRet;
        }

        /// <summary>
        /// 设置本地配置文件信息
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static bool setVersionConfig(string strAssemblyPath, string strKey, string strUrl)
        {
            string strRet = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                //string strAssemblyPath = GetPath(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                strAssemblyPath += "\\Version.xml";
                xmlDoc.Load(strAssemblyPath);

                XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;
                foreach (XmlNode node in childNodes)
                {
                    if (node.Name.ToLower() == "appsettings")
                    {
                        XmlNodeList _childNode = node.ChildNodes;
                        foreach (XmlNode _node in _childNode)
                        {
                            if (_node.Name == "add")
                            {
                                if (string.IsNullOrEmpty(_node.Attributes.GetNamedItem("key").Value))
                                {
                                    _node.Attributes.GetNamedItem("key").Value = strKey;
                                }
                                if (_node.Attributes.GetNamedItem("key").Value == strKey)
                                {
                                    _node.Attributes.GetNamedItem("value").Value = strUrl;
                                }
                            }
                        }
                    }
                }
                xmlDoc.Save(strAssemblyPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置服务器配置文件信息
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static bool setServerConfig(string Url,string strKey, string strValue)
        {
            string strRet = string.Empty;
            string strAssemblyPath = Url;
            string ExceptionStr = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Url);
                XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;               
                foreach (XmlNode node in childNodes)
                {
                    if (node.Name.ToLower() == "appsettings")
                    {
                        XmlNodeList _childNode = node.ChildNodes;
                        foreach (XmlNode _node in _childNode)
                        {
                            if (_node.Name == "add")
                            {
                                if (string.IsNullOrEmpty(_node.Attributes.GetNamedItem("key").Value))
                                {
                                    _node.Attributes.GetNamedItem("key").Value = strKey;
                                }
                                if (_node.Attributes.GetNamedItem("key").Value == strKey)
                                {
                                    _node.Attributes.GetNamedItem("value").Value = strValue;
                                }
                            }
                        }
                    }
                }
                xmlDoc.Save(strAssemblyPath);
                return true;
            }
            catch(Exception ex)
            {
                ExceptionStr = ex.Message+ex.InnerException+ex.StackTrace;
                return false;
            }
        }



        #endregion    

        #region WriteXml
        public static void WriteXml(XmlTextWriter writer, string Name, string Value)
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("configuration");
            writer.WriteStartElement("appSettings");

            writer.WriteStartElement("add");
            writer.WriteAttributeString("key", Name);
            writer.WriteAttributeString("value", Value);

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.Flush();
            writer.Close();
        }
        #endregion

        private static string GetPath(string AssemblyFullName)
        {
            int nLastSlash = AssemblyFullName.LastIndexOf("\\");
            return AssemblyFullName.Substring(0, nLastSlash);
        }

        #region CreateXML
        public static void create()
        {
            string strAssemblyPath = GetPath(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            XmlDocument xmldoc = new XmlDocument();
            XmlNode xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmldoc.AppendChild(xmlnode);
            //加入一个根元素
            XmlElement xmlelem = xmldoc.CreateElement("", "configuration", "");
            xmldoc.AppendChild(xmlelem);
            //加入另外一个元素
            XmlElement xmlelem2 = xmldoc.CreateElement("appSettings");
            xmlelem2 = xmldoc.CreateElement("", "appSettings", "");
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

            XmlNode node3 = xmldoc.CreateElement("add");
            XmlNode node4 = xmldoc.CreateNode(XmlNodeType.Attribute, "key", "");
            XmlNode node5 = xmldoc.CreateNode(XmlNodeType.Attribute, "value", "");
            node3.Attributes.SetNamedItem(node4);
            node3.Attributes.SetNamedItem(node5);
            xmldoc.ChildNodes.Item(1).ChildNodes.Item(0).AppendChild(node3);
            try
            {
                xmldoc.Save(strAssemblyPath + "\\Config.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}

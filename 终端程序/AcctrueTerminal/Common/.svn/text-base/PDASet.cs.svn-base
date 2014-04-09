using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Xml;
using System.Net;
using AcctrueTerminal.Config;
using System.Windows.Forms;

namespace AcctrueTerminal.Common
{
   public static class PDASet
   {
        #region 系统公用属性
        public static string sPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName); 
        public static string ServerVersion { get; set; }
        public static string ClientVersion { get; set; }
        public static bool IsOff { get; set; }

        public static string AssemblyVersion
        {
            get
            {
               return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        public static ArrayList _list;
        public static ArrayList List
        {
          get 
          { 
              if(_list==null)
              {
                  _list=new ArrayList();
              }
              return PDASet._list; 
          }
          set { PDASet._list = value; }
        }

        public static int _InitTabPageCount;

        public static int InitTabPageCount
        {
            get { return PDASet._InitTabPageCount; }
            set { PDASet._InitTabPageCount = value; }
        }
       #endregion

        #region 系统连接地址
        /// <summary>
        /// PDA系统连接配置文件
        /// </summary>
        public static string ConfigName = "Config.xml";
        /// <summary>
        /// WEB系统业务连接地址
       /// </summary>
        public static string AcctrueWebBusAddress = "AccServerAddress";
       /// <summary>
        /// WEB系统登陆连接地址
       /// </summary>
        public static string AcctrueWebLoginAddress = "AccLoginAddress";
       /// <summary>
        /// PDA系统业务连接地址
       /// </summary>
        public static string AcctruePdaServerAddress = "AccPdaServer";
        /// <summary>
        /// PDA系统版本配置文件
        /// </summary>
        public static string VersionName = "Version.xml"; 
        /// <summary>
        /// PDA系统版本配置文件中的KEY键值
        /// </summary>
        public static string VersionKey = "Version";
        /// <summary>
        /// PDA系统更新文件
        /// </summary>
        public static string UpdateFile = "TerminalUpdate.exe";
       /// <summary>
       /// 服务器配置文件URL
       /// </summary>
        public static string ConfigUrl { get; set; }
       /// <summary>
       /// 服务器版本文URL
       /// </summary>
        public static string VersionUrl { get; set; }

        #endregion

        #region 系统自动更新
        public static void CheckUpdate()
        {
            XmlDocument XmlDocConfig = new XmlDocument();
            string[] name = Config.Config.GetConfig(AcctruePdaServerAddress,sPath).Split('/');
            ConfigUrl = name[0] + "//" + name[2] + "/" + name[3] + "/" + ConfigName;
            VersionUrl = name[0] + "//" + name[2] + "/" + name[3] + "/" + VersionName;
            try
            {
                WebRequest s = WebRequest.Create(ConfigUrl);
                s.Timeout = 10000;//响应时间 
                WebResponse a = s.GetResponse();
            }
            catch
            {
                 UIHelper.PromptMsg("服务器版本文件不存在！");
                return;
            }
            //读取服务器版本号
            //XmlDocConfig.Load(url);
            PDASet.ServerVersion = Config.Config.GetServerConfig(VersionKey,VersionUrl);

            //判断终端本地文件是否存在      
            if (!System.IO.File.Exists(sPath + "\\" +VersionName))
            {
                try
                {
                    //创建文件并且写入值
                    System.IO.File.Create(sPath + "\\" + VersionName).Close();
                    XmlTextWriter writer = new XmlTextWriter(sPath + "\\" + VersionName, System.Text.Encoding.UTF8);
                    Config.Config.WriteXml(writer, VersionKey, AssemblyVersion);

                }
                catch (Exception ms)
                {
                    UIHelper.ErrorMsg(ms.Message);
                    return;
                }
            }
            //读取终端版本号              
            //XmlDocConfig.Load(SPath + "\\Version.xml");
            //ClientVersion = Config.Config.GetVersionConfig(VersionKey, VersionName);

            if (!PDASet.ClientVersion.Equals(PDASet.ServerVersion))
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName =sPath + "\\" + UpdateFile;
                p.Start();
                //Dispose();
                //Close();
                Application.Exit();
                return;
            }
        }        
        #endregion       

   }
}
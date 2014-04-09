using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Way.EAP.DataAccess.Data;
using Way.EAP;
using System.Configuration;
using System.Data;
using Way.EAP.DataAccess.Regulation;

namespace Acc.Business.Model
{
    [EntityClassAttribut(false)]
    public class BusinessBase : ModelBase
    {
        //public BusinessBase()
        //{
        //    string type = ConfigurationManager.AppSettings["servertype"];
        //    if (!string.IsNullOrEmpty(type))
        //    {
        //        if (type == "sqlserver")
        //            this.Regulation = new SQLServerRegulation();
        //        if (type == "oracle")
        //            this.Regulation = new OracleRegulation();
        //    }
        //    else
        //        this.Regulation = new SQLServerRegulation();
        //    if (ConfigurationManager.AppSettings["AutoTable"] != null && ConfigurationManager.AppSettings["AutoTable"] == "true")
        //    {
        //        IDataAction action = this.GetDataAction();
        //        EntityBase eb = this as EntityBase;
        //        string tablename = eb.ToString();
        //        if (!action.ContainsTable(tablename))
        //        {
        //            IEntityList list = Common.GetEntityList(this.GetType());
        //            list.DataAction = action;
        //            list.CreateTable(eb);
        //        }
        //        else
        //        {
        //            IEntityBase ieb = eb;
        //            Dictionary<string, EntityFieldAttribute> fields = ieb.GetField();
        //            foreach (string s in fields.Keys)
        //            {
        //                if (!action.ContainsColumns(tablename, s))
        //                {
        //                    EntityFieldAttribute ef = fields[s];
        //                    Type t = ieb.GetPropertyType(s);
        //                    bool iskey = ieb.GetPrimaryKey().Contains(s);
        //                    if (ef != null)
        //                        action.AlterTableAddColumn(tablename, new ColumnType() { IsKey = iskey, ColumnName = s, DbType = Type.GetTypeCode(t), Length = ef.Length ?? 50, Scale = ef.Scale, IsIdentity = ef.IsIdentity, IsNull = true });
        //                }
        //            }
        //        }
        //    }
        //}
        public BusinessBase()
        {
            IDataAction action = this.GetDataAction();
            EntityBase eb = this as EntityBase;
            string tablename = eb.ToString();
            if (!action.ContainsTable(tablename))
            {
                IEntityList list = Common.GetEntityList(this.GetType());
                list.DataAction = action;
                list.CreateTable(eb);
            }
            else
            {
                IEntityBase ieb = eb;
                Dictionary<string, EntityFieldAttribute> fields = ieb.GetField();
                foreach (string s in fields.Keys)
                {
                    if (!action.ContainsColumns(tablename, s))
                    {
                        EntityFieldAttribute ef = fields[s];
                        Type t = ieb.GetPropertyType(s);
                        bool iskey = ieb.GetPrimaryKey().Contains(s);
                        if (ef != null)
                            action.AlterTableAddColumn(tablename, new ColumnType() { IsKey = iskey, ColumnName = s, DbType = Type.GetTypeCode(t), Length = ef.Length ?? 50, Scale = ef.Scale, IsIdentity = ef.IsIdentity, IsNull = true });
                    }
                }
            }
        }

        
        [EntityPrimaryKey]
        [EntityField(IsIdentity=true)]
        public virtual int ID { get; set; }

        protected override Way.EAP.DataAccess.Data.IDataAction OnGetDataAction()
        {
            string type = ConfigurationManager.AppSettings["servertype"];
            string server = ConfigurationManager.AppSettings["server"];
            string database = ConfigurationManager.AppSettings["database"];
            string uid = ConfigurationManager.AppSettings["uid"];
            string pwd = ConfigurationManager.AppSettings["pwd"];
            DataBase db = null;
            if (type == "sqlserver")
                db = new SQLServerDataBase(server, database, uid, pwd);
            if (type == "oracle")
                db = new OracleDataBase(server, database, uid, pwd);
            return new DataBaseManage(db);
        }
        private bool islogging { get; set; }
        protected override bool IsLogging()
        {
            return islogging;
        }
        public void setLog(bool log)
        {
            islogging = log;
        }
    }
}

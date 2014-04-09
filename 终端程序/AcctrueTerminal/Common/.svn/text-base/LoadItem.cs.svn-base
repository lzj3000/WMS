using System;

using System.Collections.Generic;
using System.Text;

namespace AcctrueTerminal.Common
{
    public class LoadItem
    {
        //public loadItem();

        public string[] columns { get; set; }
        public string page { get; set; }
        public string rows { get; set; }
        public string rowsql { get; set; }
        public string selecttype { get; set; }
        public string sort { get; set; }
        public string where { get; set; }
        public SQLWhere[] whereList { get; set; }
    }

    public class SQLWhere
    {
        //public SQLWhere();
        public SQLWhere()
        {
            this.Relation = "and";
            this.Symbol = "=";
        }

        public string ColumnName { get; set; }
        public string EndFH { get; set; }
        public string Relation { get; set; }
        public string StartFH { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        //protected virtual string getwhere(string alias);
        //public string rwhere(string alias);
        //public string where();
        //public string where(string alias);
    }
}

using System;

using System.Collections.Generic;
using System.Text;

namespace AcctrueTerminal.Model.IsOffModel
{
   public class StockOutMaterials
    {
        public int ID { get; set; }
        public int PARENTID { get; set; }
        public string BATCHNO { get; set; }
        public int DEPOTWBS { get; set; }
        public string DEPOTWBS_CODE { get; set; }
        public int PORTNAME { get; set; }
        public string PORTNAME_PORTNO { get; set; }
        public int MATERIALCODE { get; set; }
        public string MATERIALCODE_FNAME { get; set; }
        public string MCODE { get; set; }
        public string FMODEL { get; set; }
        public int FUNITID { get; set; }
        public string FUNITID_UNITNAME { get; set; }
        public decimal NUM { get; set; }
        public int SourceRowID { get; set; }
    }
}

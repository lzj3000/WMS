using System;

using System.Collections.Generic;
using System.Text;

namespace AcctrueTerminal.Common
{
    public enum CheckEnum
    {
       Login=1,
       Query= 2,
       Edit=3,
       Foreignkey=4      
    }

    public enum OrderType
    {
       入库=1,
       出库=2,
       盘点=3
    }
}

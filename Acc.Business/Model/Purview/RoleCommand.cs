using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;


namespace Acc.Business.Model
{
    [EntityClassAttribut("Acc_Bus_RoleCommand", "角色命令")]
    public class RoleCommand : ModelCommand
    {
        [EntityForeignKey(typeof(RoleModel), "ID", "ModelName")]
        [EntityField(50)]
        public int RoleModelID { get; set; }
    }
    [EntityClassAttribut("Acc_Bus_RoleModelData", "角色模块数据")]
    public class RoleModelData : SystemModelData
    {
        [EntityForeignKey(typeof(RoleModel), "ID", "ModelName")]
        [EntityField(50)]
        public int RoleModelID { get; set; }
    }
}

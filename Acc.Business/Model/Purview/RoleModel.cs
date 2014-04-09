using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model
{
    /// <summary>
    /// 角色模块
    /// </summary>
    [EntityClassAttribut("Acc_Bus_RoleModel", "角色模块")]
    public class RoleModel : BusinessBase
    {
        [EntityControl("角色", false, false, 2)]
        [EntityForeignKey(typeof(Role), "ID", "RoleName")]
        [EntityField(50)]
        public int RoleID { get; set; }

        [EntityControl("模块", false, true, 3)]
        [EntityForeignKey(typeof(SystemModel), "ID", "ModelName")]
        [EntityField(50)]
        public int ModelID { get; set; }

        private HierarchicalEntityView<RoleModel, RoleCommand> _roleCommands;

        public HierarchicalEntityView<RoleModel, RoleCommand> RoleCommands
        {
            get
            {
                if (_roleCommands == null)
                    _roleCommands = new HierarchicalEntityView<RoleModel, RoleCommand>(this);
                return _roleCommands;
            }
        }

        private HierarchicalEntityView<RoleModel, RoleModelData> _roleModelDatas;

        public HierarchicalEntityView<RoleModel, RoleModelData> RoleModelDatas
        {
            get
            {
                if (_roleModelDatas == null)
                    _roleModelDatas = new HierarchicalEntityView<RoleModel, RoleModelData>(this);
                return _roleModelDatas;
            }
        }
    }
}

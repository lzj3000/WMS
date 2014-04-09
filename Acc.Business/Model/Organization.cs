using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;

namespace Acc.Business.Model
{
    /// <summary>
    /// 组织机构
    /// </summary>
    [EntityClassAttribut("Acc_Bus_Organization", "组织机构", IsOnAppendProperty = true)]
    public class Organization : BasicInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        [EntityControl("名称", false, true, 2)]
        [EntityField(200,IsNotNullable=true)]
        public string OrganizationName { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        [EntityControl("上级机构", false, true, 3)]
        [EntityForeignKey(typeof(Organization), "ID", "OrganizationName")]
        [EntityField(50)]
        public int ParentID { get; set; }
        /// <summary>
        /// 所在地区
        /// </summary>
        [EntityControl("所在地区", false, true, 4)]
        [EntityForeignKey(typeof(Region), "ID", "RegionName")]
        [EntityField(50)]
        public int RegionID { get; set; }

        [EntityControl("负责人", false, true, 5)]
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName")]
        [EntityField(50)]
        public int OfficeWorkerID { get; set; }

        ///<summary>
        ///机构类型
        /// </summary>
        [EntityControl("机构类型", false, false, 20)]
        [EntityField(10)]
        [ValueTypeProperty]
        public int OrganizationType { get; set; }

        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "OrganizationType")
            {
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "内部机构";
                pro1.Value = "1";

                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "经销商";
                pro2.Value = "2";

                list.Add(pro1);
                list.Add(pro2);

            }
            return list.ToArray();
        }


        private HierarchicalEntityView<Organization, Organization> _childItems;
        /// <summary>
        /// 下级机构集合
        /// </summary>
        public HierarchicalEntityView<Organization, Organization> ChildItems
        {
            get
            {
                if (_childItems == null)
                    _childItems = new HierarchicalEntityView<Organization, Organization>(this);
                return _childItems;
            }
        }

        private HierarchicalEntityView<Organization, OfficeWorker> _officeWorkerItems;

        /// <summary>
        /// 机构内职员集合
        /// </summary>
        [HierarchicalEntityControl(isselect = true, c = "Acc.Business.Controllers.OfficeWorkerController",ischeck=true)]
        public HierarchicalEntityView<Organization, OfficeWorker> OfficeWorkerItems
        {
            get
            {
                if (_officeWorkerItems == null)
                {
                    _officeWorkerItems = new HierarchicalEntityView<Organization, OfficeWorker>(this);
                    _officeWorkerItems.IsAssociateDelete = false;
                }
                return _officeWorkerItems;
            }
        }

    }
}

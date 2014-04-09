using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Acc.ECharts;
using Acc.Business.Model;
using Acc.Contract.Data.ControllerData;
using System.Reflection;
using Newtonsoft.Json;

namespace Acc.Business.Controllers
{
    /// <summary>
    /// 图表
    /// </summary>
    public class EChartsController : ControllerBase
    {
        /// <summary>
        /// 图表选项  用于控制图标名称 样式 等等
        /// </summary>
        private option opt { set; get; }

         //where条件
        private Dictionary<string, WhereParameter> whereParams;

        public EChartsController()
            : this(new OfficeWorker())
        { }

        public EChartsController(IModel model):base(model)
        {
          //  opt = new option();  //如果放到此处只会初始化一次,并不是每次调用都初始化一次
          //程序自动缓存
        }

        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        public string GetOptions()
        {
            opt = new option();
            SetOptions(opt);
            ValidOption(opt);
           return opt.ToString();
        }

        /// <summary>
        /// 自定义返回样式和数据
        /// </summary>
        /// <param name="o"></param>
        protected virtual void SetOptions(option o)
        { 
        
        }

        protected void GetWhereParams()
        {
            whereParams = new Dictionary<string, WhereParameter>();
            //获取公共实例属性       
            PropertyInfo[] properties = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                switch (property.Name)
                {
                    case "ActionItem":
                    case "fdata":
                    case "LoadItem":
                    case "outc":
                    case "printIndex":
                        break;
                    default:
                        if (property.IsDefined(typeof(WhereParameter), true))
                        {
                            WhereParameter param = property.GetCustomAttributes(typeof(WhereParameter), false)[0] as WhereParameter;
                            if (param.visible)
                            {
                                if (string.IsNullOrEmpty(param.field))
                                {
                                    param.field = property.Name;
                                }
                                whereParams.Add(param.field, param);
                            }
                        }
                        break;
                }
            }
        }

        public string GetWheres()
        {
            GetWhereParams();
            string result = JsonConvert.SerializeObject(whereParams);
            return result;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public string EChartSearch()
        {
           return GetOptions();
        }

        /// <summary>
        /// 验证Option是否合法
        /// </summary>
        protected virtual void ValidOption(option opt)
        {
            if (opt.toolbox.show)
            {
                if (opt.toolbox.feature.magicType.Count < 0)
                {
                    throw new EChartException("显示提示框,必须添加图形转换功能(toolbox.feature.magicType)");
                }
            }
        }

        //时间参数最好设置返回值为字符串  否则会有异常(整体写完在研究原因)
        //[WhereParameter(wType = WhereParameter.WhereType.DateTime, title = "起始时间", field = "StartDate")]
        //public string StartDate { set; get; }
    }
}

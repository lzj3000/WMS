using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract;
using Acc.Contract.Data;

namespace Acc.Business
{
    public class BusinessCenter
    {
        private BusinessCenter() { }
        private static readonly object _lock = new object();
        private static BusinessCenter _center;
        public static BusinessCenter Center
        {
            get
            {
                if (_center == null)
                {
                    lock (_lock)
                    {
                        if (_center == null)
                        {
                            _center = new BusinessCenter();
                        }
                    }
                }
                return _center;
            }
        }
      

    }
}

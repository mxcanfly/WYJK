using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Framework.EnumHelper
{
    /// <summary>
    /// 枚举属性值设置
    /// </summary>
    public class EnumDisplayNameAttribute : Attribute
    {
        private string _displayName;

        public EnumDisplayNameAttribute(string displayName)
        {
            this._displayName = displayName;
        }

        public string DisplayName
        {
            get { return _displayName; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WYJK.Framework.EnumHelper;

namespace WYJK.Framework.EnumHelper
{
    /// <summary>
    /// 户籍性质
    /// </summary>
    public enum HouseholdPropertyEnum
    {
        [EnumDisplayName("本市农村")]
        InRural = 1,

        [EnumDisplayName("本市城镇")]
        InTown = 2,

        [EnumDisplayName("外市农村")]
        OutRural = 3,

        [EnumDisplayName("外市城镇")]
        OutTown = 4
    }

    /// <summary>
    /// 参保状态
    /// </summary>
    public enum SocialSecurityStatusEnum
    {
        [EnumDisplayName("未参保")]
        UnInsured = 1,
        [EnumDisplayName("正常")]
        NormalInsured = 2

    }

    public class HouseholdPropertyClass
    {
        public static List<HouseholdProperty> GetList(Type enumType)

        {

            List<HouseholdProperty> list = new List<HouseholdProperty>();

            foreach (object e in Enum.GetValues(enumType))

            {

                list.Add(new HouseholdProperty() { Text = EnumExt.GetEnumCustomDescription(e), Value = ((int)e) });

            }

            return list;

        }
    }

    /// <summary>
    /// 户籍属性类
    /// </summary>
    public class HouseholdProperty
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}
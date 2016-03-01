using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WYJK.Framework
{
    /// <summary>
    /// 支付相关方法
    /// </summary>
    public static class PaymentManager
    {
        #region 创建支付宝支付的加密数据
        /// <summary>
        /// 创建支付宝支付的加密数据
        /// </summary>
        /// <param name="orderSn">订单号</param>
        /// <param name="body">订单商品描述</param>
        /// <param name="totalFee">订单金额</param>
        /// <param name="subject"></param>
        /// <param name="platform">客户端名称</param>
        /// <param name="version">客户端版本</param>
        /// <returns></returns>
        public static string CreateAlipay(string orderSn, string body, decimal totalFee, string subject, string platform, string version)
        {
            // 外部交易号 这里取当前时间，商户可根据自己的情况修改此参数，但保证唯一性
            string outTradeNo = orderSn.ToString(CultureInfo.InvariantCulture);

            AlipayConfig config = AlipayConfig.Default;

            string appenv = $"system={platform}^version={version}";
            //获取待签名字符串
            string content = AlipayManager.GetAlipayString(config.Partner, outTradeNo, body, totalFee, subject, version, appenv);


            //生成签名
            string mysign = RSAFromPkcs8.Sign(content, config.PrivateKey, config.InputCharset);
            //返回参数格式
            string strReturn = content + "&sign=\"" + HttpUtility.UrlEncode(mysign + "", Encoding.UTF8) + "\"&sign_type=\"RSA\"";
            return strReturn;
        } 
        #endregion
    }
}

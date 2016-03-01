using WYJK.Framework.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace WYJK.Framework
{
    public static class AlipayManager
    {
        static AlipayManager()
        {
            
        }

        #region 获取请求字符串
        /// <summary>
        /// 获取请求字符串
        /// </summary>
        /// <param name="partner">商户编号</param>
        /// <param name="orderSn">订单号</param>
        /// <param name="body">请求描述</param>
        /// <param name="totalFee">总金额</param>
        /// <param name="subject"></param>
        /// <param name="version">APPB版本号</param>
        /// <param name="appenv">客户端标识</param>
        /// <returns></returns>
        public static string GetAlipayString(string partner, string orderSn, string body, decimal totalFee, string subject, string version, string appenv)
        {
            AlipayConfig config = AlipayConfig.Default;
            return $"partner=\"{config.Partner}\"&seller_id=\"{config.Seller}\"&out_trade_no=\"{orderSn}\"&subject=\"{subject}\"&" +
                   $"body=\"{body}\"&total_fee=\"{totalFee.ToString("F")}\"&notify_url=\"{HttpUtility.UrlEncode(config.NotifyUrl, Encoding.UTF8)}\"&service=\"{config.Service}\"&" +
                $"payment_type=\"{config.PaymentType}\"&_input_charset=\"{config.InputCharset}\"&it_b_pay=\"{config.Itbpay}\"&appenv=\"{appenv}\"";

        }
        #endregion

        #region 支付宝手机回调校验方法
        /// <summary>
        /// 支付宝手机回调校验方法
        /// </summary>
        /// <param name="config"></param>
        /// <param name="parameter"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static async Task<bool> VerifyMobileNotify(AlipayConfig config, Dictionary<string, string> parameter, string sign)
        {
            if (config.SignType == "00001")
            {
                parameter["notify_data"] = RSAFromPkcs8.DecryptData(parameter["notify_data"], config.PublicKey, config.InputCharset);
            }
            string responseTxt = "true";
            try
            {
                //XML解析notify_data数据，获取notify_id
                string notifyId = "";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(parameter["notify_data"]);

                notifyId = xmlDoc.SelectSingleNode("/notify/notify_id")?.InnerText;

                if (notifyId != "")
                {
                    string veryfyUrl = config.VerifyUrl + "partner=" + config.Partner + "&notify_id=" + notifyId;
                    using (WebClient client = new WebClient())
                    {
                        responseTxt = await client.DownloadStringTaskAsync(veryfyUrl);
                    }
                }
            }
            catch (Exception e)
            {
                responseTxt = e.ToString();
            }

            //获取返回时的签名验证结果
            bool isSign = GetSignVeryfy(config, parameter, sign, false);


            return (responseTxt == "true" && isSign);
        }
        #endregion

        #region 内部方法
        internal static bool GetSignVeryfy(AlipayConfig config,Dictionary<string, string> inputPara, string sign, bool isSort)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //过滤空值、sign与sign_type参数
            sPara = FilterPara(inputPara);

            if (isSort)
            {
                //根据字母a到z的顺序把参数排序
                sPara = SortPara(sPara);
            }
            else
            {
                sPara = SortNotifyPara(sPara);
            }

            //获取待签名字符串
            string preSignStr = CreateLinkString(sPara);

            //获得签名验证结果
            bool isSgin = false;
            if (string.IsNullOrEmpty(sign) == false)
            {
                switch (config.SignType)
                {
                    case "MD5":
                        isSgin = SignByMd5(preSignStr , config.SecurityKey, config.InputCharset) == sign;
                        break;
                    case "RSA":
                        isSgin = RSAFromPkcs8.Verify(preSignStr, sign, config.PublicKey, config.InputCharset);
                        break;
                    case "0001":
                        isSgin = RSAFromPkcs8.Verify(preSignStr, sign, config.PublicKey, config.InputCharset);
                        break;
                    default:
                        break;
                }
            }

            return isSgin;
        }


        /// <summary>
        /// 根据字母a到z的顺序把参数排序
        /// </summary>
        /// <param name="dicArrayPre">排序前的参数组</param>
        /// <returns>排序后的参数组</returns>
        internal static Dictionary<string, string> SortPara(Dictionary<string, string> dicArrayPre)
        {
            SortedDictionary<string, string> dicTemp = new SortedDictionary<string, string>(dicArrayPre);
            Dictionary<string, string> dicArray = new Dictionary<string, string>(dicTemp);

            return dicArray;
        }
        /// <summary>
        /// 异步通知时，对参数做固定排序
        /// </summary>
        /// <param name="dicArrayPre">排序前的参数组</param>
        /// <returns>排序后的参数组</returns>
        internal static Dictionary<string, string> SortNotifyPara(Dictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>
            {
                {"service", dicArrayPre["service"]},
                {"v", dicArrayPre["v"]},
                {"sec_id", dicArrayPre["sec_id"]},
                {"notify_data", dicArrayPre["notify_data"]}
            };

            return sPara;
        }
        /// <summary>
        /// 除去数组中的空值和签名参数
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        internal static Dictionary<string, string> FilterPara(Dictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if ("sign".Equals(temp.Key,StringComparison.OrdinalIgnoreCase) && "sign_type".Equals(temp.Key,StringComparison.OrdinalIgnoreCase) == false && string.IsNullOrEmpty(temp.Value) == false)
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }
        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="dicArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        internal static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }
        /// <summary>
        /// 签名字符串
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="inputCharset">编码格式</param>
        /// <returns>签名结果</returns>
        internal static string SignByMd5(string prestr, string key, string inputCharset)
        {
            StringBuilder sb = new StringBuilder(32);

            prestr = prestr + key;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(inputCharset).GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }
        #endregion
    }

    #region 支付宝支付配置信息
    /// <summary>
    /// 支付宝支付配置信息
    /// </summary>
    public class AlipayConfig
    {
        #region 属性
        /// <summary>
        /// 设置未付款交易的超时时间，一旦超时，该笔交易就会自动被关闭。
        /// 取值范围：1m～15d。
        /// m-分钟，h-小时，d-天，1c-当天（无论交易何时创建，都在0点关闭）。
        /// 该参数数值不接受小数点，如1.5h，可转换为90m。
        /// </summary>
        public string Itbpay { set; get; }
        /// <summary>
        /// 商户网站使用的编码格式，固定为utf-8。
        /// </summary>
        public string InputCharset { set; get; }
        /// <summary>
        /// 支付宝安全校验码
        /// </summary>
        public string SecurityKey { set; get; }
        /// <summary>
        /// 支付类型。默认值为：1（商品购买）。
        /// </summary>
        public string PaymentType { set; get; }

        /// <summary>
        /// 接口名称。固定值。
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// 合作商户ID
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// 回调校验地址
        /// </summary>
        public string VerifyUrl { set; get; }

        /// <summary>
        /// 卖家帐号
        /// </summary>
        public string Seller { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 商户私钥
        /// </summary>
        public string PrivateKey { get; set; }
        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// 加密方式
        /// </summary>
        public string SignType { get; set; }
        /// <summary>
        /// Notify异步通知URL
        /// </summary>
        public string NotifyUrl { get; set; }

        #endregion

        /// <summary>
        /// 获取默认支付宝账号
        /// </summary>
        public static AlipayConfig Default => new AlipayConfig
        {
            Partner = "2088121833752796",
            InputCharset = "utf-8",
            Itbpay = "5d",
            SignType = "RSA",
            PaymentType = "1",
            Service = "mobile.securitypay.pay",
            PublicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB",
            Body = "",
            NotifyUrl = "http://WYJK.diguo.xuehuwang.com:9001/api/payment/AlipayNotify",
            PrivateKey = "MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAON+rtQm1FDXZtcT78LKVKrtKdSZa5KulyhWYhP74GCRULXIvMyzrv5vuW+OmhONJHNmumowIu+cYtPn3hCE9+wDa7Z6DM3mHnXiFSVCflhVeqxUCQU4DOL3U3ZZmxYSm/3EH6uZ3mfHciofysvyoI+vwc3zAlJS6z8ITbLYLfO7AgMBAAECgYBnTd+V9wvyqd3JTQRTMA3CkG+uWvy+XwnFB3UCHh6Fu3crFTymt/F/GLzcK6VLu4wR21RLZBB5Pkqib2gnmDn71x8jpRsTvDsxDx9IJ1gE7mDfk1jH2c+JrwGvdF2M8Fbp5jKaA47Ob5gIoNcOtLGTLtpFV3NwXNhlwSJdwlr+AQJBAP2zv9Spm6Vt3/y4AyzVcbtWGOj3P29eJIksAx9l+rhPCOH031X4lbU+ul3hm07LPIJHUiUm+zeZ3H6E9b8RbX8CQQDljirpHVbwLtHQ2nSbttV6/NEuBocjUfK+b076O7O7b/ccR2yOa3JKk0wIqF47x35kJCaX91m4JVHNbi9LRc/FAkAlfO4XqohJRZcXbMlrUo7fs7Uyl3ZUKoETk+FSPmtx2JvjZ5+owHa+tWosfS3J0tY6GffVbZEpgh7Gwzbc6OJZAkAsphFMlCtTvheLQuJJYy90o3XgON7SDN9lEOtQmooj2+w5cN75eIabYLj6Oh1SDURVH/7tseueeIvHpDXIs0RtAkAssTVeEQdQn4/647Mj+j+x3vR/GymeGOQvvY57aX6Oxx+ZYjq/XUHdrZYra0joyW/EeQq8CbL7jCbC94A+VfBK",
            Seller = "qd_yxtx@163.com",
            Subject = "",
            SecurityKey = "j4tc9rtnti59etthrdqxbuwuh5v5fl1r"
        };
    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using WYJK.Framework.Setting;

namespace WYJK.Framework.Helpers
{
    /// <summary>
    /// 经纬度辅助类
    /// </summary>
    public static class DistanceHelper
    {
        #region 根据经纬度计算距离

        /// <summary>
        /// 根据经纬度计算距离
        /// </summary>
        /// <param name="lat1">纬度</param>
        /// <param name="lng1">经度</param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            const double R = 6371229; // 地球的半径

            var x = (lng2 - lng1)*Math.PI*R*Math.Cos(((lat1 + lat2)/2)*Math.PI/180)/180;
            var y = (lat2 - lat1)*Math.PI*R/180;
            var distance = Math.Sqrt(x*x + y*y);

            return distance;
        }

        #endregion

        #region 根据地址获取百度对应的经纬度
        /// <summary>
        /// 根据地址获取百度对应的经纬度
        /// </summary>
        /// <param name="address">详细地址</param>
        /// <returns>经纬度数组</returns>
        public static async Task<GeographyPoint> GeoCoderAsync(string address)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                NameValueCollection collection = new NameValueCollection
                {
                    {"output", "xml"},
                    {"ak", WebConfigurationManager.BaiduSetting.AppKey},
                    {"address", address}
                };

                string baseAddress = "http://api.map.baidu.com/geocoder/v2/";

                client.QueryString = collection;
                string xml = await client.DownloadStringTaskAsync(new Uri(baseAddress));
                XElement root = XDocument.Parse(xml).Root;
                if (root?.Element("status")?.Value == "0")
                {
                    double lng = double.Parse(root.Element("result")?.Element("location")?.Element("lng")?.Value ??"");
                    double lat = double.Parse(root.Element("result")?.Element("location")?.Element("lat")?.Value ??"");
                    
                    return new GeographyPoint(lat,lng);
                    
                }
                return new GeographyPoint();
            }
        }

        #endregion

        #region 将 WGS84 坐标转换为百度坐标
        /// <summary>
        /// 将 WGS84 坐标转换为百度坐标
        /// </summary>
        /// <param name="wgs84"></param>
        /// <returns></returns>
        public static async Task<GeographyPoint> GeoConvAsync(GeographyPoint wgs84)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                NameValueCollection collection = new NameValueCollection
                {
                    {"output", "xml"},
                    {"ak", WebConfigurationManager.BaiduSetting.AppKey},
                    {"coords", wgs84.Longitude + ","+wgs84.Latitude},
                    {"from","1" },
                    {"to","5" }
                };

                string baseAddress = "http://api.map.baidu.com/geoconv/v1/";

                client.QueryString = collection;
                string xml = await client.DownloadStringTaskAsync(new Uri(baseAddress));
                XElement root = XDocument.Parse(xml).Root;
                if (root?.Element("status")?.Value == "0")
                {
                    double lng = double.Parse(root.Element("result")?.Element("point")?.Element("x")?.Value ?? "");
                    double lat = double.Parse(root.Element("result")?.Element("point")?.Element("y")?.Value ?? "");

                    return new GeographyPoint(lat, lng);

                }
                return new GeographyPoint();
            }
        } 
        #endregion
    }
    /// <summary>
    /// 表示一个GPS坐标
    /// </summary>
    public class GeographyPoint
    {
        public GeographyPoint()
        {
            
        }

        public GeographyPoint(double lat, double lng)
        {
            Longitude = lng;
            Latitude = lat;
        }
        #region 百度 -> 火星
        /// <summary>
        /// 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法  将 BD-09 坐标转换成GCJ-02 坐标 
        /// </summary>
        /// <param name="baiduLat">纬度</param>
        /// <param name="baiduLng">经度</param>
        /// <returns></returns>
        public static GeographyPoint Baidu09ToGcj02(double baiduLat, double baiduLng)
        {
            double x = baiduLng - 0.0065, y = baiduLat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * Math.PI);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * Math.PI);
            double ggLng = z * Math.Cos(theta);
            double ggLat = z * Math.Sin(theta);
            return new GeographyPoint(ggLat, ggLng);
        }

        #endregion

        #region 火星 -> WGS84
        /// <summary>
        /// 火星坐标系 (GCJ-02) 转换为 WGS-84
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public static GeographyPoint Gcj02ToWgs84(double lat, double lng)
        {
            double wgsLng = lng * 2 - lng;
            double wgsLat = lat * 2 - lat;
            return new GeographyPoint(wgsLat, wgsLng);
        }
        #endregion

        #region 百度 -> WGS84
        /// <summary>
        /// 百度坐标转换为WGS84坐标
        /// </summary>
        /// <param name="baiduLat"></param>
        /// <param name="baiduLng"></param>
        /// <returns></returns>
        public static GeographyPoint Baidu09ToWGS84(double baiduLat, double baiduLng)
        {
            GeographyPoint gcj02 = Baidu09ToGcj02(baiduLat, baiduLng);
            return Gcj02ToWgs84(gcj02.Latitude, gcj02.Longitude);
        } 
        #endregion

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { set; get; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { set; get; }
    }
}

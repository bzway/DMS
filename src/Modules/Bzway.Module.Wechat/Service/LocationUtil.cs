using System.Collections.Generic;
using System.Text;

namespace Bzway.Module.Wechat.Wechat
{
    public static class LocationUtil
    {
        /// <summary>
        /// 获取谷歌今天静态地图Url。API介绍：https://developers.google.com/maps/documentation/staticmaps/?hl=zh-CN
        /// </summary>
        /// <param name="size"> "640x640"</param>
        /// <returns></returns>
        public static string GetGoogleStaticMap(int scale, IList<Markers> markersList, string size)
        {
            markersList = markersList ?? new List<Markers>();
            StringBuilder markersStr = new StringBuilder();
            foreach (var markers in markersList)
            {
                markersStr.Append("&markers=");
                if (markers.Size != MarkerSize.mid)
                {
                    markersStr.AppendFormat("size={0}%7C", markers.Size);
                }
                if (!string.IsNullOrEmpty(markers.Color))
                {
                    markersStr.AppendFormat("color:{0}%7C", markers.Color);
                }
                markersStr.Append("label:");
                if (!string.IsNullOrEmpty(markers.Label))
                {
                    markersStr.AppendFormat("{0}%7C", markers.Label);
                }
                markersStr.AppendFormat("{0},{1}", markers.X, markers.Y);
            }
            string parameters = string.Format("center=&zoom=&size={0}&maptype=roadmap&format=jpg&sensor=false&language=zh&{1}",
                                             size, markersStr.ToString());
            string url = "http://maps.googleapis.com/maps/api/staticmap?" + parameters;
            return url;
        }
    }
    /// <summary>
    /// 标记大小
    /// </summary>
    public enum MarkerSize
    {
        Default = mid,
        tiny = 0, mid = 1, small = 2
    }

    public class Markers
    {
        /// <summary>
        /// （可选）指定集合 {tiny, mid, small} 中的标记大小。如果未设置 size 参数，标记将以其默认（常规）大小显示。
        /// </summary>
        public MarkerSize Size { get; set; }
        /// <summary>
        /// （可选）指定 24 位颜色（例如 color=0xFFFFCC）或集合 {black, brown, green, purple, yellow, blue, gray, orange, red, white} 中预定义的一种颜色。
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// （可选）指定集合 {A-Z, 0-9} 中的一个大写字母数字字符。
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 经度longitude
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// 纬度latitude
        /// </summary>
        public double Y { get; set; }
    }
}

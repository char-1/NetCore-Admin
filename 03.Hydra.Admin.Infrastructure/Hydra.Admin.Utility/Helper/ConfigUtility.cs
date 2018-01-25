using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;

namespace Hydra.Admin.Utility.Helper
{
    public static class ConfigUtility<T> where T : new()
    {
        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        /// <returns></returns>
        public static T XmlConfig(string configPath)
        {
            T config;
            using (var fs = new FileStream(configPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var xs = new XmlSerializer(typeof(T));
                config = (T)xs.Deserialize(fs);
            }
            return config;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static T JsonConfig(string configPath)
        {
            T config;
            string myStr = string.Empty;
            using (var fs = new FileStream(configPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fs))
                {
                    myStr = streamReader.ReadToEnd();
                }
            }
            if (!string.IsNullOrEmpty(myStr))
            {
                config = JsonConvert.DeserializeObject<T>(myStr);
                return config;
            }
            else
                return default(T);
        }
    }
}

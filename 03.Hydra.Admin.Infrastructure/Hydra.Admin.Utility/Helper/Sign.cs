using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
namespace Hydra.Admin.Utility.Helper
{
    public class Sign
    {
        private const string Key = "89c3a5b4b09e40517e212aa8bf36e072";
        /// <summary>
        /// API参数对称加/解密KEY值
        /// </summary>
        private static Dictionary<string, object> _parameters = new Dictionary<string, object>();
        public static Dictionary<string, object> GetParameters(string myStr, bool aes = false)
        {
            var myDic = new Dictionary<string, object>();
            try
            {
                if (aes)
                {
                    myStr = EncryptHelper.AesDecrypt(myStr);
                }
                if (myStr == string.Empty)
                {

                }
                else
                {
                    myDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(myStr);
                }
            }
            catch
            {
                // ignored
            }
            return myDic;
        }

        public static bool Validate(Dictionary<string, object> parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            var isSuccess = false;
            _parameters = parameters;
            if (!_parameters.Keys.Contains("sign"))
                return false;
            if (CreateSign().Equals(_parameters["sign"].ToString().ToUpper()))
                isSuccess = true;
            return isSuccess;
        }
        public static string CreateSign()
        {
            var sb = new StringBuilder();
            var akeys = new ArrayList(_parameters.Keys);
            akeys.Sort(new A());
            foreach (string k in akeys)
            {
                string v = "";
                var value = _parameters[k];
                if (value.GetType().Name.Equals("JObject") || value.GetType().Name.Equals("JArray"))
                    v = JsonConvert.SerializeObject(value);
                else
                    v = value + "";
                if (string.Compare("", v, StringComparison.Ordinal) != 0
                    && string.Compare("sign", k, StringComparison.Ordinal) != 0 &&
                    string.Compare("key", k, StringComparison.Ordinal) != 0)
                {
                    sb.Append(k + "=" + v.ToLower() + "&");
                }
            }

            sb.Append("key=" + Key);
            return Md5(sb.ToString(), "utf-8").ToUpper();

        }
        /// <summary>
        /// 客户端加密
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string CreateSign(Dictionary<string, object> parameters)
        {
            var sb = new StringBuilder();
            var akeys = new ArrayList(parameters.Keys);
            akeys.Sort(new A());
            foreach (string k in akeys)
            {
                var v = parameters[k] + "";
                if (string.Compare("", v, StringComparison.Ordinal) != 0
                    && string.Compare("sign", k, StringComparison.Ordinal) != 0 &&
                    string.Compare("key", k, StringComparison.Ordinal) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + Key);
            return Md5(sb.ToString(), "utf-8").ToUpper();

        }
        public static string Md5(string encypStr, string charset)
        {
            var m5 = new MD5CryptoServiceProvider();
            //创建md5对象
            byte[] inputBye;
            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            var outputBye = m5.ComputeHash(inputBye);
            var retStr = BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        public static object GetParameters(object v)
        {
            throw new NotImplementedException();
        }
    }

    class A : IComparer
    {
        #region IComparer Members

        public int Compare(object x, object y)
        {
            return string.Compare((x + ""), (y + ""), StringComparison.Ordinal);
        }

        #endregion
    }
}

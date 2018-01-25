using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Hydra.Admin.Utility.Helper
{
    public static class TextHelper
    {
        /// <summary>
        /// 检查字符串是不是为空/Null/空白字符
        /// </summary>
        /// <param name="self">要检查的字符串</param>
        /// <returns></returns>
        public static bool IsNullOrBlank(this string self)
        {
            return string.IsNullOrWhiteSpace(self);
        }

        public static bool NotNullOrBlank(this string self)
        {
            return !string.IsNullOrWhiteSpace(self);
        }

        public static bool EqualsIgnoreCase(this string self, string other)
        {
            return self.Equals(other, StringComparison.OrdinalIgnoreCase);
        }
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
    }
}

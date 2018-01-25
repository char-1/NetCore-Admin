using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Hydra.Admin.Utility.Helper
{
    public class NetHelper
    {
        //获取本地的IP地址
        public static string GetIPAddress()
        {
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
    }
}

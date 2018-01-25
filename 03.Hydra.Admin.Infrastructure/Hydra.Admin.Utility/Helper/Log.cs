using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace Hydra.Admin.Utility.Helper
{
    public class Log
    {
        public static void Debug(object message)
        {
            LogManager.GetLogger(GetCurrentMethodFullName()).Debug(message);
        }
        public static void Info(object message)
        {
            LogManager.GetLogger(GetCurrentMethodFullName()).Info(message);
        }
        public static void Info(string message, Exception ex)
        {
            LogManager.GetLogger(GetCurrentMethodFullName()).Info(ex, message);
        }
        public static void Warn(object message)
        {
            LogManager.GetLogger(GetCurrentMethodFullName()).Warn(message);
        }
        public static void Warn(string message, Exception ex)
        {
            LogManager.GetLogger(GetCurrentMethodFullName()).Warn(ex, message);
        }
        public static void Error(object message)
        {
            LogManager.GetLogger(GetCurrentMethodFullName()).Error(message);
        }
        public static void Error(string message, Exception ex)
        {
            LogManager.GetLogger(GetCurrentMethodFullName()).Error(ex, message);
        }
        private static string GetCurrentMethodFullName()
        {
            try
            {
                StackFrame frame;
                string str2;
                int num = 2;
                StackTrace trace = new StackTrace();
                int length = trace.GetFrames().Length;
                do
                {
                    frame = trace.GetFrame(num++);
                    str2 = frame.GetMethod().DeclaringType.ToString();
                }
                while (str2.EndsWith("Excption") && (num < length));
                string name = frame.GetMethod().Name;
                return (str2 + "." + name);
            }
            catch
            {
                return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            }
        }
    }
}

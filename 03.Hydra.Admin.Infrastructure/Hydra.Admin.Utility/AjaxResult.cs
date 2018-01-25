namespace Hydra.Admin.Utility
{
    public class AjaxResult
    {
        public AjaxResult()
        {
            state = ResultType.success.ToString();
            message = string.Empty;
        }
        /// <summary>
        /// 操作结果类型
        /// </summary>
        public object state { get; set; }
        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public dynamic data { get; set; }

    }
    /// <summary>
    /// 表示 ajax 操作结果类型的枚举
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 消息结果类型
        /// </summary>
        info,
        /// <summary>
        /// 成功结果类型
        /// </summary>
        success,
        /// <summary>
        /// 警告结果类型
        /// </summary>
        warning,
        /// <summary>
        /// 异常结果类型
        /// </summary>
        error,
        /// <summary>
        /// 未授权
        /// </summary>
        noAuth,
        /// <summary>
        /// 授权无效
        /// </summary>
        faildAuth
    }
}
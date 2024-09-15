
namespace VueAdmin.Api.Dtos
{
    /// <summary>
    /// api返回结果
    /// </summary>  
    public class ResultDto
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string Code { get; set; } = ResCode.Fail;
        /// <summary>
        /// 消息
        /// </summary>      
        public string Msg { get; set; } = "fail";
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 数据赋值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public virtual void SetData(object data, string code = ResCode.Success, string msg = "success")
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
        }
    }

    /// <summary>
    /// 结果码
    /// </summary>
    public class ResCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const string Success = "000";
        /// <summary>
        /// 失败
        /// </summary>
        public const string Fail = "400";
        /// <summary>
        /// 无权限
        /// </summary>
        public const string Nopermit = "401";


    }


}

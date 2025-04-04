
using VueAdmin.Core.Enums;

namespace VueAdmin.Api.Dtos
{
    /// <summary>
    /// api返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultDto<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = (int)ResultCode.Fail;
        /// <summary>
        /// 消息
        /// </summary>      
        public string Msg { get; set; } = "fail";

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        ///  数据赋值
        /// </summary>    
        /// <param name="msg"></param>
        public virtual void Ok(string msg = "success")
        {
            this.Code = (int)ResultCode.Success;
            this.Msg = msg;
        }

        /// <summary>
        ///  数据赋值
        /// </summary>   
        /// <param name="msg"></param>
        public virtual void Error(string msg = "fail")
        {
            this.Code = (int)ResultCode.Fail;
            this.Msg = msg;
        }

        /// <summary>
        /// 数据赋值
        /// </summary>
        /// <param name="data"></param>      
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public virtual void SetData(T data, int code = (int)ResultCode.Success, string msg = "success")
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
        }
    }



}

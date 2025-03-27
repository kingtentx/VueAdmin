
using VueAdmin.Core.Enums;

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
        public int Code { get; set; } = (int)ResultCode.Fail;
        /// <summary>
        /// 消息
        /// </summary>      
        public string Msg { get; set; } = "fail";
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }

        public bool Success { get; set; } = false;
        /// <summary>
        /// 数据赋值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public virtual void SetData(object data, int code = (int)ResultCode.Success, string msg = "success")
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
            this.Success = true;
        }
    }

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
        /// 
        /// </summary>
        public int Count { get; set; } = 0;
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
        public bool Success { get; set; } = false;
        /// <summary>
        ///  数据赋值
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public virtual void SetData(int code = (int)ResultCode.Success, string msg = "success")
        {
            this.Code = code;
            this.Msg = msg;
            this.Success = true;
        }

        /// <summary>
        /// 数据赋值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public virtual void SetData(T data, int count = 0, int code = (int)ResultCode.Success, string msg = "success")
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
            this.Count = count;
            this.Success = true;
        }
    }



}

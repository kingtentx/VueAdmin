using VueAdmin.Core.Enums;

namespace VueAdmin.Web.Models
{
    /// <summary>
    /// 后台返回结果集
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = ResultCode.Fail.GetHashCode();
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = "fail";
        /// <summary>
        /// data
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 数据赋值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public virtual void SetData(object data, int code = (int)ResultCode.Success, string message = "success")
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }

    /// <summary>
    /// api返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = (int)ResultCode.Fail;
        /// <summary>
        /// 消息
        /// </summary>      
        public string Message { get; set; } = "fail";
        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; } = 0;
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        ///  数据赋值
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public virtual void SetData(int code = (int)ResultCode.Success, string message = "success")
        {
            this.Code = code;
            this.Message = message;
        }

        /// <summary>
        /// 数据赋值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public virtual void SetData(T data, int count = 0, int code = (int)ResultCode.Success, string message = "success")
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
            this.Count = count;
        }
    }
}

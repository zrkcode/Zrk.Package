using System;
using System.Collections.Generic;
using System.Text;

namespace Zrk.Common.Results
{
    public class ApiResult
    {
        private bool iserror = false;

        /// <summary>
        /// 是否产生错误
        /// </summary>
        public bool IsError { get { return iserror; } }

        /// <summary>
        /// 错误信息或者成功信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 成功时返回的数据
        /// </summary>
        public object Data { get; set; }

        #region Error
        /// <summary>
        /// 错误
        /// </summary>
        /// <returns></returns>
        public static ApiResult Error()
        {
            return new ApiResult()
            {
                iserror = true
            };
        }
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <returns></returns>
        public static ApiResult Error(string message)
        {
            return new ApiResult()
            {
                iserror = true,
                Message = message
            };
        }
        #endregion

        #region Success
        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static ApiResult Success()
        {
            return new ApiResult()
            {
                iserror = false
            };
        }
        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static ApiResult Success(string message)
        {
            return new ApiResult()
            {
                iserror = false,
                Message = message
            };
        }
        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static ApiResult Success(object data)
        {
            return new ApiResult()
            {
                iserror = false,
                Data = data
            };
        }
        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static ApiResult Success(string message, object data)
        {
            return new ApiResult()
            {
                iserror = false,
                Data = data,
                Message = message
            };
        }
        #endregion
    }
}

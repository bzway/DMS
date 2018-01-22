using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace System
{
    public class Result
    {
        public static Result<Data> Fail<Data>(ResultCode error, string message = "")
        {
            return new Result<Data>()
            {
                Code = (int)error,
                Time = DateTime.UtcNow.Ticks,
                Data = default(Data),
                Message = string.IsNullOrEmpty(message) ? error.ToString() : message,
            };
        }
        public static Result<Data> OK<Data>(Data data)
        {
            return new Result<Data>()
            {
                Data = data,
                Time = DateTime.UtcNow.Ticks,
                Code = (int)ResultCode.OK,
                Message = string.Empty,
            };
        }
    }


    public class Result<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public long Time { get; set; }
        public T Data { get; set; }
        public static Result<T> Fail(ResultCode responseCode, string Message = "", T data = default(T))
        {
            if (string.IsNullOrEmpty(Message))
            {
                Message = responseCode.ToString();
            }
            return new Result<T>()
            {
                Time = DateTime.UtcNow.Ticks,
                Message = Message,
                Code = (int)responseCode,
                Data = data,
            };
        }
        public static Result<T> Fail(HttpStatusCode httpStatusCode, string Message = "", T data = default(T))
        {
            if (string.IsNullOrEmpty(Message))
            {
                Message = httpStatusCode.ToString();
            }
            return new Result<T>()
            {
                Time = DateTime.UtcNow.Ticks,
                Message = Message,
                Code = (int)httpStatusCode,
                Data = data,
            };
        }
        public static Result<T> Success(T data = default(T), string Message = "")
        {
            return new Result<T>()
            {
                Time = DateTime.UtcNow.Ticks,
                Message = Message,
                Code = (int)HttpStatusCode.OK,
                Data = data,
            };
        }
    }
    public enum ResultCode : int
    {

        OK = 200,
        Error = -1,
    }
}
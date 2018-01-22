using System;
using System.Collections.Generic;

namespace WebApp.Models
{

    public class MessageResponseModel
    {
        public int TotalItem { get { return this.List.Count; } }
        public string LastVersion { get; set; }
        public List<Operation> List { get; set; }
        public MessageResponseModel()
        {
            this.List = new List<Operation>();
        }
    }

    public class MessageRequestModel
    {
        public string LastVersion { get; set; }

        public string[] Ack { get; set; }
    }

    public class Operation
    {
        public Operation()
        {
            //MessageList = new List<Messages>();
        }

        public int Type { get; set; }
        public string SessionId { get; set; }
        public int SessionType { get; set; }
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string FromImage { get; set; }
        public string ToId { get; set; }
        //public string ToName { get; set; }
        //public string ToImage { get; set; }
        public string OriginalId { get; set; }
        public string CreatedOn { get; set; }
        public List<Messages> MessageList { get; set; }
    }

    public class Messages
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public int ContentType { get; set; }
        public string Content { get; set; }
        public string CreateTime { get; set; }
    }


    public class SendResponseModel
    {
        public int Id { get; set; }
        public string ToName { get; set; }
        public string ToImage { get; set; }
    }

    public class SendRequestModel
    {
        public string SessionId { get; set; }
        public string Content { get; set; }
        public int ContentType { get; set; }
        public string CreateTime { get; set; }
    }


    public class ReceiveResponseModel
    {
        public string Message { get; set; }
    }

    public class ReceiveRequestModel
    {
        public string SessionId { get; set; }
        public string Content { get; set; }
        public MessageContentType ContentType { get; set; }
    }
    public enum MessageContentType : int
    { }
    public class TransferResponseModel
    {

    }

    public class TransferRequestModel
    {
        public string SessionId { get; set; }
        public string ToNewId { get; set; }
    }

    public class CloseSessionResponseModel
    {

    }

    public class CloseSessionRequestModel
    {
        public string SessionId { get; set; }
    }

    public class SessionModel
    {
        public string SessionId { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        public string FromId { get; set; }
        /// <summary>
        /// CSRID
        /// </summary>
        public string ToId { get; set; }
        public SessionType Type { get; set; }
        public SessionStatus Status { get; set; }
        public int? OrignalId { get; set; }
        public string Remark { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

    }
    public enum SessionType : int
    { }
    public enum SessionStatus : int
    { }
    public class CheckSessionRequestModel
    {
        public string FromId { get; set; }
    }

    public class StartSessionResponseModel
    {

    }

    public class StartSessionRequestModel
    {
        public int? IsDelay { get; set; }
        public string FromId { get; set; }
    }

    public class RestartSessionResponseModel
    {

    }

    public class RestartSessionRequestModel
    {
        public string SessionId { get; set; }
    }

    public class HistoryMessageResponseModel
    {
        public HistoryMessageResponseModel()
        {
            list = new List<HistoryMessage>();
        }

        public List<HistoryMessage> list { get; set; }
    }

    public class HistoryMessageRequestModel
    {
        public string SessionId { get; set; }

        public string LastVersionDate { get; set; }

        public int MessageNum { get; set; }
    }

    public class HistoryMessage
    {
        public string SessionId { get; set; }
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string FromImage { get; set; }
        public string ToName { get; set; }
        public string ToImage { get; set; }
        public string ToId { get; set; }
        public int MessageType { get; set; }
        public int ContentType { get; set; }
        public string Content { get; set; }
        public string CreatedOn { get; set; }
    }
}
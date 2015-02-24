//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iCelerium.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MessageLog
    {
        public int Id { get; set; }
        public System.DateTime SendTime { get; set; }
        public Nullable<System.DateTime> ReceiveTime { get; set; }
        public Nullable<int> StatusCode { get; set; }
        public string StatusText { get; set; }
        public string MessageTo { get; set; }
        public string MessageFrom { get; set; }
        public string MessageText { get; set; }
        public string MessageType { get; set; }
        public string MessageId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorText { get; set; }
        public string Gateway { get; set; }
        public string MessagePDU { get; set; }
        public string UserId { get; set; }
        public string UserInfo { get; set; }
        public string Accounting { get; set; }
    }
}
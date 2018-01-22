using Bzway.Data.Core;
using System;
namespace Bzway.Module.MemberClub.Entity
{
    public class Transaction : EntityBase
    {
        public string TransactionCode { get; set; }

        public string MemberCode { get; set; }

        public string ProductCode { get; set; }

        public string ShopCode { get; set; }

        public decimal Amount { get; set; }

        public decimal Discount { get; set; }

        public decimal Remission { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime TransactionTime { get; set; }

        public string Remark { get; set; }
    }

}

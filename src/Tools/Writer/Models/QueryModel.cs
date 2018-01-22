using System;

namespace WebApp.Models
{
    public class AccountSearchCondition
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public AccountSearchType searchtype { get; set; }
    }

    public enum AccountSearchType
    {
        包含 = 0,
        等于 = 1
    }
}

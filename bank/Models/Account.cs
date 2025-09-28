using Microsoft.VisualBasic;
using System;
using System.Security.AccessControl;

namespace bank.Models
{
    public class Account
    {
        public int userId { get; set; }
        public int accNumber { get; set; }
        public string? accType { get; set; }
            public double accBalance { get; set; }
            public string? accStatus { get; set; }
}
}

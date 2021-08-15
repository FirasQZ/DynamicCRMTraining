using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RibbonAccountMVC.Models
{
    public class AccountEntityModels
    {
        public Guid AccountID { get; set; }
        public string AccountName { get; set; }
        public string WebsiteURL { get; set; }
        public string telephone1 { get; set; }
        public string emailaddress1 { get; set; }
        public String owninguser { get; set; }
        public Money  Revenue { get; set; }
        public decimal RevenueValue { get; set; }
        public int AccountStatus { get; set; }

    }
}
using log4net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using RibbonAccountMVC.DynamicsConnection;
using RibbonAccountMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Web;

namespace RibbonAccountMVC.DAL
{
    public class DAL_AccountEntity // data access layer
    {
        private static readonly ILog log = LogHelper.GetLogger();


        // return list contain abject as detail of account
        public List<AccountEntityModels> RetriveRecords(string accountId)
        {
            AccountEntityModels accountModel;
            List<AccountEntityModels> info = new List<AccountEntityModels>();
            try
            {
                using (OrganizationServiceContext orgSvcContext = new OrganizationServiceContext(OAuthConnection.getInstance()))
                {
                    var AccountInfo = from a in orgSvcContext.CreateQuery("account")
                                     join b in orgSvcContext.CreateQuery("systemuser")
                                     on a["owninguser"] equals b["systemuserid"]
                                     where (String)a["accountid"] == accountId
                                     select new AccountEntityModels
                                     {
                                         AccountID = (Guid)a.GetAttributeValue<Guid>("accountid"),
                                         AccountName = a.GetAttributeValue<String>("name"),
                                         emailaddress1 = a.GetAttributeValue<String>("emailaddress1"),
                                         owninguser = b.GetAttributeValue<String>("fullname"),
                                         telephone1 = a.GetAttributeValue<String>("telephone1"),
                                         WebsiteURL = a.GetAttributeValue<String>("websiteurl"),
                                         Revenue = a.GetAttributeValue<Money>("revenue")
                                         
                                     };
                    accountModel = new AccountEntityModels();
                    foreach (AccountEntityModels entity in AccountInfo)
                    {
                        if (entity.AccountName != null)
                            accountModel.AccountName = entity.AccountName;
                        if (entity.AccountID != null)
                            accountModel.AccountID = entity.AccountID;
                        if (entity.emailaddress1 != null)
                            accountModel.emailaddress1 = entity.emailaddress1;
                        if (entity.owninguser != null)
                            accountModel.owninguser = entity.owninguser;
                        if (entity.WebsiteURL != null)
                            accountModel.WebsiteURL = entity.WebsiteURL;
                        if (entity.telephone1 != null)
                            accountModel.telephone1 = entity.telephone1;
                        if (entity.Revenue != null)
                            accountModel.RevenueValue = Convert.ToDecimal(entity.Revenue.Value);
                        info.Add(accountModel);
                    }
                }
                if (info.Count>0)
                {
                    log.Info("Account is exsist");
                    return info;
                }
                else
                {
                    log.Info("Account is not exsist");
                    accountModel.AccountStatus = 1;
                    info.Add(accountModel);
                    return info;
                }
            }
            catch (Exception ex)
            {
                log.Info("exception error when retrive account");
                return null;
            }
        }
      
    }
}
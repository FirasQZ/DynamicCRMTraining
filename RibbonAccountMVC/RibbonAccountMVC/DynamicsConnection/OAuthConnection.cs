using log4net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using RibbonAccountMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RibbonAccountMVC.DynamicsConnection
{
    public class OAuthConnection
    {

        private static IOrganizationService _service = null;
        private static readonly ILog log = LogHelper.GetLogger();



        private OAuthConnection()
        {
        }



        public static IOrganizationService getInstance()
        {
            try
            {
                if (_service == null)
                {
                    _service = CreateConnect();
                    log.Info("success connection, New Instance");
                }
                log.Info("Instance allready esist");
                return _service;
            }
            catch (Exception ex)
            {
                log.Info("fial Connection");
                return null;
            }
        }




        // -- create a connection with dynamic CRM 365
        public static IOrganizationService CreateConnect()
        {
            try
            {
                CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["OAuthConnection"].ConnectionString);
                if (crmSvc.IsReady)
                {
                    log.Info("CRM service Client is Ready");
                    _service = (IOrganizationService)crmSvc.OrganizationWebProxyClient != null ? (IOrganizationService)crmSvc.OrganizationWebProxyClient : (IOrganizationService)crmSvc.OrganizationServiceProxy;
                    return _service;

                }
                else
                {
                    return null;
                }              
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static IOrganizationService getContext()
        {            
            return _service;
        }
    }
}
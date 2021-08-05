using log4net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRMWithoutCore
{
    class CRMConnection
    {
        // -- using this class connection as a singilton class
        
        
        private static IOrganizationService _service = null;
        private static readonly ILog log = LogHelper.GetLogger();



        private CRMConnection()
        {
        }



        public static IOrganizationService getInstance()
        {
            try
            {
                if (_service == null)
                {

                    _service = CreateConnect();
                    log.Info("New Instance");

                }
                log.Info("Instance allready esist");
                return _service;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        
        
        
        // -- create a connection with dynamic CRM 365
        public static IOrganizationService CreateConnect()
        {
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["OAuthConnection2"].ConnectionString);
            _service = (IOrganizationService)crmSvc.OrganizationWebProxyClient != null ? (IOrganizationService)crmSvc.OrganizationWebProxyClient : (IOrganizationService)crmSvc.OrganizationServiceProxy;
            Console.WriteLine("success Connection");
            return _service;

        }

        public static IOrganizationService getContext()
        {

            return _service;
        }
    }
}

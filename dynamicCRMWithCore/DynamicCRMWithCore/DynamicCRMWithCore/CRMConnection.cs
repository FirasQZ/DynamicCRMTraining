using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            ServiceClient crmSvc = new ServiceClient(configuration["connectionString"]);
            Console.WriteLine("success Connection");
            return crmSvc;

        }
    }
}

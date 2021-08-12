using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRMPlugin2
{
    public class AccountPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                //serviceProvider can do tracing service,ex: onile hard to make debug then use trace to show where is exception error accure.
                ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                // it is contaion data for entity apply a plugin ex account , contact,case...etc
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                // get this service to deal with crm.
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                // update , delete , retrive , retrive multible ... etc).
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                // context contain data about entity contain target input parameter ( this is sure an plugin is register on entity). and this targer it is a entity.
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    // get a entity name ex (account,case,contect...etc) do [ casting entity object ] .
                    Entity entity = (Entity)context.InputParameters["Target"];
                    if (entity.LogicalName != "account")
                        return;
                    Entity account = (Entity)context.InputParameters["Target"];
                    Entity preImageAccount = (Entity)context.PreEntityImages["Image"];
                    Entity postImageAccount = (Entity)context.PostEntityImages["Image"];

                    string preImagePhoneNumber = preImageAccount.GetAttributeValue<string>("telephone1");
                    string postImagePhoneNumber = postImageAccount.GetAttributeValue<string>("telephone1");

                    tracingService.Trace("Pre-image phone number: {0}, Post-image phone number: {1}", preImagePhoneNumber, postImagePhoneNumber);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}

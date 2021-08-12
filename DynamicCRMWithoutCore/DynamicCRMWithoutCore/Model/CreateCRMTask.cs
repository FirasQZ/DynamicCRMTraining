using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamic365Plugin.Model
{
    class CreateCRMTask : IPlugin
    {
        // first method run in plugin and run a logic , this method mut public , this method serviceProvider parameter contain information about plugin executed.
        // using plugin registration tool to regester plugin and work flow.
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

                    Entity task = new Entity("task");
                    task["subject"] = "task plugin training subject";
                    task["description"] = "task plugin training description";
                    service.Create(task);


                }
                else
                {

                }
            }
            catch(Exception ex)
            {

            }
           
        }
    }
}

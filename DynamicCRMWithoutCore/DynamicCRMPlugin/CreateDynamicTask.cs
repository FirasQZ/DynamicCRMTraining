using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRMPlugin
{
    public class CreateDynamicTask : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {  
            try
            {
                ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                if (context == null)
                {
                    throw new InvalidPluginExecutionException("Please try again for removed Account");
                }

                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
                if (context.InputParameters.Contains("Target"))
                {
                    EntityReference entity = (EntityReference)context.InputParameters["Target"];
                    if (entity.LogicalName != "account")
                        return;
                    EntityReference account = (EntityReference)context.InputParameters["Target"];
                    Entity preImageUOPAccount = (Entity)context.PreEntityImages["UOP"];
                    if (preImageUOPAccount != null)
                    {
                        Guid preImageAccountId = preImageUOPAccount.GetAttributeValue<Guid>("accountid");
                        String preImageAccountName = preImageUOPAccount.GetAttributeValue<String>("name");
                        // create a new account have same account name olde account deleted
                        createAccount(service, preImageAccountName);
                        // delete current account
                        deleteAccount(service, preImageAccountId);
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException("Account is deleted");
                    }
                }
                else
                {

                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw new InvalidPluginExecutionException("Please try again for removed Account" + ex.Message);
            }
        }

        // delete account
        public void deleteAccount(IOrganizationService _service, Guid id)
        {
            try
            {
               _service.Delete("account", id);               
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw new InvalidPluginExecutionException("Dont deleted Account" + ex.Message);

            }
        }
        // create new account 
        public void createAccount(IOrganizationService _service, String accountName)
        {
            try
            {
                Entity newAccount = new Entity("account");
                newAccount["name"] = "new_"+ accountName;
                _service.Create(newAccount);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("Dont created New Account" + ex.Message);

            }
        }
    }
}

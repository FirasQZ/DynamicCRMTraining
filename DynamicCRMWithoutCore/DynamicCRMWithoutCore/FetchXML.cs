using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRMWithoutCore
{
    class FetchXML
    {



        // -- get Account entity then get primary Contact entity
        public void getAccountContact(IOrganizationService _service)
        {
            string xml = @"<fetch mapping='logical'>  
                               <entity name='account'>   
                                  <attribute name='primarycontactid'/>   
                                  <attribute name='name'/>   
                                    <filter type='and'>   
                                        <condition attribute='statecode' operator='eq' value='0' />   
                                    </filter>                                    
                               </entity>   
                            </fetch> ";
            EntityCollection contactcollection = _service.RetrieveMultiple(new FetchExpression(xml));
            foreach (Entity entity in contactcollection.Entities)
            {
                var primarycontactid = entity.GetAttributeValue<EntityReference>("primarycontactid");
                if (primarycontactid != null)
                {
                    Guid id = primarycontactid.Id;
                    System.Console.WriteLine("Account : ");
                    System.Console.WriteLine(entity.Attributes["name"]);
                    getContact(_service, id);
                }
            }
        }
        public static void getContact(IOrganizationService _service, Guid accountId)
        {
            string xml = @"<fetch mapping='logical'>  
                               <entity name='contact'>   
                                  <attribute name='contactid'/>   
                                  <attribute name='firstname'/>   
                                  <attribute name='lastname'/>   
                                    <filter type='and'>   
                                        <condition attribute='contactid' operator='eq' value='" + accountId + "' />" + @"   
                                    </filter>                                    
                               </entity>   
                            </fetch> ";
            EntityCollection contactcollection = _service.RetrieveMultiple(new FetchExpression(xml));
            foreach (Entity entity in contactcollection.Entities)
            {
                System.Console.WriteLine("Contact : ");
                Console.WriteLine("First Name: " + entity.Attributes["firstname"] + " - Last Name: " + entity.Attributes["lastname"]);
            }
        }





        // -- get List of Account Entity
        public void getAccount(IOrganizationService _service)
        {
            string xml = @"<fetch mapping='logical'>  
                               <entity name='account'>   
                                  <attribute name='primarycontactid'/>   
                                  <attribute name='name'/>   
                                    <filter type='and'>   
                                        <condition attribute='statecode' operator='eq' value='0' />   
                                    </filter>                                    
                               </entity>   
                            </fetch> ";
            EntityCollection contactcollection = _service.RetrieveMultiple(new FetchExpression(xml));
            System.Console.WriteLine("List of Account : ");
            int Counter = 1;
            foreach (Entity entity in contactcollection.Entities)
            {
                System.Console.WriteLine(Counter+" - Account name : " +entity.Attributes["name"]);
                Counter++;
            }
        }

    }
}

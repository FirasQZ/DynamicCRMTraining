using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRMWithoutCore
{
    class QueryExpressions
    {
        public Guid id;
        public QueryExpression query;
        public ArrayList ListOfAccountID = new ArrayList();
        public ArrayList ListOfCaseID = new ArrayList();
        private static Guid _myUserId;
        private static Guid _otherUserId;


        /// <summary>
        ///  Account proccess
        /// </summary>
        /// <param name=""></param>

        // -- get all primary contact related with account using LINQ
        public void getAccountAndContact(IOrganizationService _service)
        {
            query = new QueryExpression("account");
            FilterExpression fillter = query.Criteria.AddFilter(LogicalOperator.And);
            fillter.AddCondition("statecode", ConditionOperator.Equal, 0);
            query.ColumnSet.AddColumns("name");
            query.ColumnSet.AddColumns("primarycontactid");

            EntityCollection result = _service.RetrieveMultiple(query);
            foreach (var c in result.Entities)
            {
                var primarycontactid = c.GetAttributeValue<EntityReference>("primarycontactid"); ;

                if (primarycontactid != null)
                {
                    Console.WriteLine("Acount : " + c.Attributes["name"]);
                    Guid id = primarycontactid.Id;
                    getContact(_service, id.ToString());
                }
            }

        }
        public void getContact(IOrganizationService _service, string x)
        {
            query = new QueryExpression("contact");
            query.ColumnSet.AddColumns("firstname", "lastname", "contactid");
            FilterExpression fillter = query.Criteria.AddFilter(LogicalOperator.And);
            fillter.AddCondition("contactid", ConditionOperator.Equal, x);
            EntityCollection result = _service.RetrieveMultiple(query);
            Console.WriteLine("Primary Contact : ");
            foreach (var c in result.Entities)
            {
                Console.WriteLine("First Name: " + c.Attributes["firstname"] + " - Last Name: " + c.Attributes["lastname"]);
            }
        }




        // -- get account Entity from dynamic using  QueryExpression
        public void getAccount(IOrganizationService _service)
        {
            query = new QueryExpression("account");
            FilterExpression fillter = query.Criteria.AddFilter(LogicalOperator.And);
            fillter.AddCondition("statecode", ConditionOperator.Equal, 0);
            query.ColumnSet.AddColumns("name");
            query.ColumnSet.AddColumns("accountid");
            EntityCollection result = _service.RetrieveMultiple(query);
            Console.WriteLine("List of Account : ");
            int counter = 1;
            foreach (var c in result.Entities)
            {
                ListOfAccountID.Add(c.Attributes["accountid"]);
                Console.WriteLine(counter + " - Acount Name : " + c.Attributes["name"]);
                counter++;
            }

        }




        // -- update information account using QueryExpression ex : name of account

        // latebound
        public void UpdateInfoAccountLateBound(Guid id, QueryExpression query, IOrganizationService _service)
        {
            System.Console.WriteLine("Please set New  Account Name : .......... ");
            string AccountName = Console.ReadLine();
            Entity Account = new Entity(query.EntityName);
            Account.Id = id;
            Account.Attributes["name"] = AccountName;
            _service.Update(Account);
            Console.WriteLine("Updated Account");

        }
        //earlybound
        public void updateAccountEarlyBound(Guid id,IOrganizationService _service)
        {
            using (var context = new ServiceContext(_service))
            {
                Account record = context.AccountSet.FirstOrDefault(c => c.AccountId == id);
                if (record != null)
                {
                    record.Telephone1 = "0594161250";
                    context.UpdateObject(record);
                    if (context.SaveChanges().Count > 0)
                    {
                        Console.WriteLine("Account Updated Successfully");
                        Console.ReadKey();
                    }
                }

            }
        }





        // -- delete account using QueryExpression 
        // latebound
        public void DeleteAccountLateBound(Guid id, QueryExpression query, IOrganizationService _service)
        {
            Entity Account = new Entity(query.EntityName);
            _service.Delete("account", id);
            System.Console.WriteLine("Account is deleted");
        }
        // -- early bound 
        public void DeleteAccountEarlyBound(Guid id,IOrganizationService _service)
        {
            using (var context = new ServiceContext(_service))
            {
                Account record = context.AccountSet.FirstOrDefault(c => c.AccountId == id);
                if (record != null)
                {
                    context.DeleteObject(record);
                    if (context.SaveChanges().Count > 0)
                    {
                        Console.WriteLine("Account Deleted Successfully");
                    }
                }

            }
        }



        // -- Deactivate account, update statecode 0 is active 1 is deactive
        // latebound
        public void DeactivateAccountLateBound(Guid id, QueryExpression query, IOrganizationService _service)
        {
            Entity Account = new Entity(query.EntityName);
            Account.Id = id;
            Account.Attributes["statecode"] = new OptionSetValue(1);
            _service.Update(Account);
            Console.WriteLine("Updated Status , account is deactivate ");
        }
        // -- earlybound
        public void DeactivateAccountEarlyBound(Guid id, IOrganizationService _service)
        {
            using (var context = new ServiceContext(_service))
            {
                Account record = context.AccountSet.FirstOrDefault(c => c.AccountId == id);
                if (record != null)
                {
                    record.StateCode = AccountState.Inactive;
                    context.UpdateObject(record);
                    if (context.SaveChanges().Count > 0)
                    {
                        Console.WriteLine("Account deactivated Successfully");
                    }
                }

            }
        }




        /// <summary>
        ///  Case proccess
        /// </summary>
        /// <param name="_service"></param>
        /// 

        // -- get all case in dynamic  using QueryExpression 
        public void getCase(IOrganizationService _service, QueryExpression query)
        {

            query = new QueryExpression("incident");
            query.ColumnSet.AddColumns("title", "ticketnumber", "incidentid","statecode");
            EntityCollection result = _service.RetrieveMultiple(query);
            Console.WriteLine("All Case : ");
            int counter = 0;
                 foreach (var c in result.Entities)
            {
                counter++;
                var incidentid = c.GetAttributeValue<Guid>("incidentid");
                Guid id = incidentid;
                ListOfCaseID.Add(id);
                Console.WriteLine(counter + "- Title: " + c.Attributes["title"] + "   ID: " + id + "   Owner: " + c.Attributes["incidentid"] + "   statecode: " + c.Attributes["statecode"]);
            }
        }




        // -- close Case using QueryExpression 
        public void resolvelCase(IOrganizationService _service, Guid id)
        {

            var incidentResolution = new IncidentResolution
            {
                Subject = "Resolved Sample Incident",
                IncidentId = new EntityReference(Incident.EntityLogicalName, id),
                Description = "test"
            };
            // Close the incident with the resolution.
            int x = (int)IncidentResolutionState.Completed;
            var closeIncidentRequest = new CloseIncidentRequest
            {
                IncidentResolution = incidentResolution,
                Status = new OptionSetValue(5)
            };
            _service.Execute(closeIncidentRequest);
            Console.WriteLine("  Incident closed.");

        }








        /// <summary>
        ///  Assign proccess
        /// </summary>
        /// <param name="_service"></param>
        /// 

        public void getUserSystem(IOrganizationService _service)
        {
            var userRequest = new WhoAmIRequest();
            WhoAmIResponse user = (WhoAmIResponse)_service.Execute(userRequest);
            // Current user.
            _myUserId = user.UserId;
            var querySystemUser = new QueryExpression
            {
                EntityName = SystemUser.EntityLogicalName,
                ColumnSet = new ColumnSet(new String[] { "systemuserid", "fullname" }),
                Criteria = new FilterExpression()
            };

            querySystemUser.Criteria.AddCondition("businessunitid",ConditionOperator.Equal, user.BusinessUnitId);
            querySystemUser.Criteria.AddCondition("systemuserid",ConditionOperator.Equal, _myUserId);
            // Excluding SYSTEM user.
            querySystemUser.Criteria.AddCondition("lastname",ConditionOperator.NotEqual, "SYSTEM");
            // Excluding INTEGRATION user.
            querySystemUser.Criteria.AddCondition("lastname",ConditionOperator.NotEqual, "INTEGRATION");

            DataCollection<Entity> otherUsers = _service.RetrieveMultiple(querySystemUser).Entities;

            int count = _service.RetrieveMultiple(querySystemUser).Entities.Count;
            if (count > 0)
            {
                _otherUserId = (Guid)otherUsers[count - 1].Attributes["systemuserid"];

                Console.WriteLine("Retrieved new owner {0} for assignment.",otherUsers[count - 1].Attributes["fullname"]);
            }
            else
            {
                Console.WriteLine("No user");
            }
        }
        
        
    }
}

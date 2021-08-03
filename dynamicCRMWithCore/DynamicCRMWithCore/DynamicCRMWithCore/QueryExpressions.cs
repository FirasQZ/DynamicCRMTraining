using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        // -- latebound
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




        public void PagingQueryExpression(Guid _id, IOrganizationService _service,int queryCount,int pageNumber,int recordCount)
        {

            // Define the condition expression for retrieving records.
            ConditionExpression pagecondition = new ConditionExpression();
            pagecondition.AttributeName = "parentcustomerid";
            pagecondition.Operator = ConditionOperator.Equal;
            pagecondition.Values.Add(_id);

            // Define the order expression to retrieve the records.
            OrderExpression order = new OrderExpression();
            order.AttributeName = "firstname";
            order.OrderType = OrderType.Ascending;

            // Create the query expression and add condition.
            QueryExpression pagequery = new QueryExpression();
            pagequery.EntityName = "contact";
            pagequery.Criteria.AddCondition(pagecondition);
            pagequery.Orders.Add(order);
            pagequery.ColumnSet.AddColumns("firstname", "lastname");

            // Assign the pageinfo properties to the query expression.
            pagequery.PageInfo = new PagingInfo();
            pagequery.PageInfo.Count = queryCount;
            pagequery.PageInfo.PageNumber = pageNumber;

            // The current paging cookie. When retrieving the first page, 
            // pagingCookie should be null.
            pagequery.PageInfo.PagingCookie = null;
            Console.WriteLine("Retrieving sample account records in pages...\n");
            Console.WriteLine("#\tAccount Name\t\tEmail Address");

            while (true)
            {
                // Retrieve the page.
                EntityCollection results = _service.RetrieveMultiple(pagequery);
                if (results.Entities != null)
                {
                    // Retrieve all records from the result set.
                    foreach (Contact acct in results.Entities)
                    {
                        Console.WriteLine("{0}.\t{1}\t{2}", ++recordCount, acct.FirstName,
                                           acct.EMailAddress1);
                    }
                }

                // Check for more records, if it returns true.
                if (results.MoreRecords)
                {
                    Console.WriteLine("\n****************\nPage number {0}\n****************", pagequery.PageInfo.PageNumber);
                    Console.WriteLine("#\tAccount Name\t\tEmail Address");

                    // Increment the page number to retrieve the next page.
                    pagequery.PageInfo.PageNumber++;

                    // Set the paging cookie to the paging cookie returned from current results.
                    pagequery.PageInfo.PagingCookie = results.PagingCookie;
                }
                else
                {
                    // If no more records are in the result nodes, exit the loop.
                    break;
                }
            }
        }   


        /// <summary>
        ///  "Incident" Case proccess
        /// </summary>
        /// <param name="Incident"></param>
        /// 

        // -- get all case in dynamic  using QueryExpression 
        public void getCase(IOrganizationService _service, QueryExpression query)
        {

            query = new QueryExpression("incident");
            query.ColumnSet.AddColumns("title", "ticketnumber", "incidentid");
            EntityCollection result = _service.RetrieveMultiple(query);
            Console.WriteLine("All Case : ");
            int counter = 0;
            foreach (var c in result.Entities)
            {
                counter++;
                var incidentid = c.GetAttributeValue<Guid>("incidentid");
                Guid id = incidentid;
                ListOfCaseID.Add(id);
                Console.WriteLine(counter + "- Title: " + c.Attributes["title"] + "   ID: " + id + "   Owner: " + c.Attributes["incidentid"]);
            }
        }


        // -- close Case using QueryExpression 
        public void cancelCase(IOrganizationService _service, Guid id)
        {
/*
            var incidentResolution = new IncidentResolution
            {
                Subject = "Resolved Sample Incident",

                IncidentId = new EntityReference(Incident.EntityLogicalName, id)
            };

            // Close the incident with the resolution.
            var closeIncidentRequest = new CloseIncidentRequest
            {   
                IncidentResolution = incidentResolution,
                Status = new OptionSetValue((int)IncidentState.Canceled) // ProblemSolved
            };
            _service.Execute(closeIncidentRequest);
*/
        }

    }
}

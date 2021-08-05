using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Crm.Sdk.Messages;
using DynamicCRMWithoutCore.Model;

namespace DynamicCRMWithoutCore
{
    class LINQ
    {
        public Guid id;
        public ArrayList ListOfAccountID = new ArrayList();


        // -- get all primary contact related with account using LINQ
        public void getAccountAndContact(IOrganizationService _service)
        {
            try
            {
                using (OrganizationServiceContext orgSvcContext = new OrganizationServiceContext(_service))
                {
                    var query_join = from c in orgSvcContext.CreateQuery("contact")
                                     join a in orgSvcContext.CreateQuery("account")
                                     on c["contactid"] equals a["primarycontactid"]
                                     select new
                                     {
                                         contact_fname = c["firstname"],
                                         contact_lname = c["lastname"],
                                         account_name = a["name"],
                                     };
                    foreach (var c in query_join)
                    {
                        Console.WriteLine("Account : " + c.account_name);
                        Console.WriteLine("Contact  : ");
                        Console.WriteLine("First Name: " + c.contact_fname + " - Last Name: " + c.contact_lname);
                    }
                }
                System.Console.ReadLine();

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                System.Console.ReadLine();
            }
        }
        



        // -- get list of account from dynamic using LINQ
        public  void getAccount(IOrganizationService _service)
        {
            try
            {
                using (OrganizationServiceContext orgSvcContext = new OrganizationServiceContext(_service))
                {
                    var query = from a in orgSvcContext.CreateQuery("account")
                                     select new
                                     {
                                         account_name = a["name"],
                                         account_id = a["accountid"],
                                     };
                    Console.WriteLine("List of Account : ");
                    int counter = 1;
                    foreach (var c in query)
                    {
                        ListOfAccountID.Add(c.account_id);
                        Console.WriteLine(counter + " - Account Name : " + c.account_name);
                        counter++;
                    }
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                System.Console.ReadLine();
            }
        }




        // -- Delete  account from dynamic using LINQ
        public void deleteAccount(IOrganizationService _service,Guid id)
    {
        try
        {
            using (OrganizationServiceContext orgSvcContext = new OrganizationServiceContext(_service))
            {
                var query = from a in orgSvcContext.CreateQuery("account")
                                 where (Guid)a["accountid"] == id
                                 select new
                                 {
                                     account_name = a["name"],
                                 };
                foreach (var c in query)
                {
                        _service.Delete("account",id);
                }
            }
            System.Console.ReadLine();

        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.ToString());
            System.Console.ReadLine();
        }
    }




        // -- Update Indo  account from dynamic using LINQ  ex:Account Name
        public void updateInfoAccount(IOrganizationService _service, Guid id)
        {
            // using early bound
        }




        // -- Deactivation Indo  account from dynamic using LINQ  ex:Account Name
        public void deactivateAccount(IOrganizationService _service, Guid id)
        {
            // using early bound
        }



        // -- get Incident from dynamic 
        public Guid? getIncident(IOrganizationService _service)
        {
            try
            {
                using (OrganizationServiceContext orgSvcContext = new OrganizationServiceContext(_service))
                {
                    var query = from a in orgSvcContext.CreateQuery("incident")
                                select new incidentModel
                                {
                                    incidentName = a.GetAttributeValue<String>("title"),
                                    incidentId = a.GetAttributeValue<Guid>("incidentid")
                                };
                    Console.WriteLine("List of Incident : ");
                    List<Guid> ListOfincIdent_idID = new List<Guid>();
                    foreach (var c in query)
                    {
                        ListOfincIdent_idID.Add(c.incidentId);
                        Console.WriteLine(" - incident Name : " + c.incidentName);
                    }

                    Console.WriteLine("Please select incident : ");
                    int incident_id = Convert.ToInt32(Console.ReadLine());
                    return (Guid)ListOfincIdent_idID[incident_id];
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                System.Console.ReadLine();
                return null;
            }
        }

        // -- get user system 
        public Guid? getUserSystem(IOrganizationService _service)
        {
            try
            {
                Console.WriteLine(".....................................................");
                List<Guid> user_id = new List<Guid>();
                using (OrganizationServiceContext orgSvcContext = new OrganizationServiceContext(_service))
                {
                    var query = from a in orgSvcContext.CreateQuery(SystemUser.EntityLogicalName)
                                select new systemUserModel
                                {
                                    userName = a.GetAttributeValue<String>("fullname"),
                                    userId = a.GetAttributeValue<Guid>("systemuserid")
                                };
                    foreach (var c in query)
                    {
                        user_id.Add(c.userId);
                        Console.WriteLine(" - user Name : " + c.userName);
                    }
                    Console.WriteLine("Please select user assign : ");
                    int userAssign = Convert.ToInt32(Console.ReadLine());
                    return (Guid)user_id[userAssign];
                }
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

        
        public void assignIncidentToNewOwner(IOrganizationService _service)
        {
            try
            {
                // -- get all cases
                Guid ? incident_id = getIncident(_service);
                // -- get user system where phone = 0598801612 
                Guid ? userAssign = getUserSystem(_service);
                AssignRequest assign = new AssignRequest
                {
                    Assignee = new EntityReference(SystemUser.EntityLogicalName, (Guid)userAssign),
                    Target = new EntityReference(Incident.EntityLogicalName, (Guid)incident_id)
                };
                _service.Execute(assign);
            }
            catch (Exception ex)
            {
            }
        }
    }
}

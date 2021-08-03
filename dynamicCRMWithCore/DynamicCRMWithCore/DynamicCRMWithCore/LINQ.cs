using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



        public void PagingLINQ(IOrganizationService _service, Guid id,int pageSize)
        {
            ServiceContext svcContext = new ServiceContext(_service);
            var accountsByPage = (from a in svcContext.AccountSet
                                  select new Contact
                                  {
                                      FirstName = a.Name,
                                  });
            System.Console.WriteLine("Skip 3 accounts, then Take 5 accounts");
            System.Console.WriteLine("======================================");
            foreach (var a in accountsByPage.Skip(1 * pageSize).Take(pageSize))
            {
                System.Console.WriteLine(a.FirstName);
            }

        }
    }
}

using DynamicCRMWithoutCore;
using log4net;
using Microsoft.Xrm.Sdk;
using System;

[assembly: log4net.Config.XmlConfigurator(Watch = true)] // my log exsist on appconfig
namespace DynamicCRMWithCore
{
    class Program
    {
        // -- log 
        private static readonly ILog log = LogHelper.GetLogger();

        static void Main(string[] args)
        {
            // -- lets start with create connection with dynamic CRM
            IOrganizationService organizationService = CRMConnection.getInstance();
         
            while (true)
            {
                Console.WriteLine("Please chose Entity to be start.....");
                Console.WriteLine("1. Account  ");
                Console.WriteLine("2. Case Entity");
                int Entity = Convert.ToInt32(Console.ReadLine());
                switch (Entity)
                {
                    case 1:
                        Console.WriteLine("Please chose type of work to get List of account");
                        Console.WriteLine("1. Query Expression ");
                        Console.WriteLine("2. LINQ");
                        Console.WriteLine("3. FetchXML");
                        // -- type of work to get account 
                        int Type = Convert.ToInt32(Console.ReadLine());
                        switch (Type)
                        {
                            case 1:
                                // -- get account Entity using Query Expression
                                QueryExpressions obj_QueryExpressions = new QueryExpressions();
                                obj_QueryExpressions.getAccount(organizationService);
                                log.Info("Print all active accounts name");
                                Console.WriteLine("Please select account to apply action :");
                                int accountID = Convert.ToInt32(Console.ReadLine());

                                Console.WriteLine("Please select with action need on account :");
                                Console.WriteLine("1. Delete account");
                                Console.WriteLine("2. Update account name");
                                Console.WriteLine("3. Deactivation account");
                                Console.WriteLine("4. Paging");

                                int ActionQExpression = Convert.ToInt32(Console.ReadLine());
                                switch (ActionQExpression)
                                {
                                    case 1:
                                        Console.WriteLine("Please select type action 1. Early Bound  2. Late Bound :");
                                        int ActionBoundDelete = Convert.ToInt32(Console.ReadLine());
                                        switch (ActionBoundDelete)
                                        {
                                            case 1: obj_QueryExpressions.DeleteAccountEarlyBound((Guid)obj_QueryExpressions.ListOfAccountID[ActionQExpression], organizationService); break;
                                            case 2: obj_QueryExpressions.DeleteAccountLateBound((Guid)obj_QueryExpressions.ListOfAccountID[ActionQExpression], obj_QueryExpressions.query, organizationService); break;
                                        }
                                        break;
                                    case 2:
                                        Console.WriteLine("Please select type action 1. Early Bound  2. Late Bound :");
                                        int ActionBoundUpdated = Convert.ToInt32(Console.ReadLine());
                                        switch (ActionBoundUpdated)
                                        {
                                            case 1: obj_QueryExpressions.updateAccountEarlyBound((Guid)obj_QueryExpressions.ListOfAccountID[ActionQExpression], organizationService); break;
                                            case 2: obj_QueryExpressions.UpdateInfoAccountLateBound((Guid)obj_QueryExpressions.ListOfAccountID[ActionQExpression], obj_QueryExpressions.query, organizationService); break;
                                        }
                                        break;
                                    case 3:
                                        Console.WriteLine("Please select type action 1. Early Bound  2. Late Bound :");
                                        int ActionBoundDeactivate = Convert.ToInt32(Console.ReadLine());
                                        switch (ActionBoundDeactivate)
                                        {
                                            case 1: obj_QueryExpressions.DeactivateAccountEarlyBound((Guid)obj_QueryExpressions.ListOfAccountID[ActionQExpression], organizationService); break;
                                            case 2: obj_QueryExpressions.DeactivateAccountLateBound((Guid)obj_QueryExpressions.ListOfAccountID[ActionQExpression], obj_QueryExpressions.query, organizationService); break;
                                        }
                                        break;
                                    case 4:
                                        obj_QueryExpressions.PagingQueryExpression((Guid)obj_QueryExpressions.ListOfAccountID[ActionQExpression], organizationService,4,1,0);
                                        break;
                                }
                                break;

                            case 2:
                                // -- get account Entity using LINQ
                                LINQ obj_LINQ = new LINQ();
                                obj_LINQ.getAccount(organizationService);

                                Console.WriteLine("Please select with action need on account :");
                                Console.WriteLine("1. Delete account");
                                Console.WriteLine("2. Update account name");
                                Console.WriteLine("3. Deactivation account");

                                int ActionLINQ = Convert.ToInt32(Console.ReadLine());
                                switch (ActionLINQ)
                                {
                                    case 1:
                                        obj_LINQ.deleteAccount(organizationService, (Guid)obj_LINQ.ListOfAccountID[1]);
                                        break;
                                    case 2:
                                        obj_LINQ.updateInfoAccount(organizationService, (Guid)obj_LINQ.ListOfAccountID[1]);
                                        break;
                                    case 3:
                                        obj_LINQ.deactivateAccount(organizationService, (Guid)obj_LINQ.ListOfAccountID[1]);
                                        break;
                                    case 4:
                                        obj_LINQ.PagingLINQ(organizationService, (Guid)obj_LINQ.ListOfAccountID[1],2);
                                        break;
                                }
                                break;

                            case 3:
                                // -- get account Entity using FetchXML.
                                FetchXML obj_fetchXML = new FetchXML();
                                obj_fetchXML.getAccount(organizationService);
                                break;
                        }
                        break;

                    case 2:
                        // -- get account Entity using Query Expression
                        QueryExpressions obj_QueryExpressionsCase = new QueryExpressions();
                        obj_QueryExpressionsCase.getCase(organizationService, obj_QueryExpressionsCase.query);

                        Console.WriteLine("Please chose Case whould you like proccess");
                        Console.WriteLine("Please chose  proccess");
                        Console.WriteLine("1. cancel Case");

                        int Case = Convert.ToInt32(Console.ReadLine());
                        switch (Case)
                        {
                            case 1:
                                obj_QueryExpressionsCase.cancelCase(organizationService, (Guid)obj_QueryExpressionsCase.ListOfCaseID[Case]);
                                break;
                        }

                        break;
                }
            }
        }
    }
}

﻿using Microsoft.OData.Client;
using ODataUtility.Microsoft.Dynamics.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataConsoleApplication
{
    public static class QueryExamples
    {
       

            public static void ReadLegalEntities(Resources d365)
        {
            //GeneralLedgerCustInvoiceJournalHeader generalJournal = new GeneralLedgerCustInvoiceJournalHeader();

            foreach (var legalEntity in d365.LegalEntities.AsEnumerable())
            {
                Console.WriteLine("Name: {0}", legalEntity.Name);
            }
        }

        //public static void GetInlineQueryCount(Resources d365)
        //{
        //    var vendorsQuery = d365.Vendors.IncludeTotalCount();
        //    var vendors = vendorsQuery.Execute() as QueryOperationResponse<Vendor>;

        //    Console.WriteLine("Total vendors is {0}", vendors.TotalCount);
        //}

        public static void GetTopRecords(Resources d365)
        {
            var vendorsQuery = d365.Vendors.AddQueryOption("$top", "10");
            var vendors = vendorsQuery.Execute() as QueryOperationResponse<Vendor>;

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorName);
            }
        }
        public static void CreateCustomer(Resources d365)
        {
            try
            {
                var CustomerCollection = new DataServiceCollection<Customer>(d365);

                Customer obj = new Customer();
                CustomerCollection.Add(obj);
                obj.Name = "Test 02";
                obj.CustomerAccount = "KSC-000011";
                obj.CustomerGroupId = "Group 01";
                obj.DataAreaId = "ksc";
                obj.PartyType = "Organization";


                obj.DAXIntegrationId = Guid.Empty;
                //obj.CustomerWithholdingContributionType = CustWhtContributionType_BR.Other;
                //d365.Customers[0] = obj;
               // d365.AddToCustomers(obj);
                d365.SaveChanges(SaveChangesOptions.BatchWithSingleChangeset);


                //d365.SaveChanges();
            }
            catch (Exception ex)
            {
                string mesd = ex.Message;
            }
        }
        public static void GetCustomerTopRecords(Resources d365)
        {
            var CustomersQuery = d365.Customers.AddQueryOption("$top", "10");
            var Customers = CustomersQuery.Execute() as QueryOperationResponse<Customer>;

            foreach (var cust in Customers)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", cust.Name);
            }
        }
        public static void FilterLinqSyntax(Resources d365)
        {
            var vendors = d365.Vendors.Where(x => x.VendorAccountNumber == "1001");

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }

        public static void FilterSyntax(Resources d365)
        {
            var vendors = d365.Vendors.AddQueryOption("$filter", "VendorAccountNumber eq '1001'").Execute();
            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }

        public static void SortSyntax(Resources d365)
        {
            var vendors = d365.Vendors.OrderBy(x => x.VendorAccountNumber);

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }


        public static void FilterByCompany(Resources d365)
        {
            var vendors = d365.Vendors.Where(x => x.DataAreaId == "USMF");

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }

        public static void ExpandNavigationalProperty(Resources d365Client)
        {
            var salesOrdersWithLines = d365Client.SalesOrderHeaders.Expand("SalesOrderLine").Where(x => x.SalesOrderNumber == "012518").Take(5);

            foreach (var salesOrder in salesOrdersWithLines)
            {
                Console.WriteLine(string.Format("Sales order ID is {0}", salesOrder.SalesOrderNumber));

                foreach (var salesLine in salesOrder.SalesOrderLine)
                {
                    Console.WriteLine(string.Format("Sales order line with description {0} contains item id {1}", salesLine.LineDescription, salesLine.ItemNumber));
                }
            }
        }

        public static void FilterOnNavigationalProperty(Resources d365Client)
        {
            var salesOrderLines = d365Client.SalesOrderLines.Where(x => x.SalesOrderHeader.SalesOrderStatus == SalesStatus.Invoiced);

            foreach (var salesOrderLine in salesOrderLines)
            {
                Console.WriteLine(salesOrderLine.ItemNumber);
            }

        }
    }
}

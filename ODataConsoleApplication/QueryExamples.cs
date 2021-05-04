using Microsoft.OData.Client;
using ODataUtility.Microsoft.Dynamics.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static void CreateCustomer2(Resources context)
        {
            Customer myCustomer = new Customer();
            DataServiceCollection<Customer> customersCollection = new DataServiceCollection<Customer>(context);

            customersCollection.Add(myCustomer);

            myCustomer.CustomerAccount = "US-X11111";
            myCustomer.Name = "ABC Trees  111";
            myCustomer.CustomerGroupId = "01";
            myCustomer.SalesCurrencyCode = "USD";
            //myCustomer.CreditRating = "Excellent";
           // myCustomer.AddressCountryRegionId = "USA";

            #region Create multiple customers

            //Customer myCustomer2 = new Customer();

            //customersCollection.Add(myCustomer2);

            //myCustomer2.CustomerAccount = "US-X22222";
            //myCustomer2.Name = "ABC Rains vb";
            //myCustomer2.CustomerGroupId = "01";
            //myCustomer2.SalesCurrencyCode = "USD";
            //myCustomer2.CreditRating = "Excellent";
            //myCustomer2.AddressCountryRegionId = "USA";

            #endregion

            DataServiceResponse response = null;
            try
            {
                response = context.SaveChanges(SaveChangesOptions.PostOnlySetProperties | SaveChangesOptions.BatchWithSingleChangeset);
                Console.WriteLine("created ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
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
                Console.WriteLine("Customer with ID {0} retrived.", cust.Name);
            }
        }
        public static void FilterLinqSyntax(Resources d365)
        {
            //var vendors = d365.Vendors.Where(x => x.VendorAccountNumber == "Vnd-000001");
            var vendors = d365.Vendors;
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
        public static void CreateJournal(Resources context)
        {
            //JournalTable journaltable = new JournalTable();
            GeneralLedgerCustInvoiceJournalLine generalJournalLine = new GeneralLedgerCustInvoiceJournalLine();
            GeneralLedgerCustInvoiceJournalLine generalJournalLine2 = new GeneralLedgerCustInvoiceJournalLine();
            DataServiceCollection<GeneralLedgerCustInvoiceJournalLine> generalJournalLineCollection = new DataServiceCollection<GeneralLedgerCustInvoiceJournalLine>(context);
            generalJournalLineCollection.Add(generalJournalLine);
            generalJournalLineCollection.Add(generalJournalLine2);

            generalJournalLine.DocumentDate = DateTime.Now;
            generalJournalLine.AccountType = LedgerJournalACType.Cust;
            generalJournalLine.CustomerAccountDisplayValue = "000018748";
            generalJournalLine.JournalBatchNumber = "JV-000002";
            //generalJournalLine.Voucher = "2";
            generalJournalLine.Company = "KSC";
            generalJournalLine.DebitAmount = 10;
            generalJournalLine.OffsetAccountType = LedgerJournalACType.Ledger;
            generalJournalLine.OffsetAccountDisplayValue = "----";
            //generalJournalLine.MethodOfPayment = "CHECK";
            generalJournalLine.OffsetCompany = "KSC";
            //generalJournalLine.InvoiceId = "test2";
            generalJournalLine.Currency = "SAR";
            generalJournalLine.DataAreaId = "ksc";
            generalJournalLine.Voucher = "123";

            //generalJournalLine2.InvoiceDate = new System.DateTime();
            generalJournalLine2.InvoiceDate = DateTime.Now;
            generalJournalLine2.AccountType = LedgerJournalACType.Cust;
            generalJournalLine2.CustomerAccountDisplayValue = "000018748";
            generalJournalLine2.JournalBatchNumber = "JV-000002";
            //generalJournalLine2.Voucher = "2";
            generalJournalLine2.Company = "KSC";
            generalJournalLine2.CreditAmount = 10;
            generalJournalLine2.OffsetAccountType = LedgerJournalACType.Cust;
            generalJournalLine2.OffsetAccountDisplayValue = "----";
            generalJournalLine2.OffsetCompany = "KSC";
            //generalJournalLine2.InvoiceId = "afeafe";
            generalJournalLine2.Currency = "SAR";
            generalJournalLine2.DataAreaId = "ksc";
            generalJournalLine.Voucher = "123";
            DataServiceResponse response = null;
            try
            {
                response = context.SaveChanges(SaveChangesOptions.PostOnlySetProperties | SaveChangesOptions.BatchWithSingleChangeset);
                Console.WriteLine("created ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
            }
            
        }
//public static void CreateGenralJournal(Resources d365Client)
//{
//    MCRLedgerJournal journalTable;
//    LedgerJournalTable ledgerJournalTable;
//    Counter recordsInserted;

//    journalTable = new MCRLedgerJournal_Daily(LedgerJournalType::Daily, "GnrJrnl");
//    //Creates the journal header table
//    ledgerJournalTable = journalTable.createLedgerJournalTable("GnrJrnl");

//    journalTable.parmLedgerJournalTable(ledgerJournalTable);
//    journalTable.parmMCRCCGeneralLedgerId();
//    journalTable.parmLedgerAccountType(_revenuePosting.AccountType);
//    journalTable.parmLedgerOffsetAccountType(_revenuePosting.OffsetAccountType);
//    journalTable.parmExchRate(_revenuePosting.ExchRate);
//    journalTable.parmCurrencyCode(_revenuePosting.CurrencyCode);
//    journalTable.parmLineNum();
//    journalTable.parmLedgerAccount(_revenuePosting.LedgerDimension);
//    journalTable.parmledgerOffsetAccount(_revenuePosting.OffsetLedgerDimension);
//    journalTable.parmTransDate(_revenuePosting.TransDate);
//    journalTable.parmTransTxt(_revenuePosting.Txt);
//    //create the journal trans table
//    journalTable.createLedgerJournalTrans(abs(_revenuePosting.AmountCurCredit), abs(_revenuePosting.AmountCurDebit), LedgerJournalACType::Ledger);

//    recordsInserted++;

//    return recordsInserted;
//}

//private static bool PostTablewiseJV(PayrollArea Payrollarea, DataTable dt, string journalHeaderName)
//{
//    bool retFlag = false;
//    try
//    {

//        GeneralJournalServiceClient gjsClient = new AxGLService.GeneralJournalServiceClient();
//        CallContext context = new CallContext();

//        context.Company = Payrollarea.FICompany.Code;

//        AxdLedgerGeneralJournal journal = new AxdLedgerGeneralJournal();
//        AxdEntity_LedgerJournalTable journalHeader = new AxdEntity_LedgerJournalTable();
//        journalHeader.JournalName = "General";
//        journalHeader.Name = journalHeaderName;

//        AxdEntity_LedgerJournalTrans[] allTransaction = new AxdEntity_LedgerJournalTrans[dt.Rows.Count];

//        int count = 0;
//        foreach (DataRow dr in dt.Rows)
//        {
//            #region << Check StopFlag >>
//            //if (objSpace.GetObjectsQuery<ProjectLog>().Where(x => x.StopFlag).Any())
//            //{
//            //    break;
//            //}
//            #endregion

//            #region << Create Jv to Post >>
//            string Valid = Convert.ToString(dr["Valid"]);
//            string Companyaccounts = Convert.ToString(dr["Company accounts"]);
//            string Journalbatchnumber = Convert.ToString(dr["Journal batch number"]);
//            int RecId = Convert.ToInt32(dr["RecId"]);
//            int Linenumber = Convert.ToInt32(dr["Line number"]);
//            DateTime LinenumberDate = DateTime.ParseExact(Convert.ToString(dr["Line number.Date"]), "dd/MM/yyyy", CultureInfo.InvariantCulture);
//            string LinenumberAccounttype = Convert.ToString(dr["Line number.Account type"]);
//            string LedgerDimensionMainAccount = Convert.ToString(dr["LedgerDimension.MainAccount"]);
//            string LedgerDimensionMainAccountName = Convert.ToString(dr["LedgerDimension.MainAccountName"]);
//            string DefaultDimensionLocation = Convert.ToString(dr["DefaultDimension.Location"]);
//            string DefaultDimensionEmployees = Convert.ToString(dr["DefaultDimension.Employees"]);
//            string LedgerDimensionLocation = Convert.ToString(dr["LedgerDimension.Location"]);
//            string LedgerDimensionBrand = Convert.ToString(dr["LedgerDimension.Brand"]);
//            string LedgerDimensionDepartment = Convert.ToString(dr["LedgerDimension.Department"]);
//            string LedgerDimensionEmployees = Convert.ToString(dr["LedgerDimension.Employees"]);
//            string Currency = Convert.ToString(dr["Currency"]);
//            string Exchangerate = Convert.ToString(dr["Exchange rate"]);
//            decimal Debit = Convert.ToDecimal(dr["Debit"]);
//            decimal Credit = Convert.ToDecimal(dr["Credit"]);
//            string Description = Convert.ToString(dr["Description"]);
//            string Voucher = Convert.ToString(dr["Voucher"]);
//            string Offsetaccounttype = Convert.ToString(dr["Offset account type"]);
//            string OffsetLedgerDimensionMainAccount = Convert.ToString(dr["OffsetLedgerDimension.MainAccount"]);
//            string Offsetcompanyaccounts = Convert.ToString(dr["Offset company accounts"]);

//            AxdEntity_LedgerJournalTrans journalLine = new AxdEntity_LedgerJournalTrans();
//            journalLine.JournalNum = Journalbatchnumber;
//            journalLine.Company = Companyaccounts;
//            journalLine.AccountType = LinenumberAccounttype == "Ledger" ? AxdEnum_LedgerJournalACType.Ledger
//                                    : LinenumberAccounttype == "Bank" ? AxdEnum_LedgerJournalACType.Bank
//                                    : AxdEnum_LedgerJournalACType.Cust;
//            journalLine.AccountTypeSpecified = true;

//            //------LedgerDimension --------------------------------------------
//            AxdType_MultiTypeAccount account = new AxdType_MultiTypeAccount();
//            account.Account = LedgerDimensionMainAccount;
//            account.DisplayValue = LedgerDimensionMainAccountName;

//            AxdType_DimensionAttributeValue dimValue1 = new AxdType_DimensionAttributeValue();
//            dimValue1.Name = "Location";
//            dimValue1.Value = LedgerDimensionLocation;

//            AxdType_DimensionAttributeValue dimValue2 = new AxdType_DimensionAttributeValue();
//            dimValue2.Name = "Department";
//            dimValue2.Value = LedgerDimensionDepartment;

//            AxdType_DimensionAttributeValue dimValue3 = new AxdType_DimensionAttributeValue();
//            dimValue3.Name = "Employees";
//            dimValue3.Value = LedgerDimensionEmployees;

//            //AxdType_DimensionAttributeValue dimValue4 = new AxdType_DimensionAttributeValue();
//            //dimValue4.Name = "Brand";
//            //dimValue4.Value = LedgerDimensionBrand;

//            account.Values = new AxdType_DimensionAttributeValue[3] { dimValue1, dimValue2, dimValue3 };
//            journalLine.LedgerDimension = account;
//            //------LedgerDimension End--------------------------------------------

//            //------DefaultDimension--------------------------------------------
//            //AxdType_MultiTypeDefaultAccount defaultDimension = new AxdType_MultiTypeDefaultAccount();
//            //defaultDimension.Account = LedgerDimensionMainAccount;
//            //journalLine.DefaultDimension

//            AxdType_DimensionAttributeValueSet defaccount = new AxdType_DimensionAttributeValueSet();
//            AxdType_DimensionAttributeValue difdimValue1 = new AxdType_DimensionAttributeValue();
//            difdimValue1.Name = "Employees";
//            difdimValue1.Value = DefaultDimensionEmployees;

//            AxdType_DimensionAttributeValue difdimValue2 = new AxdType_DimensionAttributeValue();
//            difdimValue2.Name = "Location";
//            difdimValue2.Value = DefaultDimensionLocation;

//            defaccount.Values = new AxdType_DimensionAttributeValue[2] { difdimValue1, difdimValue2 };
//            journalLine.DefaultDimension = defaccount;
//            //------DefaultDimension End--------------------------------------------

//            journalLine.TransDate = LinenumberDate;
//            journalLine.TransDateSpecified = true;
//            journalLine.CurrencyCode = Currency;
//            journalLine.Txt = Description;

//            if (Debit > 0)
//            {
//                journalLine.AmountCurDebit = Debit;// 120;
//                journalLine.AmountCurDebitSpecified = true;
//            }
//            else
//            {
//                journalLine.AmountCurCredit = Credit;//120;
//                journalLine.AmountCurCreditSpecified = true;
//            }

//            allTransaction[count] = journalLine;
//            count = count + 1;
//            #endregion
//        }

//        journalHeader.LedgerJournalTrans = allTransaction;
//        journal.LedgerJournalTable = new AxdEntity_LedgerJournalTable[1] { journalHeader };

//        string axuser = "";
//        string axpasswrd = "";
//        try
//        {
//            axuser = ConfigurationManager.AppSettings["FI_SYSTEM_USERNAME"].ToString();
//            axpasswrd = ConfigurationManager.AppSettings["FI_SYSTEM_PASSWORD"].ToString();
//        }
//        catch (Exception ex)
//        { }
//        gjsClient.ClientCredentials.Windows.ClientCredential.UserName = axuser;
//        gjsClient.ClientCredentials.Windows.ClientCredential.Password = axpasswrd;

//        gjsClient.create(context, journal);

//        retFlag = true;
//    }
//    catch (Exception ex)
//    {
//        throw ex;
//    }

//    return retFlag;
//}
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

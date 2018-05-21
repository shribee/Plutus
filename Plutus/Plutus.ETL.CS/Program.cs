using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plutus.ETL.CS.Controller;
using Plutus.ETL.CS.Processor;
using Plutus.ETL.CS.Model;

namespace Plutus.ETL.CS
{
    /// <summary>
    /// Main class for application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method for entry into application
        /// </summary>
        public static void Main(string[] args)
        {
            ApplicationSettings application = ApplicationSettings.Instance;
            application.Initialize();

            ApplicationController.IdentifyNewFiles(application);

            Console.WriteLine("Data Processing started...");
            
            if (application.ResetData)
            {
                Console.WriteLine("Clearing FACT Transaction");
                ClearDown.ClearTransaction(application);

                Console.WriteLine("Clearing ODS Bank");
                ClearDown.ClearBank(application);

                Console.WriteLine("Clearing ODS BankEdit");
                ClearDown.ClearBankEdit(application);

                Console.WriteLine("Clearing ODS ExpenseEdit");
                ClearDown.ClearExpenseEdit(application);

                Console.WriteLine("Clearing ODS PayeeMapping");
                ClearDown.ClearPayeeMapping(application);

                Console.WriteLine("Clearing DIM Date");
                ClearDown.ClearDIMSRCEntity("[DIM].[Date]");

                Console.WriteLine("Clearing DIM DividendThreshold");
                ClearDown.ClearDIMSRCEntity("[DIM].[DividendThreshold]");

                Console.WriteLine("Clearing DIM Payee");
                ClearDown.ClearDIMSRCEntity("[DIM].[Payee]");

                Console.WriteLine("Clearing DIM Salary");
                ClearDown.ClearDIMSRCEntity("[DIM].[Salary]");

                Console.WriteLine("Clearing DIM Transaction Type");
                ClearDown.ClearDIMSRCEntity("[DIM].[TransactionType]");

                Console.WriteLine("Clearing SRC BankIntouch");
                ClearDown.ClearDIMSRCEntity("[SRC].[BankIntouch]");

                Console.WriteLine("Clearing SRC BankCaterAllen");
                ClearDown.ClearDIMSRCEntity("[SRC].[BankCaterAllen]");

                Console.WriteLine("Clearing SRC Expense");
                ClearDown.ClearDIMSRCEntity("[SRC].[Expense]");

                Console.WriteLine("Clearing SRC Invoice");
                ClearDown.ClearDIMSRCEntity("[SRC].[Invoice]");
            }

            Console.WriteLine("Processing SRC Bank Cater Allen");
            ProcessBankCaterAllen.ProcessAllFiles(application);

            Console.WriteLine("Processing SRC Bank Intouch");
            ProcessBankIntouch.ProcessAllFiles(application);

            Console.WriteLine("Processing SRC Expense");
            ProcessExpense.ProcessAllFiles(application);

            Console.WriteLine("Processing SRC Invoice");
            ProcessInvoice.ProcessAllFiles(application);

            Console.WriteLine("Processing DIM Date");
            ProcessDate.LoadDates(application);

            Console.WriteLine("Processing DIM Dividend Threshold");
            ProcessDividendThreshold.ProcessAllFiles(application);

            Console.WriteLine("Processing DIM Payee");
            ProcessPayee.ProcessAllFiles(application);

            Console.WriteLine("Processing DIM Salary");
            ProcessSalary.ProcessAllFiles(application);

            Console.WriteLine("Processing DIM TransactionType");
            ProcessTransactionType.ProcessAllFiles(application);

            Console.WriteLine("Processing ODS Bank");
            ProcessBank.ProcessAllFiles(application);

            Console.WriteLine("Processing ODS Bank Edit");
            ProcessBankEdit.ProcessAllFiles(application);

            Console.WriteLine("Processing ODS Expense Edit");
            ProcessExpenseEdit.ProcessAllFiles(application);

            Console.WriteLine("Processing ODS Payee Mapping");
            ProcessPayeeMapping.ProcessAllFiles(application);

            Console.WriteLine("Processing FACT Transaction Salary");
            ProcessTransaction.ProcessSalaryTransaction(application);

            Console.WriteLine("Processing FACT Transaction Expense");
            ProcessTransaction.ProcessExpenseTransaction(application);

            Console.WriteLine("Processing FACT Transaction Bank");
            ProcessTransaction.ProcessBankTransaction(application);

            Console.WriteLine("Data Processing complete!");
            Console.ReadLine();
        }
    }
}

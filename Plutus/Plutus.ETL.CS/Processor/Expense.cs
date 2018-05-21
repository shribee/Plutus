using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Plutus.ETL.CS.Controller;
using Plutus.ETL.CS.Model;
using Plutus.ETL.CS.Utility;

namespace Plutus.ETL.CS.Processor
{
    /// <summary>
    /// Static class to process Expense data
    /// </summary>
    public static class ProcessExpense
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// Static method to cycle through file list and process all BankIntouch files
        /// </summary>
        public static void ProcessAllFiles(ApplicationSettings application)
        {
            foreach (DirectoryFile directoryFile in application.InputFileList)
            {
                if (directoryFile.FileStatus != Model.Enum.FileStatus.Processed)
                {
                    if (directoryFile.FileType == Model.Enum.FileType.Expense)
                    {
                        ProcessExpenseData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process all Intouch Expense CSVs
        /// </summary>
        public static void ProcessExpenseData(DirectoryFile directoryFile)
        {
            List<Expense> ExpenseODS = new List<Expense>();
            List<Expense> ExpenseNew = new List<Expense>();
            int newRows = 0;
            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");
                List<string[]> ExpenseString = new List<string[]>();
                List<Expense> ExpenseFile = new List<Expense>();
                FileUtility.CSVStringReader(directoryFile.FileContent, ",", ExpenseString, 9, true);
                foreach (string[] expenseString in ExpenseString)
                {
                    Expense Expense = new Expense
                    {
                        Date = Convert.ToDateTime(expenseString[0]),
                        EmployeeName = expenseString[1],
                        Description = expenseString[2],
                        Detail = expenseString[3],
                        Total = Convert.ToDecimal(expenseString[4]),
                        VAT = Convert.ToDecimal(expenseString[5]),
                        Net = Convert.ToDecimal(expenseString[6])
                    };
                    ExpenseFile.Add(Expense);
                }
                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var query = (from row in context.Expense
                                 select row).ToList();
                    ExpenseODS = query;
                    foreach (Expense expense in ExpenseFile)
                    {
                        if (!ExpenseODS.Contains(expense))
                        {
                            ExpenseNew.Add(expense);
                            newRows++;
                        }
                    }
                    foreach (Expense expense in ExpenseNew)
                    {
                        context.Expense.Add(expense);
                    }
                    context.SaveChanges();
                    logger.Info($"Added [{newRows.ToString()}] rows to ODS from file [{directoryFile.FileName}]");
                    logger.Info($"Finished processing file [{directoryFile.FileName}]");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while trying to process file [{directoryFile.FileName}]. Exception [{e.ToString()}]");
                throw;
            }
        }
    }
}

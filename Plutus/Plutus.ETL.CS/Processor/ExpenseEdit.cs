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
    /// Static class to process ExpenseEdit Intouch data
    /// </summary>
    public static class ProcessExpenseEdit
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
                    if (directoryFile.FileType == Model.Enum.FileType.ExpenseEdit)
                    {
                        ProcessExpenseEditData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process all Intouch ExpenseEdit CSVs
        /// </summary>
        public static void ProcessExpenseEditData(DirectoryFile directoryFile)
        {
            List<ExpenseEdit> ExpenseEditODS = new List<ExpenseEdit>();
            List<ExpenseEdit> ExpenseEditNew = new List<ExpenseEdit>();
            int newRows = 0;

            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");
                List<string[]> ExpenseEditFileString = new List<string[]>();
                List<ExpenseEdit> ExpenseEditFile = new List<ExpenseEdit>();
                FileUtility.CSVStringReader(directoryFile.FileContent, ",", ExpenseEditFileString, 9, true);
                foreach (string[] expenseEditString in ExpenseEditFileString)
                {
                    ExpenseEdit ExpenseEdit = new ExpenseEdit
                    {
                        Date = Convert.ToDateTime(expenseEditString[0]),
                        Description = expenseEditString[1],
                        Detail = expenseEditString[2],
                        Total = Convert.ToDecimal(expenseEditString[3]),
                        EffectiveDate = Convert.ToDateTime(expenseEditString[4]),
                        EffectiveDescription = expenseEditString[5]
                    };
                    ExpenseEditFile.Add(ExpenseEdit);
                }

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var query = (from row in context.ExpenseEdit
                                 select row).ToList();
                    ExpenseEditODS = query;
                    foreach (ExpenseEdit expenseEdit in ExpenseEditFile)
                    {
                        if (!ExpenseEditODS.Contains(expenseEdit))
                        {
                            ExpenseEditNew.Add(expenseEdit);
                            newRows++;
                        }
                    }
                    foreach (ExpenseEdit expenseEdit in ExpenseEditNew)
                    {
                        context.ExpenseEdit.Add(expenseEdit);
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

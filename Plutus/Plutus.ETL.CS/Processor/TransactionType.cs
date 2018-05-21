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
    /// Static class to process TransactionType data
    /// </summary>
    public static class ProcessTransactionType
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
                    if (directoryFile.FileType == Model.Enum.FileType.TransactionType)
                    {
                        ProcessTransactionTypeData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process TransactionType CSV
        /// </summary>
        public static void ProcessTransactionTypeData(DirectoryFile directoryFile)
        {
            List<Model.TransactionType> TransactionTypeDIM = new List<Model.TransactionType>();
            List<Model.TransactionType> TransactionTypeNew = new List<Model.TransactionType>();
            int newRows = 0;
            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");
                List<string[]> TransactionTypeString = new List<string[]>();
                List<Model.TransactionType> TransactionTypeFile = new List<Model.TransactionType>();

                FileUtility.CSVStringReader(directoryFile.FileContent, ",", TransactionTypeString, 9, true);
                foreach (string[] transactionTypeString in TransactionTypeString)
                {
                    Model.TransactionType transactionType = new Model.TransactionType
                    {
                        TransactionTypeID = Convert.ToInt32(transactionTypeString[0]),
                        TransactionCategory = transactionTypeString[1],
                        TransactionSubCategory = transactionTypeString[2],
                        Activity = transactionTypeString[3],
                    };
                    TransactionTypeFile.Add(transactionType);
                }

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    context.Database.ExecuteSqlCommand("delete from DIM.TransactionType");
                    foreach (Model.TransactionType transactionType in TransactionTypeFile)
                    {
                        context.TransactionType.Add(transactionType);
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

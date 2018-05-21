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
    /// Static class to process Bank Intouch data
    /// </summary>
    public static class ProcessBank
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
                    if (directoryFile.FileType == Model.Enum.FileType.Bank)
                    {
                        ProcessBankData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process all Intouch Bank CSVs
        /// </summary>
        public static void ProcessBankData(DirectoryFile directoryFile)
        {
            List<Bank> BankODS = new List<Bank>();
            List<Bank> BankNew = new List<Bank>();
            int newRows = 0;

            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");
                List<string[]> BankFileString = new List<string[]>();
                List<Bank> BankFile = new List<Bank>();
                FileUtility.CSVStringReader(directoryFile.FileContent, ",", BankFileString, 9, true);
                foreach (string[] bankString in BankFileString)
                {
                    Bank Bank = new Bank
                    {
                        FITID = Convert.ToInt64(bankString[0]),
                        PostedDate = Convert.ToDateTime(bankString[1]),
                        Amount = Convert.ToDecimal(bankString[2]),
                        PayeeFriendlyName = bankString[3],
                        Activity = bankString[4],
                        TransferType = bankString[5]
                    };
                    BankFile.Add(Bank);
                }

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var query = (from row in context.Bank
                                 select row).ToList();
                    BankODS = query;
                    foreach (Bank bank in BankFile)
                    {
                        if (!BankODS.Contains(bank))
                        {
                            BankNew.Add(bank);
                            newRows++;
                        }
                    }
                    foreach (Bank bank in BankNew)
                    {
                        context.Bank.Add(bank);
                    }
                    context.SaveChanges();
                    logger.Info($"Added [{newRows.ToString()}] rows to ODS from file [{directoryFile.FileName}]");
                    logger.Info($"Finished processing file [{directoryFile.FileName}]");
                }
            }
            catch(Exception e)
            {
                logger.Error($"Exception while trying to process file [{directoryFile.FileName}]. Exception [{e.ToString()}]");
                throw;
            }
        }

    }
}

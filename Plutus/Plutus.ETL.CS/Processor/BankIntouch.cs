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
    public static class ProcessBankIntouch
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
                    if (directoryFile.FileType == Model.Enum.FileType.BankIntouch)
                    {
                        ProcessBankIntouchData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process all Intouch Bank CSVs
        /// </summary>
        private static void ProcessBankIntouchData(DirectoryFile directoryFile)
        {
            List<BankIntouch> BankIntouchODS = new List<BankIntouch>();
            List<BankIntouch> BankIntouchNew = new List<BankIntouch>();
            int newRows = 0;

            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");
                List<string[]> BankIntouchFileString = new List<string[]>();
                List<BankIntouch> BankIntouchFile = new List<BankIntouch>();
                FileUtility.CSVStringReader(directoryFile.FileContent, ",", BankIntouchFileString, 9, true);
                foreach (string[] bankIntouchString in BankIntouchFileString)
                {
                    BankIntouch BankIntouch = new BankIntouch
                    {
                        Date = Convert.ToDateTime(bankIntouchString[0]),
                        BankDescription = bankIntouchString[1],
                        AccountNumber = bankIntouchString[2],
                        EmployeeName = bankIntouchString[3],
                        Description = bankIntouchString[4],
                        UserComments = bankIntouchString[5],
                        Amount = Convert.ToDecimal(bankIntouchString[6]),
                        Net = Convert.ToDecimal(bankIntouchString[7]),
                        VAT = Convert.ToDecimal(bankIntouchString[8])
                    };

                    BankIntouchFile.Add(BankIntouch);
                }

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var query = (from row in context.BankIntouch
                                 select row).ToList();
                    BankIntouchODS = query;
                    foreach (BankIntouch bankIntouch in BankIntouchFile)
                    {
                        if (!BankIntouchODS.Contains(bankIntouch) && !String.IsNullOrEmpty(bankIntouch.Description))
                        {
                            BankIntouchNew.Add(bankIntouch);
                            newRows++;
                        }
                    }
                    foreach (BankIntouch bankIntouch in BankIntouchNew)
                    {
                        context.BankIntouch.Add(bankIntouch);
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

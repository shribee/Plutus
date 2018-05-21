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
    /// Static class to process BankEdit Intouch data
    /// </summary>
    public static class ProcessBankEdit
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
                    if (directoryFile.FileType == Model.Enum.FileType.BankEdit)
                    {
                        ProcessBankEditData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process all Intouch BankEdit CSVs
        /// </summary>
        public static void ProcessBankEditData(DirectoryFile directoryFile)
        {
            List<BankEdit> BankEditODS = new List<BankEdit>();
            List<BankEdit> BankEditNew = new List<BankEdit>();
            int newRows = 0;

            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");
                List<string[]> BankEditFileString = new List<string[]>();
                List<BankEdit> BankEditFile = new List<BankEdit>();
                FileUtility.CSVStringReader(directoryFile.FileContent, ",", BankEditFileString, 9, true);
                foreach (string[] bankEditString in BankEditFileString)
                {
                    BankEdit BankEdit = new BankEdit
                    {
                        FITID = Convert.ToInt64(bankEditString[0]),
                        EffectiveDate = Convert.ToDateTime(bankEditString[1]),
                        EffectiveAmount = Convert.ToDecimal(bankEditString[2])
                    };
                    BankEditFile.Add(BankEdit);
                }

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var query = (from row in context.BankEdit
                                 select row).ToList();
                    BankEditODS = query;
                    foreach (BankEdit bankEdit in BankEditFile)
                    {
                        if (!BankEditODS.Contains(bankEdit))
                        {
                            BankEditNew.Add(bankEdit);
                            newRows++;
                        }
                    }
                    foreach (BankEdit bankEdit in BankEditNew)
                    {
                        context.BankEdit.Add(bankEdit);
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

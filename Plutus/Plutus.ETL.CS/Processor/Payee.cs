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
    /// Static class to process Payee data
    /// </summary>
    public static class ProcessPayee
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
                    if (directoryFile.FileType == Model.Enum.FileType.Payee)
                    {
                        ProcessPayeeData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process Payee CSV
        /// </summary>
        public static void ProcessPayeeData(DirectoryFile directoryFile)
        {
            List<Payee> PayeeDIM = new List<Payee>();
            List<Payee> PayeeNew = new List<Payee>();
            int newRows = 0;
            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");
                List<string[]> PayeeString = new List<string[]>();
                List<Payee> PayeeFile = new List<Payee>();

                FileUtility.CSVStringReader(directoryFile.FileContent, ",", PayeeString, 9, true);
                foreach (string[] payeeString in PayeeString)
                {
                    Payee payee = new Payee
                    {
                        PayeeID = Convert.ToInt32(payeeString[0]),
                        PayeeFriendlyName = payeeString[1]
                    };
                    PayeeFile.Add(payee);
                }

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    context.Database.ExecuteSqlCommand("delete from DIM.Payee");
                    foreach (Payee payee in PayeeFile)
                    {
                        context.Payee.Add(payee);
                    }
                    context.SaveChanges();
                    logger.Info($"Added [{newRows.ToString()}] rows to DIM from file [{directoryFile.FileName}]");
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

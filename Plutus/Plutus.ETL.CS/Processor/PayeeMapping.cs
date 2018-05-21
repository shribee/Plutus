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
    /// Static class to process PayeeMappingMapping data
    /// </summary>
    public static class ProcessPayeeMapping
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
                    if (directoryFile.FileType == Model.Enum.FileType.PayeeMapping)
                    {
                        ProcessPayeeMappingData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process PayeeMapping CSV
        /// </summary>
        public static void ProcessPayeeMappingData(DirectoryFile directoryFile)
        {
            List<PayeeMapping> PayeeMappingDIM = new List<PayeeMapping>();
            List<PayeeMapping> PayeeMappingNew = new List<PayeeMapping>();
            int newRows = 0;
            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");
                List<string[]> PayeeMappingString = new List<string[]>();
                List<PayeeMapping> PayeeMappingFile = new List<PayeeMapping>();

                FileUtility.CSVStringReader(directoryFile.FileContent, ",", PayeeMappingString, 9, true);
                foreach (string[] payeeMappingString in PayeeMappingString)
                {
                    PayeeMapping payeeMapping = new PayeeMapping
                    {
                        ID = Convert.ToInt32(payeeMappingString[0]),
                        PayeeID = Convert.ToInt32(payeeMappingString[1]),
                        PayeeFriendlyName = payeeMappingString[2],
                        PayeeSourceName = payeeMappingString[3],
                        PayeeSource = payeeMappingString[4]
                    };
                    PayeeMappingFile.Add(payeeMapping);
                }

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    context.Database.ExecuteSqlCommand("delete from ODS.PayeeMapping");
                    foreach (PayeeMapping payeeMapping in PayeeMappingFile)
                    {
                        context.PayeeMapping.Add(payeeMapping);
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

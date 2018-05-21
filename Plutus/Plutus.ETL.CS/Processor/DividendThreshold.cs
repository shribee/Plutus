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
    /// Static class to process Dividend Threshold data
    /// </summary>
    public static class ProcessDividendThreshold
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
                    if (directoryFile.FileType == Model.Enum.FileType.DividendThreshold)
                    {
                        ProcessDividendThresholdData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process DividendThreshold CSV
        /// </summary>
        public static void ProcessDividendThresholdData(DirectoryFile directoryFile)
        {
            List<DividendThreshold> DividendThresholdDIM = new List<DividendThreshold>();
            List<DividendThreshold> DividendThresholdNew = new List<DividendThreshold>();
            int newRows = 0;
            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");
                List<string[]> DividendThresholdString = new List<string[]>();
                List<DividendThreshold> DividendThresholdFile = new List<DividendThreshold>();

                FileUtility.CSVStringReader(directoryFile.FileContent, ",", DividendThresholdString, 7, true);
                foreach (string[] dividendThresholdString in DividendThresholdString)
                {
                    DividendThreshold dividendThreshold = new DividendThreshold
                    {
                        ThresholdID = Convert.ToInt32(dividendThresholdString[0]),
                        FinYear = dividendThresholdString[1],
                        MinAmount = Convert.ToDecimal(dividendThresholdString[2]),
                        MaxAmount = Convert.ToDecimal(dividendThresholdString[3]),
                        TaxPct = Convert.ToDecimal(dividendThresholdString[4]),
                        IsOptimumBand = Convert.ToInt16(dividendThresholdString[5]),
                        NetAmountCarryOver = Convert.ToDecimal(dividendThresholdString[6])
                    };
                    DividendThresholdFile.Add(dividendThreshold);
                }

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    context.Database.ExecuteSqlCommand("delete from DIM.DividendThreshold");
                    foreach (Model.DividendThreshold dividendThreshold in DividendThresholdFile)
                    {
                        context.DividendThreshold.Add(dividendThreshold);
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

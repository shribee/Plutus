using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plutus.ETL.CS.Model;
using NLog;
using System.Xml.Linq;
using Plutus.ETL.CS.Controller;

namespace Plutus.ETL.CS.Processor
{
    /// <summary>
    /// Static class to process Bank Cater Allen data
    /// </summary>
    public static class ProcessBankCaterAllen
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
                    if (directoryFile.FileType == Model.Enum.FileType.BankCaterAllen)
                    {
                        ProcessBankCaterAllenData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process all Bank Cater Allen XMLs
        /// </summary>
        public static void ProcessBankCaterAllenData(DirectoryFile directoryFile)
        {
            List<BankCaterAllen> BankCaterAllenODS = new List<BankCaterAllen>();
            List<BankCaterAllen> BankCaterAllenNew = new List<BankCaterAllen>();
            int newRows = 0;
            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");

                XDocument xdoc1 = XDocument.Parse(directoryFile.FileContent);
                var query = from txn in xdoc1.Descendants("STMTTRN")
                            select new BankCaterAllen
                            {
                                FITID = Convert.ToInt64(txn.Element("FITID").Value),
                                TxnType = txn.Element("TRNTYPE").Value,
                                PostedDate = Convert.ToDateTime(
                                    txn.Element("DTPOSTED").Value.Substring(0, 4)
                                    + "-" + txn.Element("DTPOSTED").Value.Substring(4, 2)
                                    + "-" + txn.Element("DTPOSTED").Value.Substring(6, 2)),
                                Amount = Convert.ToDecimal(txn.Element("TRNAMT").Value),
                                Name = txn.Element("NAME").Value
                            };
                List<BankCaterAllen> BankCaterAllenFile = query.ToList();

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var dbquery = (from row in context.BankCaterAllen
                                   select row).ToList();
                    BankCaterAllenODS = dbquery;
                    foreach (BankCaterAllen bankCaterAllen in BankCaterAllenFile)
                    {
                        if (!BankCaterAllenODS.Contains(bankCaterAllen))
                        {
                            BankCaterAllenNew.Add(bankCaterAllen);
                            newRows++;
                        }
                    }
                    foreach (BankCaterAllen bankCaterAllen in BankCaterAllenNew)
                    {
                        context.BankCaterAllen.Add(bankCaterAllen);
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

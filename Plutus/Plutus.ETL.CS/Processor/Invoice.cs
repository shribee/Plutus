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
    /// Static class to process Invoice data
    /// </summary>
    public static class ProcessInvoice
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
                    if (directoryFile.FileType == Model.Enum.FileType.Invoice)
                    {
                        ProcessInvoiceData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process all Invoice CSVs
        /// </summary>
        public static void ProcessInvoiceData(DirectoryFile directoryFile)
        {
            List<Invoice> InvoiceODS = new List<Invoice>();
            List<Invoice> InvoiceNew = new List<Invoice>();
            int newRows = 0;

            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");
                List<string[]> InvoiceFileString = new List<string[]>();
                List<Invoice> InvoiceFile = new List<Invoice>();
                FileUtility.CSVStringReader(directoryFile.FileContent, ",", InvoiceFileString, 9, true);
                foreach (string[] invoiceString in InvoiceFileString)
                {
                    Invoice Invoice = new Invoice
                    {
                        Date = Convert.ToDateTime(invoiceString[0]),
                        Description = invoiceString[1],
                        ContractReference = invoiceString[2],
                        ClientName = invoiceString[3],
                        InvoiceReference = invoiceString[4],
                        Total = Convert.ToDecimal(invoiceString[5]),
                        VAT = Convert.ToDecimal(invoiceString[6]),
                        Net = Convert.ToDecimal(invoiceString[7])
                    };

                    InvoiceFile.Add(Invoice);
                }

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var query = (from row in context.Invoice
                                 select row).ToList();
                    InvoiceODS = query;
                    foreach (Invoice invoice in InvoiceFile)
                    {
                        if (!InvoiceODS.Contains(invoice))
                        {
                            InvoiceNew.Add(invoice);
                            newRows++;
                        }
                    }
                    foreach (Invoice invoice in InvoiceNew)
                    {
                        context.Invoice.Add(invoice);
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

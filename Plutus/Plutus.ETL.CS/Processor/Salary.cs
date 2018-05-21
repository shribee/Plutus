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
    /// Static class to process Salary data
    /// </summary>
    public static class ProcessSalary
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
                    if (directoryFile.FileType == Model.Enum.FileType.Salary)
                    {
                        ProcessSalaryData(directoryFile);
                        directoryFile.FileStatus = Model.Enum.FileStatus.Processed;
                    }
                }
            }
        }

        /// <summary>
        /// Process Salary CSV
        /// </summary>
        public static void ProcessSalaryData(DirectoryFile directoryFile)
        {
            List<Salary> SalaryDIM = new List<Salary>();
            List<Salary> SalaryNew = new List<Salary>();
            int newRows = 0;
            try
            {
                logger.Info($"Started processing file [{directoryFile.FileName}]");
                List<string[]> SalaryString = new List<string[]>();
                List<Salary> SalaryFile = new List<Salary>();

                FileUtility.CSVStringReader(directoryFile.FileContent, ",", SalaryString, 13, true);
                foreach (string[] salaryString in SalaryString)
                {
                    Salary salary = new Salary
                    {
                        SalaryDate = Convert.ToDateTime(salaryString[1]),
                        TaxCode = salaryString[2],
                        TotalPay = Convert.ToDecimal(salaryString[3]),
                        TaxDeducted = Convert.ToDecimal(salaryString[4]),
                        EmployeeNIC = Convert.ToDecimal(salaryString[5]),
                        EmployeePension = Convert.ToDecimal(salaryString[6]),
                        SickPay = Convert.ToDecimal(salaryString[7]),
                        ParentingPay = Convert.ToDecimal(salaryString[8]),
                        StudentLoan = Convert.ToDecimal(salaryString[9]),
                        NetPay = Convert.ToDecimal(salaryString[10]),
                        EmployerNIC = Convert.ToDecimal(salaryString[11]),
                        EmployerPension = Convert.ToDecimal(salaryString[12])
                    };
                    SalaryFile.Add(salary);
                }

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    context.Database.ExecuteSqlCommand("delete from DIM.Salary");
                    foreach (Salary salary in SalaryFile)
                    {
                        context.Salary.Add(salary);
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

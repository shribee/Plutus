using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plutus.ETL.CS.Controller;
using NLog;

namespace Plutus.ETL.CS.Model
{
    /// <summary>
    /// Static class containing methods to clear down DB
    /// </summary>
    public static class ClearDown
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Static method to clear DIM and SRC tables
        /// </summary>
        public static void ClearDIMSRCEntity(string entityName)
        {
            try
            {
                logger.Info($"Started clearing [{entityName}] rows");
                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    context.Database.ExecuteSqlCommand($"truncate table {entityName}");
                    context.SaveChanges();
                    logger.Info($"Successfully cleared rows from {entityName}");
                }
            }
            catch(Exception e)
            {
                logger.Error($"Exception while trying to clear rows from {entityName}. Exception [{e.ToString()}]");
                throw;
            }
        }

        /// <summary>
        /// Static method to empty Transactions
        /// </summary>
        public static void ClearTransaction(ApplicationSettings application)
        {
            int originalRows = 0;
            int newRows = 0;
            try
            {
                logger.Info($"Started clearing transaction rows");
                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var queryTransaction = (from row in context.Transaction
                                            select row).ToList();
                    originalRows = queryTransaction.Count;
                    context.Database.ExecuteSqlCommand("truncate table [FACT].[Transaction]");
                    context.SaveChanges();
                    newRows = queryTransaction.Count;
                    if (newRows == 0)
                    {
                        logger.Info($"Successfully cleared Fact. Original rows [{originalRows.ToString()}]" +
                            $"; New rows [{newRows.ToString()}]");
                        logger.Info($"Finished clearing Transaction");
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while trying to clear transaction rows. Exception [{e.ToString()}]");
                throw;
            }
        }

        /// <summary>
        /// Static method to empty Bank
        /// </summary>
        public static void ClearBank(ApplicationSettings application)
        {
            int originalRows = 0;
            int newRows = 0;
            try
            {
                logger.Info($"Started clearing bank rows");
                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var queryTransaction = (from row in context.Bank
                                            select row).ToList();
                    originalRows = queryTransaction.Count;
                    context.Database.ExecuteSqlCommand("truncate table [ODS].[Bank]");
                    context.SaveChanges();
                    newRows = queryTransaction.Count;
                    if (newRows == 0)
                    {
                        logger.Info($"Successfully cleared ODS. Original rows [{originalRows.ToString()}]" +
                            $"; New rows [{newRows.ToString()}]");
                        logger.Info($"Finished clearing Bank");
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while trying to clear bank rows. Exception [{e.ToString()}]");
                throw;
            }
        }

        /// <summary>
        /// Static method to empty BankEdit
        /// </summary>
        public static void ClearBankEdit(ApplicationSettings application)
        {
            int originalRows = 0;
            int newRows = 0;
            try
            {
                logger.Info($"Started clearing bankedit rows");
                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var queryTransaction = (from row in context.BankEdit
                                            select row).ToList();
                    originalRows = queryTransaction.Count;
                    context.Database.ExecuteSqlCommand("truncate table [ODS].[BankEdit]");
                    context.SaveChanges();
                    newRows = queryTransaction.Count;
                    if (newRows == 0)
                    {
                        logger.Info($"Successfully cleared ODS. Original rows [{originalRows.ToString()}]" +
                            $"; New rows [{newRows.ToString()}]");
                        logger.Info($"Finished clearing BankEdit");
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while trying to clear bankedit rows. Exception [{e.ToString()}]");
                throw;
            }
        }

        /// <summary>
        /// Static method to empty ExpenseEdit
        /// </summary>
        public static void ClearExpenseEdit(ApplicationSettings application)
        {
            int originalRows = 0;
            int newRows = 0;
            try
            {
                logger.Info($"Started clearing ExpenseEdit rows");
                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var queryExpenseEdit = (from row in context.ExpenseEdit
                                        select row).ToList();
                    originalRows = queryExpenseEdit.Count;
                    context.Database.ExecuteSqlCommand("truncate table [ODS].[ExpenseEdit]");
                    context.SaveChanges();
                    newRows = queryExpenseEdit.Count;
                    if (newRows == 0)
                    {
                        logger.Info($"Successfully cleared ODS. Original rows [{originalRows.ToString()}]" +
                            $"; New rows [{newRows.ToString()}]");
                        logger.Info($"Finished clearing ExpenseEdit");
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while trying to clear ExpenseEdit rows. Exception [{e.ToString()}]");
                throw;
            }
        }

        /// <summary>
        /// Static method to empty PayeeMapping
        /// </summary>
        public static void ClearPayeeMapping(ApplicationSettings application)
        {
            int originalRows = 0;
            int newRows = 0;
            try
            {
                logger.Info($"Started clearing PayeeMapping rows");
                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var queryPayeeMapping = (from row in context.PayeeMapping
                                            select row).ToList();
                    originalRows = queryPayeeMapping.Count;
                    context.Database.ExecuteSqlCommand("truncate table [ODS].[PayeeMapping]");
                    context.SaveChanges();
                    newRows = queryPayeeMapping.Count;
                    if (newRows == 0)
                    {
                        logger.Info($"Successfully cleared ODS. Original rows [{originalRows.ToString()}]" +
                            $"; New rows [{newRows.ToString()}]");
                        logger.Info($"Finished clearing PayeeMapping");
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while trying to clear PayeeMapping rows. Exception [{e.ToString()}]");
                throw;
            }
        }

    }
}

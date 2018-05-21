using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Plutus.ETL.CS.Model;
using Plutus.ETL.CS.Controller;

namespace Plutus.ETL.CS.Processor
{
    /// <summary>
    /// Static class to process Transaction data
    /// </summary>
    public static class ProcessTransaction
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Static method to load Salary Transactions
        /// </summary>
        public static void ProcessSalaryTransaction(ApplicationSettings application)
        {
            List<Salary> SalaryDIM = new List<Salary>();
            List<Transaction> TransactionNew = new List<Transaction>();
            int newRows = 0;
            try
            {
                logger.Info($"Started reading DIM Salary rows");
                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var querySalary = (from row in context.Salary
                                       select row).ToList();
                    SalaryDIM = querySalary;
                    foreach (Salary salary in SalaryDIM)
                    {
                        Transaction transaction = new Transaction
                        {
                            SourceID = "S_" + salary.SalaryDate.ToString("yyyyMMdd"),
                            PostedDate = salary.SalaryDate,
                            EffectiveDate = salary.SalaryDate,
                            TransactionTypeID = -1,
                            PayeeID = -1,
                            TransferType = "SE",
                            Amount = salary.NetPay,
                            EffectiveAmount = salary.NetPay,
                            TransactionSource = "Salary"
                        };
                        TransactionNew.Add(transaction);
                        newRows++;
                    }
                    AddTransaction(application, TransactionNew, "Salary");
                    logger.Info($"Sent [{newRows.ToString()}] Salary rows for Fact Insert");
                    logger.Info($"Finished reading DIM Salary rows");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while trying to read DIM Salary rows. Exception [{e.ToString()}]");
                throw;
            }
        }

        /// <summary>
        /// Static method to load Expense Transactions
        /// </summary>
        public static void ProcessExpenseTransaction(ApplicationSettings application)
        {
            List<Transaction> TransactionList = new List<Transaction>();
            int newRows = 0;
            try
            {
                logger.Info($"Started reading SRC Expense rows");
                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var queryExpense = (
                                    from e in context.Expense
                                    join ee in context.ExpenseEdit
                                    on new { e.Date, e.Description, e.Detail, e.Total }
                                    equals new { ee.Date, ee.Description, ee.Detail, ee.Total }
                                    into eejoin
                                    from expense in eejoin.DefaultIfEmpty()
                                    join tt in context.TransactionType
                                    on new { Activity = expense.EffectiveDescription ?? e.Description }
                                    equals new { tt.Activity }
                                    orderby e.Date, e.Total
                                    select new
                                    {
                                        PostedDate = e.Date
                                        ,
                                        EffectiveDate = (DateTime?)expense.EffectiveDate ?? e.Date
                                        ,
                                        tt.TransactionTypeID
                                        ,
                                        Total = e.Total*-1
                                        ,
                                        EffectiveTotal = e.Total*-1
                                        ,
                                        Rank = (
                                            from o in context.Expense
                                            where
                                                (o.Date == e.Date && o.Total > e.Total)
                                                || (o.Date == e.Date && o.Total == e.Total && o.ID > e.ID)
                                            select o).Count() + 1
                                    });
                    foreach (var expense in queryExpense)
                    {
                        string expenseID = "E_"
                                            + expense.PostedDate.ToString("yyyyMMdd") + "_"
                                            + expense.Rank.ToString();
                        Transaction transaction = new Transaction
                        {
                            SourceID = expenseID,
                            PostedDate = expense.PostedDate,
                            EffectiveDate = expense.EffectiveDate,
                            TransactionTypeID = expense.TransactionTypeID,
                            PayeeID = -1,
                            TransferType = "NONE",
                            Amount = expense.Total,
                            EffectiveAmount = expense.EffectiveTotal,
                            TransactionSource = "Expense"
                        };
                        TransactionList.Add(transaction);
                        newRows++;
                    }
                    AddTransaction(application, TransactionList, "Expense");
                    logger.Info($"Sent [{newRows.ToString()}] Expense rows for Fact Insert");
                    logger.Info($"Finished reading SRC Expense rows");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while trying to read SRC Expense rows. Exception [{e.ToString()}]");
                throw;
            }
        }

        /// <summary>
        /// Static method to load Bank Transactions
        /// </summary>
        public static void ProcessBankTransaction(ApplicationSettings application)
        {
            List<Transaction> TransactionList = new List<Transaction>();
            int newRows = 0;
            try
            {
                logger.Info($"Started reading ODS Bank rows");
                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var queryBank = (
                                    from b in context.Bank
                                    join p in context.Payee
                                    on b.PayeeFriendlyName equals p.PayeeFriendlyName
                                    join tt in context.TransactionType
                                    on new { b.Activity }
                                    equals new { tt.Activity }
                                    join be in context.BankEdit
                                    on new { b.FITID }
                                    equals new { be.FITID }
                                    into bejoin
                                    from bank in bejoin.DefaultIfEmpty()
                                    orderby b.PostedDate, b.Amount
                                    select new
                                    {
                                        b.FITID
                                        , b.PostedDate
                                        , EffectiveDate = (DateTime?)bank.EffectiveDate ?? b.PostedDate
                                        , tt.TransactionTypeID
                                        , p.PayeeID
                                        , b.TransferType
                                        , b.Amount
                                        , EffectiveAmount = (Decimal?)bank.EffectiveAmount ?? b.Amount
                                    });
                    foreach (var bank in queryBank)
                    {
                        Transaction transaction = new Transaction
                        {
                            SourceID = bank.FITID.ToString(),
                            PostedDate = bank.PostedDate,
                            EffectiveDate = bank.EffectiveDate,
                            TransactionTypeID = bank.TransactionTypeID,
                            PayeeID = bank.PayeeID,
                            TransferType = bank.TransferType,
                            Amount = bank.Amount,
                            EffectiveAmount = bank.EffectiveAmount,
                            TransactionSource = "Bank"
                        };
                        TransactionList.Add(transaction);
                        newRows++;
                    }
                    AddTransaction(application, TransactionList, "Bank");
                    logger.Info($"Sent [{newRows.ToString()}] Bank rows for Fact Insert");
                    logger.Info($"Finished reading ODS Bank rows");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while trying to read ODS Bank rows. Exception [{e.ToString()}]");
                throw;
            }
        }

        /// <summary>
        /// Take in a list of transactions and add them to Entity if not already exists
        /// </summary>
        private static void AddTransaction(ApplicationSettings application, List<Transaction> TransactionList, string source)
        {
            List<string> TransactionFactSourceID = new List<string>();
            List<Transaction> TransactionNew = new List<Transaction>();
            int newRows = 0;
            try
            {
                logger.Info($"Started processing [{source}] transaction rows");
                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    var queryTransaction = (from row in context.Transaction
                                            where row.TransactionSource == source
                                            select row.SourceID).ToList();
                    TransactionFactSourceID = queryTransaction;
                    foreach (Transaction transaction in TransactionList)
                    {
                        if (!TransactionFactSourceID.Contains(transaction.SourceID))
                        {
                            TransactionNew.Add(transaction);
                        }
                        newRows++;
                    }
                    foreach (Transaction transaction in TransactionNew)
                    {
                        context.Transaction.Add(transaction);
                    }
                    context.SaveChanges();
                    logger.Info($"Added [{newRows.ToString()}] rows from [{source}] to Fact");
                    logger.Info($"Finished processing [{source}] transaction rows");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while trying to process [{source}] transaction rows. Exception [{e.ToString()}]");
                throw;
            }
        }
    }
}

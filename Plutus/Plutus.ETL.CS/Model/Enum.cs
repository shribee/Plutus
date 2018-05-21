using System;
using System.Collections.Generic;
using System.Text;

namespace Plutus.ETL.CS.Model
{
    /// <summary>
    /// Public static class to contained all application enums
    /// </summary>
    public static class Enum
    {
        /// <summary>
        /// Enum to mark the life-cycle of a File
        /// </summary>
        public enum FileStatus
        {
            /// <summary>
            /// Initial state when file is read from file system
            /// </summary>
            ReadFromFileSystem = 0,
            /// <summary>
            /// State after file has been processed
            /// </summary>
            Processed
        }

        /// <summary>
        /// Enum to indicate type of file
        /// </summary>
        public enum FileType
        {
            /// <summary>
            /// File containing Bank transaction received from Intouch
            /// </summary>
            BankIntouch = 0,
            
            /// <summary>
            /// File containing Bank transaction received from Cater Allen
            /// </summary>
            BankCaterAllen,
            
            /// <summary>
            /// File containing Invoice
            /// </summary>
            Invoice,
            
            /// <summary>
            /// File containing Expense
            /// </summary>
            Expense,
            
            /// <summary>
            /// File containing Dividend Threshold
            /// </summary>
            DividendThreshold,

            /// <summary>
            /// File containing DIM Payee (one per payee)
            /// </summary>
            Payee,

            /// <summary>
            /// File containing Payee mapping to source names (one per payee)
            /// </summary>
            PayeeMapping,
            
            /// <summary>
            /// File containing Salary details
            /// </summary>
            Salary,
            
            /// <summary>
            /// File containing Transaction types
            /// </summary>
            TransactionType,

            /// <summary>
            /// File containing Bank transaction from Cater Allen enriched with transaction type, transfer type
            /// </summary>
            Bank,

            /// <summary>
            /// File containing Bank transaction from Cater Allen with effective date different from transaction date
            /// </summary>
            BankEdit,

            /// <summary>
            /// File containing Expense records with effective date different from transaction date
            /// </summary>
            ExpenseEdit,

            /// <summary>
            /// Invalid Files
            /// </summary>
            Invalid,

        }
    }
}

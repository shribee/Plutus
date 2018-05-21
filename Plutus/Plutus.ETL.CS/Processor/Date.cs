using System;
using NLog;
using Plutus.ETL.CS.Controller;
using Plutus.ETL.CS.Model;

namespace Plutus.ETL.CS.Processor
{
    /// <summary>
    /// Static class to Load dates
    /// </summary>
    public static class ProcessDate
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Static method to process Date
        /// </summary>
        public static void LoadDates(ApplicationSettings application)
        {
            DateTime startDate;
            DateTime endDate;
            int newRows = 0;
            try
            {
                logger.Info($"Started processing Date");

                startDate = application.StartDate;
                endDate = application.EndDate;

                using (BusinessAccountingEntities context = new BusinessAccountingEntities())
                {
                    context.Database.ExecuteSqlCommand("delete from DIM.Date");
                    while (DateTime.Compare(startDate, endDate) < 0)
                    {
                        Date newDateRow = new Date
                        {
                            LongDate = startDate
                        };
                        newRows++;
                        context.Date.Add(newDateRow);
                        startDate = startDate.AddDays(1);
                    }
                    context.SaveChanges();
                    logger.Info($"Added [{newRows.ToString()}] rows to Date");
                    logger.Info($"Finished processing Date");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while trying to process Date. Exception [{e.ToString()}]");
                throw;
            }
        }
    }
}

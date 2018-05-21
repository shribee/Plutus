using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Plutus.ETL.CS.Model;
using NLog;

namespace Plutus.ETL.CS.Controller
{
    /// <summary>
    /// Singleton class to hold application settings
    /// </summary>
    public sealed class ApplicationSettings
    {
        /// <summary>
        /// Create logger for the class
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ApplicationSettings()
        {
        }
        /// <summary>
        /// Single instance reference of singleton
        /// </summary>
        public static ApplicationSettings Instance { get { return Nested.instance; } }
        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly ApplicationSettings instance = new ApplicationSettings();

        }

        private List<DirectoryFile> _inputFileList;
        /// <summary>
        /// List of files in the CSV folder
        /// </summary>
        public List<DirectoryFile> InputFileList
        {
            get => _inputFileList;
            set => _inputFileList = value;
        }

        private string _inputDataFolder;
        /// <summary>
        /// Sub-folder in which csv files are located
        /// </summary>
        public string InputDataFolder
        {
            get => _inputDataFolder;
            set
            {
                string setting = "InputData";

                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException($"[{setting}] setting cannot be blank/null");
                }
                else if (!Directory.Exists(value))
                {
                    throw new DirectoryNotFoundException($"[{setting}] directory not found");
                }
                else if (value.Substring(value.Length - 1) != "\\")
                {
                    throw new ArgumentException($"[{setting}] must end in \\");
                }
                else
                {
                    _inputDataFolder = value;
                }
            }
        }

        private DateTime _startDate;
        /// <summary>
        /// Starting Date in the Date dimension
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                try
                {
                    DateTime startDate = Convert.ToDateTime(value);

                    if (DateTime.Compare(startDate, Convert.ToDateTime("2050-12-31")) > 0)
                    {
                        throw new ArgumentOutOfRangeException
                            ("End date cannot be greater than [2050-12-31]");
                    }
                    _startDate = startDate;
                }
                catch(Exception e)
                {
                    logger.Fatal($"Fatal exception while loading/converting StartDate value: [{value}]. Exception [{e.ToString()}]");
                    throw;
                }
            }
        }

        private DateTime _endDate;
        /// <summary>
        /// Ending Date in the Date dimension
        /// </summary>
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                try
                {
                    DateTime endDate = Convert.ToDateTime(value);

                    if (DateTime.Compare(endDate, Convert.ToDateTime("2050-12-31")) >= 0)
                    {
                        throw new ArgumentOutOfRangeException
                            ("End date cannot be greater than [2050-12-31]");
                    }
                    _endDate = endDate;
                }
                catch (Exception e)
                {
                    logger.Fatal($"Fatal exception while loading/converting EndDate value: [{value}]. Exception [{e.ToString()}]");
                    throw;
                }
            }
        }

        private bool _resetData;
        /// <summary>
        /// Flag to indicate whether to clear and reload Fact, ODS tables
        /// </summary>
        public bool ResetData
        {
            get => _resetData;
            set => _resetData = value;
        }

        /// <summary>
        /// Initialize Singleton
        /// </summary>
        public void Initialize()
        {
            try
            {
                InputDataFolder = Properties.Settings.Default.InputData.ToString();
                StartDate = Convert.ToDateTime(Properties.Settings.Default.StartDate);
                EndDate = Convert.ToDateTime(Properties.Settings.Default.EndDate);
                ResetData = Properties.Settings.Default.ResetData;

                if (DateTime.Compare(StartDate, EndDate) >= 0)
                {
                    throw new ArgumentOutOfRangeException
                        ("Fatal error. Start Date must be less than End Date");
                }
                InputFileList = new List<DirectoryFile>();
                logger.Info("Successfully loaded settings");
            }
            catch
            {
                logger.Fatal("Fatal exception while loading settings");
                throw;
            }
        }

    }
}

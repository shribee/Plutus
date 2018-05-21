using System;
using Plutus.ETL.CS.Utility;
using Plutus.ETL.CS.Model;
using Plutus.ETL.CS.Processor;
using NLog;
using System.IO;
using System.Linq;

namespace Plutus.ETL.CS.Controller
{
    /// <summary>
    /// Static Controller process for the Application containing only methods to drive the Application
    /// </summary>
    public static class ApplicationController
    {
        /// <summary>
        /// Create logger for the class
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Scan input folder for available files
        /// </summary>
        public static void IdentifyNewFiles(ApplicationSettings application)
        {
            foreach (string directory in Directory.GetDirectories(application.InputDataFolder))
            {
                logger.Info($"Reading files from folder [{directory}]");
                Model.Enum.FileType fileType;
                Model.Enum.FileStatus fileStatus = Model.Enum.FileStatus.ReadFromFileSystem;
                System.Enum.TryParse(Path.GetFileName(directory), out fileType);
                if (!System.Enum.TryParse(Path.GetFileName(directory), out fileType))
                {
                    fileType = Model.Enum.FileType.Invalid;
                    logger.Info($"Skipping invalid folder [{directory}]");
                    continue;
                }
                
                foreach (string file in Directory.GetFiles(directory))
                {
                    DirectoryFile newFile = new DirectoryFile(
                        folderName: directory + "\\"
                        , fullFileName: file
                        , fileName: Path.GetFileName(file)
                        , fileStatus: fileStatus
                        , fileType: fileType
                        );
                    newFile.FileContent = FileUtility.ReadFile(newFile.FullFileName);
                    application.InputFileList.Add(newFile);
                }
                logger.Info($"Successfully read [{Directory.GetFiles(directory).Count()}] files from folder [{directory}]");

            }

        }
        
    }
}

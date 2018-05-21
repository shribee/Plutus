using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plutus.ETL.CS.Model
{
    /// <summary>
    /// Public class for file system directory file objects
    /// </summary>
    public class DirectoryFile
    {
        private string _folderName;
        /// <summary>
        /// File folder name minus file name
        /// </summary>
        public string FolderName
        {
            get => _folderName;
            set
            {
                if (!(string.IsNullOrEmpty(value)))
                {
                    _folderName = value;
                }
                else
                {
                    throw new ArgumentNullException("File folder name cannot be null/empty");
                }
            }
        }

        private string _fullFileName;
        /// <summary>
        /// Full File name plus folder name
        /// </summary>
        public string FullFileName
        {
            get => _fullFileName;
            set
            {
                if (!(string.IsNullOrEmpty(value)))
                {
                    _fullFileName = value;
                }
                else
                {
                    throw new ArgumentNullException("Full File name cannot be null/empty");
                }
            }
        }

        private string _fileName;
        /// <summary>
        /// File name minus folder name
        /// </summary>
        public string FileName
        {
            get => _fileName;
            set
            {
                if(!(string.IsNullOrEmpty(value)))
                {
                    _fileName = value;
                }
                else
                {
                    throw new ArgumentNullException("File name cannot be null/empty");
                }
            }
        }

        private Enum.FileType _fileType;
        /// <summary>
        /// Enum file type
        /// </summary>
        public Enum.FileType FileType
        {
            get => _fileType;
            set => _fileType = value;
        }

        private Enum.FileStatus _fileStatus;
        /// <summary>
        /// Enum file status
        /// </summary>
        public Enum.FileStatus FileStatus
        {
            get => _fileStatus;
            set => _fileStatus = value;
        }

        private string _fileContent;
        /// <summary>
        /// Contents of file
        /// </summary>
        public string FileContent
        {
            get => _fileContent;
            set
            {
                if (!(value == null))
                {
                    _fileContent = value;
                }
                else
                {
                    throw new ArgumentNullException("File content cannot be null");
                }
            }
        }

        /// <summary>
        /// Constructor with most parameters
        /// </summary>
        public DirectoryFile
        (string folderName
            , string fullFileName
            , string fileName
            , Enum.FileStatus fileStatus
            , Enum.FileType fileType
        )
        {
            FolderName = folderName;
            FullFileName = fullFileName;
            FileName = fileName;
            FileStatus = fileStatus;
            FileType = fileType;
        }

        /// <summary>
        /// Constructor with all parameters
        /// </summary>
        public DirectoryFile
        (
            string folderName
            , string fullFileName
            , string fileName
            , Enum.FileStatus fileStatus
            , Enum.FileType fileType
            , string fileContent
        )
        {
            FolderName = folderName;
            FullFileName = fullFileName;
            FileName = fileName;
            FileStatus = fileStatus;
            FileType = fileType;
            FileContent = fileContent;
        }
        
        /// <summary>
        /// Constructor with most basic mandatory parameters
        /// </summary>
        public DirectoryFile(string folderName, string fullFileName, string fileName, Enum.FileStatus fileStatus)
        {
            FolderName = folderName;
            FullFileName = fullFileName;
            FileName = fileName;
            FileStatus = fileStatus;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Plutus.ETL.CS.Controller;
using Plutus.ETL.CS.Model;
using NLog;
using Microsoft.VisualBasic.FileIO;

namespace Plutus.ETL.CS.Utility
{
    /// <summary>
    /// Staic class for interacting with the file system
    /// </summary>
    public static class FileUtility
    {
        /// <summary>
        /// Create logger for the class
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// Takes in a full file name and populates a supplied string with its contents
        /// </summary>
        public static string ReadFile(string fileName)
        {
            string content = "";
            using (var reader = new StreamReader(fileName, Encoding.UTF8))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }

        /// <summary>
        /// Takes in csv file name and populates a supplied List of string array
        /// </summary>
        public static void CSVReader(string csvFullName, string delimiter, List<string[]> csvContent, int columnNum, bool containsHeader = true)
        {
            int headerSkip = 0;
            using (StreamReader SR = new StreamReader(csvFullName))
            {
                while (SR.Peek() > -1)
                {
                    string line = SR.ReadLine();
                    if ((containsHeader) && headerSkip == 0)
                    {
                        headerSkip++;
                    }
                    else
                    {
                        TextFieldParser parser = new TextFieldParser(new StringReader(line))
                        {
                            HasFieldsEnclosedInQuotes = true
                        };
                        parser.SetDelimiters(delimiter);

                        string[] fields = new string[columnNum];
                        string[] Row = new string[columnNum];
                        int i = 1;
                        while (!parser.EndOfData)
                        {
                            fields = parser.ReadFields();

                            foreach (string field in fields)
                            {
                                if (i < columnNum)
                                {
                                    Row[i - 1] = field;
                                }
                                i++;
                            }
                        }
                        parser.Close();
                        csvContent.Add(Row);
                    }
                }
            }
        }

        /// <summary>
        /// Takes in csv string and populates a supplied List of string array
        /// </summary>
        public static void CSVStringReader(string csvString, string delimiter, List<string[]> csvContent, int columnNum, bool containsHeader = true)
        {
            int headerSkip = 0;
            StringReader SR = new StringReader(csvString);
            while (SR.Peek() > -1)
            {
                string line = SR.ReadLine();
                if ((containsHeader) && headerSkip == 0)
                {
                    headerSkip++;
                }
                else 
                {
                    TextFieldParser parser = new TextFieldParser(new StringReader(line))
                    {
                        HasFieldsEnclosedInQuotes = true
                    };
                    parser.SetDelimiters(delimiter);

                    string[] fields = new string[columnNum];
                    string[] Row = new string[columnNum];
                    int i = 1;
                    while (!parser.EndOfData)
                    {
                        fields = parser.ReadFields();

                        foreach (string field in fields)
                        {
                            if (i < columnNum)
                            {
                                Row[i - 1] = field;
                            }
                            i++;
                        }
                    }
                    parser.Close();
                    csvContent.Add(Row);
                }
            }
        }
    }
    
}


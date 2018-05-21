using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plutus.ETL.CS.Controller;
using Plutus.ETL.CS.Model;
using Plutus.ETL.CS.Utility;
using System.Linq;

namespace Plutus.ETL.CS.Test
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        public void MainTest()
        {
            DirectoryFile intouchFile1 = new DirectoryFile("abc", "abc", Model.Enum.FileStatus.ReadFromFileSystem);
            Assert.AreEqual(intouchFile1.FolderName, "abc");
        }

        [TestMethod]
        public void AddContentTest()
        {
            DirectoryFile intouchFile1 = new DirectoryFile("abc", "abc", Model.Enum.FileStatus.ReadFromFileSystem);
            intouchFile1.FileContent = FileUtility.ReadFile("..\\..\\TestFiles\\bank_20160517220202_test.csv");
            ApplicationController.ProcessBankIntouch(intouchFile1);
            Assert.AreEqual(intouchFile1.FileContent, "abc");
        }

        [TestMethod]
        public void DBContextTest1()
        {
            DirectoryFile intouchFile1 = new DirectoryFile("abc", "abc", Model.Enum.FileStatus.ReadFromFileSystem);
            intouchFile1.FileContent = FileUtility.ReadFile("..\\..\\TestFiles\\bank_20160517220202_test2.csv");
            using (BusinessAccountingEntities context = new BusinessAccountingEntities())
            {
                context.Database.ExecuteSqlCommand("delete from ODS.BankIntouch");
                context.SaveChanges();
            }
            ApplicationController.ProcessBankIntouch(intouchFile1);
            int i = 0;
            using (BusinessAccountingEntities context = new BusinessAccountingEntities())
            {
                var query = (from row in context.BankIntouch
                             select row).ToList();
                i = query.Count;
            }
            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public void DBContextTest2()
        {
            DirectoryFile intouchFile1 = new DirectoryFile("abc", "abc", Model.Enum.FileStatus.ReadFromFileSystem);
            intouchFile1.FileContent = FileUtility.ReadFile("..\\..\\TestFiles\\bank_20160517220202_test2.csv");
            DirectoryFile intouchFile2 = new DirectoryFile("abc", "abc", Model.Enum.FileStatus.ReadFromFileSystem);
            intouchFile2.FileContent = FileUtility.ReadFile("..\\..\\TestFiles\\bank_20160517220202_test3.csv");
            using (BusinessAccountingEntities context = new BusinessAccountingEntities())
            {
                context.Database.ExecuteSqlCommand("delete from ODS.BankIntouch");
                context.SaveChanges();
            }
            ApplicationController.ProcessBankIntouch(intouchFile1);
            ApplicationController.ProcessBankIntouch(intouchFile2);
            int i = 0;
            using (BusinessAccountingEntities context = new BusinessAccountingEntities())
            {
                var query = (from row in context.BankIntouch
                             select row).ToList();
                i = query.Count;
            }
            Assert.AreEqual(4, i);
        }
    }
}

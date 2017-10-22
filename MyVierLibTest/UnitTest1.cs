using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyViewerLib;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyVierLibTest
{
    [TestClass]
    public class UnitTest1
    {
        string iniPath = @".\test\settings\settings.ini";

        private string getPicDir()
        {
            Kernel32 kernel = new Kernel32();
            var targetDir = kernel.GetIniValue(iniPath, "settings", "PictureDirectory");
            return targetDir;
        }

        private string getSqliteFilePath()
        {
            Kernel32 kernel = new Kernel32();
            var targetDir = kernel.GetIniValue(iniPath, "settings", "DBFilePath");
            return targetDir;
        }

        [TestMethod]
        public void IniFileReadTest()
        {
            var picDir = getPicDir();
            Assert.AreEqual(@"E:\WindowsWorkFiles\Documents\visual studio 2017\Projects\MyViewer\MyVierLibTest\test", picDir);
        }

        [TestMethod]
        public void FolderReadTest()
        {

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(getPicDir());
            IEnumerable<System.IO.DirectoryInfo> subFolders =
                di.EnumerateDirectories("*", System.IO.SearchOption.AllDirectories);

            var count = 0;
            //サブフォルダを列挙する
            foreach (System.IO.DirectoryInfo subFolder in subFolders)
            {
                count++;
            }

            Assert.AreEqual(7, count);
        }
        [TestMethod]
        public void SqlTest()
        {

            var dao = new Dao(getSqliteFilePath());
            var res = dao.GetAllTableEntity();
            foreach (Tag data in res)
            {
                Assert.AreEqual(0, data.TagId);
                Assert.AreEqual(@"hoge", data.TagName);
            }

        }

    }
}

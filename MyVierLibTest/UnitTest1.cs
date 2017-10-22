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

            Assert.AreEqual(8, count);
        }
        [TestMethod]
        public void SqlTest()
        {

            var dao = new Dao(getSqliteFilePath());
            var res = dao.GetAllTableEntity();
            Assert.AreEqual(0, res[0].TagId);
            Assert.AreEqual(@"hoge", res[0].TagName);

        }

        [TestMethod]
        public void SearchFolderIdListFromTagNameTest()
        {
            var dao = new Dao(getSqliteFilePath());
            var ret = dao.GetHavingTagFolderIdList(@"あああ");
            Assert.AreEqual(2, ret.Count);
            Assert.AreEqual(0, ret[0]);
            Assert.AreEqual(4, ret[1]);


            var ret2 = dao.GetHavingTagFolderIdList(@"hoge");
            Assert.AreEqual(1, ret2.Count);
            Assert.AreEqual(0, ret2[0]);

            var ret3 = dao.GetHavingTagFolderIdList(@"99999999");
            Assert.AreEqual(0, ret3.Count);

            var ret4 = dao.GetHavingTagFolderIdList(@"仮名");
            Assert.AreEqual(0, ret4.Count);
        }

        [TestMethod]
        public void SearchFolderPathFromFolderId()
        {
            var dao = new Dao(getSqliteFilePath());
            var ret = dao.GetFolderPath(4);
            Assert.AreEqual(@"E:\[bbb][あああ]\[settings]\[aaa][fdsa]", ret);

            var ret2 = dao.GetFolderPath(5);
            Assert.AreEqual(@"", ret2);

            var ret3 = dao.GetFolderPath(1);
            Assert.AreEqual(@"E:\[aaa]\[bbb]", ret3);

        }
        [TestMethod]
        public void CountInsideFileNum()
        {
            var dao = new Dao(getSqliteFilePath());
            var ret = dao.GetSumOfFileNum(new List<Int64> { 0, 1, 2 });
            Assert.AreEqual(40, ret);

            var ret2 = dao.GetSumOfFileNum(new List<Int64> { 999, 9999 });
            Assert.AreEqual(0, ret2);

            var ret3 = dao.GetSumOfFileNum(new List<Int64> { 0,0,1 });
            Assert.AreEqual(30, ret3);

        }
    }
}

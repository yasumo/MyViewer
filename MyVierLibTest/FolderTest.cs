using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyViewerLib;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyVierLibTest
{
    [TestClass]
    public class FolderTest
    {

        [TestMethod]
        public void IniFileReadTest()
        {
            var picDir = TestUtils.GetPicDir();
            Assert.AreEqual(@"E:\WindowsWorkFiles\Documents\visual studio 2017\Projects\MyViewer\MyVierLibTest\test", picDir);
        }

        [TestMethod]
        public void GetFileTest()
        {
            var picDir = TestUtils.GetPicDir();
            var fo = new FolderOperator();
            List<string> ext = new List<string> { ".jpg", ".png" };
            int num = 0;
            foreach (var filePath in fo.GetAllFilePathList(fo.GetAllFolderPathList(picDir), ext)){
                num++;
                Console.WriteLine(filePath);
            }
            Assert.AreEqual(7, num);



        }

        [TestMethod]
        public void FolderReadTest()
        {

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(TestUtils.GetPicDir());
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
        public void GetTagListTest()
        {
            var fo = new FolderOperator();
            {
                var tagList = fo.GetTagList(@"[aaa]\[bbb]");
                Assert.AreEqual(2, tagList.Count);
            }
            {
                var tagList = fo.GetTagList(@"E:\[あああ][いいい]\[bbb]\");
                Assert.AreEqual(3, tagList.Count);
                Assert.IsTrue(tagList.Contains(@"あああ"));
                Assert.IsTrue(tagList.Contains(@"いいい"));
                Assert.IsTrue(tagList.Contains(@"bbb"));
            }
            {
                var tagList = fo.GetTagList(@"E:\あああ][いいい]\bbb]\");
                Assert.AreEqual(1, tagList.Count);
                Assert.IsTrue(tagList.Contains(@"いいい"));
            }
            {
                var tagList = fo.GetTagList(@"E:\あああ]いいい]\bbb]\");
                Assert.AreEqual(0, tagList.Count);
            }
        }

    }
}

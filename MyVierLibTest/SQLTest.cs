using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyViewerLib;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyVierLibTest
{
    [TestClass]
    public class SQLTest
    {
        [TestMethod]
        public void SqlTest()
        {

            var dao = new Dao(TestUtils.GetSqliteFilePath());
            var res = dao.GetAllTableEntity();
            Assert.AreEqual(0, res[0].TagId);
            Assert.AreEqual(@"hoge", res[0].TagName);

        }
        [TestMethod]
        public void GetOtherTagNumTest()
        {
            var dao = new Dao(TestUtils.GetSqliteFilePath());
            {
                var ret = dao.GetOtherTagNum(new List<string> { @"あああ", @"uuu" });

                Assert.AreEqual(5, ret.Count);
                Assert.AreEqual(ret[0], (9, @"fda", 17));
                Assert.AreEqual(ret[1], (0, @"hoge", 10));
                Assert.AreEqual(ret[2], (2, @"いいい", 10));
                Assert.AreEqual(ret[3], (7, @"あいうえお", 10));
                Assert.AreEqual(ret[4], (10, @"csaf", 7));
            }

        }

        [TestMethod]
        public void SearchFolderIdListFromTagNameTest()
        {
            var dao = new Dao(TestUtils.GetSqliteFilePath());
            {
                var ret = dao.GetFolderIdListHaving(@"あああ",null);
                Assert.AreEqual(3, ret.Count);
                Assert.AreEqual(0, ret[0]);
                Assert.AreEqual(4, ret[1]);
                Assert.AreEqual(2, ret[2]);
            }
            {
                var ret = dao.GetFolderIdListHaving(@"hoge", null);
                Assert.AreEqual(1, ret.Count);
                Assert.AreEqual(0, ret[0]);
            }
            {
                var ret = dao.GetFolderIdListHaving(@"99999999", null);
                Assert.AreEqual(0, ret.Count);
            }
            {
                var ret = dao.GetFolderIdListHaving(@"仮名", null);
                Assert.AreEqual(0, ret.Count);
            }

            {
                var ret = dao.GetFolderIdListHaving(@"hoge", new List<Int64> { 0, 4 });
                Assert.AreEqual(1, ret.Count);
                Assert.AreEqual(0, ret[0]);
            }

            {
                var ret = dao.GetFolderIdListHaving(new List<string> { @"あああ", @"hoge" });
                Assert.AreEqual(1, ret.Count);
                Assert.AreEqual(0, ret[0]);
            }
        }

        [TestMethod]
        public void SearchFolderPathFromFolderId()
        {
            var dao = new Dao(TestUtils.GetSqliteFilePath());
            {
                var ret = dao.GetFolderPath(4);
                Assert.AreEqual(@"E:\[bbb][あああ]\[settings]\[aaa][fdsa]", ret);
            }
            {
                var ret = dao.GetFolderPath(5);
                Assert.AreEqual(@"", ret);
            }
            {
                var ret = dao.GetFolderPath(1);
                Assert.AreEqual(@"E:\[aaa]\[bbb]", ret);
            }
        }
        [TestMethod]
        public void CountInsideFileNum()
        {
            var dao = new Dao(TestUtils.GetSqliteFilePath());
            {
                var ret = dao.GetSumOfFileNum(new List<Int64> { 0, 1, 2 });
                Assert.AreEqual(38, ret);
            }
            {
                var ret = dao.GetSumOfFileNum(new List<Int64> { 999, 9999 });
                Assert.AreEqual(0, ret);
            }
            {
                var ret = dao.GetSumOfFileNum(new List<Int64> { 0, 0, 1 });
                Assert.AreEqual(31, ret);
            }
        }
    }
}

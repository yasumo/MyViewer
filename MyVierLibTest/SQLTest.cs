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
        public void ClearTmpTableTest() {
            using (var dao = new Dao(TestUtils.GetWriteSqliteFilePath()))
            {
                var res1 = dao.GetAllTag();
                Assert.AreEqual(14, res1.Count);

                dao.ClearTmpTable();
                var res2 = dao.GetAllTag();
                Assert.AreEqual(0, res2.Count);
                dao.Close();
            }
        }

        //同じテーブル使ってるので、他のテストが通らなくなる糞実装
        [TestMethod]
        public void InsertTest()
        {
            using (var dao = new Dao(TestUtils.GetWriteSqliteFilePath()))
            {
                var nextId1 = dao.GetNextId("FOLDER", "FOLDER_ID");
                Assert.AreEqual(5L, nextId1);
                var id = dao.InsertFolderTable(@"E:\hogehoge\[aaa]\bbb", 100L);
                Assert.AreEqual(nextId1, id);
                var nextId2 = dao.GetNextId("FOLDER", "FOLDER_ID");
                Assert.AreEqual(6L, nextId2);

                nextId1 = dao.GetNextId("FOLDER_TAG", "FOLDER_TAG_ID");
                Assert.AreEqual(17L, nextId1);
                id = dao.InsertFolderTagTable(0L, 100L);
                Assert.AreEqual(nextId1, id);
                nextId2 = dao.GetNextId("FOLDER_TAG", "FOLDER_TAG_ID");
                Assert.AreEqual(18L, nextId2);

                nextId1 = dao.GetNextId("TAG", "TAG_ID");
                Assert.AreEqual(14, nextId1);
                id = dao.SearchOrInsertTagTable("aaaB");
                Assert.AreEqual(nextId1, id);
                nextId2 = dao.GetNextId("TAG", "TAG_ID");
                Assert.AreEqual(15L, nextId2);


                id = dao.SearchOrInsertTagTable("aaa");
                Assert.AreEqual(4L, id);
            }

        }

        [TestMethod]
        public void InsertMulutiTest()
        {
            using (var dao = new Dao(TestUtils.GetWriteSqliteFilePath()))
            {
                var tagIdList = new List<long>();
                tagIdList.Add(10);
                tagIdList.Add(12);
                tagIdList.Add(13);
                tagIdList.Add(14);
                var folderIdList = new List<long>();
                folderIdList.Add(1);
                folderIdList.Add(2);
                folderIdList.Add(3);
                folderIdList.Add(4);
                dao.InsertMulutiFolderTagTable(folderIdList, tagIdList);
            }
            using (var dao = new Dao(TestUtils.GetWriteSqliteFilePath()))
            {
                var insideFileNumList = new List<long>();
                insideFileNumList.Add(10);
                insideFileNumList.Add(12);
                insideFileNumList.Add(13);
                insideFileNumList.Add(14);
                var folderPathList = new List<string>();
                folderPathList.Add(@"E:\aaa");
                folderPathList.Add(@"E:\bbb");
                folderPathList.Add(@"E:\ccc");
                folderPathList.Add(@"E:\aaa");

                dao.InsertMulutiFolderTable(folderPathList, insideFileNumList);
            }
        }

        [TestMethod]
        public void NextIdTest()
        {
            using (var dao = new Dao(TestUtils.GetWriteSqliteFilePath()))
            {
                var nextId1 = dao.GetNextId("FOLDER", "FOLDER_ID");
                Assert.AreEqual(5, nextId1);

                dao.ClearTmpTable();
                var nextId2 = dao.GetNextId("FOLDER", "FOLDER_ID");
                Assert.AreEqual(1, nextId2);
            }

        }
        [TestMethod]
        public void SqlTest()
        {

            using (var dao = new Dao(TestUtils.GetWriteSqliteFilePath()))
            {
                var res = dao.GetAllTag();
                Assert.AreEqual(0, res[0].TagId);
                Assert.AreEqual(@"hoge", res[0].TagName);
            }
        }
        [TestMethod]
        public void GetOtherTagNumTest()
        {
            using (var dao = new Dao(TestUtils.GetWriteSqliteFilePath()))
            {
                    var ret = dao.GetOtherTag(new List<string> { @"あああ", @"uuu" });

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
            using (var dao = new Dao(TestUtils.GetWriteSqliteFilePath()))
            {
                {
                    var ret = dao.GetFolderIdListHaving(@"あああ", null);
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
        }

        [TestMethod]
        public void SearchFolderPathFromFolderId()
        {
            using (var dao = new Dao(TestUtils.GetWriteSqliteFilePath()))
            {
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
        }
        [TestMethod]
        public void CountInsideFileNum()
        {
            using (var dao = new Dao(TestUtils.GetWriteSqliteFilePath()))
            {
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
}

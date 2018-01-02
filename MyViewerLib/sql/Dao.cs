﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyViewerLib
{
    public class Dao
    {
        string datasourcePath;
        public Dao(string datasourcePath)
        {
            this.datasourcePath = datasourcePath;
        }

        private SQLiteConnectionStringBuilder createConnectionString()
        {
            return new SQLiteConnectionStringBuilder
            {
                DataSource = datasourcePath
            };

        }

        //フォルダテーブルとフォルダタグテーブルを消す
        public void ClearTmpTable()
        {
            var cs = createConnectionString();
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                c.Open();
                using (DataContext dc = new DataContext(c))
                {
                    //dc.ExecuteCommand("UPDATE Products SET QuantityPerUnit = {0} WHERE ProductID = {1}", "24 boxes", 5);
                    dc.ExecuteCommand(@"DELETE from TAG");
                    dc.ExecuteCommand(@"DELETE from FOLDER");
                    dc.ExecuteCommand(@"DELETE from FOLDER_TAG");
                    dc.ExecuteCommand(@"DELETE from THUMBNAIL");
                }

                c.Close();
            }
        }

        //テーブルの最大値の次の値を取る
        public long GetNextId( string tableName, string idName)
        {
            var cs = createConnectionString();
            long nextId = 0;
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                c.Open();
                using (DataContext dc = new DataContext(c))
                {
                    long? max = dc.ExecuteQuery<long?>(@"select max ("+idName+") from "+tableName ).FirstOrDefault();
                    nextId = max.GetValueOrDefault(0) + 1;
                }

                c.Close();
            }

            return nextId;
        }

        //各種インサート
        public void MyExecuteCommand(string sql) {
            var cs = createConnectionString();
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                c.Open();
                using (DataContext dc = new DataContext(c))
                {
                    dc.ExecuteCommand(sql);
                }

                c.Close();
            }

        }
        public long InsertFolderTable(string folderPath, long insideFileNum)
        {
            var id = GetNextId("FOLDER", "FOLDER_ID");
            var sql = "INSERT INTO FOLDER VALUES(" + id.ToString() + ",\"" + folderPath + "\"," + insideFileNum.ToString() + ")";
            MyExecuteCommand(sql);
            return id;
        }
        public long InsertFolderTagTable(long folderId, long tagId)
        {
            var id = GetNextId("FOLDER_TAG", "FOLDER_TAG_ID");
            var sql = "INSERT INTO FOLDER_TAG VALUES(" + id.ToString() + "," + folderId.ToString() + "," + tagId.ToString() + ")";
            MyExecuteCommand(sql);
            return id;
        }

        //タグを追加してタグIDを返す、タグが既に存在してたらそのIDを返す
        public long SerchOrInsertTagTable(string tagName)
        {

            var cs = createConnectionString();
            Tag res;
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {

                c.Open();

                using (DataContext dc = new DataContext(c))
                {
                    Table<Tag> rec = dc.GetTable<Tag>();
                    res = (
                        from x in rec.Where(x => x.TagName == tagName)
                        select x).SingleOrDefault();
                }
                c.Close();
            }

            long id = 0l;
            if (res != null) {
                id = res.TagId;
            }
            else
            {
                id = GetNextId("TAG", "TAG_ID");
                var sql = "INSERT INTO TAG VALUES(" + id.ToString() + ",\"" + tagName + "\")";
                MyExecuteCommand(sql);
            }

            return id;
        }

        public void InsertThumbnailTable(string filePathMd5,string createTime)
        {
            var sql = "INSERT INTO THUMBNAIL VALUES(\"" + filePathMd5 + "\",\"" + createTime + "\")";
            MyExecuteCommand(sql);
        }


        //すべてのタグを取得
        public List<Tag> GetAllTag()
        {
            var cs = createConnectionString();
            List<Tag> ret;
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                
                c.Open();

                
                using (DataContext dc = new DataContext(c))
                {
                    Table<Tag> rec = dc.GetTable<Tag>();
                    var res =
                        from x in rec
                        select x;

                    ret = new List<Tag>(res);
                }

                c.Close();
            }
            return ret;
        }

        //フォルダIDリストが持ってるファイルの総数取得
        public long GetSumOfFileNum(List<Int64> folderIdList)
        {
            Int64? ret = 0L;

            var cs = createConnectionString();
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                
                c.Open();

                using (DataContext dc = new DataContext(c))
                {
                    //Folder
                    Table<Folder> folderTable = dc.GetTable<Folder>();
                    ret = (from f in folderTable.Where(x=> folderIdList.Contains(x.FolderId))
                           select f.InsideFileNum).Sum();

                }

                c.Close();
            }

            return ret.GetValueOrDefault(0L);
        }

        //フォルダIDからフォルダIDパスを取得
        public string GetFolderPath(Int64 folderId)
        {
            var cs = createConnectionString();
            string ret = @"";
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                
                c.Open();

                
                using (DataContext dc = new DataContext(c))
                {
                    Table<Folder> folderTable = dc.GetTable<Folder>();
                    var res = (
                        from f in folderTable.Where(x=> x.FolderId == folderId)
                        select f).SingleOrDefault();
                    if (res != null)
                    {
                        ret = res.FolderPath;
                    }

                }
                c.Close();
            }
            return ret;
        }

        //GetOtherTagから呼ばれるやつ
        private List<Int64> GetOtherTagIdList(List<Int64> folderIdList,List<string>tagNameList)
        {
            var cs = createConnectionString();
            List<Int64> ret;
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                
                c.Open();

                
                using (DataContext dc = new DataContext(c))
                {
                    Table<FolderTag> folderTagTable = dc.GetTable<FolderTag>();
                    Table<Tag> tagTable = dc.GetTable<Tag>();

                    var res = (from tf in folderTagTable.Where(x => folderIdList.Contains(x.FolderId))
                               from t in tagTable.Where(x => tf.TagId == x.TagId && !tagNameList.Contains(x.TagName))
                               select tf.TagId).Distinct();
                    ret = new List<Int64>(res);
                }
                c.Close();
            }

            return ret;
        }

        //タグネームを持っているやつが持っている他のタグのリストを返す
        public List<( Int64 tagId, string tagName, Int64 tagNum)> GetOtherTag(List<string> tagNameList)
        {
            var baseFolderIdList = GetFolderIdListHaving(tagNameList);
            var otherTagIdList = GetOtherTagIdList(baseFolderIdList, tagNameList);

            var cs = createConnectionString();
            var ret = new List<(Int64,string, Int64)>();
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                
                c.Open();

                
                using (DataContext dc = new DataContext(c))
                {
                    Table<Tag> tagTable = dc.GetTable<Tag>();
                    Table<FolderTag> folderTagTable = dc.GetTable<FolderTag>();
                    Table<Folder> folderTable = dc.GetTable<Folder>();
                    var res = (from ft in folderTagTable.Where(x => otherTagIdList.Contains(x.TagId) && baseFolderIdList.Contains(x.FolderId))
                               from t in tagTable.Where(x => x.TagId == ft.TagId)
                               from f in folderTable.Where(x => x.FolderId == ft.FolderId)
                               
                               select new
                               {
                                   t.TagId,
                                   t.TagName,
                                   f.InsideFileNum
                               });

                    var groupby = res.GroupBy(x => x.TagId)
                                     .Select(x => new {TagId =  x.Key,TagName = x.Select(y=>y.TagName).Single(), Sum = x.Sum(y => (long?)y.InsideFileNum) })
                                     .OrderByDescending(x=>x.Sum );



                    foreach (var x in groupby)
                    {
                        var tagId = x.TagId;
                        var tagName = x.TagName.ToString();
                        var tagNum = x.Sum.GetValueOrDefault(0L);
                        ret.Add((tagId,tagName, tagNum));
                    }
                }

                c.Close();
            }

            return ret;
        }

        //タグネームのリストをすべて持っているフォルダID
        public List<Int64> GetFolderIdListHaving(List<string>tagNameList)
        {
            List<Int64> ret = null;
            foreach(var tagName in tagNameList)
            {
                ret = GetFolderIdListHaving(tagName, ret);
            }
            return ret;
        }

        //FolderIDリストを持ちかつタグネームを持っているフォルダIDリストをAND条件で絞り込む
        public List<Int64> GetFolderIdListHaving(string tagName, List<Int64> folderIdList)
        {
            var cs = createConnectionString();
            List<Int64> ret = new List<Int64>();
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                
                c.Open();

                
                using (DataContext dc = new DataContext(c))
                {
                    Table<Tag> tagTable = dc.GetTable<Tag>();
                    Table<FolderTag> folderTagTable = dc.GetTable<FolderTag>();

                    if (folderIdList == null || folderIdList.Count==0)
                    {
                        var res = from t in tagTable.Where(x => x.TagName == tagName)
                                  from ft in folderTagTable.Where(x => x.TagId == t.TagId)
                                  select new
                                  {
                                      ft.FolderId,
                                  };
                        foreach (var x in res)
                        {
                            ret.Add(x.FolderId);
                        }
                    }
                    else
                    {
                        var res = from t in tagTable.Where(x => x.TagName == tagName)
                                  from ft in folderTagTable.Where(x => x.TagId == t.TagId && folderIdList.Contains(x.FolderId))
                                  select new
                                  {
                                      ft.FolderId,
                                  };
                        foreach (var x in res)
                        {
                            ret.Add(x.FolderId);
                        }
                    }

                }

                c.Close();
            }
            return ret;
        }

       
    }
}


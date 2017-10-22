using System;
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

        public List<Tag> GetAllTableEntity()
        {
            var cs = createConnectionString();
            List<Tag> ret;
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                // データベースを開く
                c.Open();

                // ここにデータベース処理コードを書く
                using (DataContext dc = new DataContext(c))
                {
                    Table<Tag> rec = dc.GetTable<Tag>();
                    var res =
                        from x in rec
                        select x;

                    ret = new List<Tag>(res);
                }
                // データベースを閉じる
                c.Close();
            }
            return ret;
        }

        public long GetSumOfFileNum(List<Int64> folderIdList)
        {
            Int64? ret = 0L;

            var cs = createConnectionString();
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                // データベースを開く
                c.Open();

                // ここにデータベース処理コードを書く
                using (DataContext dc = new DataContext(c))
                {
                    //Folder
                    Table<Folder> folderTable = dc.GetTable<Folder>();
                    ret = (from f in folderTable.Where(x=> folderIdList.Contains(x.FolderId))
                           select f.InsideFileNum).Sum();

                }

                // データベースを閉じる
                c.Close();
            }

            return ret.GetValueOrDefault(0L);
        }

        public string GetFolderPath(Int64 folderId)
        {
            var cs = createConnectionString();
            string ret = @"";
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                // データベースを開く
                c.Open();

                // ここにデータベース処理コードを書く
                using (DataContext dc = new DataContext(c))
                {
                    //Folder
                    Table<Folder> folderTable = dc.GetTable<Folder>();
                    var res = (
                        from f in folderTable.Where(x=> x.FolderId == folderId)
                        select f).SingleOrDefault();
                    if (res != null)
                    {
                        ret = res.FolderPath;
                    }

                }
                // データベースを閉じる
                c.Close();
            }
            return ret;
        }

        private List<Int64> GetOtherTagIdList(List<Int64> folderIdList,List<string>tagNameList)
        {
            var cs = createConnectionString();
            List<Int64> ret;
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                // データベースを開く
                c.Open();

                // ここにデータベース処理コードを書く
                using (DataContext dc = new DataContext(c))
                {
                    Table<FolderTag> folderTagTable = dc.GetTable<FolderTag>();
                    Table<Tag> tagTable = dc.GetTable<Tag>();

                    var res = (from tf in folderTagTable.Where(x => folderIdList.Contains(x.FolderId))
                               from t in tagTable.Where(x => tf.TagId == x.TagId && !tagNameList.Contains(x.TagName))
                               select tf.TagId).Distinct();
                    ret = new List<Int64>(res);
                }
                // データベースを閉じる
                c.Close();
            }

            return ret;
        }
        public List<( Int64 tagId, string tagName, Int64 tagNum)> GetOtherTagNum(List<string> tagNameList)
        {
            var baseFolderIdList = GetFolderIdListHaving(tagNameList);
            var otherTagIdList = GetOtherTagIdList(baseFolderIdList, tagNameList);

            var cs = createConnectionString();
            var ret = new List<(Int64,string, Int64)>();
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                // データベースを開く
                c.Open();

                // ここにデータベース処理コードを書く
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
                // データベースを閉じる
                c.Close();
            }

            return ret;
        }

        public List<Int64> GetFolderIdListHaving(List<string>tagNameList)
        {
            List<Int64> ret = null;
            foreach(var tagName in tagNameList)
            {
                ret = GetFolderIdListHaving(tagName, ret);
            }
            return ret;
        }

        public List<Int64> GetFolderIdListHaving(string tagName, List<Int64> folderIdList)
        {
            var cs = createConnectionString();
            List<Int64> ret = new List<Int64>();
            using (SQLiteConnection c = new SQLiteConnection(cs.ToString()))
            {
                // データベースを開く
                c.Open();

                // ここにデータベース処理コードを書く
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
                // データベースを閉じる
                c.Close();
            }
            return ret;
        }

       
    }
}


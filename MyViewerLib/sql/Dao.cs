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
                    Table<Folder> rec = dc.GetTable<Folder>();
                    ret = (from x in rec
                           where folderIdList.Contains(x.FolderId)
                           select x.InsideFileNum).Sum();

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
                    Table<Folder> rec = dc.GetTable<Folder>();
                    var res = (
                        from x in rec
                        where x.FolderId == folderId
                        select x).SingleOrDefault();
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

        public List<(string tagName, int tagNum)> GetOtherTagNum(List<string> tagNameList)
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
                    var res =
                        from ft in folderTagTable.Where(x => x.TagId == x.TagId)
                        select new
                        {
                            ft.FolderId,
                        };
                }
                // データベースを閉じる
                c.Close();
            }

            return null;
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


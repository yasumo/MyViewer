﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyViewerLib
{
    public class Dao:IDisposable
    {
        string datasourcePath;

        SQLiteConnectionStringBuilder cs;
        SQLiteConnection c;
        DataContext dc;
        DbTransaction transaction;
        bool ConnectionOpen = false;
        bool TransactionOpen = false;

        public Dao(string datasourcePath)
        {

            this.datasourcePath = datasourcePath;
            cs = createConnectionString();
            c = new SQLiteConnection(cs.ToString());
            c.Open();
            ConnectionOpen = true;
            dc = new DataContext(c);
        }
        private SQLiteConnectionStringBuilder createConnectionString()
        {
            return new SQLiteConnectionStringBuilder
            {
                DataSource = datasourcePath
            };
        }

        //transaction関連
        public void StartTransaction()
        {
            TransactionOpen = true;
            transaction = c.BeginTransaction();
        }
        //transaction関連
        public void Commit()
        {
            TransactionOpen = false;
            transaction.Commit();
            transaction.Dispose();
        }


        //dispose関連
        public void Close()
        {
            if (dc != null) {
                dc.Dispose();
            }
            if (ConnectionOpen) { 
                c.Close();
                c.Dispose();
                ConnectionOpen = false;
            }
            if (TransactionOpen)
            {
                Commit();
            }
        }


        //フォルダテーブルとフォルダタグテーブルを消す
        public void ClearTmpTable()
        {
            //dc.ExecuteCommand("UPDATE Products SET QuantityPerUnit = {0} WHERE ProductID = {1}", "24 boxes", 5);
            dc.ExecuteCommand(@"DELETE from TAG");
            dc.ExecuteCommand(@"DELETE from FOLDER");
            dc.ExecuteCommand(@"DELETE from FOLDER_TAG");
        }

        //テーブルの最大値の次の値を取る
        public long GetNextId( string tableName, string idName)
        {
            long nextId = 0;
            long? max = dc.ExecuteQuery<long?>(@"select max (" + idName + ") from " + tableName).FirstOrDefault();
            nextId = max.GetValueOrDefault(0) + 1;

            return nextId;
        }

        //各種インサート
        public void MyExecuteCommand(string sql) {
            dc.ExecuteCommand(sql);
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
        //複数インサート
        public List<long> InsertMulutiFolderTable(List<string> folderPathList, List<long> insideFileNumList)
        {
            var retIdList = new List<long>();
            if (!(folderPathList.Count > 0 && folderPathList.Count == insideFileNumList.Count))
            {
                return retIdList;
            }

            for (int i = 0; i < folderPathList.Count; i++)
            {
                //最大値の連番を取る
                retIdList.Add(GetNextId("FOLDER", "FOLDER_ID") + i);
            }
            var values = "(" + retIdList[0].ToString() + ",\"" + folderPathList[0] + "\"," + insideFileNumList[0].ToString() + ")";

            for (int i = 1; i < retIdList.Count; i++)
            {
                values += ",(" + retIdList[i].ToString() + ",\"" + folderPathList[i] + "\"," + insideFileNumList[i].ToString() + ")";

            }
            var sql = @"INSERT INTO FOLDER VALUES " + values;
            MyExecuteCommand(sql);

            return retIdList;
        }

        public List<long> InsertMulutiFolderTagTable(List<long> folderIdList, List<long> tagIdList)
        {
            var retIdList = new List<long>();
            if (!(folderIdList.Count > 0 && folderIdList.Count==tagIdList.Count))
            {
                return retIdList;
            }

            for (int i=0;i<tagIdList.Count;i++)
            {
                //最大値の連番を取る
                retIdList.Add(GetNextId("FOLDER_TAG", "FOLDER_TAG_ID") + i);
            }

            var values = "(" + retIdList[0].ToString() + ", " + folderIdList[0].ToString() + ", " + tagIdList[0].ToString() + ")";
            for (int i = 1; i < tagIdList.Count; i++)
            {
                values += ",(" + retIdList[i].ToString() + ", " + folderIdList[i].ToString() + ", " + tagIdList[i].ToString() + ")";

            }
            var sql = @"INSERT INTO FOLDER_TAG VALUES " + values;
            MyExecuteCommand(sql);


            return retIdList;
        }

        //タグを追加してタグIDを返す、タグが既に存在してたらそのIDを返す
        public long SearchOrInsertTagTable(string tagName)
        {

            Tag res;
            Table<Tag> rec = dc.GetTable<Tag>();
            res = (
                from x in rec.Where(x => x.TagName == tagName)
                select x).SingleOrDefault();

            long id = 0L;
            if (res != null)
            {
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



        //すべてのタグを取得
        public List<Tag> GetAllTag()
        {
            List<Tag> ret;
            Table<Tag> rec = dc.GetTable<Tag>();
            var res =
                from x in rec
                select x;

            ret = new List<Tag>(res);
            return ret;
        }

        //すべてのタグと、それに紐付くファイル数を取得
        public List<(Int64 tagId, string tagName, long tagNum)> GetAllTagAndNum()
        {
            var ret = new List<(Int64 tagId,string tagName, long tagNum)>();
            Table<Tag> tagTable = dc.GetTable<Tag>();
            Table<Folder> folderTable = dc.GetTable<Folder>();
            Table<FolderTag> folderTagTable = dc.GetTable<FolderTag>();
            var res = (from ft in folderTagTable
                       from t in tagTable.Where(x => x.TagId == ft.TagId)
                       from f in folderTable.Where(x => x.FolderId == ft.FolderId)

                       select new
                       {
                           t.TagId,
                           t.TagName,
                           f.InsideFileNum
                       });

            var groupby = res.GroupBy(x => x.TagId)
                             .Select(x => new { TagId = x.Key, TagName = x.Select(y => y.TagName).Single(), Sum = x.Sum(y => (long?)y.InsideFileNum) })
                             .OrderByDescending(x => x.Sum);

            foreach (var x in groupby)
            {
                var tagId = x.TagId;
                var tagName = x.TagName.ToString();
                var tagNum = x.Sum.GetValueOrDefault(0L);
                ret.Add((tagId, tagName, tagNum));
            }

            return ret;
        }

        //フォルダIDリストが持ってるファイルの総数取得
        public long GetSumOfFileNum(List<Int64> folderIdList)
        {
            Int64? ret = 0L;


            //Folder
            Table<Folder> folderTable = dc.GetTable<Folder>();
            ret = (from f in folderTable.Where(x => folderIdList.Contains(x.FolderId))
                   select f.InsideFileNum).Sum();


            return ret.GetValueOrDefault(0L);
        }

        //フォルダIDからフォルダIDパスを取得
        public string GetFolderPath(Int64 folderId)
        {
            string ret = @"";
            Table<Folder> folderTable = dc.GetTable<Folder>();
            var res = (
                from f in folderTable.Where(x => x.FolderId == folderId)
                select f).SingleOrDefault();
            if (res != null)
            {
                ret = res.FolderPath;
            }

            return ret;
        }


        //引数のフォルダIDが持つ、引数のタグネーム以外の
        private List<Int64> GetOtherTagIdList(List<Folder> folderList,List<string>tagNameList)
        {
            List<Int64> ret = new List<Int64>();

            Table<FolderTag> folderTagTable = dc.GetTable<FolderTag>();
            Table<Tag> tagTable = dc.GetTable<Tag>();

            var folderIdListTmp = new List<Int64>();
            foreach (var folder in folderList)
            {
                folderIdListTmp.Add(folder.FolderId);
                if (folderIdListTmp.Count > 500)
                {
                    var res = (from tf in folderTagTable.Where(x => folderIdListTmp.Contains(x.FolderId))
                               from t in tagTable.Where(x => tf.TagId == x.TagId && !tagNameList.Contains(x.TagName))
                               select tf.TagId).Distinct();
                    ret.AddRange(new List<Int64>(res));
                    folderIdListTmp = new List<Int64>();
                }
            }
            if (folderIdListTmp.Count > 0)
            {
                var res = (from tf in folderTagTable.Where(x => folderIdListTmp.Contains(x.FolderId))
                           from t in tagTable.Where(x => tf.TagId == x.TagId && !tagNameList.Contains(x.TagName))
                           select tf.TagId).Distinct();
                ret.AddRange(new List<Int64>(res));
            }
            
            ret.Distinct();

            return ret;
        }

        //タグネームを持っているやつが持っている他のタグのリストを返す
        public List<( Int64 tagId, string tagName, Int64 tagNum)> GetOtherTag(List<string> tagNameList)
        {
            var baseFolderList = GetFolderIdListHaving(tagNameList);
            var otherTagIdList = GetOtherTagIdList(baseFolderList, tagNameList);

            var baseFolderIdList = new List<Int64>();
            foreach (var folder in baseFolderList)
            {
                baseFolderIdList.Add(folder.FolderId);
            }


            var ret = new List<(Int64,string, Int64)>();
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
                             .Select(x => new { TagId = x.Key, TagName = x.Select(y => y.TagName).Single(), Sum = x.Sum(y => (long?)y.InsideFileNum) })
                             .OrderByDescending(x => x.Sum);



            foreach (var x in groupby)
            {
                var tagId = x.TagId;
                var tagName = x.TagName.ToString();
                var tagNum = x.Sum.GetValueOrDefault(0L);
                ret.Add((tagId, tagName, tagNum));
            }
        


            return ret;
        }

        //タグネームのリストをすべて持っているフォルダID
        public List<Folder> GetFolderIdListHaving(List<string>tagNameList)
        {
            List<Folder> ret = new List<Folder>();
            Table<Tag> tagTable = dc.GetTable<Tag>();
            Table<FolderTag> folderTagTable = dc.GetTable<FolderTag>();
            Table<Folder> folderTable = dc.GetTable<Folder>();

            var res = from t in tagTable.Where(x => tagNameList.Contains(x.TagName))
                      from ft in folderTagTable.Where(x => x.TagId == t.TagId)
                      from f in folderTable.Where(x => x.FolderId == ft.FolderId)
                      select f;

            var groupby = res.GroupBy(x => x.FolderId)
                .Select(x => new { FolderId = x.Key, FolderPath = x.Select(y => y.FolderPath).Single(), InsideFileNum = x.Select(y => y.InsideFileNum).Single(), Count = x.Count() })
                .Where(x => x.Count == tagNameList.Count);

            foreach (var x in groupby)
            {
                var retFolder = new Folder();
                retFolder.FolderId = x.FolderId;
                retFolder.FolderPath = x.FolderPath;
                retFolder.InsideFileNum = x.InsideFileNum;
                ret.Add(retFolder);
            }

            return ret;
        }





        public void Dispose()
        {
            Close();
        }
    }
}


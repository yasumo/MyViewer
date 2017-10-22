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
            var aConnectionString = createConnectionString();
            List<Tag> ret;
            using (SQLiteConnection aConnection = new SQLiteConnection(aConnectionString.ToString()))
            {
                // データベースを開く
                aConnection.Open();

                // ここにデータベース処理コードを書く
                using (DataContext aContext = new DataContext(aConnection))
                {
                    //Tag
                    Table<Tag> rec = aContext.GetTable<Tag>();
                    var res =
                        from x in rec
                        select x;

                    ret = new List<Tag>(res);


                }
                // データベースを閉じる
                aConnection.Close();
            }
            return ret;

        }
        public void hogehoge()
        {
            SQLiteConnectionStringBuilder aConnectionString = new SQLiteConnectionStringBuilder
            {
                DataSource = @".\settings\MyViewer.sqlite" // データベースの保存先
            };

            using (SQLiteConnection aConnection = new SQLiteConnection(aConnectionString.ToString()))
            {
                // データベースを開く
                aConnection.Open();
                // ここにデータベース処理コードを書く
                using (DataContext aContext = new DataContext(aConnection))
                {

                    //Folder
                    Table<Folder> recf = aContext.GetTable<Folder>();
                    IQueryable<Folder> resf =
                        from x in recf
                        select x;

                    Console.WriteLine("=== Folder ===");
                    foreach (Folder data in resf)
                    {
                        Console.WriteLine("=== {0} ===", data.FolderId);
                        Console.WriteLine(data.FolderPath);
                    }

                    //FolderTag
                    Table<FolderTag> recft = aContext.GetTable<FolderTag>();
                    IQueryable<FolderTag> resft =
                        from x in recft
                        select x;

                    Console.WriteLine("=== FolderTag ===");
                    foreach (FolderTag data in resft)
                    {
                        Console.WriteLine("=== {0} ===", data.FolderTagId);
                        Console.WriteLine(data.TagId);
                        Console.WriteLine(data.FolderId);
                    }

                    //Thumbnail
                    Table<Thumbnail> rectn = aContext.GetTable<Thumbnail>();
                    IQueryable<Thumbnail> restn =
                        from x in rectn
                        select x;

                    Console.WriteLine("=== Thumbnail ===");
                    foreach (Thumbnail data in restn)
                    {
                        Console.WriteLine("=== {0} ===", data.FilePathMD5);
                        Console.WriteLine(data.CreateTime);
                    }
                }
                // データベースを閉じる
                aConnection.Close();

            }


        }
    }
}


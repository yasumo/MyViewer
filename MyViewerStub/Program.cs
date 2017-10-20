﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.SQLite;
using MyViewerLib.LinqTest;
using MyViewerLib;
using System.Data.Linq.Mapping;

namespace MyViewerStub
{
    class Program
    {
        static void Main(string[] args)
        {
            sqltest();
            //foldertest();
            initest();
            Console.ReadLine(); // Enterキー押下でコマンドプロンプトが閉じる

        }

        static void initest()
        {
            Kernel32 hoge = new Kernel32();
            Console.WriteLine(hoge.GetIniValue(@".\settings\settings.ini", "settings", "PictureDirectory"));
        }

        static void foldertest()
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"E:\data\dropbox\Dropbox\pic");
            IEnumerable<System.IO.DirectoryInfo> subFolders =
                di.EnumerateDirectories("*", System.IO.SearchOption.AllDirectories);

            //サブフォルダを列挙する
            foreach (System.IO.DirectoryInfo subFolder in subFolders)
            {
                Console.WriteLine(subFolder.FullName);
            }
        }

        static void sqltest()
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
                    //Tag
                    Table<Tag> rec = aContext.GetTable<Tag>();
                    IQueryable<Tag> res =
                        from x in rec
                        select x;

                    Console.WriteLine("=== Tag ===");
                    foreach (Tag data in res)
                    {
                        Console.WriteLine("=== {0} ===", data.TagId);
                        Console.WriteLine(data.TagName);
                    }

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

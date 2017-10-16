using System;
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
            //sqltest();
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
                DataSource = @"E:\WindowsWorkFiles\Documents\visual studio 2017\Projects\MyViewer\res\LinqTest.sqlite" // データベースの保存先
            };
            using (SQLiteConnection aConnection = new SQLiteConnection(aConnectionString.ToString()))
            {
                // データベースを開く
                aConnection.Open();

                // ここにデータベース処理コードを書く
                using (DataContext aContext = new DataContext(aConnection))
                {
                    Table<TTestData> aTableTest = aContext.GetTable<TTestData>();
                    IQueryable<TTestData> aQueryResult =
                        from x in aTableTest
                        select x;

                    Console.WriteLine("=== test ===");
                    foreach (TTestData aData in aQueryResult)
                    {
                        Console.WriteLine("=== {0} ===", aData.Id);
                        Console.WriteLine(aData.Title);
                        Console.WriteLine(aData.Detail);
                    }

                }
                // データベースを閉じる
                aConnection.Close();



            }
        }
    }
}

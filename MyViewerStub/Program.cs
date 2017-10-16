using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.SQLite;
using MyViewerLib.LinqTest;
using System.Data.Linq.Mapping;

namespace MyViewerStub
{
    class Program
    {
        static void Main(string[] args)
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

                    Console.ReadLine(); // Enterキー押下でコマンドプロンプトが閉じる
                }
                // データベースを閉じる
                aConnection.Close();
            }
        }
    }
}

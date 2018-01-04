using MyViewerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideShow.Models
{
    public class IndexUtils
    {
        //フォルダとタグテーブルに追加するやつ
        private static void InsertFolderTagTable(Dao dao, List<string> folderPathList, List<long> folderIdList)
        {
            if (folderIdList.Count != folderPathList.Count)
            {
                throw new ArgumentException("imageFileNotting:" + folderIdList.Count.ToString() + ":" + folderIdList.Count.ToString());
            }

            var fo = new FolderOperator();

            var inserttagIdList = new List<long>();
            var insertFolderIdList = new List<long>();
            for (int i = 0; i < folderIdList.Count; i++)
            {
                var folderId = folderIdList[i];

                //そのフォルダのタグを分解して
                foreach (var tag in fo.GetTagList(folderPathList[i]))
                {
                    //タグIDを発行してもらい
                    var tagid = dao.SearchOrInsertTagTable(tag);
                    inserttagIdList.Add(tagid);
                    insertFolderIdList.Add(folderId);
                }
                // 一括登録する
                if (insertFolderIdList.Count > 2000)
                {
                    var folderTagIdList = dao.InsertMulutiFolderTagTable(insertFolderIdList, inserttagIdList);
                    inserttagIdList = new List<long>();
                    insertFolderIdList = new List<long>();

                    Console.WriteLine(@"{0}:{1}", folderTagIdList.Count, folderPathList[i]);
                }
            }
            if (inserttagIdList.Count > 0)
            {
                var folderTagIdList = dao.InsertMulutiFolderTagTable(insertFolderIdList, inserttagIdList);
                Console.WriteLine(@"{0}:other", folderTagIdList.Count);
            }

        }

        //フォルダを全部検索してファイル数やらタグやら登録するやつ
        public static void Create()
        {
            FolderOperator.DeleteOldFiles(Settings.GetThumbDir(), 30);
            using (var dao = new Dao(Settings.GetSqliteFilePath()))
            {
                dao.StartTransaction();
                dao.ClearTmpTable();

                var fo = new FolderOperator();

                var folderPathList = new List<string>();
                var folderFileNumList = new List<long>();
                var folderIdList = new List<long>();
                var folderNum = 0;
                //画像のベースフォルダのすべてのフォルダに対して
                foreach (var folderPath in fo.GetAllFolderPathList(Settings.GetPicDir()))
                {
                    //画像ファイル拡張子のファイルの数を数えて
                    var filenum = fo.GetFileNum(folderPath, SlideShowConst.PIC_EXT_LIST);

                    //画像ファイル拡張子のファイルの数が1個以上であれば
                    if (filenum > 0)
                    {
                        //一括登録するやつに追加
                        folderPathList.Add(folderPath);
                        folderFileNumList.Add(filenum);
                        if (folderPathList.Count >= 2000)
                        {
                            Console.WriteLine(folderNum);
                            folderIdList = dao.InsertMulutiFolderTable(folderPathList, folderFileNumList);
                            InsertFolderTagTable(dao, folderPathList, folderIdList);
                            folderPathList = new List<string>();
                            folderFileNumList = new List<long>();
                        }
                    }
                    folderNum++;
                }
                if (folderPathList.Count > 0)
                {
                    folderIdList = dao.InsertMulutiFolderTable(folderPathList, folderFileNumList);
                    InsertFolderTagTable(dao, folderPathList, folderIdList);
                }
                dao.Commit();
            }
        }
    }
}

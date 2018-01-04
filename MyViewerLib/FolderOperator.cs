using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.IO;

namespace MyViewerLib
{
    public class FolderOperator
    {
        //フォルダパスからタグリストを取得するメソッド
        public List<string> GetTagList(string folderPath)
        {
            var ret = new List<string>();

            Regex re = new Regex(@"\[(?<tag>.*?)\]");

            for (Match m = re.Match(folderPath); m.Success; m = m.NextMatch())
            {
                var tag = m.Groups["tag"].Value;
                ret.Add(tag);
            }

            return new List<string>(ret.Distinct());
        }


        //複数フォルダの複数拡張子のファイルを一階層分すべて取得するメソッド
        public IEnumerable<string> GetAllFilePathList(IEnumerable<string> folderList, List<string> ext)
        {
            foreach(var folderPath in folderList)
            {
                var fileList = GetFilePathList(folderPath, ext);
                foreach (var file in fileList)
                {
                    yield return file;
                }
            }
        }

        //単一フォルダの複数拡張子のファイルを一階層分すべて取得するメソッド
        public IEnumerable<string> GetFilePathList(string folderPath, List<string> extList)
        {
            var lowerExt = new List<string>();
            foreach(var ext in extList)
            {
                lowerExt.Add(ext.ToLower());
            }

            return Directory.EnumerateFiles(
                    folderPath, // 検索開始ディレクトリ
                    "*", // 検索パターン
                    SearchOption.TopDirectoryOnly)
                    .Where(path =>lowerExt.Contains(Path.GetExtension(path).ToLower()));

        }

        //単一フォルダの複数拡張子のファイルを一階層分取得して、その数を返すメソッド
        public int GetFileNum(string folderPath, List<string> extList)
        {
            var fileList = GetFilePathList(folderPath, extList);
            return fileList.Count();
        }


        //ベースパス以下にあるフォルダのパスをリストですべて取得するメソッド
        public IEnumerable<string> GetAllFolderPathList(string baseFolderPath)
        {
            return Directory.EnumerateDirectories(
                    baseFolderPath, // 検索開始ディレクトリ
                    "*", // 検索パターン
                    SearchOption.AllDirectories);

        }

        //特定日付より古いファイルを削除する
        public static int DeleteOldFiles(string targetDir, int day)
        {
            if (!Directory.Exists(targetDir))
            {
                return 0;
            }

            int deleteCount = 0;
            //更新日時がday日より以前のファイル
            var targetFiles = Directory.EnumerateFiles(
                    targetDir, // 検索開始ディレクトリ
                    "*", // 検索パターン
                    SearchOption.AllDirectories).Where(f => File.GetLastWriteTime(f) < DateTime.Now.AddDays(-day));
            foreach (var file in targetFiles)
            {
                deleteCount++;
                File.Delete(file);
            }
            return deleteCount;
        }


    }
}

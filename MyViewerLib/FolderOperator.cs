using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            return ret;
        }


        //TODO:一階層分のファイルをすべて取得するメソッド
        public List<string> GetAllFilePathList(List<string> folderList, List<string> ext)
        {
            var ret = new List<string>();

            return ret;
        }

        //TODOベースパスからフォルダのリストをすべて取得するメソッド
        public List<string> GetAllFolderPathList(string baseFolderPath)
        {
            var ret = new List<string>();

            return ret;
        }
        //TODO

    }
}

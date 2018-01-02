using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyViewerLib
{
    public class FolderOperator
    {
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

        public static string GetSqliteFilePathStr(string iniPath)
        {
            KernelUtils kernel = new KernelUtils();
            var targetDir = kernel.GetIniValue(iniPath, "settings", "DBFilePath");
            return targetDir;
        }

        public static string GetPicDirPathStr(string iniPath)
        {
            KernelUtils kernel = new KernelUtils();
            var targetDir = kernel.GetIniValue(iniPath, "settings", "PictureDirectory");
            return targetDir;
        }

        //TODO
        public List<string> GetFileList(List<string> folderList ,List<string> ext)
        {
            var ret = new List<string>();

            return ret;
        }
    }
}

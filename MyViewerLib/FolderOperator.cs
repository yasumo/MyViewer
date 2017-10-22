using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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


        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(

        string lpApplicationName,
        string lpKeyName,
        string lpDefault,
        StringBuilder lpReturnedstring,
        int nSize,
        string lpFileName);

        public string GetIniValue(string path, string section, string key)
        {
            StringBuilder sb = new StringBuilder(256);
            GetPrivateProfileString(section, key, string.Empty, sb, sb.Capacity, path);
            return sb.ToString();
        }

        //TODO
        public List<string> GetFileList(List<string> folderList ,List<string> ext)
        {
            var ret = new List<string>();

            return ret;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyViewerLib
{
    public class SettingUtils
    {
        public static string GetSqliteFilePath(string iniPath)
        {
            KernelUtils kernel = new KernelUtils();
            var targetDir = kernel.GetIniValue(iniPath, "settings", "DBFilePath");
            return targetDir;
        }

        public static string GetPicDirPath(string iniPath)
        {
            KernelUtils kernel = new KernelUtils();
            var targetDir = kernel.GetIniValue(iniPath, "settings", "PictureDirectory");
            return targetDir;
        }

    }
}

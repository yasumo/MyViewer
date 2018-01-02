using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyViewerLib;

namespace MyVierLibTest
{
    public class TestUtils
    {
        public static string IniPath = @".\test\settings\settings.ini";

        public static string GetSqliteFilePath()
        {
            var targetDir = SettingUtils.GetSqliteFilePathStr(IniPath);
            return targetDir;
        }

        public static string GetPicDir()
        {
            var targetDir = SettingUtils.GetPicDirPathStr(IniPath);
            return targetDir;
        }
    }
}

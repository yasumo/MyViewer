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
        public static string IniPathRead = @".\test\settings\settings_read.ini";
        public static string IniPathWrite = @".\test\settings\settings_write.ini";

        public static string GetReadSqliteFilePath()
        {
            var targetDir = SettingUtils.GetSqliteFilePath(IniPathRead);
            return targetDir;
        }

        public static string GetWriteSqliteFilePath()
        {
            var targetDir = SettingUtils.GetSqliteFilePath(IniPathWrite);
            return targetDir;
        }

        public static string GetPicDir()
        {
            var targetDir = SettingUtils.GetPicDirPath(IniPathRead);
            return targetDir;
        }
    }
}

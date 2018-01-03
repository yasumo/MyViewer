using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyViewerLib;

namespace SlideShow.Models
{
    public class Settings
    {
        public static string IniPath = @".\Resources\settings.ini";

        public static string GetSqliteFilePath()
        {
            var targetDir = SettingUtils.GetSqliteFilePath(IniPath);
            return targetDir;
        }


        public static string GetPicDir()
        {
            var targetDir = SettingUtils.GetPicDirPath(IniPath);
            return targetDir;
        }

        public static string GetThumbDir()
        {
            var targetDir = SettingUtils.GetThumbDirPath(IniPath);
            return targetDir;
        }
    }
}

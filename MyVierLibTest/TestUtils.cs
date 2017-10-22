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
            FolderOperator kernel = new FolderOperator();
            var targetDir = kernel.GetIniValue(IniPath, "settings", "DBFilePath");
            return targetDir;
        }

        public static string GetPicDir()
        {
            FolderOperator kernel = new FolderOperator();
            var targetDir = kernel.GetIniValue(TestUtils.IniPath, "settings", "PictureDirectory");
            return targetDir;
        }
    }
}

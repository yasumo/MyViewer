using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyViewerLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MyVierLibTest
{
    [TestClass]
    public class ImageTest
    {
        [TestMethod]
        public void ThumbnailPathTest()
        {
            var picDir = TestUtils.GetPicDir();
            var thumbnailDir = TestUtils.GetThumbDir();

            var io = new ImageOperator(picDir,thumbnailDir);
            var (folderPath,fileName) = io.CreateThumbnailPath(@"E:\aaa\bbb\[aaa][bbb]\tehos.jpg");
            var thumbnailFilePath = folderPath + Path.DirectorySeparatorChar + fileName;
            Assert.AreEqual("E:\\WindowsWorkFiles\\Documents\\visual studio 2017\\Projects\\MyViewer\\MyVierLibTest\\bin\\Debug\\thumbnailtest\\bb\\bb18be855d0e7bdddcbf557645307638.jpg", thumbnailFilePath);
            
        }

        [TestMethod]
        public void CreateThumbnailTest()
        {
            var picDir = TestUtils.GetPicDir();
            var thumbnailDir = TestUtils.GetThumbDir();
            var io = new ImageOperator(picDir, thumbnailDir);

            var fo = new FolderOperator();
            List<string> ext = new List<string> { ".jpg", ".png" };
            var filePathList = fo.GetAllFilePathList(fo.GetAllFolderPathList(picDir), ext);

            var thumbnailList = io.CreateAllThumbnail(filePathList);
            foreach (var thumbnail in thumbnailList)
            {
                Console.WriteLine(thumbnail);
            }

        }
    }
}

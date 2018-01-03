using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyViewerLib;
using System.Collections.Generic;
using System.Diagnostics;

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
            var thumbnailFilePath = io.CreateThumbnailPath(@"E:\aaa\bbb\[aaa][bbb]\tehos.jpg");
            Assert.AreEqual("E:\\WindowsWorkFiles\\Documents\\visual studio 2017\\Projects\\MyViewer\\MyVierLibTest\\bin\\Debug\\thumbnailtest\\bb18be855d0e7bdddcbf557645307638.jpg", thumbnailFilePath);
            
        }
    }
}

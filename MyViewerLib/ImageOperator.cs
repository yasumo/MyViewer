using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;


namespace MyViewerLib
{
    public class ImageOperator
    {
        string thumbnailBaseDir;
        public ImageOperator(string thumbnailBaseDir)
        {
            this.thumbnailBaseDir = thumbnailBaseDir;
        }

        public Bitmap GetThumbnail(string imageFilePath)
        {
            var thumbnailFilePath = CreateThumbnailPath(imageFilePath);
            Bitmap bmp;

            if (File.Exists(thumbnailFilePath))
            {
                bmp = (Bitmap)Image.FromFile(imageFilePath);
            }
            else
            {
                bmp = CreateThumbnail(imageFilePath);
            }
            return bmp;
        }

        //TODO:画像ファイルを受け取ってサムネイルを作るやつ
        public Bitmap CreateThumbnail(string imageFilePath)
        {
            if (File.Exists(imageFilePath))
            {
                throw new ArgumentException("imageFileNotting:"+imageFilePath);
            }

            Bitmap img = new Bitmap(200, 100);
            return img;

        }

        //古いサムネイルを削除する
        public void DeleteOldThumbnail()
        {
            FolderOperator.DeleteOldFiles(this.thumbnailBaseDir,30);
        }

        //TODO:ファイルパスからサムネイルのパスを作成
        public string CreateThumbnailPath(string filePath)
        {
            return "hoge";
        }


    }
}

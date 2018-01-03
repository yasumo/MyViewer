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
        string picBaseDir;
        string thumbnailBaseDir;

        public ImageOperator(string picBaseDir,string thumbnailBaseDir)
        {
            this.picBaseDir = picBaseDir;
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

        //ファイルパスからサムネイルのパスを作成
        public string CreateThumbnailPath(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            var fileName = CalcMd5(filePath) + ext;
            var retFilePath = this.thumbnailBaseDir + Path.DirectorySeparatorChar + fileName;
            return retFilePath;
        }

        private string CalcMd5(string srcStr)
        {

            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

            // md5ハッシュ値を求める
            byte[] srcBytes = System.Text.Encoding.UTF8.GetBytes(srcStr);
            byte[] destBytes = md5.ComputeHash(srcBytes);

            // 求めたmd5値を文字列に変換する
            System.Text.StringBuilder destStrBuilder;
            destStrBuilder = new System.Text.StringBuilder();
            foreach (byte curByte in destBytes)
            {
                destStrBuilder.Append(curByte.ToString("x2"));
            }

            // 変換後の文字列を返す
            return destStrBuilder.ToString();
        }
    }
}

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
            if (!Directory.Exists(thumbnailBaseDir))
            {
                Directory.CreateDirectory(thumbnailBaseDir);
            }
        }

        //サムネイルが無ければ作成し返す、あればそのまま返す
        public string GetThumbnail(string imageFilePath)
        {
            var (folderPath, fileName) = CreateThumbnailPath(imageFilePath);
            var thumbnailFilePath = folderPath + Path.DirectorySeparatorChar + fileName;

            if (!File.Exists(thumbnailFilePath))
            {
                Image img;
                img = CreateThumbnail(imageFilePath);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                img.Save(thumbnailFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                img.Dispose();
            }
           
            return thumbnailFilePath;
        }



        //画像ファイルを受け取ってサムネイルを作るやつ
        public Image CreateThumbnail(string imageFilePath)
        {
            if (!File.Exists(imageFilePath))
            {
                throw new ArgumentException("imageFileNotting:"+imageFilePath);
            }
            // 画像オブジェクトの作成
            Image orig = Image.FromFile(imageFilePath);

            // サムネイルの作成
            //TODO:アス比固定縮小
            Image thumbnail = orig.GetThumbnailImage(
              100, 100, delegate { return false; }, IntPtr.Zero);
            orig.Dispose();

            return thumbnail;

        }

        //ファイルパスをサムネイルにして全部返す
        public IEnumerable<string> CreateAllThumbnail(IEnumerable<string> imageFilePathList)
        {
            foreach(var filePath in imageFilePathList)
            {
                yield return GetThumbnail(filePath);
            }
        }

        //古いサムネイルを削除する
        public void DeleteOldThumbnail()
        {
            FolderOperator.DeleteOldFiles(this.thumbnailBaseDir,30);
        }

        //ファイルパスからサムネイルのパスを作成
        public (string folderPath,string fileName) CreateThumbnailPath(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            var fileName = CalcMd5(filePath) + ext;
            var folderPath = this.thumbnailBaseDir + Path.DirectorySeparatorChar　+ fileName.Substring(0,2) ;
            return (folderPath, fileName);
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

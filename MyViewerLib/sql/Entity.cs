using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace MyViewerLib
{


    [Table(Name = "TAG")]
    public class Tag
    {
        //TagId
        [Column(Name = "TAG_ID", DbType = "INT", CanBeNull = false, IsPrimaryKey = true)]
        public Int64 TagId { get; set; }

        // TagName
        [Column(Name = "TAG_NAME", DbType = "NVARCHAR", CanBeNull = false)]
        public String TagName { get; set; }

    }

    [Table(Name = "FOLDER_TAG")]
    public class FolderTag
    {
        //FolderTagId
        [Column(Name = "FOLDER_TAG_ID", DbType = "INT", CanBeNull = false, IsPrimaryKey = true)]
        public Int64 FolderTagId { get; set; }

        // TagId
        [Column(Name = "TAG_ID", DbType = "INT", CanBeNull = false)]
        public Int64 TagId { get; set; }

        // FolderId
        [Column(Name = "FOLDER_ID", DbType = "INT", CanBeNull = false)]
        public Int64 FolderId { get; set; }

    }

    [Table(Name = "FOLDER")]
    public class Folder
    {
        //FolderId
        [Column(Name = "FOLDER_ID", DbType = "INT", CanBeNull = false, IsPrimaryKey = true)]
        public Int64 FolderId { get; set; }

        // FolderPath
        [Column(Name = "FOLDER_PATH", DbType = "NVARCHAR", CanBeNull = false)]
        public String FolderPath { get; set; }

        // InsideFileNum
        [Column(Name = "INSIDE_FILE_NUM", DbType = "INT", CanBeNull = false)]
        public Int64? InsideFileNum { get; set; }

    }


    [Table(Name = "THUMBNAIL")]
    public class Thumbnail
    {
        // FilePathMD5
        [Column(Name = "FILE_PATH_MD5", DbType = "NVARCHAR", CanBeNull = false)]
        public String FilePathMD5 { get; set; }

        // CreateTime
        [Column(Name = "CREATE_TIME", DbType = "DATETIME", CanBeNull = false)]
        public DateTime CreateTime { get; set; }

    }




}

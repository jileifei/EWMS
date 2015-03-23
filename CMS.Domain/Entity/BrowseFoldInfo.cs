using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Domain
{
    [Serializable]
    public class BrowseFoldInfo
    {
        #region ID
        private Int32 id;

        public Int32 ID {
            get { return id;}
            set { id = value; }
        }
        #endregion

        #region ExtName
        private string f_extName;
        public string ExtName
        {
            get { return f_extName; }
            set { f_extName = value; }
        }
        #endregion

        #region FileType
        private Int32 f_fileType;
        public Int32 FileType
        {
            get { return f_fileType; }
            set { f_fileType = value; }
        }
        #endregion

        #region Filename

        private string f_filename;

        /// <summary>Gets or sets f_filename</summary>
        public string Filename
        {
            get { return f_filename; }
            set { f_filename = value; }
        }
        #endregion

        #region LocalPath

        private string f_filePath;

        /// <summary>Gets or sets f_filePath</summary>
        public string LocalPath
        {
            get { return f_filePath; }
            set { f_filePath = value; }
        }
        #endregion

        #region Filesize
        private string f_fileSize;
        public string Filesize
        { 
            get { return f_fileSize; }
            set { f_fileSize = value; }
        }
        #endregion

        #region FileURL
        private string f_fileURL;
        public string FileURL
        {
            get { return f_fileURL; }
            set { f_fileURL = value; }
        }
        #endregion

        #region UploadDate
        private DateTime f_uploadDate;
        public DateTime UploadDate
        {
            get { return f_uploadDate; }
            set { f_uploadDate = value; }
        }
        #endregion

        #region PublicTime
        private DateTime f_publictime;
        public DateTime PublicTime
        {
            get { return f_publictime; }
            set { f_publictime = value; }
        }
        #endregion
    }
}

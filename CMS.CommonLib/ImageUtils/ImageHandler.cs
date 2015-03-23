using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data;
using System.IO;
using System.Threading;
using System.Collections;

namespace CMS.CommonLib.ImageUtils
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class ImageHandler
    {
        private static InterpolationMode intmode = InterpolationMode.HighQualityBicubic;
        /// <summary>
        /// 允许的图片文件扩展名
        /// </summary>
        static internal readonly string AllowExt = ".(jpg|jpeg|gif|bmp|png|tif|tiff)$";

        /// <summary>
        /// 水印位置
        /// </summary>
        enum AnchorPosition
        {
            Top,
            Center,
            Bottom,
            Left,
            Right
        }

        /// <summary>
        /// 生成指定宽度的图形
        /// </summary>
        /// <param name="origFileName">原文件保存路径,生成的图片会保存在该图片的路径下</param>
        /// <param name="origImage">原图片信息,为null则自动从原文件保存路径中获取</param>
        /// <param name="Size">要生成的图片的宽度</param>
        /// <param name="type">图片尺寸类型</param>
        /// <returns></returns>
        public static bool GenImage(string origFileName, Image origImage, int Size, string type)
        {
            string saveFile = origFileName;

            if (origFileName == null)
            {
                throw new Exception("原始文件路径不能为空!");
            }
            string exName = origFileName.Substring(origFileName.LastIndexOf(".")).ToLower();
            saveFile = System.IO.Path.GetDirectoryName(origFileName) + "\\" + "@size_" + System.IO.Path.GetFileNameWithoutExtension(origFileName) + exName;
            switch (type)
            {
                case "t":
                    {
                        saveFile = saveFile.Replace("@size", "t");
                        
                        System.Drawing.Image imgPhoto = null;

                        if (origImage.Width > Size || origImage.Height > Size)
                        {
                            imgPhoto = CropPhoto(origImage, 75, 75, saveFile);
                            imgPhoto.Dispose();
                            return true;
                        }

                        break;
                    }

                case "s":
                    {
                        saveFile = saveFile.Replace("@size", "s");

                        System.Drawing.Image imgPhoto = null;

                        if (origImage.Width > Size || origImage.Height > Size)
                        {
                            imgPhoto = CropPhoto(origImage, 100, 100, saveFile);
                            imgPhoto.Dispose();
                            return true;
                        }
                    }
                    break;

                case "m":
                    saveFile = saveFile.Replace("@size", "m");
                    break;

                case "b":
                    saveFile = saveFile.Replace("@size", "b");
                    break;
            }

            Image thumbImage = null;

            try
            {
                if (origImage == null)
                {
                    if (File.Exists(origFileName))
                    {
                        using (Image img = Image.FromFile(origFileName))
                        {
                            thumbImage = ThumbnaiImage(img, Size, Convert.ToInt32((Convert.ToSingle(img.Height) / Convert.ToSingle(img.Width) * Size)));
                        }
                    }
                }
                else
                {
                    thumbImage = ThumbnaiImage(origImage, Size, Convert.ToInt32((Convert.ToSingle(origImage.Height) / Convert.ToSingle(origImage.Width) * Size)));
                }

                Bitmap originale = new Bitmap(thumbImage);
                save(ref originale, saveFile, 50);
                originale.Dispose();
                thumbImage.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (thumbImage != null)
                    thumbImage.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 制作缩略图,返回被缩略的图像,按照宽度做缩略
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="dstWidth"></param>
        /// <param name="dstHeight"></param>
        /// <returns></returns>
        static Image ThumbnaiImage(Image img, int dstWidth, int dstHeight)
        {
            if (img == null) return null;
            int origHeight = dstHeight;
            Bitmap tempImage = null;
            Image retImage = null;
            try
            {
                tempImage = new Bitmap(img.Width, img.Height);
                using (Graphics g = Graphics.FromImage(tempImage))
                {
                    g.DrawImage(img, 0, 0, img.Width, img.Height);
                    retImage = tempImage.GetThumbnailImage(dstWidth, dstHeight, null, IntPtr.Zero);
                }
                tempImage.Dispose();
                tempImage = new Bitmap(dstWidth, origHeight);
                using (Graphics g = Graphics.FromImage(tempImage))
                {
                    g.Clear(Color.Transparent);
                    g.DrawImage(retImage, 0, 0, retImage.Width, retImage.Height);
                    retImage.Dispose();
                    retImage = null;
                    retImage = tempImage;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retImage;
        }

        //******************************************************************************************************//

        #region 新的截图

        /// <summary>
        /// 处理图片
        /// </summary>
        /// <param name="photoPhysicsPath">图片物理路径</param>
        /// <param name="width"> 宽度 </param>
        /// <param name="targetfilename">保存的物理路径</param>
        /// <returns></returns>
        public static void CropPhoto(string photoPhysicsPath, int width, string targetfilename)
        {
            System.Drawing.Image originPhoto = System.Drawing.Image.FromFile(photoPhysicsPath);
            originPhoto = ThumbnaiImage(originPhoto, width, Convert.ToInt32((Convert.ToSingle(originPhoto.Height) / Convert.ToSingle(originPhoto.Width) * width)));
            Bitmap originale = new Bitmap(originPhoto);
            save(ref originale, targetfilename, 85);
        }

        public static System.Drawing.Image CropPhoto(System.Drawing.Image imgPhoto, int width, int height, string targetfilename)
        {
            imgPhoto = Crop(ref imgPhoto, width, height, AnchorPosition.Center);
            Bitmap originale = new Bitmap(imgPhoto);
            save(ref originale, targetfilename, 85);
            return originale;

        }

        # region 剪切图片

        static System.Drawing.Image Crop(ref System.Drawing.Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                switch (Anchor)
                {
                    case AnchorPosition.Top:
                        destY = 0;
                        break;
                    case AnchorPosition.Bottom:
                        destY = (int)(Height - (sourceHeight * nPercent));
                        break;
                    default:
                        destY = (int)((Height - (sourceHeight * nPercent)) / 2);
                        break;
                }
            }
            else
            {
                nPercent = nPercentH;
                switch (Anchor)
                {
                    case AnchorPosition.Left:
                        destX = 0;
                        break;
                    case AnchorPosition.Right:
                        destX = (int)(Width - (sourceWidth * nPercent));
                        break;
                    default:
                        destX = (int)((Width - (sourceWidth * nPercent)) / 2);
                        break;
                }
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        #endregion

        //重新截图
        public static void Resize(ref Bitmap imgPhoto, int destWidth, int destHeight, int quality)
        {
            resizemode(quality);
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            if (sourceWidth == destWidth && sourceHeight == destHeight) return; //quit if the resize is not required
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;
            // Creates a new bitmap with the same propertyes of the original one
            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            // Set the resolution that are equal to the original image
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            // Creates a new graphic object from the new bitmap (that is actually "blank")
            Graphics imgGraphic = Graphics.FromImage(bmPhoto);
            // Set the interpolation mode (es Hight quality bicubic)
            imgGraphic.InterpolationMode = intmode;
            // Draws the original image into the new "blank" image but with the target size and without cropping
            imgGraphic.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);
            // Dispose the graphic object
            imgGraphic.Dispose();
            // Assign the new bitmap on the old one
            imgPhoto.Dispose();
            imgPhoto = bmPhoto;
        }

        public static void resizemode(int indice)
        {
            switch (indice)
            {
                case 0:
                    intmode = InterpolationMode.HighQualityBicubic;
                    break;
                case 1:
                    intmode = InterpolationMode.Bicubic;
                    break;
                case 2:
                    intmode = InterpolationMode.High;
                    break;
                case 3:
                    intmode = InterpolationMode.HighQualityBilinear;
                    break;
                case 4:
                    intmode = InterpolationMode.Bilinear;
                    break;
                case 5:
                    intmode = InterpolationMode.Low;
                    break;
                case 6:
                    intmode = InterpolationMode.NearestNeighbor;
                    break;
                default:
                    intmode = InterpolationMode.HighQualityBicubic;
                    break;
            }
        }

        public static string getextension(string filename)
        {
            string extension = "";
            int lun = filename.Length;
            int poi = filename.LastIndexOf(".") + 1;
            extension = filename.Substring(poi, lun - poi).ToLower();
            return extension;
        }

        private static void save(ref Bitmap tosave, string targetfilename, int quality)
        {
            switch (getextension(targetfilename).ToLower())
            {
                case "gif":
                    tosave.Save(targetfilename, ImageFormat.Gif);
                    break;
                case "bmp":
                    tosave.Save(targetfilename, ImageFormat.Bmp);
                    break;
                case "exif":
                    tosave.Save(targetfilename, ImageFormat.Exif);
                    break;
                case "ico":
                    tosave.Save(targetfilename, ImageFormat.Icon);
                    break;
                case "png":
                    tosave.Save(targetfilename, ImageFormat.Png);
                    break;
                case "tiff":
                    tosave.Save(targetfilename, ImageFormat.Tiff);
                    break;
                case "wmf":
                    tosave.Save(targetfilename, ImageFormat.Wmf);
                    break;
                default:
                    saveimagecodeinfo(ref tosave, targetfilename, quality);
                    break;
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static void saveimagecodeinfo(ref Bitmap imgr, string filen, int qual)
        {
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            myImageCodecInfo = GetEncoderInfo("image/jpeg");
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, (long)qual);
            myEncoderParameters.Param[0] = myEncoderParameter;
            imgr.Save(filen, myImageCodecInfo, myEncoderParameters);
            myEncoderParameters.Dispose();
            myEncoderParameter.Dispose();
        }

        #endregion

        //******************************************************************************************************//


        /// <summary>
        /// 检测扩展名的有效性
        /// </summary>
        /// <param name="sExt">文件名扩展名</param>
        /// <returns>如果扩展名有效,返回true,否则返回false.</returns>
        public static bool CheckValidExt(string sExt)
        {

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(AllowExt, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return regex.IsMatch(sExt);
        }

        public static void GenImagePointSize(string url, int width, string type)
        {
            if (!CheckValidExt(url))
            {
                return;
            }
            if (!File.Exists(url))
            {
                return;
            }

            using (Image img = Image.FromFile(url))
            {
                if (img.Width > width)
                {
                    GenImage(url, img, width, type);
                }
                else
                {
                    GenImage(url, img, img.Width, type);
                }
            }
        }

        public static void InsertPhoto(Hashtable files, string title, string blogurl)
        {
            string url = "";
            string asize = string.Empty;
            foreach (int t in files.Keys)
            {
                url = files[t].ToString();
                if (!CheckValidExt(url))
                {
                    break;
                }
                if (!File.Exists(url))
                {
                    break;
                }

                using (Image img = Image.FromFile(url))
                {
                    if (img.Width > 75)
                    {
                        GenImage(url, img, 75, "t");
                    }
                    else
                    {
                        GenImage(url, img, img.Width, "t");
                    }


                    if (img.Width > 100)
                    {
                        GenImage(url, img, 100, "s");
                    }
                    else
                    {
                        GenImage(url, img, img.Width, "s");
                    }

                    if (img.Width > 240)
                    {
                        GenImage(url, img, 240, "m");
                    }
                    else
                    {
                        GenImage(url, img, img.Width, "m");
                    }

                    if (img.Width > 500)
                    {
                        GenImage(url, img, 500, "b");
                    }
                    else
                    {

                        GenImage(url, img, img.Width, "b");
                    }

                }
            }
        }

        # region 调用工具优化图片大小


        public static void ThreadOptJpgImg(string imagePath)
        {
            try
            {
                Thread imgOptThread = new Thread(OptJpgImg);
                imgOptThread.Start(imagePath);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 调用工具优化图片大小
        /// </summary>
        /// <param name="imagePath"></param>
        public static void OptJpgImg(object imagePath)
        {
            try
            {
                string exefile = ConfigurationManager.AppSettings["JpegoptimPath"] + "jpegoptim.exe";
                if (File.Exists(exefile))
                {
                    string fileExt = Path.GetExtension(imagePath.ToString()).ToLower();
                    if (fileExt == ".jpg" || fileExt == ".jpeg")
                    {
                        System.Diagnostics.Process exep = new System.Diagnostics.Process();
                        exep.StartInfo.FileName = exefile;
                        exep.StartInfo.Arguments = "--strip-com --strip-exif --strip-iptc -m85 " + imagePath.ToString();
                        exep.StartInfo.WorkingDirectory = ConfigurationManager.AppSettings["JpegoptimPath"];
                        exep.StartInfo.UseShellExecute = false;
                        exep.StartInfo.CreateNoWindow = true;
                        exep.StartInfo.UseShellExecute = false;
                        exep.StartInfo.RedirectStandardInput = true;
                        exep.StartInfo.RedirectStandardOutput = true;
                        exep.StartInfo.RedirectStandardError = true;
                        exep.StartInfo.CreateNoWindow = false;
                        //开始执行
                        exep.Start();
                        exep.BeginErrorReadLine();
                        exep.WaitForExit();
                        exep.Close();
                        exep.Dispose();
                    }
                }
            }
            catch
            {
            }
        }

        # endregion

        public static void InsertPhoto(Hashtable files)
        {
            string url = "";
            string asize = string.Empty;
            foreach (int t in files.Keys)
            {
                url = files[t].ToString();
                if (!CheckValidExt(url))
                {
                    break;
                }
                if (!File.Exists(url))
                {
                    break;
                }

                using (Image img = Image.FromFile(url))
                {
                    if (img.Width > 75)
                    {
                        GenImage(url, img, 75, "t");
                    }
                    else
                    {
                        GenImage(url, img, img.Width, "t");
                    }


                    if (img.Width > 100)
                    {
                        GenImage(url, img, 100, "s");
                    }
                    else
                    {
                        GenImage(url, img, img.Width, "s");
                    }

                    if (img.Width > 240)
                    {
                        GenImage(url, img, 240, "m");
                    }
                    else
                    {
                        GenImage(url, img, img.Width, "m");
                    }

                    if (img.Width > 500)
                    {
                        GenImage(url, img, 500, "b");
                    }
                    else
                    {

                        GenImage(url, img, img.Width, "b");
                    }
                }
            }
        }
    }
}

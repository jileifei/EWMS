using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

namespace CMS.AdminUI.Controllers
{
    public class UploadController : Controller
    {
        public ActionResult Index()
        {
            # region 得到参数

            int waterMarkType;
            Int32.TryParse(Request["WaterMark"], out waterMarkType);// 得到水印类型

            string waterMarkPosition = Request["WaterMarkPosition"];// 水印位置

            bool thumHasWaterMark;
            Boolean.TryParse(Request["ThumHasWaterMark"], out thumHasWaterMark);

            int thumType;
            Int32.TryParse(Request["ThumType"], out thumType);// 得到缩略图处理方式

            string thumSize = Request["ThumSize"];// 得到缩略图尺寸

            # endregion

            // 上传
            string url = Upload(waterMarkType, thumHasWaterMark, waterMarkPosition, thumType, thumSize);
            return !string.IsNullOrEmpty(url) ? Content("{ \"result\":\"true\",\"url\":\"" + url + "\"}") : Content("{ \"result\":\"false\",\"url\":\"" + url + "\"}");
        }

        /// <summary>
        /// 执行上传
        /// </summary>
        /// <param name="warterMarkType">水印类型:0=不添加水印 1=文字水印 2=图片水印</param>
        /// <param name="thumWaterMark">缩略图添加水印</param>
        /// <param name="waterMarkPosition">水印位置</param>
        /// <param name="thumType">缩略图处理类型</param>
        /// <param name="thumSizes">缩略图尺寸</param>
        private string Upload(int warterMarkType, bool thumWaterMark, string waterMarkPosition, int thumType, string thumSizes)
        {
            string returnImgUrl = "";
            // Get the data
            HttpPostedFileBase fileUploade = Request.Files["Filedata"];
            if (fileUploade==null)
            {
                return string.Empty;
            }
            #region 上传

            if (!string.IsNullOrEmpty(fileUploade.FileName))
            {
                //生成文件名，建立文件目录
                Stream upimgfile = fileUploade.InputStream;
                var upimage = Image.FromStream(upimgfile);

                string filedir = ConfigurationManager.AppSettings["SavePath"];// 保存路径
                string extension = Path.GetExtension(fileUploade.FileName).ToLower();// 扩展名
                string filename = Guid.NewGuid().ToString() + extension;// 新的文件名
                string filedirs = Path.Combine(filedir, DateTime.Now.ToString("yyyyMMdd"));// 保存目录

                if (!Directory.Exists(filedirs))
                {
                    Directory.CreateDirectory(filedirs);
                }

                // 如果设置了缩略图
                if (thumType > 0)
                {
                    string sfile = "_s";
                    string filenameS1 = Path.Combine(filedirs, filename + sfile + extension);
                    GetConditions(upimage, thumSizes.Trim(), thumType, filenameS1, thumWaterMark, warterMarkType, waterMarkPosition);
                }

                // 如果设置了水印
                if (warterMarkType > 0)
                {
                    # region 水印处理

                    Bitmap b = new Bitmap(upimage.Width, upimage.Height, PixelFormat.Format24bppRgb);
                    Graphics g = Graphics.FromImage(b);
                    g.Clear(Color.White);
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.High;
                    g.DrawImage(upimage, 0, 0, upimage.Width, upimage.Height);
                    if (warterMarkType == 1)// 文字水印
                    {
                        addWatermarkText(g, ConfigurationManager.AppSettings["WarterMarkText"], ConfigurationManager.AppSettings["WarterMarkTextUrl"], waterMarkPosition, upimage.Width, upimage.Height);
                    }
                    else// 图片水印
                    {
                        addWatermarkImage(g, Server.MapPath(ConfigurationManager.AppSettings["WaterMarkLogo"]), waterMarkPosition, upimage.Width, upimage.Height);
                    }
                    b.Save(Path.Combine(filedirs, filename));
                    b.Dispose();
                    upimage.Dispose();
                    upimgfile.Dispose();

                    # endregion
                }
                else
                {
                    fileUploade.SaveAs(Path.Combine(filedirs, filename));
                    upimage.Dispose();
                    upimgfile.Dispose();
                }

                string furl = Path.Combine(filedirs, filename).Replace(filedir, "");

                returnImgUrl = ConfigurationManager.AppSettings["ImgDomain"] + furl.Replace("\\", "/");
            }

            #endregion

            return returnImgUrl;
        }

        # region 生成缩略图

        # region 计算小图尺寸生成小图

        /// <summary>
        /// 计算小图尺寸生成小图
        /// </summary>
        /// <param name="upimage">上传图片</param>
        /// <param name="thumSizes">缩略图值</param>
        /// <param name="thumType">缩略图处理方式</param>
        /// <param name="filenameS1">文件名</param>
        /// <param name="thumWaterMark">是否添加水印</param>
        /// <param name="warterMarkType">水印类型 1=文字水印 2=图片水印</param>
        /// <param name="waterMarkPosition">水印位置</param>
        private void GetConditions(Image upimage, string thumSizes, int thumType, string filenameS1, bool thumWaterMark, int warterMarkType, string waterMarkPosition)
        {
            int s1H = 0;
            int s1W = 0;

            switch (thumType)
            {
                case 1: // 自定义尺寸
                    string[] hws = thumSizes.ToLower().Split('*');
                    if (hws.Length == 2)
                    {
                        s1W = Convert.ToInt32(hws[0]);
                        s1H = Convert.ToInt32(hws[1]);
                    }
                    else
                    {
                        throw new Exception("自定数据有问题！");
                    }
                    break;
                case 2:// 指定最长边
                    if (upimage.Width > upimage.Height)
                    {
                        s1W = Convert.ToInt32(thumSizes);
                        s1H = (int)((double)upimage.Height / upimage.Width * s1W);
                    }
                    else
                    {
                        s1H = Convert.ToInt32(thumSizes);
                        s1W = (int)((double)upimage.Width / upimage.Height * s1H);
                    }
                    break;
                case 3:// 指定最短边
                    if (upimage.Width > upimage.Height)
                    {
                        s1H = Convert.ToInt32(thumSizes);
                        s1W = (int)((double)upimage.Width / upimage.Height * s1H);
                    }
                    else
                    {
                        s1W = Convert.ToInt32(thumSizes);
                        s1H = (int)((double)upimage.Height / upimage.Width * s1W);
                    }
                    break;
                case 4:// 指定宽度
                    s1W = Convert.ToInt32(thumSizes);
                    s1H = (int)((double)upimage.Height / upimage.Width * s1W);
                    break;
                case 5:// 指定高度
                    s1H = Convert.ToInt32(thumSizes);
                    s1W = (int)((double)upimage.Width / upimage.Height * s1H);
                    break;
            }

            MakeThumbnail(upimage, filenameS1, s1W, s1H, "HW", thumWaterMark, warterMarkType, waterMarkPosition);
        }

        # endregion

        #region 生成缩略小图

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="uploadImage">源图</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的形式(HW,指定高宽缩放（可能变形）; W,指定宽，高按比例; H,指定高，宽按比例; Cut,指定高宽裁减(不变形);)</param>
        /// <param name="thumWaterMark">是否添加水印</param>
        /// <param name="warterMarkType">水印类型:1:文字水印 2：图片水印</param>
        /// <param name="waterMarkPosition">水印位置</param>
        private void MakeThumbnail(Image uploadImage, string thumbnailPath, int width, int height, string mode, bool thumWaterMark, int warterMarkType, string waterMarkPosition)
        {
            Image originalImage = uploadImage;

            int towidth = width;
            int toheight = height;

            switch (mode.ToUpper())
            {
                case "HW"://指定高宽缩放（可能变形） 
                    break;
                case "W"://指定宽，高按比例 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形） 
                    if ((double)originalImage.Width / originalImage.Height > (double)towidth / toheight)
                    {
                    }
                    break;
            }
            Image retImage;
            Bitmap tempImage = new Bitmap(originalImage.Width, originalImage.Height);
            using (Graphics g = Graphics.FromImage(tempImage))
            {
                //设置高质量插值法
                g.InterpolationMode = InterpolationMode.High;

                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);
                g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);
                retImage = tempImage.GetThumbnailImage(towidth, toheight, null, IntPtr.Zero);
            }
            tempImage.Dispose();
            tempImage = new Bitmap(towidth, toheight);
            using (Graphics g = Graphics.FromImage(tempImage))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(retImage, 0, 0, retImage.Width, retImage.Height);
                retImage.Dispose();
                retImage = null;
                retImage = tempImage;
            }

            try
            {
                if (thumWaterMark)
                {
                    # region 水印处理

                    Bitmap b = new Bitmap(tempImage.Width, tempImage.Height, PixelFormat.Format24bppRgb);
                    Graphics g = Graphics.FromImage(b);
                    g.Clear(Color.White);
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.High;
                    g.DrawImage(tempImage, 0, 0, tempImage.Width, tempImage.Height);
                    if (warterMarkType == 1)// 文字水印
                    {                     
                        addWatermarkText(g, ConfigurationManager.AppSettings["WarterMarkText"], ConfigurationManager.AppSettings["WarterMarkTextUrl"], waterMarkPosition, tempImage.Width, tempImage.Height);
                    }
                    else// 图片水印
                    {
                        addWatermarkImage(g, Server.MapPath(ConfigurationManager.AppSettings["WaterMarkLogo"]), waterMarkPosition, tempImage.Width, tempImage.Height);
                    }
                    b.Save(thumbnailPath);
                    b.Dispose();

                    # endregion
                }
                else
                {
                    //以JPG格式保存缩略图片
                    tempImage.Save(thumbnailPath, ImageFormat.Jpeg);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                tempImage.Dispose();
            }
        }

        #endregion

        # endregion

        #region 添加水印

        #region 文字水印

        /// <summary>
        /// 加水印文字
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="watermarkText">水印文字内容</param>
        /// <param name="watermarkUrl">水印文字Url</param>
        /// <param name="watermarkPosition">水印位置</param>
        /// <param name="width">被加水印图片的宽</param>
        /// <param name="height">被加水印图片的高</param>
        private void addWatermarkText(Graphics picture, string watermarkText, string watermarkUrl, string watermarkPosition, int width, int height)
        {
            int[] sizes = { 32, 28, 26, 24, 20,16, 14, 12, 10, 8, 6, 4, 2 };
          
            Font crFont = null;
            SizeF crSize = new SizeF();

            // 得到文字大小
            for (int i = 0; i < 13; i++)
            {
                crFont = new Font("宋体", sizes[i], FontStyle.Bold);
                if (watermarkText.Length > watermarkUrl.Length)
                {
                    crSize = picture.MeasureString(watermarkText, crFont);
                }
                else
                {
                    crSize = picture.MeasureString(watermarkUrl, crFont);
                }

                if ((ushort)crSize.Width < (ushort)width )
                    break;
            }

            float xpos = 0;
            float ypos = 0;
            switch (watermarkPosition)
            {
                case "WM_TOP_LEFT":
                    xpos = (width * (float).01) + (crSize.Width / 2);
                    ypos = height * (float).01;
                    break;
                case "WM_TOP_RIGHT":
                    xpos = (width * (float).99) - (crSize.Width / 2);
                    ypos = height * (float).01;
                    break;
                case "WM_CENTER":
                    xpos = (width * (float).48) - (crSize.Width / 2);
                    ypos = (height * (float).50) - crSize.Height;
                    break;
                case "WM_BOTTOM_RIGHT":
                    xpos = (width * (float).99) - (crSize.Width / 2);
                    ypos = (height * (float).99) - crSize.Height - crSize.Height;
                    break;
                case "WM_BOTTOM_LEFT":
                    xpos = (width * (float).01) + (crSize.Width / 2);
                    ypos = (height * (float).99) - crSize.Height - crSize.Height;
                    break;
            }
            //两次写字体，可以出现阴影效果
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(127, 0, 0, 0));
            picture.DrawString(watermarkText, crFont, semiTransBrush2, xpos + 1, ypos + 1, strFormat);
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(127, 200, 200, 200));
            picture.DrawString(watermarkText, crFont, semiTransBrush, xpos, ypos, strFormat);

            if (!string.IsNullOrEmpty(watermarkUrl))
            {
                //网址      
                SolidBrush semiTransBrushURL = new SolidBrush(Color.FromArgb(127, 0, 0, 0));
                picture.DrawString(watermarkUrl, crFont, semiTransBrushURL, xpos + 1, ypos + 1 + crSize.Height, strFormat);
                SolidBrush semiTransBrushUrls = new SolidBrush(Color.FromArgb(127, 200, 200, 200));
                picture.DrawString(watermarkUrl, crFont, semiTransBrushUrls, xpos, ypos + crSize.Height, strFormat);
            }

            semiTransBrush2.Dispose();
            semiTransBrush.Dispose();
        }
        #endregion

        #region 图片水印

        /// <summary>
        /// 加水印图片
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="waterMarkPicPath">水印图片的地址</param>
        /// <param name="watermarkPosition">水印位置</param>
        /// <param name="width">被加水印图片的宽</param>
        /// <param name="height">被加水印图片的高</param>
        private void addWatermarkImage(Graphics picture, string waterMarkPicPath, string watermarkPosition, int width, int height)
        {
            Image watermark = new Bitmap(waterMarkPicPath);

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(120, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements =
            {
                new[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                new[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                new[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                new[] {0.0f, 0.0f, 0.0f, 0.3f, 0.0f},
                new[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;
            double bl;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((width > watermark.Width * 4) && (height > watermark.Height * 4))
            {
                bl = 1;
            }
            else if ((width > watermark.Width * 4) && (height < watermark.Height * 4))
            {
                bl = Convert.ToDouble(height / 4) / Convert.ToDouble(watermark.Height);

            }
            else
            {

                if ((width < watermark.Width * 4) && (height > watermark.Height * 4))
                {
                    bl = Convert.ToDouble(width / 4) / Convert.ToDouble(watermark.Width);
                }
                else
                {
                    if ((width * watermark.Height) > (height * watermark.Width))
                    {
                        bl = Convert.ToDouble(height / 4) / Convert.ToDouble(watermark.Height);

                    }
                    else
                    {
                        bl = Convert.ToDouble(width / 4) / Convert.ToDouble(watermark.Width);

                    }

                }
            }

            int watermarkWidth = Convert.ToInt32(watermark.Width * bl);
            int watermarkHeight = Convert.ToInt32(watermark.Height * bl);

            switch (watermarkPosition)
            {
                case "WM_TOP_LEFT":
                    xpos = 10;
                    ypos = 10;
                    break;
                case "WM_TOP_RIGHT":
                    xpos = width - watermarkWidth - 10;
                    ypos = 10;
                    break;
                case "WM_CENTER":
                    xpos = (width - watermarkWidth - 10) / 2;
                    ypos = (height - watermarkHeight - 10) / 2;
                    break;
                case "WM_BOTTOM_RIGHT":
                    xpos = width - watermarkWidth - 10;
                    ypos = height - watermarkHeight - 10;
                    break;
                case "WM_BOTTOM_LEFT":
                    xpos = 10;
                    ypos = height - watermarkHeight - 10;
                    break;
            }

            picture.DrawImage(watermark, new Rectangle(xpos, ypos, watermarkWidth, watermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);


            watermark.Dispose();
            imageAttributes.Dispose();
        }

        #endregion

        #endregion
    }
}

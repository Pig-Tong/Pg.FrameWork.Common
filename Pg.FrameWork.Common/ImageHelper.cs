using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Pg.FrameWork.Common.Write;
using System.Drawing.Imaging;
using ImageProcessor.Plugins.WebP.Imaging.Formats;

namespace Pg.FrameWork.Common
{
    public class ImageHelper
    {
        public static ImgResult SaveImg(HttpPostedFile file, int maxSize, string path, bool isCut = false, bool isToWebp = false, int cutWidth = 0, int cutHeight = 0)
        {
            ImgResult imgResult = new ImgResult(false);
            if (file == null)
            {
                imgResult.IsSucceed = false;
                imgResult.Message = "上传对象为空";
                return imgResult;
            }
            if (file.ContentLength == 0)
            {
                imgResult.IsSucceed = false;
                imgResult.Message = "没有选择上传图片";
                return imgResult;
            }
            if (string.IsNullOrWhiteSpace(file.FileName))
            {
                imgResult.IsSucceed = false;
                imgResult.Message = "没有找到上传图片的格式";
                return imgResult;
            }
            string a = Path.GetExtension(file.FileName).ToLower();
            if (a != ".jpg" && a != ".jpge" && a != ".gif" && a != ".bmp" && a != ".png")
            {
                imgResult.IsSucceed = false;
                imgResult.Message = "图片格式不正确";
                return imgResult;
            }
            long num = file.ContentLength;
            if (num > maxSize * 1024 * 1024)
            {
                imgResult.IsSucceed = false;
                imgResult.Message = string.Format("图片大小超过 {0} M", maxSize);
                return imgResult;
            }
            try
            {
                imgResult = SaveImg(file, path);
                if (!imgResult.IsSucceed)
                {
                    return imgResult;
                }
                if (isCut)
                {
                    ImgResult imgResult2 = CutImg1(imgResult.FileName + "." + imgResult.FileType, imgResult.FilePath, path, cutWidth, cutHeight);
                    if (!imgResult2.IsSucceed)
                    {
                        imgResult.IsSucceed = false;
                        imgResult.Message = imgResult2.Message;
                        return imgResult;
                    }
                }
                if (!isToWebp)
                {
                    return imgResult;
                }
                ImgResult imgResult3 = ToWebp(imgResult.FileName + "." + imgResult.FileType, imgResult.FilePath, path);
                if (!imgResult3.IsSucceed)
                {
                    imgResult.IsSucceed = false;
                    imgResult.Message = imgResult3.Message;
                    return imgResult;
                }
                return imgResult;
            }
            catch
            {
                imgResult.Message = "保存图片失败";
                imgResult.IsSucceed = false;
                return imgResult;
            }
        }

        public static ImgResult SaveImg(HttpPostedFile file, string path)
        {
            ImgResult imgResult = new ImgResult(false);
            try
            {
                string text = Guid.NewGuid().ToString().Replace("-", "");
                string text2 = "jpg";
                string filename = path + "\\" + text + "." + text2;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(filename);
                imgResult.IsSucceed = true;
                imgResult.FilePath = path;
                imgResult.FileName = text;
                imgResult.FileType = text2;
                return imgResult;
            }
            catch
            {
                imgResult.IsSucceed = false;
                return imgResult;
            }
        }

        public static ImgResult CutImg(string sourceFileName, string sourcePath, string newPath, int newWidth, int newHeight)
        {
            ImgResult imgResult = new ImgResult(false);
            try
            {
                string[] array = sourceFileName.Split('.');
                string text = array[0];
                string text2 = array[1];
                string filename = string.Format("{0}\\{1}S.{2}", newPath, text, text2);
                using (Image image = GetNewImage(sourcePath + "\\" + sourceFileName, newWidth, newHeight))
                {
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    image.Save(filename);
                    imgResult.IsSucceed = true;
                    imgResult.FilePath = newPath;
                    imgResult.FileName = string.Format("{0}S", text);
                    imgResult.FileType = text2;
                    return imgResult;
                }
            }
            catch
            {
                imgResult.IsSucceed = false;
                imgResult.Message = "生成缩略图出现异常";
                return imgResult;
            }
        }

        public static ImgResult CutImg1(string sourceFileName, string sourcePath, string newPath, int newWidth, int newHeight)
        {
            ImgResult imgResult = new ImgResult(false);
            try
            {
                string[] array = sourceFileName.Split('.');
                string text = array[0];
                string text2 = array[1];
                using (Image image = GetNewImage(sourcePath + "\\" + sourceFileName))
                {
                    int width = image.Size.Width;
                    int height = image.Size.Height;
                    int num;
                    int num2;
                    if ((decimal)width / (decimal)height <= (decimal)newWidth / (decimal)newHeight)
                    {
                        num = newWidth;
                        num2 = newHeight * height / width;
                    }
                    else
                    {
                        num = newWidth * width / height;
                        num2 = newHeight;
                    }
                    string filename = string.Format("{0}\\{1}S.{2}", newPath, text, text2);
                    using (Image image2 = new Bitmap(num, num2))
                    {
                        using (Graphics graphics = Graphics.FromImage(image2))
                        {
                            graphics.InterpolationMode = InterpolationMode.High;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;
                            graphics.Clear(Color.Black);
                            graphics.DrawImage(image, new Rectangle(0, 0, num, num2), new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
                        }
                        using (Image image3 = new Bitmap(newWidth, newHeight))
                        {
                            using (Graphics graphics2 = Graphics.FromImage(image3))
                            {
                                graphics2.InterpolationMode = InterpolationMode.High;
                                graphics2.SmoothingMode = SmoothingMode.HighQuality;
                                graphics2.Clear(Color.Black);
                                int srcX = (num - newWidth) / 2;
                                int srcY = (num2 - newHeight) / 2;
                                graphics2.DrawImage(image2, new Rectangle(0, 0, newWidth, newHeight), srcX, srcY, newWidth, newHeight, GraphicsUnit.Pixel);
                                graphics2.Dispose();
                                image3.Save(filename, ImageFormat.Jpeg);
                            }
                        }
                    }
                }
                imgResult.IsSucceed = true;
                imgResult.FilePath = newPath;
                imgResult.FileName = string.Format("{0}S", text);
                imgResult.FileType = text2;
                return imgResult;
            }
            catch
            {
                imgResult.IsSucceed = false;
                imgResult.Message = "生成缩略图出现异常";
                return imgResult;
            }
        }

        public static ImgResult CutForCustom(string sourceFileName, string sourcePath, string newPath, int maxWidth, int maxHeight, int quality)
        {
            ImgResult imgResult = new ImgResult(false);
            Image image = Image.FromFile(sourcePath + "\\" + sourceFileName);
            try
            {
                string[] array = sourceFileName.Split('.');
                string text = array[0];
                string text2 = array[1];
                string filename = string.Format("{0}\\{1}C.{2}", newPath, text, text2);
                if (image.Width <= maxWidth && image.Height <= maxHeight)
                {
                    image.Save(filename, ImageFormat.Jpeg);
                }
                else
                {
                    double num = (double)maxWidth / (double)maxHeight;
                    double num2 = (double)image.Width / (double)image.Height;
                    if (num == num2)
                    {
                        Image image2 = new Bitmap(maxWidth, maxHeight);
                        Graphics graphics = Graphics.FromImage(image2);
                        graphics.InterpolationMode = InterpolationMode.High;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.Clear(Color.White);
                        graphics.DrawImage(image, new Rectangle(0, 0, maxWidth, maxHeight), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                        image2.Save(filename, ImageFormat.Jpeg);
                    }
                    else
                    {
                        Rectangle srcRect = new Rectangle(0, 0, 0, 0);
                        Rectangle destRect = new Rectangle(0, 0, 0, 0);
                        Image image3;
                        Graphics graphics2;
                        if (num > num2)
                        {
                            image3 = new Bitmap(image.Width, (int)Math.Floor((double)image.Width / num));
                            graphics2 = Graphics.FromImage(image3);
                            srcRect.X = 0;
                            srcRect.Y = (int)Math.Floor(((double)image.Height - (double)image.Width / num) / 2.0);
                            srcRect.Width = image.Width;
                            srcRect.Height = (int)Math.Floor((double)image.Width / num);
                            destRect.X = 0;
                            destRect.Y = 0;
                            destRect.Width = image.Width;
                            destRect.Height = (int)Math.Floor((double)image.Width / num);
                        }
                        else
                        {
                            image3 = new Bitmap((int)Math.Floor((double)image.Height * num), image.Height);
                            graphics2 = Graphics.FromImage(image3);
                            srcRect.X = (int)Math.Floor(((double)image.Width - (double)image.Height * num) / 2.0);
                            srcRect.Y = 0;
                            srcRect.Width = (int)Math.Floor((double)image.Height * num);
                            srcRect.Height = image.Height;
                            destRect.X = 0;
                            destRect.Y = 0;
                            destRect.Width = (int)Math.Floor((double)image.Height * num);
                            destRect.Height = image.Height;
                        }
                        graphics2.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics2.SmoothingMode = SmoothingMode.HighQuality;
                        graphics2.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                        Image image4 = new Bitmap(maxWidth, maxHeight);
                        Graphics graphics3 = Graphics.FromImage(image4);
                        graphics3.InterpolationMode = InterpolationMode.High;
                        graphics3.SmoothingMode = SmoothingMode.HighQuality;
                        graphics3.Clear(Color.White);
                        graphics3.DrawImage(image3, new Rectangle(0, 0, maxWidth, maxHeight), new Rectangle(0, 0, image3.Width, image3.Height), GraphicsUnit.Pixel);
                        ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
                        ImageCodecInfo encoder = null;
                        ImageCodecInfo[] array2 = imageEncoders;
                        foreach (ImageCodecInfo imageCodecInfo in array2)
                        {
                            if (imageCodecInfo.MimeType == "image/jpeg" || imageCodecInfo.MimeType == "image/bmp" || imageCodecInfo.MimeType == "image/png" || imageCodecInfo.MimeType == "image/gif")
                            {
                                encoder = imageCodecInfo;
                            }
                        }
                        EncoderParameters encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                        image4.Save(filename, encoder, encoderParameters);
                        graphics3.Dispose();
                        image4.Dispose();
                        graphics2.Dispose();
                        image3.Dispose();
                    }
                }
                imgResult.IsSucceed = true;
                imgResult.FilePath = newPath;
                imgResult.FileName = string.Format("{0}C", text);
                imgResult.FileType = text2;
                return imgResult;
            }
            catch
            {
                imgResult.IsSucceed = false;
                imgResult.Message = "生成缩略图出现异常";
                return imgResult;
            }
            finally
            {
                image.Dispose();
            }
        }

        public static ImgResult ToWebp(string sourceFileName, string sourcePath, string newPath)
        {
            ImgResult imgResult = new ImgResult(false);
            try
            {
                string[] array = sourceFileName.Split('.');
                string text = array[0];
                using (Image original = Image.FromFile(sourcePath + "\\" + sourceFileName))
                {
                    string text2 = string.Format("{0}\\{1}W.{2}", newPath, text, "webp");
                    Bitmap bitmap = new Bitmap(original);
                    new WebPFormat().Save(text2, original, 0);
                    //WebPFormat.SaveToFile(text2, bitmap);
                }
                imgResult.IsSucceed = true;
                imgResult.FilePath = newPath;
                imgResult.FileName = string.Format("{0}W", text);
                imgResult.FileType = "webp";
                return imgResult;
            }
            catch (System.Exception ex)
            {
                imgResult.IsSucceed = false;
                imgResult.Message = ex.Message;
                LogService.WriteLog(ex);
                return imgResult;
            }
        }

        private static Image GetNewImage(string oldImgPath, int newWidth = 0, int newHeight = 0)
        {
            Image image = Image.FromFile(oldImgPath);
            if (newWidth == 0 || newHeight == 0)
            {
                return image;
            }
            return image.GetThumbnailImage(newWidth, newHeight, IsTrue, IntPtr.Zero);
        }

        public static string DownLoadImg(string url, string savePath, string newUrl, bool isCompress = false, bool isToWebp = false)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            string[] array = url.Split('/');
            if (array.Length <= 0)
            {
                return string.Empty;
            }
            string text = array[array.Length - 1];
            if (text == "")
            {
                return string.Empty;
            }
            string[] array2 = text.Split('.');
            if (array2.Length != 2)
            {
                return string.Empty;
            }
            string text2 = string.Format("{0}.jpg", array2[0]);
            string text3 = savePath + "\\" + text2;
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, text3);
            if (isCompress)
            {
                string text4 = string.Format("{0}_c.jpg", array2[0]);
                string outPath = savePath + "\\" + text4;
                if (GetPicThumbnail(text3, outPath, 70))
                {
                    RemoveFile(text3);
                    if (isToWebp)
                    {
                        ToWebp(text4, savePath, savePath);
                    }
                    return string.Format("{0}{1}", newUrl, text4);
                }
            }
            if (isToWebp)
            {
                ToWebp(text2, savePath, savePath);
            }
            return string.Format("{0}{1}", newUrl, text2);
        }

        public static string DownLoadImg(string url, string savePath, string newUrl, int maxWidth, int maxHeight)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            string[] array = url.Split('/');
            if (array.Length <= 0)
            {
                return string.Empty;
            }
            string text = array[array.Length - 1];
            if (text == "")
            {
                return string.Empty;
            }
            string[] array2 = text.Split('.');
            if (array2.Length != 2)
            {
                return string.Empty;
            }
            string text2 = string.Format("{0}_y.jpg", array2[0]);
            string text3 = savePath + "\\" + text2;
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, text3);
            ImgResult imgResult = CutForCustom(text2, savePath, savePath, maxWidth, maxHeight, 100);
            if (imgResult.IsSucceed)
            {
                RemoveFile(text3);
                return string.Format("{0}{1}", newUrl, string.Format("{0}.jpg", imgResult.FileName));
            }
            return string.Format("{0}{1}", newUrl, text2);
        }

        public static bool GetPicThumbnail(string sFile, string outPath, int flag)
        {
            Image image = Image.FromFile(sFile);
            ImageFormat rawFormat = image.RawFormat;
            EncoderParameters encoderParameters = new EncoderParameters();
            EncoderParameter encoderParameter = new EncoderParameter(value: new long[1]
            {
                flag
            }, encoder: Encoder.Quality);
            encoderParameters.Param[0] = encoderParameter;
            try
            {
                ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo imageCodecInfo = null;
                for (int i = 0; i < imageEncoders.Length; i++)
                {
                    if (imageEncoders[i].FormatDescription.Equals("JPEG"))
                    {
                        imageCodecInfo = imageEncoders[i];
                        break;
                    }
                }
                if (imageCodecInfo != null)
                {
                    image.Save(outPath, imageCodecInfo, encoderParameters);
                }
                else
                {
                    image.Save(outPath, rawFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                image.Dispose();
                image.Dispose();
            }
        }

        public static void RemoveFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static bool IsTrue()
        {
            return true;
        }
    }
}

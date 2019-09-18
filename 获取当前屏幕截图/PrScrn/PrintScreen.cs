using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace PrScrn
{
    public static class PrintScreen
    {
        public static int Interval = 1000;

        public static string Path =@"D:\Fomart\";

        public static int MaxNumber = 10;

        

        #region 开始或结束线程
        /// <summary>
        /// 开始或结束线程
        /// </summary>
        /// <param name="flat">ture为开始，flase为结束</param>
        public static void StratThread(bool flat)
        {
            Thread th = new Thread(new ThreadStart(GetImages));
            if (flat)
            {
                th.Start();
            }
            else 
            {
                th.Abort();
            }
        }
        #endregion

        #region 获取屏幕截图(s)
        public static void GetImages()
        {
            while (true)
            {
                GetFileName(Path, MaxNumber);

                Rectangle bounds = Screen.GetBounds(Screen.GetBounds(Point.Empty));

                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    }

                    
                    MemoryStream ms = new MemoryStream();
                    bitmap.Save(Path + Guid.NewGuid().ToString() + ".jpeg", ImageFormat.Jpeg);
                }
                Thread.Sleep(Interval);
            }
        }
        #endregion

        #region 获取屏幕截图
        public static bool GetImage() 
        {
            try
            {
                GetFileName(Path, MaxNumber);

                Rectangle bounds = Screen.GetBounds(Screen.GetBounds(Point.Empty));

                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    }


                    MemoryStream ms = new MemoryStream();
                    bitmap.Save(Path + Guid.NewGuid().ToString() + ".jpeg", ImageFormat.Jpeg);
                    return true;
                }
            }
            catch 
            {
                return false;
            }
        }
        #endregion

        #region 获取截图的文件，并在Max时删除之前的文件
        /// <summary>
        /// 获取截图的文件，并在Max时删除之前的文件
        /// </summary>
        /// <param name="FilePath">截图储存路径</param>
        /// <param name="Max">最大截图数量</param>
        /// <returns></returns>
        public static string[] GetFileName(string FilePath, int Max)
        {
            string[] file = null;
            if (Directory.Exists(FilePath.Substring(0,FilePath.Length-2)))
            {
                file = Directory.GetFiles(FilePath);
            }
            else
            {
                Directory.CreateDirectory(FilePath);
                file = Directory.GetFiles(FilePath);
            }
            if (file.Length >= Max)
            {
                for (int i = 0; i < file.Length; i++)
                {
                    FileInfo fi = new FileInfo(file[i]);
                    fi.Delete();
                }
            }
            else
            {

            }
            return file;
        }
        #endregion

    }
}

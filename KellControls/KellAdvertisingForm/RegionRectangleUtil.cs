using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace KellControls.RegionRectangleBmp
{
    public enum InOutDirection
    {
        In,
        Out,
        Other
    }

    public struct Vector2
    {
        public PointF Start;
        public PointF End;
    }

    public enum GrayMethod
    {
        WeightAveraging,
        Maximum,
        Average
    }

    public static class RegionRectangleUtil
    {
        public static unsafe Bitmap FastClipBitmap(Bitmap srcBmp, Rectangle rect)
        {
            int X = (rect.X < 0) ? 0 : rect.X;
            int Y = (rect.Y < 0) ? 0 : rect.Y;
            int Width = ((rect.Width + rect.X) > srcBmp.Width) ? srcBmp.Width : (rect.Width + rect.X);
            int Height = ((rect.Height + rect.Y) > srcBmp.Height) ? srcBmp.Height : (rect.Height + rect.Y);
            Bitmap dstImage = new Bitmap(Width - X, Height - Y);
            BitmapData srcData = srcBmp.LockBits(new Rectangle(X, Y, dstImage.Width, dstImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = dstImage.LockBits(new Rectangle(0, 0, dstImage.Width, dstImage.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int offset1 = srcData.Stride - (4 * dstImage.Width);
            int offset2 = dstData.Stride - (4 * dstImage.Width);
            byte* src = (byte*)srcData.Scan0;
            byte* dst = (byte*)dstData.Scan0;
            for (int y = Y; y < Height; y++)
            {
                for (int x = X; x < Width; x++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        dst[i] = src[i];
                    }
                    src += 4;
                    dst += 4;
                }
                src += offset1;
                dst += offset2;
            }
            srcBmp.UnlockBits(srcData);
            dstImage.UnlockBits(dstData);
            srcBmp.Dispose();
            return dstImage;
        }

        public static unsafe Bitmap FastClipBitmap(Bitmap srcBmp, GraphicsPath path)
        {
            RectangleF rect = path.GetBounds();
            int X = (((int)rect.X) < 0) ? 0 : ((int)rect.X);
            int Y = (((int)rect.Y) < 0) ? 0 : ((int)rect.Y);
            int Width = (((int)(rect.Width + rect.X)) > srcBmp.Width) ? srcBmp.Width : ((int)(rect.Width + rect.X));
            int Height = (((int)(rect.Height + rect.Y)) > srcBmp.Height) ? srcBmp.Height : ((int)(rect.Height + rect.Y));
            Bitmap dstImage = new Bitmap(Width - X, Height - Y);
            BitmapData srcData = srcBmp.LockBits(new Rectangle(X, Y, dstImage.Width, dstImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = dstImage.LockBits(new Rectangle(0, 0, dstImage.Width, dstImage.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int offset1 = srcData.Stride - (4 * dstImage.Width);
            int offset2 = dstData.Stride - (4 * dstImage.Width);
            byte* src = (byte*)srcData.Scan0;
            byte* dst = (byte*)dstData.Scan0;
            for (int y = Y; y < Height; y++)
            {
                for (int x = X; x < Width; x++)
                {
                    int i;
                    if (path.IsVisible(x, y))
                    {
                        i = 0;
                        while (i < 4)
                        {
                            dst[i] = src[i];
                            i++;
                        }
                    }
                    else
                    {
                        for (i = 0; i < 3; i++)
                        {
                            dst[i] = 255;
                        }
                        dst[3] = 0;
                    }
                    src += 4;
                    dst += 4;
                }
                src += offset1;
                dst += offset2;
            }
            srcBmp.UnlockBits(srcData);
            dstImage.UnlockBits(dstData);
            srcBmp.Dispose();
            return dstImage;
        }
        public static Bitmap Thresholding(Bitmap b, int blkORwht, byte threshold)
        {
            byte[,] GrayArray = BinaryArray(b, threshold);
            return BinaryImage(GrayArray, blkORwht, threshold);
        }

        public static Bitmap Thresholding(Bitmap b, Rectangle rect, int blkORwht, byte threshold)
        {
            Bitmap bb = FastClipBitmap((Bitmap)b.Clone(), rect);
            byte[,] GrayArray = BinaryArray(bb, threshold);
            Bitmap dstImage = BinaryImage(GrayArray, blkORwht, threshold);
            bb.Dispose();
            return dstImage;
        }

        public static unsafe byte[,] Image2Array(Bitmap b, bool graied)
        {
            int width = b.Width;
            int height = b.Height;
            byte[,] GrayArray = new byte[width, height];
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte* p = (byte*)data.Scan0;
            int offset = data.Stride - (width * 4);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte gray = 0;
                    if (graied)
                    {
                        gray = p[0];
                    }
                    else
                    {
                        gray = (byte)((((19661 * p[2]) + (38666 * p[1])) + (7209 * p[0])) >> 16);
                    }
                    GrayArray[x, y] = gray;
                    p += 4;
                }
                p += offset;
            }
            b.UnlockBits(data);
            return GrayArray;
        }

        public static unsafe Bitmap Gray(Bitmap bmp)
        {
            if (bmp != null)
            {
                int width = bmp.Width;
                int height = bmp.Height;
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                byte* p = (byte*)data.Scan0;
                int offset = data.Stride - (4 * width);
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte R = p[2];
                        byte G = p[1];
                        byte B = p[0];
                        byte gray = (byte)((((19661 * R) + (38666 * G)) + (7209 * B)) >> 16);
                        p[2] = gray;
                        p[0] = p[1] = gray;
                        p += 4;
                    }
                    p += offset;
                }
                bmp.UnlockBits(data);
            }
            return bmp;
        }

        public static unsafe Bitmap Gray(Bitmap b, GrayMethod grayMethod)
        {
            byte R;
            byte G;
            byte B;
            byte gray;
            int y;
            int x;
            int width = b.Width;
            int height = b.Height;
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* p = (byte*)data.Scan0;
            int offset = data.Stride - (width * 4);
            switch (grayMethod)
            {
                case GrayMethod.Maximum:
                    for (y = 0; y < height; y++)
                    {
                        x = 0;
                        while (x < width)
                        {
                            R = p[2];
                            G = p[1];
                            B = p[0];
                            gray = ((gray = (B >= G) ? B : G) >= R) ? gray : R;
                            p[2] = gray;
                            p[0] = p[1] = gray;
                            p += 4;
                            x++;
                        }
                        p += offset;
                    }
                    break;

                case GrayMethod.Average:
                    y = 0;
                    while (y < height)
                    {
                        x = 0;
                        while (x < width)
                        {
                            R = p[2];
                            G = p[1];
                            B = p[0];
                            gray = (byte)(((R + G) + B) / 3);
                            p[2] = gray;
                            p[0] = p[1] = gray;
                            p += 4;
                            x++;
                        }
                        p += offset;
                        y++;
                    }
                    break;

                default:
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            R = p[2];
                            G = p[1];
                            B = p[0];
                            gray = (byte)((((19661 * R) + (38666 * G)) + (7209 * B)) >> 16);
                            p[2] = gray;
                            p[0] = p[1] = gray;
                            p += 4;
                        }
                        p += offset;
                    }
                    break;
            }
            b.UnlockBits(data);
            return b;
        }


        public static byte[,] BinaryArray(Bitmap b, byte threshold)
        {
            int width = b.Width;
            int height = b.Height;
            b = Gray(b, GrayMethod.WeightAveraging);
            byte[,] GrayArray = Image2Array(b, true);
            b.Dispose();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (GrayArray[x, y] >= threshold)
                    {
                        GrayArray[x, y] = 255;
                    }
                    else
                    {
                        GrayArray[x, y] = 0;
                    }
                }
            }
            return GrayArray;
        }

        public static unsafe Bitmap BinaryImage(byte[,] GrayArray, int blkORwht, byte threshold)
        {
            Color bgColor = Color.Empty;
            Color fgColor = Color.Empty;
            if (blkORwht == 0)
            {
                bgColor = Color.White;
                fgColor = Color.Black;
            }
            else
            {
                bgColor = Color.Black;
                fgColor = Color.White;
            }
            int width = GrayArray.GetLength(0);
            int height = GrayArray.GetLength(1);
            byte bgAlpha = bgColor.A;
            byte bgRed = bgColor.R;
            byte bgGreen = bgColor.G;
            byte bgBlue = bgColor.B;
            byte fgAlpha = fgColor.A;
            byte fgRed = fgColor.R;
            byte fgGreen = fgColor.G;
            byte fgBlue = fgColor.B;
            Bitmap b = new Bitmap(width, height);
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte* p = (byte*)data.Scan0;
            int offset = data.Stride - (width * 4);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (GrayArray[x, y] > threshold)
                    {
                        p[3] = fgAlpha;
                        p[2] = fgRed;
                        p[1] = fgGreen;
                        p[0] = fgBlue;
                    }
                    else
                    {
                        p[3] = bgAlpha;
                        p[2] = bgRed;
                        p[1] = bgGreen;
                        p[0] = bgBlue;
                    }
                    p += 4;
                }
                p += offset;
            }
            b.UnlockBits(data);
            return b;
        }

        public static unsafe Bitmap GetBmpByRoberts(Bitmap b)
        {
            int width = b.Width;
            int height = b.Height;
            Bitmap dstImage = new Bitmap(width, height);
            BitmapData srcData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = dstImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int stride = srcData.Stride;
            int offset = stride - (width * 4);
            byte* src = (byte*)srcData.Scan0;
            byte* dst = (byte*)dstData.Scan0;
            src += stride;
            dst += stride;
            for (int y = 1; y < height; y++)
            {
                src += 4;
                dst += 4;
                for (int x = 1; x < width; x++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int A = src[(i - stride) - 4];
                        int B = src[i - stride];
                        int C = src[i - 4];
                        int D = src[i];
                        dst[i] = (byte)Math.Sqrt((double)(((A - D) * (A - D)) + ((B - C) * (B - C))));
                    }
                    dst[3] = src[3];
                    src += 4;
                    dst += 4;
                }
                src += offset;
                dst += offset;
            }
            b.UnlockBits(srcData);
            dstImage.UnlockBits(dstData);
            b.Dispose();
            return dstImage;
        }

        public static void GetRectangle4P(PointF[] ps, out PointF p1, out PointF p2, out PointF p3, out PointF p4)
        {
            p1 = PointF.Empty;
            p2 = PointF.Empty;
            p3 = PointF.Empty;
            p4 = PointF.Empty;
            PointF[] pp = Get4PFromRectangle(ps);
            if (pp != null)
            {
                p1 = pp[0];
                p2 = pp[1];
                p3 = pp[2];
                p4 = pp[3];
            }
        }

        public static PointF[] Get4PFromRectangle(PointF[] rectangle)
        {
            PointF[] fourPs = null;
            int i;
            if (rectangle.Length < 4)
            {
                MessageBox.Show("检测点少于四个，无法取矩形！");
                return null;
            }
            List<PointF> al = new List<PointF>();
            for (i = 0; i < (rectangle.Length - 3); i += 3)
            {
                PointF a = new PointF(rectangle[i].X, rectangle[i].Y);
                PointF b = new PointF(rectangle[i].X, rectangle[i].Y);
                PointF c = new PointF(rectangle[i].X, rectangle[i].Y);
                if (Math.Abs(Dotmultiply(a, c, b)) < 1E-06)
                {
                    al.Add(b);
                }
            }
            PointF a1 = new PointF(rectangle[rectangle.Length - 1].X, rectangle[rectangle.Length - 1].Y);
            PointF b1 = new PointF(rectangle[0].X, rectangle[0].Y);
            PointF c1 = new PointF(rectangle[1].X, rectangle[1].Y);
            if (Math.Abs(Dotmultiply(a1, c1, b1)) < 1E-06)
            {
                al.Add(b1);
            }
            PointF a2 = new PointF(rectangle[rectangle.Length - 2].X, rectangle[rectangle.Length - 2].Y);
            PointF b2 = new PointF(rectangle[rectangle.Length - 1].X, rectangle[rectangle.Length - 1].Y);
            PointF c2 = new PointF(rectangle[0].X, rectangle[0].Y);
            if (Math.Abs(Dotmultiply(a2, c2, b2)) < 1E-06)
            {
                al.Add(b2);
            }
            if (al.Count < 4)
            {
                if (al.Count == 3)
                {
                    al.Add(Rect4thP((PointF)al[0], (PointF)al[1], (PointF)al[2]));
                }
            }
            else if (al.Count > 4)
            {
                PointF firstP = (PointF)al[0];
                int second = al.Count / 4;
                PointF secondP = (PointF)al[second];
                int third = (al.Count * 2) / 4;
                PointF thirdP = (PointF)al[third];
                int fourth = (al.Count * 3) / 4;
                PointF fourthP = (PointF)al[fourth];
                al.Clear();
                al.Add(firstP);
                al.Add(secondP);
                al.Add(thirdP);
                al.Add(fourthP);
            }
            if (al.Count < 4)
            {
                MessageBox.Show("拐点少于四个，无法取矩形！");
                return null;
            }
            fourPs = new PointF[4];
            for (i = 0; i < 4; i++)
            {
                fourPs[i] = (PointF)al[i];
            }
            return fourPs;
        }

        public static PointF Rect4thP(PointF a, PointF b, PointF c)
        {
            Vector2 v1 = new Vector2();
            v1.Start = new PointF(b.X, b.Y);
            v1.End = new PointF(a.X, a.Y);
            Vector2 v2 = new Vector2();
            v2.Start = new PointF(b.X, b.Y);
            v2.End = new PointF(c.X, c.Y);
            return new PointF((v2.End.X + v1.End.X) - v1.Start.X, (v2.End.Y + v1.End.Y) - v1.Start.Y);//向量相加
        }


        public static double Dotmultiply(PointF p1, PointF p2, PointF p0)
        {
            return (double)(((p1.X - p0.X) * (p2.X - p0.X)) + ((p1.Y - p0.Y) * (p2.Y - p0.Y)));
        }

        public static unsafe bool GetRectByRng(Bitmap bmp, Region rgn, InOutDirection dir, int blkORwht, byte threshold, out PointF pp1, out PointF pp2, out PointF pp3, out PointF pp4, out List<PointF> rectPs)
        {
            bool flag = false;
            pp1 = PointF.Empty;
            pp2 = PointF.Empty;
            pp3 = PointF.Empty;
            pp4 = PointF.Empty;
            rectPs = null;
            if (bmp != null)
            {
                int offset;
                int j;
                int i;
                PointF pp;
                Graphics g = Graphics.FromImage((Image)bmp.Clone());
                RectangleF rect = rgn.GetBounds(g);
                int X = (((int)rect.X) < 0) ? 0 : ((int)rect.X);
                int Y = (((int)rect.Y) < 0) ? 0 : ((int)rect.Y);
                int Width = (((int)(rect.Width + rect.X)) > bmp.Width) ? bmp.Width : ((int)(rect.Width + rect.X));
                int Height = (((int)(rect.Height + rect.Y)) > bmp.Height) ? bmp.Height : ((int)(rect.Height + rect.Y));
                Rectangle rec = new Rectangle(X, Y, Width - X, Height - Y);
                Bitmap b = Thresholding(bmp, rec, blkORwht, threshold);
                Bitmap edgeImage = GetBmpByRoberts(b);
                b.Dispose();
                List<PointF> al1 = new List<PointF>();
                List<PointF> al2 = new List<PointF>();
                List<PointF> al3 = new List<PointF>();
                List<PointF> al4 = new List<PointF>();
                if (dir == InOutDirection.In)
                {
                    BitmapData data1 = edgeImage.LockBits(new Rectangle(0, 0, rec.Width / 2, rec.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    byte* p1 = (byte*)data1.Scan0;
                    int stride1 = data1.Stride;
                    offset = stride1 - ((4 * rec.Width) / 2);
                    for (j = Y; j < (Y + rec.Height); j++)
                    {
                        p1 = (byte*)(data1.Scan0) + (j - Y) * stride1;
                        i = X;
                        while (i < (X + (rec.Width / 2)))
                        {
                            pp = new PointF((float)i, (float)j);
                            if (rgn.IsVisible(pp))
                            {
                                if (blkORwht == 0)
                                {
                                    if (p1[0] == 0)
                                    {
                                        al1.Add(pp);
                                        break;
                                    }
                                }
                                else if (p1[0] == 255)
                                {
                                    al1.Add(pp);
                                    break;
                                }
                            }
                            p1 += 4;
                            i++;
                        }
                        p1 += offset;
                    }
                    edgeImage.UnlockBits(data1);
                    BitmapData data2 = edgeImage.LockBits(new Rectangle(rec.Width / 2, 0, rec.Width / 2, rec.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    byte* p2 = (byte*)data2.Scan0;
                    int stride2 = data2.Stride;
                    for (j = Y; j < (Y + rec.Height); j++)
                    {
                        p2 = (byte*)data2.Scan0 + (j - Y) * stride2;
                        i = (X + rec.Width) - 1;
                        while (i > (X + (rec.Width / 2)))
                        {
                            pp = new PointF((float)i, (float)j);
                            if (rgn.IsVisible(pp))
                            {
                                if (blkORwht == 0)
                                {
                                    if (p2[0] == 0)
                                    {
                                        al2.Add(pp);
                                        break;
                                    }
                                }
                                else if (p2[0] == 255)
                                {
                                    al2.Add(pp);
                                    break;
                                }
                            }
                            p2 -= 4;
                            i--;
                        }
                        p2 += stride2 + ((4 * rec.Width) / 2);
                    }
                    edgeImage.UnlockBits(data2);
                }
                else if (dir == InOutDirection.Out)
                {
                    BitmapData data3 = edgeImage.LockBits(new Rectangle(0, 0, rec.Width / 2, rec.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    byte* p3 = (byte*)data3.Scan0 + (4 * rec.Width) / 2;
                    int stride3 = data3.Stride;
                    for (j = Y; j < (Y + rec.Height); j++)
                    {
                        p3 = (byte*)((data3.Scan0 + (4 * rec.Width) / 2 + (j - Y) * stride3));
                        i = (X + rec.Width) - 1;
                        while (i > (X + (rec.Width / 2)))
                        {
                            pp = new PointF((float)i, (float)j);
                            if (rgn.IsVisible(pp))
                            {
                                if (blkORwht == 0)
                                {
                                    if (p3[0] == 0)
                                    {
                                        al1.Add(pp);
                                        break;
                                    }
                                }
                                else if (p3[0] == 255)
                                {
                                    al1.Add(pp);
                                    break;
                                }
                            }
                            p3 -= 4;
                            i--;
                        }
                        p3 += stride3 + ((4 * rec.Width) / 2);
                    }
                    edgeImage.UnlockBits(data3);
                    BitmapData data4 = edgeImage.LockBits(new Rectangle(rec.Width / 2, 0, rec.Width / 2, rec.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    byte* p4 = (byte*)data4.Scan0;
                    int stride4 = data4.Stride;
                    offset = stride4 - ((4 * rec.Width) / 2);
                    j = Y;
                    while (j < (Y + rec.Height))
                    {
                        p4 = (byte*)data4.Scan0 + (j - Y) * stride4;
                        i = X;
                        while (i < (X + (rec.Width / 2)))
                        {
                            pp = new PointF((float)i, (float)j);
                            if (rgn.IsVisible(pp))
                            {
                                if (blkORwht == 0)
                                {
                                    if (p4[0] == 0)
                                    {
                                        al2.Add(pp);
                                        break;
                                    }
                                }
                                else if (p4[0] == 255)
                                {
                                    al2.Add(pp);
                                    break;
                                }
                            }
                            p4 += 4;
                            i++;
                        }
                        p4 += offset;
                        j++;
                    }
                    edgeImage.UnlockBits(data4);
                }
                if (dir == InOutDirection.In)
                {
                    BitmapData data5 = edgeImage.LockBits(new Rectangle(0, 0, rec.Width, rec.Height / 2), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    int stride5 = data5.Stride;
                    byte* p5 = (byte*)data5.Scan0;
                    for (i = X; i < (X + rec.Width); i++)
                    {
                        p5 = (byte*)data5.Scan0 + (i - X) * 4;
                        j = Y;
                        while (j < (Y + (rec.Height / 2)))
                        {
                            pp = new PointF((float)i, (float)j);
                            if (rgn.IsVisible(pp))
                            {
                                if (blkORwht == 0)
                                {
                                    if (p5[0] == 0)
                                    {
                                        al3.Add(pp);
                                        break;
                                    }
                                }
                                else if (p5[0] == 255)
                                {
                                    al3.Add(pp);
                                    break;
                                }
                            }
                            p5 += stride5;
                            j++;
                        }
                        p5 += 4 - ((stride5 * rec.Height) / 2);
                    }
                    edgeImage.UnlockBits(data5);
                    BitmapData data6 = edgeImage.LockBits(new Rectangle(0, rec.Height / 2, rec.Width, rec.Height / 2), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    int stride6 = data6.Stride;
                    byte* p6 = (byte*)data6.Scan0 + (stride6 * rec.Height) / 2;
                    for (i = X; i < (X + rec.Width); i++)
                    {
                        p6 = (byte*)(data6.Scan0 + (stride6 * rec.Height) / 2 + (i - X) * 4);
                        j = (Y + (rec.Height / 2)) - 1;
                        while (j > Y)
                        {
                            pp = new PointF((float)i, (float)j);
                            if (rgn.IsVisible(pp))
                            {
                                if (blkORwht == 0)
                                {
                                    if (p6[0] == 0)
                                    {
                                        al4.Add(pp);
                                        break;
                                    }
                                }
                                else if (p6[0] == 255)
                                {
                                    al4.Add(pp);
                                    break;
                                }
                            }
                            p6 -= stride6;
                            j--;
                        }
                        p6 += 4;
                    }
                    edgeImage.UnlockBits(data6);
                }
                else if (dir == InOutDirection.Out)
                {
                    BitmapData data7 = edgeImage.LockBits(new Rectangle(0, 0, rec.Width, rec.Height / 2), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    int stride7 = data7.Stride;
                    byte* p7 = (byte*)data7.Scan0 + (stride7 * rec.Height) / 2;
                    for (i = X; i < (X + rec.Width); i++)
                    {
                        p7 = (byte*)(data7.Scan0 + (stride7 * rec.Height) / 2 + (i - X) * 4);
                        j = (Y + (rec.Height / 2)) - 1;
                        while (j > Y)
                        {
                            pp = new PointF((float)i, (float)j);
                            if (rgn.IsVisible(pp))
                            {
                                if (blkORwht == 0)
                                {
                                    if (p7[0] == 0)
                                    {
                                        al3.Add(pp);
                                        break;
                                    }
                                }
                                else if (p7[0] == 255)
                                {
                                    al3.Add(pp);
                                    break;
                                }
                            }
                            p7 -= stride7;
                            j--;
                        }
                        p7 += 4;
                    }
                    edgeImage.UnlockBits(data7);
                    BitmapData data8 = edgeImage.LockBits(new Rectangle(0, rec.Height / 2, rec.Width, rec.Height / 2), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    int stride8 = data8.Stride;
                    byte* p8 = (byte*)data8.Scan0;
                    for (i = X; i < (X + rec.Width); i++)
                    {
                        p8 = (byte*)(data8.Scan0 + (i - X) * 4);
                        for (j = Y; j < (Y + (rec.Height / 2)); j++)
                        {
                            pp = new PointF((float)i, (float)j);
                            if (rgn.IsVisible(pp))
                            {
                                if (blkORwht == 0)
                                {
                                    if (p8[0] == 0)
                                    {
                                        al4.Add(pp);
                                        break;
                                    }
                                }
                                else if (p8[0] == 255)
                                {
                                    al4.Add(pp);
                                    break;
                                }
                            }
                            p8 += stride8;
                        }
                        p8 += 4 - ((stride8 * rec.Height) / 2);
                    }
                    edgeImage.UnlockBits(data8);
                }
                edgeImage.Dispose();
                PointF[] ps1 = al1.ToArray();
                PointF[] ps2 = al2.ToArray();
                PointF[] ps3 = al3.ToArray();
                PointF[] ps4 = al4.ToArray();
                List<PointF> al = new List<PointF>();
                for (i = 0; i < ps1.Length; i++)
                {
                    al.Add(ps1[i]);
                }
                for (i = 0; i < ps2.Length; i++)
                {
                    if (!al.Contains(ps2[i]))
                    {
                        al.Add(ps2[i]);
                    }
                }
                for (i = 0; i < ps3.Length; i++)
                {
                    if (!al.Contains(ps3[i]))
                    {
                        al.Add(ps3[i]);
                    }
                }
                for (i = 0; i < ps4.Length; i++)
                {
                    if (!al.Contains(ps4[i]))
                    {
                        al.Add(ps4[i]);
                    }
                }
                PointF[] ps = al.ToArray();
                rectPs = al;
                GetRectangle4P(ps, out pp1, out pp2, out pp3, out pp4);
                if ((((pp1 != PointF.Empty) || (pp2 != PointF.Empty)) || (pp3 != PointF.Empty)) || (pp4 != PointF.Empty))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public static unsafe Region GetRegionFromBmp(Bitmap b, Color transparentColor)
        {
            int width = b.Width;
            int height = b.Height;
            Region rgn = new Region();
            rgn.MakeEmpty();
            Rectangle curRect = new Rectangle();
            curRect.Height = 1;
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte* p = (byte*)data.Scan0;
            int offset = data.Stride - (4 * width);
            for (int y = 0; y < height; y++)
            {
                bool isTransRgn = true;
                for (int x = 0; x < width; x++)
                {
                    Color curColor = Color.FromArgb(p[2], p[1], p[0]);
                    if ((curColor == transparentColor) || (x == (width - 1)))
                    {
                        if (!isTransRgn)
                        {
                            curRect.Width = x - curRect.X;
                            rgn.Union(curRect);
                        }
                    }
                    else if (isTransRgn)
                    {
                        curRect.X = x;
                        curRect.Y = y;
                    }
                    isTransRgn = curColor == transparentColor;
                    p += 4;
                }
                p += offset;
            }
            b.UnlockBits(data);
            b.Dispose();
            return rgn;
        }
    }
}
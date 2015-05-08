using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color=System.Drawing.Color;
using PixelFormat=System.Drawing.Imaging.PixelFormat;

namespace Interop.Milib
{
    public enum ShearDirection
    {
        Right = 1,
        Left = 2,
        Down = 3,
        Up = 4
    }

    public enum RLSA_Direction
    {
        Left = 1,
        Right = 0,
        Up = 3,
        Down = 2
    }

    public enum HalveSizeType
    {
        Average = 1,
        Subsample = 2
    }

    public enum CrossModifyType
    {
        Darken = 0,
        Lighten = 1
    }

    public static class ImageOperations
    {
        #region BlurSimple

        [DllImport("milib.dll")]
        private static extern void blur_simple(int handle);

        public static void BlurSimple(int handle)
        {
            blur_simple(handle);
        }

        #endregion
        #region GetImageDimensions

        public static Size GetImageDimensions(int handle)
        {
            int width = 0, height = 0;
            IntPtr data = IntPtr.Zero;

            get_image(handle, ref data, ref width, ref height);

            return new Size(width, height);
        }

        #endregion
        #region GetImage

        [DllImport("milib.dll")]
        private static extern void get_image(int handle, ref IntPtr data, ref int width, ref int height);

        private static ColorPalette getColorPalette()
        {
            const PixelFormat bitscolordepth = PixelFormat.Format8bppIndexed;
            Bitmap bitmap = new Bitmap(1, 1, bitscolordepth);
            ColorPalette palette = bitmap.Palette;
            bitmap.Dispose();
            return palette;
        }

        private static Bitmap getImage8bpp(int handle)
        {
            int width = 0, height = 0;
            IntPtr data = IntPtr.Zero;

            get_image(handle, ref data, ref width, ref height);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            ColorPalette cp = getColorPalette();
            for (int i = 0; i < cp.Entries.Length; ++i )
            {
                cp.Entries[i] = Color.FromArgb(0xff, i, i, i);
            }
            result.Palette = cp;

            System.Drawing.Rectangle rect = new Rectangle(0, 0, result.Width, result.Height);

            System.Drawing.Imaging.BitmapData fileBitmapData = result.LockBits(rect,
                ImageLockMode.ReadWrite,
                result.PixelFormat);

            const int PixelSize = 1;

            unsafe
            {
                byte* dataptr = (byte*)data.ToPointer();

                for (int j = 0; j < result.Height; ++j)
                {
                    for (int i = 0; i < result.Width; ++i)
                    {
                        byte* screenRow = (byte*)fileBitmapData.Scan0 + j * fileBitmapData.Stride;

                        screenRow[i * PixelSize + 0] = dataptr[j * fileBitmapData.Width + i];
                    }
                }
            }

            result.UnlockBits(fileBitmapData);

            return result;
        }

        public static Bitmap GetImage(int handle)
        {
            return GetImage(handle, PixelFormat.Format24bppRgb);
        }

        public static Bitmap GetImage(int handle, PixelFormat pixelFormat)
        {
            int width = 0, height = 0;
            IntPtr data = IntPtr.Zero;

            if (pixelFormat == PixelFormat.Format8bppIndexed)
            {
                return getImage8bpp(handle);
            }

            if (pixelFormat != PixelFormat.Format24bppRgb)
            {
                throw new Exception("unsupported pixel format: " + pixelFormat);    
            }

            get_image(handle, ref data, ref width, ref height);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            System.Drawing.Rectangle rect = new Rectangle(0, 0, result.Width, result.Height);

            System.Drawing.Imaging.BitmapData fileBitmapData = result.LockBits(rect,
                ImageLockMode.ReadWrite,
                result.PixelFormat);

            const int PixelSize = 3;

            unsafe
            {
                byte* dataptr = (byte*)data.ToPointer();

                for (int j = 0; j < result.Height; ++j)
                {
                    for (int i = 0; i < result.Width; ++i)
                    {
                        byte* screenRow = (byte*)fileBitmapData.Scan0 + j * fileBitmapData.Stride;

                        screenRow[i * PixelSize + 0] = dataptr[j * fileBitmapData.Width + i];
                        screenRow[i * PixelSize + 1] = dataptr[j * fileBitmapData.Width + i];
                        screenRow[i * PixelSize + 2] = dataptr[j * fileBitmapData.Width + i];
                    }
                }
            }

            result.UnlockBits(fileBitmapData);

            return result;
        }

        public static BitmapSource GetWpfImage(int handle)
        {
            int width = 0, height = 0;
            IntPtr data = IntPtr.Zero;

            get_image(handle, ref data, ref width, ref height);

            byte[] bits = new byte[width*height*4];

            unsafe
            {
                byte* dataptr = (byte*) data.ToPointer();

                for (int j = 0; j < height; ++j)
                {
                    for (int i = 0; i < width; ++i)
                    {
                        bits[width*4*j + i*4] = dataptr[width*j + i];
                        bits[width*4*j + i*4 + 1] = dataptr[width*j + i];
                        bits[width*4*j + i*4 + 2] = dataptr[width*j + i];
                    }
                }
            }

            BitmapSource bitmap = BitmapSource.Create(width, height, 96, 96,
                                 PixelFormats.Bgr32, null, bits, width * 4);

            return bitmap;
        }

        #endregion
        #region SetImage

        [DllImport("milib.dll")]
        private static extern void set_image(ref int handle, IntPtr data, int width, int height);

        private static int setImageIndexed(Bitmap b)
        {
            System.Drawing.Rectangle rect = new Rectangle(0, 0, b.Width, b.Height);

            System.Drawing.Imaging.BitmapData fileBitmapData = b.LockBits(rect,
                ImageLockMode.ReadWrite,
                b.PixelFormat);

            const int PixelSize = 1;

            byte[] bs = new byte[fileBitmapData.Height * fileBitmapData.Width];

            unsafe
            {
                for (int j = 0; j < fileBitmapData.Height; ++j)
                {
                    for (int i = 0; i < fileBitmapData.Width; ++i)
                    {
                        byte* screenRow = (byte*)fileBitmapData.Scan0 + j * fileBitmapData.Stride;

                        bs[j * fileBitmapData.Width + i] = screenRow[i * PixelSize];
                    }
                }
            }

            b.UnlockBits(fileBitmapData);

            int handle = 0;
            IntPtr dataPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * bs.Length);
            Marshal.Copy(bs, 0, dataPtr, bs.Length);

            set_image(ref handle, dataPtr, fileBitmapData.Width, fileBitmapData.Height);

            Marshal.FreeCoTaskMem(dataPtr);

            return handle;
        }

        public static int SetImage(Bitmap b)
        {
            if (b.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                return setImageIndexed(b);
            }

            if (b.PixelFormat != PixelFormat.Format24bppRgb)
            {
                throw new Exception("unsupported pixel format.");
            }

            System.Drawing.Rectangle rect = new Rectangle(0, 0, b.Width, b.Height);

            System.Drawing.Imaging.BitmapData fileBitmapData = b.LockBits(rect,
                ImageLockMode.ReadWrite,
                b.PixelFormat);

            const int PixelSize = 3;

            byte[] bs = new byte[fileBitmapData.Height * fileBitmapData.Width];

            unsafe
            {
                for (int j = 0; j < fileBitmapData.Height; ++j)
                {
                    for (int i = 0; i < fileBitmapData.Width; ++i)
                    {
                        byte* screenRow = (byte*)fileBitmapData.Scan0 + j * fileBitmapData.Stride;

                        bs[j * fileBitmapData.Width + i] = screenRow[i * PixelSize + 2];
                    }
                }
            }

            b.UnlockBits(fileBitmapData);

            IntPtr dataPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * bs.Length);
            Marshal.Copy(bs, 0, dataPtr, bs.Length);

            int handle = 0;
            set_image(ref handle, dataPtr, fileBitmapData.Width, fileBitmapData.Height);

            Marshal.FreeCoTaskMem(dataPtr);

            return handle;
        }

        public static int SetImage(string filename)
        {
            Bitmap b = new Bitmap(filename);
            int handle = SetImage(b);
            b.Dispose();
            return handle;
        }

        #endregion
        #region DeleteImage

        [DllImport("milib.dll")]
        private static extern void delete_image(int handle);

        public static void DeleteImage(int handle)
        {
            delete_image(handle);
        }

        #endregion
        #region DeleteSeries

        [DllImport("milib.dll")]
        private static extern void delete_series(int handle);

        public static void DeleteSeries(int handle)
        {
            delete_series(handle);
        }

        #endregion
        #region Threshold

        [DllImport("milib.dll")]
        private static extern void threshold(int handle, byte level);

        public static void Threshold(int handle, byte level)
        {
            threshold(handle, level);
        }

        #endregion
        #region AddUniformNoise

        [DllImport("milib.dll")]
        private static extern void add_uniform_noise(int handle, byte level);

        public static void AddUniformNoise(int handle, byte level)
        {
            add_uniform_noise(handle, level);
        }

        #endregion
        #region CrossModify

        [DllImport("milib.dll")]
        private static extern void cross_modify(int handle, int size, int type);

        public static void CrossModify(int handle, int size, CrossModifyType type)
        {
            cross_modify(handle, size, (int)type);
        }

        #endregion
        #region Crop

        [DllImport("milib.dll")]
        private static extern void crop(int handle, int x, int y, int width, int height);

        public static void Crop(int handle, int x, int y, int width, int height)
        {
            crop(handle, x, y, width, height);
        }

        #endregion
        #region Create

        [DllImport("milib.dll")]
        private static extern void create(ref int handle, int width, int height, byte intensity);

        public static int Create(int width, int height, byte intensity)
        {
            int h = 0;
            create(ref h, width, height, intensity);
            return h;
        }

        #endregion
        #region Copy

        [DllImport("milib.dll")]
        private static extern void copy(ref int handle, int handleToCopy);

        public static int Copy(int handle)
        {
            int newHandle = 0;
            copy(ref newHandle, handle);
            return newHandle;
        }

        #endregion
        #region SizeHalve

        [DllImport("milib.dll")]
        private static extern void size_halve(int handle, int type);

        public static void SizeHalve(int handle, HalveSizeType type)
        {
            size_halve(handle, (int)type);
        }

        #endregion
        #region SizeDouble

        [DllImport("milib.dll")]
        private static extern void size_double(int handle, int type);

        public static void SizeDouble(int handle)
        {
            size_double(handle, 1);
        }

        #endregion
        #region RLSA

        [DllImport("milib.dll")]
        private static extern void rlsa(int handle, int c, int dir);

        public static void RLSA(int handle, int c, RLSA_Direction dir)
        {
            rlsa(handle, c, (int)dir);
        }

        #endregion
        #region PreThin

        [DllImport("milib.dll")]
        private static extern void pre_thin(int handle);

        public static void PreThin(int handle)
        {
            pre_thin(handle);
        }

        #endregion
        #region LogicalAnd

        [DllImport("milib.dll")]
        private static extern void logical_and(int to_modify_handle, int mask_handle);

        public static void LogicalAnd(int toModifyHandle, int maskHandle)
        {
            logical_and(toModifyHandle, maskHandle);
        }

        #endregion
        #region GetSeries

        [DllImport("milib.dll")]
        private static extern void get_series(int handle, ref IntPtr data, ref int length);

        public static List<float> GetSeries(int handle)
        {
            List<float> result = new List<float>();

            IntPtr data = IntPtr.Zero;
            int length = 0;
            get_series(handle, ref data, ref length);

            unsafe
            {
                float* dataptr = (float*)data.ToPointer();

                for (int i = 0; i < length; ++i)
                {
                    result.Add(dataptr[i]);
                }
            }

            return result;
        }

        #endregion
        #region Histogram

        [DllImport("milib.dll")]
        private static extern void histogram(int imageHandle, ref int seriesHandle);

        public static List<float> Histogram(int imageHandle)
        {
            int seriesHandle = 0;
            histogram(imageHandle, ref seriesHandle);
            List<float> s = GetSeries(seriesHandle);
            DeleteSeries(seriesHandle);
            return s;
        }

        #endregion
        #region AverageColumnIntensity

        [DllImport("milib.dll")]
        private static extern void average_column_intensity(int imageHandle, ref int seriesHandle);

        public static List<float> AverageColumnIntensity(int imageHandle)
        {
            int seriesHandle = 0;
            average_column_intensity(imageHandle, ref seriesHandle);
            List<float> s = GetSeries(seriesHandle);
            DeleteSeries(seriesHandle);
            return s;
        }

        #endregion
        #region AverageColumnGradient

        [DllImport("milib.dll")]
        private static extern void average_column_gradient(int imageHandle, ref int seriesHandle);

        public static List<float> AverageColumnGradient(int imageHandle)
        {
            int seriesHandle = 0;
            average_column_gradient(imageHandle, ref seriesHandle);
            List<float> s = GetSeries(seriesHandle);
            DeleteSeries(seriesHandle);
            return s;
        }

        #endregion
        #region GenerateBoundingBoxes

        [DllImport("milib.dll")]
        private static extern void generate_bounding_boxes(int imageHandle, ref int dataHandle);

        public static List<Rectangle> GenerateBoundingBoxes(int imageHandle)
        {
            int seriesHandle = 0;
            generate_bounding_boxes(imageHandle, ref seriesHandle);
            List<float> rawData = GetSeries(seriesHandle);
            DeleteSeries(seriesHandle);

            List<Rectangle> result = new List<Rectangle>();
            for (int i = 0; i < rawData.Count / 4; ++i)
            {
                Rectangle r = new Rectangle( 
                    (int)rawData[i * 4], 
                    (int)rawData[i * 4 + 1], 
                    (int)(rawData[i * 4 + 2] - rawData[i * 4]) + 1, 
                    (int)(rawData[i * 4 + 3] - rawData[i * 4 + 1]) + 1);
                result.Add(r);
            }

            return result;
        }

        #endregion
        #region RemoveSmallBoxes

        public static List<Rectangle> RemoveSmallBoxes(List<Rectangle> rectangles, int threshold)
        {
            List<Rectangle> result = new List<Rectangle>();

            for (int i=0; i<rectangles.Count; ++i)
            {
                if (rectangles[i].Width > threshold && rectangles[i].Height > threshold)
                {
                    result.Add(rectangles[i]);
                }
            }

            return result;
        }

        #endregion
        #region RemoveOverlappingRectangles

        public static List<Rectangle> RemoveOverlappingBoxes(List<Rectangle> rectangles)
        {
            List<Rectangle> result = new List<Rectangle>();

            bool[] used = new bool[rectangles.Count];
            for (int i = 0; i < used.Length; ++i)
            {
                used[i] = false;
            }

            for (int i = 0; i < rectangles.Count; ++i)
            {
                if (used[i])
                {
                    continue;
                }

                bool addedAnother = false;
                for (int j = i + 1; j < rectangles.Count; ++j)
                {
                    if (boxesOverlap(rectangles[i], rectangles[j]) ||
                        boxesOverlap(rectangles[j], rectangles[i]))
                    {
                        used[j] = true;
                        if (rectangles[i].Width*rectangles[i].Height <
                            rectangles[j].Width*rectangles[j].Height)
                        {
                            result.Add(rectangles[j]);
                            addedAnother = true;
                        }
                    }
                }

                if (!addedAnother)
                {
                    result.Add(rectangles[i]);
                }
            }

            return result;
        }

        private static bool boxesOverlap(Rectangle r1, Rectangle r2)
        {
            if (r1.Left >= r2.Left && r1.Left <= r2.Right)
            {
                if (r1.Top >= r2.Top && r1.Top <= r2.Bottom)
                {
                    return true;
                }
                if (r1.Bottom >= r2.Top && r1.Bottom <= r2.Bottom)
                {
                    return true;
                }
            }

            if (r1.Right >= r2.Left && r1.Right <= r2.Right)
            {
                if (r1.Top >= r2.Top && r1.Top <= r2.Bottom)
                {
                    return true;
                }
                if (r1.Bottom >= r2.Top && r1.Bottom <= r2.Bottom)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
        #region EnlargeRectangles

        public static List<Rectangle> EnlargetRectangles(List<Rectangle> rectangles, int handle)
        {
            List<Rectangle> result = new List<Rectangle>();

            Size dim = ImageOperations.GetImageDimensions(handle);

            for (int i = 0; i < rectangles.Count; ++i)
            {
                int x = rectangles[i].X;
                int y = rectangles[i].Y;
                int width = rectangles[i].Width;
                int height = rectangles[i].Height;

                if (x >= 1)
                {
                    x = x - 1;
                }
                if (y >= 1)
                {
                    y = y - 1;
                }
                if (x + width - 1 < dim.Width - 2)
                {
                    width += 2;    
                }
                if (y + height -1 < dim.Height -2)
                {
                    height += 2;
                }

                result.Add(new Rectangle(x,y, width, height));
            }

            return result;
        }

        #endregion
        #region Transpose

        [DllImport("milib.dll")]
        private static extern void transpose(int handle);

        public static void Transpose(int handle)
        {
            transpose(handle);
        }

        #endregion
        #region Shear

        [DllImport("milib.dll")]
        private static extern void shear(int handle, int dir, float lambda);

        public static void Shear(int handle, ShearDirection dir, float lambda)
        {
            shear(handle, (int)dir, lambda);
        }

        #endregion
        #region Overlay

        [DllImport("milib.dll")]
        private static extern void overlay(int to_modify_handle, int overlay_handle, int x, int y);

        public static void Overlay(int toModifyHandle, int overlayHandle, int x, int y)
        {
            overlay(toModifyHandle, overlayHandle, x, y);
        }

        #endregion
        #region HoughTransform

        [DllImport("milib.dll")]
        private static extern void hough_transform(int handle, ref int outHandle, float minAngle, float maxAngle, int numberAngles);

        public static int HoughTransform(int handle, float minAngle, float maxAngle, int numberAngles)
        {
            int h = 0;
            hough_transform(handle, ref h, minAngle, maxAngle, numberAngles);
            return h;
        }

        #endregion
        #region HoughGradient

        [DllImport("milib.dll")]
        private static extern void hough_gradient(int handle, ref int outSeriesHandle, ref float angleOfMaxValue, float minAngle, float maxAngle, int numberAngles);

        public static float HoughGradient(int handle, float minAngle, float maxAngle, int numberAngles)
        {
            int h = 0;
            float angle = 0.0f;
            hough_gradient(handle, ref h, ref angle, minAngle, maxAngle, numberAngles);
            return angle;
        }

        #endregion
        #region Invert

        [DllImport("milib.dll")]
        private static extern void invert(int handle);

        public static void Invert(int handle)
        {
            invert(handle);
        }

        #endregion
        #region Rotate

        public static void Rotate(int handle, float angle)
        {
            angle = -angle;
            if (angle > 0)
            {
                double angleRad = angle / 180 * Math.PI;
                float lambda1 = (float)Math.Tan(angleRad);
                float lambda2 = (float)Math.Sin(angleRad);
                Shear(handle, ShearDirection.Right, lambda1);
                Shear(handle, ShearDirection.Up, lambda2);
            }
            else
            {
                double angleRad = angle / 180 * Math.PI;
                float lambda1 = -(float)Math.Tan(angleRad);
                float lambda2 = -(float)Math.Sin(angleRad);
                Shear(handle, ShearDirection.Left, lambda1);
                Shear(handle, ShearDirection.Down, lambda2);
            }
        }
        #endregion
        #region Straighten

        public static void Straighten(int handle, float maxDeviation)
        {
            float angle = HoughGradient(handle, 90 - maxDeviation, 90 + maxDeviation, 20);
            Rotate(handle, angle);
        }

        #endregion
        #region CardScan_RectifyRawImageHighGradientError
       
        [DllImport("milib.dll")]
        private static extern void cardscan_rectify_raw_image_high_gradient_error(int imageHandle);

        public static void CardScan_RectifyRawImageHighGradientError(int imageHandle)
        {
            cardscan_rectify_raw_image_high_gradient_error(imageHandle);
        }
        #endregion
        #region CardScan_DetermineCardBounds

        [DllImport("milib.dll")]
        private static extern void cardscan_determine_card_bounds(int handle, int threshold, ref int min_x, ref int max_x, ref int min_y, ref int max_y);

        public static void CardScan_DetermineCardBounds(int handle, int threshold, ref int min_x, ref int max_x, ref int min_y, ref int max_y)
        {
            cardscan_determine_card_bounds(handle, threshold, ref min_x, ref max_x, ref min_y, ref max_y);
            if (max_y - min_y > 12)
            {
                max_y -= 4;
                min_y += 4;
            }
            if (max_x - min_x > 12)
            {
                max_x -= 4;
                min_x += 4;
            }
        }

        #endregion
        #region AverageRowIntensity

        [DllImport("milib.dll")]
        private static extern void average_row_intensity(int imageHandle, ref int seriesHandle);

        public static List<float> AverageRowIntensity(int imageHandle)
        {
            int seriesHandle = 0;
            average_row_intensity(imageHandle, ref seriesHandle);
            List<float> s = GetSeries(seriesHandle);
            DeleteSeries(seriesHandle);
            return s;
        }

        #endregion
        #region DrawRectangle

        [DllImport("milib.dll")]
        private static extern void draw_rectangle(int handle, int x, int y, int width, int height, byte intensity);

        public static void DrawRectangle(int handle, Rectangle r, byte intensity)
        {
            DrawRectangle(handle, r.X, r.Y, r.Width, r.Height, intensity);
        }

        public static void DrawRectangle(int handle, int x, int y, int width, int height, byte intensity)
        {
            draw_rectangle(handle, x, y, width, height, intensity);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Interop.Milib;

namespace MilibTest
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public delegate void ProcessFunction(string filename);

        public void ProcessAll(string directoryPath, ProcessFunction processFunction)
        {
            string[] files = System.IO.Directory.GetFiles(directoryPath);

            for (int i = 0; i < files.Length; ++i)
            {
                processFunction(files[i]);
            }
        }

        public void StraightenAndCrop(string filename)
        {
            int handle = ImageOperations.SetImage(new Bitmap(filename));
            ImageOperations.CardScan_RectifyRawImageHighGradientError(handle);
            int handle2 = ImageOperations.Copy(handle);
            ImageOperations.SizeHalve(handle, HalveSizeType.Average);

            ImageOperations.Invert(handle);
            float a = 90.0f - ImageOperations.HoughGradient(handle, 76, 104, 100);

            ImageOperations.Rotate(handle2, a);

            int minx = 0;
            int miny = 0;
            int maxx = 0;
            int maxy = 0;
            ImageOperations.CardScan_DetermineCardBounds(handle2, 20, ref minx, ref maxx, ref miny, ref maxy);
            ImageOperations.Crop(handle2, minx, miny, maxx - minx + 1, maxy - miny + 1);

            int handle3 = ImageOperations.Copy(handle2);
            //ImageOperations.SizeHalve(handle3, HalveSizeType.Average);
            ImageOperations.Threshold(handle3, 9);
            int handle4 = ImageOperations.Copy(handle3);
            int handle5 = ImageOperations.Copy(handle4);

            ImageOperations.RLSA(handle3, 30, RLSA_Direction.Right);
            ImageOperations.RLSA(handle4, 30, RLSA_Direction.Left);
            ImageOperations.LogicalAnd(handle3, handle4);

            ImageOperations.GetImage(handle3).Save(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filename), "__" + System.IO.Path.GetFileName(filename)));

            List<System.Drawing.Rectangle> rs = ImageOperations.GenerateBoundingBoxes(handle3);
            rs = ImageOperations.RemoveSmallBoxes(rs, 5);

            for (int i = 0; i < rs.Count; ++i )
            {
                ImageOperations.DrawRectangle(handle5, rs[i], 128);
            }

            ImageOperations.GetImage(handle5).Save(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filename), "_" + System.IO.Path.GetFileName(filename)));

            ImageOperations.DeleteImage(handle);
            ImageOperations.DeleteImage(handle2);
            ImageOperations.DeleteImage(handle3);
            ImageOperations.DeleteImage(handle4);
            ImageOperations.DeleteImage(handle5);
        }

        public Window1()
        {
            InitializeComponent();

            //int i = Levenshtein.VectorLevenshtein("Drivers, License", "Driver Licence");

            //StraightenAndCrop("C:\\Git\\CardImages\\DriversLicenses\\A.bmp");

            //ProcessAll("c:\\Temp\\CardScanExport", StraightenAndCrop);

            /*
            //int handle = ImageOperations.SetImage(new Bitmap("c:\\Temp\\CardScanExport\\2009_14_1_23.bmp"));

            int handle = ImageOperations.SetImage(new Bitmap("c:\\Git\\CardImages\\DriversLicenses\\B.bmp"));
            int handle2 = ImageOperations.Copy(handle);
            
            DateTime t = DateTime.Now;

            //int handle = Milib.SetImage(new Bitmap("c:\\Temp\\CardScanExport\\2009_14_1_23.bmp"));
            //int handle = Milib.SetImage(new Bitmap("c:\\Temp\\CardScanExport\\2009_14_1_16.bmp"));
            //int handle = Milib.SetImage(new Bitmap("c:\\Temp\\CardScanExport\\2009_14_0_49.bmp"));
           // int handle = Milib.SetImage(new Bitmap("c:\\Git\\CardImages\\MichaelHill\\D.bmp"));
            //Milib.Crop(handle, 200, 200, 500, 370);
            ImageOperations.SizeHalve(handle, HalveSizeType.Average);
            //Milib.HalveSize(handle, Milib.HalveSizeType.Average);

            //Milib.Threshold(handle, 220);
            ImageOperations.Invert(handle);

            //Milib.GetImage(handle).Save("c:\\deleteme1.bmp");

            float a = 90.0f - ImageOperations.HoughGradient(handle, 76, 104, 100);

            List<float> gs = ImageOperations.AverageColumnGradient(handle2);

            //MessageBox.Show("hello world" + (DateTime.Now - t).ToString());

            ImageOperations.CardScan_RectifyRawImageHighGradientError(handle2);
            ImageOperations.Rotate(handle2, a);

            gs = ImageOperations.AverageRowIntensity(handle2);

            List<double> ys = new List<double>();
            for (int i = 0; i < gs.Count; ++i)
            {
                ys.Add((float)gs[i]);
            }
            aa.ys = ys;

            int minx = 0;
            int miny = 0;
            int maxx = 0;
            int maxy = 0;
            ImageOperations.CardScan_DetermineCardBounds(handle2, 20, ref minx, ref maxx, ref miny, ref maxy );
            ImageOperations.Crop(handle2, minx, miny, maxx-minx+1, maxy-miny+1);

            ImageOperations.GetImage(handle2).Save("c:\\deleteme.bmp");

           // Milib.DoubleSize(handle);

            
            //const double angle = 5.0;
            //double angleRad = angle/180*Math.PI;
            //float lambda1 = (float) Math.Tan(angleRad);
            //float lambda2 = (float) Math.Sin(angleRad);

            //Milib.Shear(handle, ShearDirection.Right, lambda1);
           
            //Milib.Shear(handle, ShearDirection.Right, lambda1);
            //Milib.Shear(handle, ShearDirection.Up, lambda2);
            

            //const double angle = -5.0;
            //double angleRad = angle / 180 * Math.PI;
            //float lambda1 = -(float)Math.Tan(angleRad);
            //float lambda2 = -(float)Math.Sin(angleRad);

            //Milib.Shear(handle, ShearDirection.Right, lambda1);

            //Milib.Shear(handle, ShearDirection.Left, lambda1);
           // Milib.Shear(handle, ShearDirection.Down, lambda2);
            //Milib.Rotate(handle, 3.0);

            //Milib.Threshold(handle, 40);
            //Milib.HalveSize(handle, Milib.HalveSizeType.Average);
            
            //Milib.GetImage(handle).Save("c:\\qweqwe.bmp");

            //List<float> l = Milib.GenerateBoundingBoxes(handle);
            //Milib.Rotate(handle, 30);
            */
        }
    }
}

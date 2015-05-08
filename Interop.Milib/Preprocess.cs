using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Interop.Milib
{
    public static class Preprocess
    {
        public static void Method_A(int handle)
        {
            Size s = ImageOperations.GetImageDimensions(handle);
            ImageOperations.Crop(handle, 140, 0, s.Width - 140, s.Height);
            //ImageOperations.CardScan_RectifyRawImageHighGradientError(handle);
            
            int handle2 = ImageOperations.Copy(handle);
            ImageOperations.SizeHalve(handle2, HalveSizeType.Average);
            ImageOperations.Invert(handle2);
            float a = 90.0f - ImageOperations.HoughGradient(handle2, 84, 96, 46);
            ImageOperations.DeleteImage(handle2);

            ImageOperations.Rotate(handle, a);
           
            int minx = 0;
            int miny = 0;
            int maxx = 0;
            int maxy = 0;
            ImageOperations.CardScan_DetermineCardBounds(handle, 20, ref minx, ref maxx, ref miny, ref maxy);
            ImageOperations.Crop(handle, minx, miny, maxx - minx + 1, maxy - miny + 1);
        }
    }
}

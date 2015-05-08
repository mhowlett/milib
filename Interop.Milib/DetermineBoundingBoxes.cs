using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Interop.Milib
{
    public static class DetermineBoundingBoxes
    {
        public static List<Rectangle> Method_A(int handle)
        {
            int handle2 = ImageOperations.Copy(handle);

            ImageOperations.Threshold(handle2, 12);
            int handle3 = ImageOperations.Copy(handle2);

            ImageOperations.RLSA(handle2, 30, RLSA_Direction.Right);
            ImageOperations.RLSA(handle3, 30, RLSA_Direction.Left);
            ImageOperations.LogicalAnd(handle2, handle3);

            List<Rectangle> rs = ImageOperations.GenerateBoundingBoxes(handle2);
            rs = ImageOperations.RemoveSmallBoxes(rs, 5);
            rs = ImageOperations.RemoveOverlappingBoxes(rs);
            rs = ImageOperations.EnlargetRectangles(rs, handle2);
            rs = ImageOperations.EnlargetRectangles(rs, handle2);
            rs = ImageOperations.EnlargetRectangles(rs, handle2);

            ImageOperations.DeleteImage(handle3);
            ImageOperations.DeleteImage(handle2);

            return rs;
        }
    }
}

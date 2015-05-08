#include "stdafx.h"
#include "milib.h"
#include "image_store.h"
#include "series_store.h"

void hough_gradient(int handle, int* outSeriesHandle, float* angleOfMaxValue, float minAngle, float maxAngle, int numberAngles)
{
	image im = image_store::get(handle);

	int outhandle;
	hough_transform(handle, &outhandle, minAngle, maxAngle, numberAngles);

	im = image_store::get(outhandle);

	series se;
	se.data = new float[im.width];
	se.length = im.width;

	for (int i=0; i<im.width; ++i)
	{
		int totalgradient = 0;
		for (int j=0; j<im.height-1; ++j)
		{
			totalgradient += abs(im.data[j*im.width+i] - im.data[(j+1)*im.width+i]);
		}
		se.data[i] = totalgradient;
	}

	int maxPos = 0;
	float max = se.data[0];
	for (int i=0; i<se.length; ++i)
	{
		 if (se.data[i] > max)
		 {
			 maxPos = i;
			 max = se.data[i];
		 }
	}

	*angleOfMaxValue = ((float)maxPos / (float)(se.length - 1)) * (maxAngle - minAngle) + minAngle;
	*outSeriesHandle = series_store::set(se);
}

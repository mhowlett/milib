#include "stdafx.h"
#include <cmath>
#include "milib.h"
#include "image_store.h"
#include "series_store.h"

void average_column_gradient(int imageHandle, int* seriesHandle)
{
	image im = image_store::get(imageHandle);

	series se;
	se.data = new float[im.width];
	se.length = im.width;

	for (int i=0; i<im.width; ++i)
	{
		int sum = 0;	
		for (int j=1; j<im.height; ++j)
		{
			sum += abs(im.data[j*im.width + i] - im.data[(j-1)*im.width + i]);
		}
		se.data[i] = sum / (im.height-1);
	}

	*seriesHandle = series_store::set(se);
}
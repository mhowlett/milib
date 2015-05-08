#include "stdafx.h"
#include "milib.h"
#include "image_store.h"
#include "series_store.h"

void average_column_intensity(int imageHandle, int* seriesHandle)
{
	image im = image_store::get(imageHandle);

	series se;
	se.data = new float[im.width];
	se.length = im.width;

	for (int i=0; i<im.width; ++i)
	{
		int sum = 0;	
		for (int j=0; j<im.height; ++j)
		{
			sum += im.data[j*im.width + i];
		}
		se.data[i] = sum / im.height;
	}

	*seriesHandle = series_store::set(se);
}
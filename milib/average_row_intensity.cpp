#include "stdafx.h"
#include "milib.h"
#include "image_store.h"
#include "series_store.h"

void average_row_intensity(int imageHandle, int* seriesHandle)
{
	image im = image_store::get(imageHandle);

	series se;
	se.data = new float[im.height];
	se.length = im.height;

	for (int i=0; i<im.height; ++i)
	{
		int sum = 0;	
		for (int j=0; j<im.width; ++j)
		{
			sum += im.data[i*im.width + j];
		}
		se.data[i] = sum / im.width;
	}

	*seriesHandle = series_store::set(se);
}
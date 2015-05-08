#include "stdafx.h"
#include "milib.h"
#include "image_store.h"
#include "series_store.h"

void histogram(int imageHandle, int* seriesHandle)
{
	image im = image_store::get(imageHandle);
	series se;
	se.length = 256;
	se.data = new float[se.length];

	for (int i=0; i<se.length; ++i)
	{
		se.data[i] = 0.0f;
	}

	for (int i=0; i<im.width*im.height; ++i)
	{
		se.data[im.data[i]] += 1.0f;
	}

	*seriesHandle = series_store::set(se);
}

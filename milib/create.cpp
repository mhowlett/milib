#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void create(int* new_handle, int width, int height, unsigned char intensity)
{
	image im;
	im.width = width;
	im.height = height;
	im.data = new unsigned char[im.width*im.height];
	for (int i=0; i<width*height; ++i)
	{
		im.data[i] = intensity;
	}

	*new_handle = image_store::set(im);
}

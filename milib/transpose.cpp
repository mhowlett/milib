#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void transpose(int handle)
{
	image im = image_store::get(handle);

	unsigned char* newData = new unsigned char[im.width*im.height];

	for (int i=0; i<im.width; ++i)
	{
		for (int j=0; j<im.height; ++j)
		{
			newData[i*im.height + j] = im.data[im.width*j + i];
		}
	}

	delete[] im.data;
	im.data = newData;
	int h = im.height;
	int w = im.width;
	im.width = h;
	im.height = w;
	image_store::erase(handle);
	image_store::set(handle, im);
}

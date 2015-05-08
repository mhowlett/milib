#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void crop(int handle, int x, int y, int width, int height)
{
	image im = image_store::get(handle);

	image newImage;
	newImage.width = width;
	newImage.height = height;
	newImage.data = new unsigned char[width*height];

	for (int i=0; i<width; ++i)
	{
		for (int j=0; j<height; ++j)
		{
			newImage.data[j*width + i] = im.data[(j+y)*im.width + i + x];
		}
	}

	delete[] im.data;
	image_store::erase(handle);
	image_store::set(handle, newImage);
}

#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void size_halve(int handle, int type)
{
	image im = image_store::get(handle);

	image newImage;
	newImage.width = im.width/2;
	newImage.height = im.height/2;
	newImage.data = new unsigned char[newImage.width*newImage.height];

	if (type == 1)
	{
		for (int i=0; i<newImage.width; ++i)
		{
			for (int j=0; j<newImage.height; ++j)
			{
				int newVal = im.data[j*2*im.width + i*2];
				newVal += im.data[(j*2+1)*im.width + i*2];
				newVal += im.data[(j*2+1)*im.width + i*2 + 1];
				newVal += im.data[j*2*im.width + i*2 + 1];
				newVal /= 4;
				newImage.data[newImage.width*j + i] = newVal;
			}
		}
	}
	else
	{
		for (int i=0; i<newImage.width; ++i)
		{
			for (int j=0; j<newImage.height; ++j)
			{
				newImage.data[newImage.width*j + i] = im.data[j*2*im.width + i*2];
			}
		}
	}

	delete[] im.data;
	image_store::erase(handle);
	image_store::set(handle, newImage);
}

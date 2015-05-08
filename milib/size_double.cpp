#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void size_double(int handle, int type)
{
	image im = image_store::get(handle);

	image newImage;
	newImage.width = im.width*2;
	newImage.height = im.height*2;
	newImage.data = new unsigned char[newImage.width*newImage.height];

	if (type == 1)
	{
		for (int i=0; i<newImage.width; ++i)
		{
			for (int j=0; j<newImage.height; ++j)
			{
				int newVal = im.data[j/2*im.width + i/2];
				newImage.data[newImage.width*j + i] = newVal;
			}
		}
	}

	delete[] im.data;
	image_store::erase(handle);
	image_store::set(handle, newImage);
}

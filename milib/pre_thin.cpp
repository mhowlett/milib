#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void pre_thin(int handle)
{
	image im = image_store::get(handle);

	unsigned char* data = im.data;
	unsigned char* newData = new unsigned char[im.width*im.height];

	for (int i=1; i<im.width-1; ++i)
	{
		for (int j=1; j<im.height-1; ++j)
		{
			int centerIndex = j*im.width + i;

			int cnt = 
				data[centerIndex-1] + 
				data[centerIndex+1] + 
				data[centerIndex-im.height] + 
				data[centerIndex+im.height];

			if (cnt < 300)
			{
				newData[centerIndex] = 0;
			}
			else if (cnt > 600)
			{
				newData[centerIndex] = 255;
			}
			else
			{
				newData[centerIndex] = data[centerIndex];
			}
		}
	}

	// edges
	for (int i=0; i<im.width; ++i)
	{
		newData[i] =
			newData[im.width + i];

		newData[(im.height-1)*im.width + i] = 
			newData[(im.height-2)*im.width + i];
	}

	for (int j=0; j<im.height; ++j)
	{
		newData[im.width*j] =
			newData[im.width*j + 1];

		newData[im.width*j + im.width-1] =
			newData[im.width*j + im.width-2];
	}

	delete[] data;
	image_store::erase(handle);
	im.data = newData;
	image_store::set(handle, im);
}

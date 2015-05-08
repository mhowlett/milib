#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void threshold(int handle, unsigned char threshold)
{
	image im = image_store::get(handle);

	for (int i=0; i<im.width*im.height; ++i)
	{
		if (im.data[i] < threshold)
		{
			im.data[i] = 0;
		}
		else
		{
			im.data[i] = 255;
		}
	}
}

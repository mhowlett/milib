#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void blur_simple(int handle)
{
	image im = image_store::get(handle);

	unsigned char *data = new unsigned char[(im.width-2)*(im.height-2)];
	for (int i=1; i<im.width-1; ++i)
	{
		for (int j=1; j<im.height-1; ++j)
		{
			float val = im.data[im.width*j+i]*0.8f;
			val += im.data[im.width*(j-1)+i]*0.05f;
			val += im.data[im.width*(j+1)+i]*0.05f;
			val += im.data[im.width*j+i-1]*0.05f;
			val += im.data[im.width*j+i+1]*0.05f;
			data[(im.width-2)*(j-1)+i-1] = (unsigned char) val;
		}
	}

	im.width = im.width-2;
	im.height = im.height-2;
	delete im.data;
	im.data = data;
	image_store::set(handle, im);
}


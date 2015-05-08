#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void invert(int handle)
{
	image im = image_store::get(handle);

	unsigned char* data = im.data;

	for (int i=0; i<im.width*im.height; ++i)
	{
		data[i] = 255 - data[i];
	}
}

#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void add_uniform_noise(int handle, unsigned char maxChange)
{
	image im = image_store::get(handle);

	for (int i=0; i<im.width*im.height; ++i)
	{
		int r = rand();
		r = (int)(maxChange * ((double)r / (double)RAND_MAX) - maxChange/2);

		int v = im.data[i] + r;
		if (v > 255) v = 255;
		if (v < 0) v = 0;
			
		im.data[i] = (unsigned char)v;
	}
}


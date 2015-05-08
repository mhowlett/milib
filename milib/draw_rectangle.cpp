#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void draw_rectangle(int handle, int x, int y, int width, int height, unsigned char intensity)
{
	image im = image_store::get(handle);

	for (int i=0; i<width; ++i)
	{
		if (i+x < im.width)
		{
			if (y < im.height)
			{
				im.data[im.width*y+i+x] = intensity;
		
			}
			if (y + height - 1 < im.height)
			{
				im.data[im.width*(y+height-1)+i+x] = intensity;
			}
		}
	}

	for (int i=0; i<height; ++i)
	{
		if (i+y < im.height)
		{
			if (x < im.width)
			{
				im.data[im.width*(i+y)+x] = intensity;
			}
			if (x + width - 1 < im.width)
			{
				im.data[im.width*(i+y) + x + width - 1] = intensity;
			}
		}
	}
}


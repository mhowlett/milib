#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void logical_and(int to_modify_handle, int mask_handle)
{
	image im = image_store::get(to_modify_handle);
	image im2 = image_store::get(mask_handle);

	for (int i=0; i<im.width; ++i)
	{
		for (int j=0; j<im.height; ++j)
		{
			if (im.data[im.width*j + i] == 0 && im2.data[im.width*j + i] == 0)
			{
				im.data[im.width*j + i] = 0;
			}
			else
			{
				im.data[im.width*j + i] = 255;
			}
		}
	}
}

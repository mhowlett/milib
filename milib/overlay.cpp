#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void overlay(int to_modify_handle, int overlay_handle, int x, int y)
{
	image im = image_store::get(to_modify_handle);
	image im2 = image_store::get(overlay_handle);

	for (int i=0; i<im2.width; ++i)
	{
		for (int j=0; j<im2.height; ++j)
		{
			int posx = x + i;
			int posy = y + j;

			if (posx >= im.width)
			{
				continue;
			}

			if (posy >= im.height)
			{
				continue;
			}

			im.data[posy*im.width + posx] = im2.data[j*im2.width+i];
		}
	}
}

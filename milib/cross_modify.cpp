#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void big_cross_darken(int handle)
{
	image im = image_store::get(handle);
	
	unsigned char* newData = new unsigned char[(im.width-4)*(im.height-4)];
	for (int i=2; i<im.width-2; ++i)
	{
		for (int j=2; j<im.height-2; ++j)
		{
			int v = im.data[im.width*j + i];
			int v2 = im.data[im.width*(j+1)+i];
			int v3 = im.data[im.width*(j-1)+i];
			int v4 = im.data[im.width*j+i+1];
			int v5 = im.data[im.width*j+i-1];
			int v6 = im.data[im.width*(j+2)+i];
			int v7 = im.data[im.width*(j-2)+i];
			int v8 = im.data[im.width*j+i+2];
			int v9 = im.data[im.width*j+i-2];

			if (v2 < v)
			{
				v = v2;
			}
			if (v3 < v)
			{
				v = v3;
			}
			if (v4 < v)
			{
				v = v4;
			}
			if (v5 < v)
			{
				v = v5;
			}
			if (v6 < v)
			{
				v = v6;
			}
			if (v7 < v)
			{
				v = v7;
			}
			if (v8 < v)
			{
				v = v8;
			}
			if (v9 < v)
			{
				v = v9;
			}
			newData[(im.width-4)*(j-2)+i-2] = v;
		}
	}

	delete[] im.data;
	im.data = newData;
	im.width = im.width-4;
	im.height = im.height-4;

	image_store::erase(handle);
	image_store::set(handle, im);
}

void cross_darken(int handle, int size)
{
	if (size == 2)
	{
		big_cross_darken(handle);
		return;
	}

	image im = image_store::get(handle);

	unsigned char* newData = new unsigned char[(im.width-2)*(im.height-2)];
	for (int i=1; i<im.width-1; ++i)
	{
		for (int j=1; j<im.height-1; ++j)
		{
			int v = im.data[im.width*j + i];
			int v2 = im.data[im.width*(j+1)+i];
			int v3 = im.data[im.width*(j-1)+i];
			int v4 = im.data[im.width*j+i+1];
			int v5 = im.data[im.width*j+i-1];
			
			if (v2 < v)
			{
				v = v2;
			}
			if (v3 < v)
			{
				v = v3;
			}
			if (v4 < v)
			{
				v = v4;
			}
			if (v5 < v)
			{
				v = v5;
			}

			newData[(im.width-2)*(j-1)+i-1] = v;
		}
	}

	delete[] im.data;
	im.data = newData;
	im.width = im.width-2;
	im.height = im.height-2;

	image_store::erase(handle);
	image_store::set(handle, im);
}


void big_cross_lighten(int handle)
{
	image im = image_store::get(handle);
	
	unsigned char* newData = new unsigned char[(im.width-4)*(im.height-4)];
	for (int i=2; i<im.width-2; ++i)
	{
		for (int j=2; j<im.height-2; ++j)
		{
			int v = im.data[im.width*j + i];
			int v2 = im.data[im.width*(j+1)+i];
			int v3 = im.data[im.width*(j-1)+i];
			int v4 = im.data[im.width*j+i+1];
			int v5 = im.data[im.width*j+i-1];
			int v6 = im.data[im.width*(j+2)+i];
			int v7 = im.data[im.width*(j-2)+i];
			int v8 = im.data[im.width*j+i+2];
			int v9 = im.data[im.width*j+i-2];

			if (v2 > v)
			{
				v = v2;
			}
			if (v3 > v)
			{
				v = v3;
			}
			if (v4 > v)
			{
				v = v4;
			}
			if (v5 > v)
			{
				v = v5;
			}
			if (v6 > v)
			{
				v = v6;
			}
			if (v7 > v)
			{
				v = v7;
			}
			if (v8 > v)
			{
				v = v8;
			}
			if (v9 > v)
			{
				v = v9;
			}
			newData[(im.width-4)*(j-2)+i-2] = v;
		}
	}

	delete[] im.data;
	im.data = newData;
	im.width = im.width-4;
	im.height = im.height-4;

	image_store::erase(handle);
	image_store::set(handle, im);
}

void cross_lighten(int handle, int size)
{
	if (size == 2)
	{
		big_cross_lighten(handle);
		return;
	}

	image im = image_store::get(handle);

	unsigned char* newData = new unsigned char[(im.width-2)*(im.height-2)];
	for (int i=1; i<im.width-1; ++i)
	{
		for (int j=1; j<im.height-1; ++j)
		{
			int v = im.data[im.width*j + i];
			int v2 = im.data[im.width*(j+1)+i];
			int v3 = im.data[im.width*(j-1)+i];
			int v4 = im.data[im.width*j+i+1];
			int v5 = im.data[im.width*j+i-1];
			
			if (v2 > v)
			{
				v = v2;
			}
			if (v3 > v)
			{
				v = v3;
			}
			if (v4 > v)
			{
				v = v4;
			}
			if (v5 > v)
			{
				v = v5;
			}

			newData[(im.width-2)*(j-1)+i-1] = v;
		}
	}

	delete[] im.data;
	im.data = newData;
	im.width = im.width-2;
	im.height = im.height-2;

	image_store::erase(handle);
	image_store::set(handle, im);
}

void cross_modify(int handle, int size, int type)
{
	if (type == 0)
	{
		cross_darken(handle, size);
		return;
	}

	cross_lighten(handle, size);
}
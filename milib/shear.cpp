#include "stdafx.h"
#include "milib.h"
#include "image_store.h"
#include <cmath>

void shear(int handle, int dir, float lambda)
{
	image im = image_store::get(handle);

	if (dir == 1)
	{
		int extra_width = im.height * lambda;
		int newWidth = im.width + extra_width;

		unsigned char* newData = new unsigned char[newWidth*im.height];
		for (int i=0; i<newWidth*im.height; ++i)
		{
			newData[i] = 0;
		}

		for (int j=0; j<im.height; ++j)
		{	
			float offset = j*lambda;
			float extra = offset - floor(offset);
			int offset_i = offset;

			for (int i=0; i<im.width-1; ++i)
			{
				newData[newWidth*j + i + offset_i] = (extra)*im.data[im.width*j + i] + (1-extra)*im.data[im.width*j + i + 1];
			}
		}

		delete[] im.data;
		im.data = newData;
		im.width = newWidth;
		image_store::erase(handle);
		image_store::set(handle, im);
		return;
	}

	if (dir == 2)
	{
		int extra_width = im.height * lambda;
		int newWidth = im.width + extra_width;

		unsigned char* newData = new unsigned char[newWidth*im.height];
		for (int i=0; i<newWidth*im.height; ++i)
		{
			newData[i] = 0;
		}

		for (int j=0; j<im.height; ++j)
		{		
			float offset = (im.height-j-1)*lambda;
			float extra = offset - floor(offset);
			int offset_i = offset;

			for (int i=1; i<im.width; ++i)
			{
				newData[newWidth*j + i + offset_i] = (1-extra)*im.data[im.width*j + i] + extra*im.data[im.width*j + i - 1];
			}
		}

		delete[] im.data;
		im.data = newData;
		im.width = newWidth;
		image_store::erase(handle);
		image_store::set(handle, im);
		return;
	}

	if (dir == 3)
	{
		int extra_height = im.width * lambda;
		int newHeight = im.height + extra_height;

		unsigned char* newData = new unsigned char[im.width*newHeight];
		for (int i=0; i<im.width*newHeight; ++i)
		{
			newData[i] = 0;
		}

		for (int i=0; i<im.width; ++i)
		{
			float offset = i*lambda;
			float extra = offset - floor(offset);
			int offset_i = offset;

			for (int j=0; j<im.height-1; ++j)
			{		
				newData[im.width*(j+offset_i) + i] = (extra)*im.data[im.width*j + i] + (1-extra)*im.data[im.width*(j+1) + i];
			}
		}

		delete[] im.data;
		im.data = newData;
		im.height = newHeight;
		image_store::erase(handle);
		image_store::set(handle, im);
		return;
	}

	if (dir == 4)
	{
		int extra_height = im.width * lambda;
		int newHeight = im.height + extra_height;

		unsigned char* newData = new unsigned char[im.width*newHeight];
		for (int i=0; i<im.width*newHeight; ++i)
		{
			newData[i] = 0;
		}

		for (int i=0; i<im.width; ++i)
		{
			float offset = (im.width-i-1)*lambda;
			float extra = offset - floor(offset);
			int offset_i = offset;

			for (int j=1; j<im.height; ++j)
			{		
				newData[im.width*(j+offset_i) + i] = (1-extra)*im.data[im.width*j + i] + extra*im.data[im.width*(j-1) + i];
			}
		}

		delete[] im.data;
		im.data = newData;
		im.height = newHeight;
		image_store::erase(handle);
		image_store::set(handle, im);
		return;
	}
}

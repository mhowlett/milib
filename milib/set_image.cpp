#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void set_image(int* handle, unsigned char *bytes, int width, int height)
{
	image newImage;
	newImage.data = new unsigned char[width*height];
	memcpy(newImage.data, bytes, width*height);
	newImage.width = width;
	newImage.height = height;
	
	*handle = image_store::set(newImage);
}

